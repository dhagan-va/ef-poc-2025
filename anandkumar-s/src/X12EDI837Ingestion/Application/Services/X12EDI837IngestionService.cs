using System.Globalization;
using System.Reflection;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.Logging;
using X12EDI837Ingestion.Application.Interfaces;
using X12EDI837Ingestion.Domain.Entities;
using X12EDI837Ingestion.Infrastructure.Repositories;
using EdiFabric.Templates.X12004010;

namespace X12EDI837Ingestion.Application.Services
{

    public sealed class X12EDI837IngestionService : IX12EDI837IngestionService
    {
        private readonly ILogger<X12EDI837IngestionService> _logger;
        private readonly IX12EDI837IngestRepo _repo;

        public X12EDI837IngestionService(
            ILogger<X12EDI837IngestionService> logger,
            IX12EDI837IngestRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task ProcessIngestionAsync(string filePath, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            _logger.LogInformation("ProcessIngestionAsync started. File={File}", filePath);

            await using var stream = File.OpenRead(filePath);

            await ProcessEDIStream(stream, filePath, ct);

            _logger.LogInformation("ProcessIngestionAsync completed. File={File}", filePath);
        }

        public async Task ProcessIngestionAsync(
                            Stream stream,
                            string sourceName,
                            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentException.ThrowIfNullOrWhiteSpace(sourceName);

            _logger.LogInformation("ProcessIngestionAsync started. Source={SourceName}", sourceName);

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            await ProcessEDIStream(stream, sourceName, cancellationToken);

            _logger.LogInformation("ProcessIngestionAsync completed. Source={SourceName}", sourceName);
        }


        // ---------------- Mapping helpers ----------------

        private static InterchangeHeader MapIsa(ISA isa, string sourceName)
            => new()
            {
                SourceFile = Path.GetFileName(sourceName),
                InterchangeControlNumber = isa.InterchangeControlNumber_13,
                SenderId = isa.InterchangeSenderID_6,
                ReceiverId = isa.InterchangeReceiverID_8,
                Date = isa.InterchangeDate_9,
                Time = isa.InterchangeTime_10
            };

        private static FunctionalGroupHeader MapGs(GS gs)
            => new()
            {
                FunctionalIdentifierCode = gs.CodeIdentifyingInformationType_1, // HC
                SenderCode = gs.SenderIDCode_2,
                ReceiverCode = gs.ReceiverIDCode_3,
                Date = gs.Date_4,
                Time = gs.Time_5,
                GroupControlNumber = gs.GroupControlNumber_6,
                Version = gs.VersionAndRelease_8
            };

        private static TransactionSetHeader MapTransactionSetFrom837(TS837P ts837)
            => new()
            {
                TransactionSetId =
                    ts837.ST?.TransactionSetIdentifierCode_01,
                TransactionSetControlNumber =
                    ts837.ST?.TransactionSetControlNumber_02,
                ImplementationConvention =
                    ts837.ST?.ImplementationConventionPreference_03,
                BhtReferenceId =
                    ts837.BHT_BeginningOfHierarchicalTransaction?.SubmitterTransactionIdentifier_03 // e.g., 244579
            };

        private static void MapClaimsAndLinesFrom837(TS837P ts837, TransactionSetHeader st)
        {
            var loop2000A = ts837.Loop2000A;
            if (loop2000A == null) return;

            foreach (var a in loop2000A)
            {
                var loop2000B = a.Loop2000B;
                if (loop2000B == null) continue;

                foreach (var b in loop2000B)
                {
                    var loop2000C = b.Loop2000C;
                    if (loop2000C == null) continue;

                    foreach (var c in loop2000C)
                    {
                        // Optional patient name loop (doesn't affect claim extraction, but you can use it if you want)
                        var patientName = c.Loop2010CA?.NM1_PatientName;
                        var patientLast = patientName?.ResponseContactLastorOrganizationName_03;
                        var patientFirst = patientName?.ResponseContactFirstName_04;


                        var loop2300 = c.Loop2300;
                        if (loop2300 == null) continue;

                        foreach (var l2300 in loop2300)
                        {
                            var clm = l2300.CLM_ClaimInformation;
                            if (clm == null) continue;

                            var claim = new Claim
                            {
                                ClaimSubmitterId = l2300.CLM_ClaimInformation.PatientControlNumber_01,
                                TotalClaimChargeAmount = TryParseDecimal(clm.TotalClaimChargeAmount_02),
                                FacilityCode = clm.HealthCareServiceLocationInformation_05.FacilityTypeCode_01,
                                ClaimFrequencyCode = clm.HealthCareServiceLocationInformation_05.ClaimFrequencyTypeCode_03,

                                // Optional if your entity supports it:
                                // PatientLastName = patientLast,
                                // PatientFirstName = patientFirst
                            };

                            // Service lines: Loop2400 -> SV1
                            var loop2400 = l2300.Loop2400;
                            if (loop2400 != null)
                            {
                                foreach (var l2400 in loop2400)
                                {
                                    var sv1 = l2400.SV1_ProfessionalService;
                                    if (sv1 == null) continue;

                                    var proc = sv1.CompositeMedicalProcedureIdentifier_01;
                                    var procCode = proc?.ProcedureCode_02;

                                    claim.ServiceLines.Add(new ServiceLine
                                    {
                                        ProcedureCode = procCode,
                                        LineItemChargeAmount = TryParseDecimal(sv1.LineItemChargeAmount_02),
                                        UnitOrBasis = sv1.UnitorBasisforMeasurementCode_03,
                                        UnitCount = TryParseDecimal(sv1.ServiceUnitCount_04)
                                    });
                                }
                            }

                            st.Claims.Add(claim);
                        }
                    }
                }
            }
        }

        private static void MapPartiesFrom837(TS837P ts837, TransactionSetHeader st)
        {
            var allNm1 = ts837.AllNM1;
            if (allNm1 == null) return;

            // 1000A - Submitter
            var sub = allNm1.Loop1000A?.NM1_SubmitterName;
            if (sub != null)
            {
                st.Parties.Add(new Party
                {
                    PartyType = "Submitter",
                    LastNameOrOrgName = sub.ResponseContactLastorOrganizationName_03,
                    FirstName = sub.ResponseContactFirstName_04,
                    IdCodeQualifier = sub.IdentificationCodeQualifier_08,
                    IdCode = sub.ResponseContactIdentifier_09
                });
            }

            // 1000B - Receiver
            var recv = allNm1.Loop1000B?.NM1_ReceiverName;
            if (recv != null)
            {
                st.Parties.Add(new Party
                {
                    PartyType = "Receiver",
                    LastNameOrOrgName = recv.ResponseContactLastorOrganizationName_03,
                    FirstName = recv.ResponseContactFirstName_04,
                    IdCodeQualifier = recv.IdentificationCodeQualifier_08,
                    IdCode = recv.ResponseContactIdentifier_09
                });
            }
        }

        // ✅ Culture-invariant decimal parsing
        private static decimal? TryParseDecimal(string? value)
            => decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var d)
                ? d
                : null;

