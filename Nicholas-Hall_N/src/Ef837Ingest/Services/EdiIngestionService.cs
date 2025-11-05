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
                (isa, gs, st) => st.TransactionSetIdentifierCode_01 switch
                {
                    "837" => typeof(TS837P).GetTypeInfo(),
                    "850" => typeof(TS850).GetTypeInfo(),
                    _ => null
                },
                new X12ReaderSettings { Split = true });

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
                            _db.ISA.Add(new IsaEntity
                            {
                                InterchangeControlNumber = isa.InterchangeControlNumber_13,
                                SenderId = isa.InterchangeSenderID_6,
                                ReceiverId = isa.InterchangeReceiverID_8,
                                RawJson = JsonSerializer.Serialize(isa)
                            });
                            break;

                        case GS gs:
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

                                if (!snip.IsValid)
                                {
                                    var controlNumber = msg837.ST?.TransactionSetControlNumber_02;

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
                                            Message = e.Errors.FirstOrDefault().Message,
                                            Severity = "Error",
                                            RawContextJson = JsonSerializer.Serialize(e)
                                        });
                                    }

                                    await _db.SaveChangesAsync(ct);

                                    _log.LogError("SNIP validation failed for 837P ControlNumber={CN} (Level={Level}).",
                                        controlNumber, snipOptions.Level);


                                }

                                if (msg837.ST != null)
                                {
                                    _db.ST.Add(new StEntity
                                    {
                                        TransactionSetId = msg837.ST.TransactionSetIdentifierCode_01,
                                        ControlNumber = msg837.ST.TransactionSetControlNumber_02,
                                        RawJson = JsonSerializer.Serialize(msg837.ST)
                                    });
                                }

                                if (msg837.SE != null)
                                {
                                    if (!int.TryParse(msg837.SE?.NumberofIncludedSegments_01, out var segCount) || segCount <= 0)
                                        throw new InvalidOperationException(
                                            $"Invalid or missing SE01 segment count. Value was: '{msg837.SE?.NumberofIncludedSegments_01}'");

                                    _db.SE.Add(new SeEntity
                                    {
                                        SegmentCount = segCount,
                                        ControlNumber = msg837.SE.TransactionSetControlNumber_02,
                                        RawJson = JsonSerializer.Serialize(msg837.SE)
                                    });
                                }

                                _db.TS837P.Add(new Ts837pEntity
                                {
                                    ControlNumber = msg837.ST?.TransactionSetControlNumber_02,
                                    PatientControlNumber =
                                        msg837?.Loop2000A?.FirstOrDefault()?
                                              .Loop2000B?.FirstOrDefault()?
                                              .Loop2300?.FirstOrDefault()?
                                              .CLM_ClaimInformation?.PatientControlNumber_01,
                                    RawJson = JsonSerializer.Serialize(msg837)
                                });

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
    }
}
