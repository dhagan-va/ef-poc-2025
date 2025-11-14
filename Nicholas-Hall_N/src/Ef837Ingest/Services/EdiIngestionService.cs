using System.Reflection;
using System.Text.Json;
using System.Linq;
using Edi837Ingestion.Data;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;   
using EdiFabric.Templates.X12004010;    
using Ef837Ingest.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

using Ef837Ingest.Edi.Validator;

namespace Edi837Ingestion.Edi
{
    public class EdiIngestionService : IEdiIngestionService
    {
        private readonly Hipaa5010Context _db;
        private readonly ILogger<EdiIngestionService> _log;

        public EdiIngestionService(Hipaa5010Context db, ILogger<EdiIngestionService> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<int> IngestAsync(Stream ediStream, CancellationToken ct = default)
        {
            using var reader = new X12Reader(
                ediStream,
                (isa, gs, st) =>
                {
                    if (st.TransactionSetIdentifierCode_01 == "837")
                    {
                        switch (st.ImplementationConventionPreference_03)
                        {
                            case "005010X222A1": 
                                return typeof(TS837P).GetTypeInfo();

                            // If/when you add Dental/Institutional support, uncomment:
                            // case "005010X224A2": // 837 Dental
                            //     return typeof(TS837D).GetTypeInfo();
                            // case "005010X223A2": // 837 Institutional
                            //     return typeof(TS837I).GetTypeInfo();

                            default:
                                return null;
                        }
                    }

                    if (st.TransactionSetIdentifierCode_01 == "850")
                        return typeof(TS850).GetTypeInfo();

                    return null;
                },
                new X12ReaderSettings { Split = true });

            ISA? currentIsa = null;
            GS? currentGs = null;


            int saved = 0;
            IDbContextTransaction? tx = null;

            try
            {
                if (_db.Database.IsRelational())
                {
                    tx = await _db.Database.BeginTransactionAsync(ct);
                }

                while (reader.Read())
                {
                    ct.ThrowIfCancellationRequested();

                    switch (reader.Item)
                    {
                        case ISA isa:
                            currentIsa = isa;
                            _db.ISA.Add(new IsaEntity
                            {
                                InterchangeControlNumber = isa.InterchangeControlNumber_13,
                                SenderId = isa.InterchangeSenderID_6,
                                ReceiverId = isa.InterchangeReceiverID_8,
                                RawJson = JsonSerializer.Serialize(isa)
                            });
                            break;

                        case GS gs:
                            currentGs = gs;
                            _db.GS.Add(new GsEntity
                            {
                                FunctionalIdCode = gs.CodeIdentifyingInformationType_1,
                                AppSenderCode = gs.SenderIDCode_2,
                                AppReceiverCode = gs.ReceiverIDCode_3,
                                GroupControlNumber = gs.GroupControlNumber_6,
                                RawJson = JsonSerializer.Serialize(gs)
                            });
                            break;

                        case ST st:
                            _db.ST.Add(new StEntity
                            {
                                TransactionSetId = st.TransactionSetIdentifierCode_01,
                                ControlNumber = st.TransactionSetControlNumber_02,
                                RawJson = JsonSerializer.Serialize(st)
                            });
                            break;

                        case TS837P msg837:
                            {
                                var snipOptions = new SnipOptions
                                {
                                    Level = SnipLevel.Snip4,
                                    FailOnError = true,
                                    ThrowOnNullMessage = true
                                };

                                var snip = SnipValidator.Validate(msg837, snipOptions);
                                var controlNumber = msg837.ST?.TransactionSetControlNumber_02;

                                if (!snip.IsValid)
                                {
                                    var segErrors = snip.ErrorContext?.Errors
                                                    ?? Enumerable.Empty<EdiFabric.Core.Model.Edi.ErrorContexts.SegmentErrorContext>();

                                    foreach (var e in segErrors)
                                    {
                                        _db.ValidationIssues.Add(new ValidationIssue
                                        {
                                            Transaction = "837P",
                                            ControlNumber = controlNumber,
                                            Level = snipOptions.Level.ToString(),
                                            SegmentId = e.LoopId,
                                            Position = e.Position,
                                            Code = e.Name,
                                            Message = e?.Errors?.FirstOrDefault()?.Message ?? "Unknown error",
                                            Severity = "Error",
                                            RawContextJson = JsonSerializer.Serialize(e)
                                        });
                                    }

                                    await _db.SaveChangesAsync(ct);

                                    _log.LogError("SNIP validation failed for 837P ControlNumber={CN} (Level={Level}).",
                                        controlNumber, snipOptions.Level);
                                }


                                // Billing provider HL
                                var loop2000A = msg837.Loop2000A?.FirstOrDefault();

                                // Subscriber HL
                                var loop2000B = loop2000A?.Loop2000B?.FirstOrDefault();

                                // First claim loop
                                var loop2300 = loop2000B?.Loop2300?.FirstOrDefault();

                                var clm = loop2300?.CLM_ClaimInformation;

                                var patientControlNumber = clm?.PatientControlNumber_01;

                                decimal? totalClaimAmount = null;
                                if (decimal.TryParse(
                                        clm?.TotalClaimChargeAmount_02,
                                        out var amt))
                                {
                                    totalClaimAmount = amt;
                                }

                                var loop2010BA = loop2000B?.AllNM1?.Loop2010BA;
                                var subNm1 = loop2010BA?.NM1_SubscriberName;

                                string? patientLastName = subNm1?.ResponseContactLastorOrganizationName_03;
                                string? patientFirstName = subNm1?.ResponseContactFirstName_04;

                                var loop2010AA = loop2000A?.AllNM1?.Loop2010AA;
                                var billingNm1 = loop2010AA?.NM1_BillingProviderName;

                                string? billingProviderNpi = billingNm1?.ResponseContactIdentifier_09;

                                DateTime? serviceFromDate = null;
                                if (loop2300 != null)
                                {
                                    serviceFromDate = GetClaimHeaderDate(loop2300.AllDTP);
                                }

                                var tsEntity = new Ts837pEntity
                                {
                                    ControlNumber = controlNumber,
                                    PatientControlNumber = patientControlNumber,
                                    RawJson = JsonSerializer.Serialize(msg837)
                                };
                                _db.TS837P.Add(tsEntity);

                                var claim = new ClaimEntity
                                {
                                    TransactionSetControlNumber = controlNumber,
                                    PatientControlNumber = patientControlNumber,
                                    PatientLastName = patientLastName,
                                    PatientFirstName = patientFirstName,
                                    TotalClaimAmount = totalClaimAmount,
                                    ServiceFromDate = serviceFromDate,
                                    BillingProviderNpi = billingProviderNpi
                                };
                                _db.Claims.Add(claim);

                                saved++;
                                break;
                            }


                        case TS850 po850:
                            _db.TS850.Add(new Ts850Entity
                            {
                                PurchaseOrderNumber = po850?.BEG?.PurchaseOrderNumber_03,
                                RawJson = JsonSerializer.Serialize(po850)
                            });
                            saved++;
                            break;

                        case SE se:
                            _db.SE.Add(new SeEntity
                            {
                                SegmentCount = int.TryParse(se.NumberofIncludedSegments_01, out var n) ? n : 0,
                                ControlNumber = se.TransactionSetControlNumber_02,
                                RawJson = JsonSerializer.Serialize(se)
                            });
                            break;
                    }
                }

                await _db.SaveChangesAsync(ct);

                if (tx is not null)
                {
                    await tx.CommitAsync(ct);
                }

                return saved;
            }
            catch (Exception ex)
            {
                if (tx is not null)
                    await tx.RollbackAsync(ct);

                _log.LogError(ex, "Failed to ingest EDI file.");
                throw;
            }
        }
        private static DateTime? TryParseDtpDate(string? format, string? value)
        {
            if (string.IsNullOrWhiteSpace(format) || string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                switch (format)
                {
                    case "D8": // single date
                        if (DateTime.TryParseExact(value, "yyyyMMdd",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out var d8))
                            return d8;
                        break;

                    case "RD8": // date range: yyyyMMdd-yyyMMdd
                        var parts = value.Split('-');
                        if (parts.Length >= 1 &&
                            DateTime.TryParseExact(parts[0], "yyyyMMdd",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out var from))
                            return from;
                        break;
                }
            }
            catch
            {
                // ignore and fall through
            }

            return null;
        }
        private static DateTime? GetClaimHeaderDate(All_DTP_837P_2? allDtp)
        {
            if (allDtp == null) return null;

            var candidates = new List<object?>
            {
                allDtp.DTP_Date_OnsetofCurrentIllnessorSymptom,
                allDtp.DTP_Date_InitialTreatmentDate,
                allDtp.DTP_Date_LastSeenDate,
                allDtp.DTP_Date_Accident,
                allDtp.DTP_Date_Admission,
                allDtp.DTP_Date_Discharge,
                allDtp.DTP_Date_DisabilityDates,
                allDtp.DTP_Date_LastWorked,
                allDtp.DTP_Date_AuthorizedReturntoWork,
                allDtp.DTP_PropertyandCasualtyDateofFirstContact,
                allDtp.DTP_Date_RepricerReceivedDate
            };

            if (allDtp.DTP_Date_AssumedandRelinquishedCareDates != null)
                candidates.AddRange(allDtp.DTP_Date_AssumedandRelinquishedCareDates);

            foreach (var candidate in candidates)
            {
                var dt = TryGetDtpDate(candidate);
                if (dt.HasValue)
                    return dt.Value;
            }

            return null;
        }

        private static DateTime? TryGetDtpDate(object? dtpSegment)
        {
            if (dtpSegment == null) return null;

            var type = dtpSegment.GetType();

            var format = type.GetProperty("DateTimePeriodFormatQualifier_02")
                ?.GetValue(dtpSegment) as string;

            var value = type.GetProperty("DateTimePeriod_03")
                ?.GetValue(dtpSegment) as string;

            return TryParseDtpDate(format, value);
        }
    }
}