        private static void EnsureInterchange(InterchangeHeader? interchange)
        {
            if (interchange is null)
                throw new InvalidOperationException("ISA not found; cannot create InterchangeHeader.");
        }

        // ✅ Safely detect HasErrors if the template exposes it (varies by EdiFabric/template versions)
        private static bool HasErrors(object item)
        {
            try
            {
                var prop = item.GetType().GetProperty("HasErrors", BindingFlags.Public | BindingFlags.Instance);
                return prop?.PropertyType == typeof(bool) && (bool)(prop.GetValue(item) ?? false);
            }
            catch
            {
                return false;
            }
        }

        private async Task ProcessEDIStream(Stream stream, string sourceName, CancellationToken ct = default)
        {
            using var reader = new X12Reader(stream, "EdiFabric.Templates.Hipaa");


            InterchangeHeader? interchange = null;
            FunctionalGroupHeader? currentGs = null;

            // Stream read to keep memory stable and preserve proper GS context
            while (reader.Read())
            {
                ct.ThrowIfCancellationRequested();
                var item = reader.Item;
                // Fail fast on parse errors emitted by the reader.
                if (item is ErrorContext err)
                    throw new InvalidOperationException($"EDI parse error: {err.Message}");


                if (HasErrors(item))
                    throw new InvalidOperationException($"EDI parse error: item {item.GetType().Name} contains errors.");

                switch (item)
                {
                    case ISA isa:
                        {
                            interchange = MapIsa(isa, sourceName);
                            break;
                        }

                    case GS gs:
                        {
                            EnsureInterchange(interchange);

                            currentGs = MapGs(gs);
                            interchange!.FunctionalGroups.Add(currentGs);
                            break;
                        }

                    case ST st:
                        {
                            
                            _logger.LogInformation("ST01={Id} ST02={Ctrl} ST03={Impl}",
                                st.TransactionSetIdentifierCode_01,
                                st.TransactionSetControlNumber_02,
                                st.ImplementationConventionPreference_03);

                            break;
                        }

                    default:
                        {
                            if (item is TS837P tx)
                            {
                                EnsureInterchange(interchange);

                                if (currentGs is null)
                                    throw new InvalidOperationException("GS not found. Cannot attach transaction sets.");

                                string? st02 = null;

                                try
                                {
                                    // ST header access (strongly typed)
                                    st02 = tx.ST?.TransactionSetControlNumber_02;

                                    var stHeader = MapTransactionSetFrom837(tx);
                                    currentGs.TransactionSets.Add(stHeader);
                                    MapPartiesFrom837(tx, stHeader);

                                    MapClaimsAndLinesFrom837(tx, stHeader);

                                    if (stHeader.Claims.Count == 0)
                                    {
                                        _logger.LogWarning(
                                            "No claims found while mapping 837 transaction. ST02={Ctrl}.",
                                            st02);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex,
                                        "Failed mapping 837 transaction. ST02={Ctrl}",
                                        st02);

                                    throw;
                                }
                            }

                            break;
                        }
                }
            }

            EnsureInterchange(interchange);

            // Persist as one unit (repo already has transaction + correct insert order)
            var interchangeId = await _repo.InsertInterchangeAsync(interchange, ct);

            _logger.LogInformation("ProcessIngestionAsync completed. InterchangeId={Id}", interchangeId);
        }

        
    }
}