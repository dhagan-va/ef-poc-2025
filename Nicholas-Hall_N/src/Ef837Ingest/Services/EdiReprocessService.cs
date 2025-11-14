using System.Text.Json;
using Ef837Ingest.Data.Entities;
using Ef837Ingest.Edi.Validator;
using Edi837Ingestion.Data;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edi837Ingestion.Services
{
    public class EdiReprocessService : IEdiReprocessService
    {
        private readonly Hipaa5010Context _db;
        private readonly ILogger<EdiReprocessService> _log;

        public EdiReprocessService(Hipaa5010Context db, ILogger<EdiReprocessService> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<SnipResult?> Reprocess837PAsync(
            string transactionSetControlNumber,
            SnipOptions? options = null,
            CancellationToken ct = default)
        {
            options ??= new SnipOptions
            {
                Level = SnipLevel.Snip2,  // for example, more relaxed on reprocess
                FailOnError = false,
                ThrowOnNullMessage = true
            };

            // Load stored TS837P JSON
            var ts837Row = await _db.TS837P
                .FirstOrDefaultAsync(t => t.ControlNumber == transactionSetControlNumber, ct);

            if (ts837Row == null)
            {
                _log.LogWarning("No TS837P found for ControlNumber={CN}", transactionSetControlNumber);
                return null;
            }

            var msg837 = JsonSerializer.Deserialize<TS837P>(ts837Row.RawJson);
            if (msg837 == null)
            {
                _log.LogError("Failed to deserialize TS837P for ControlNumber={CN}", transactionSetControlNumber);
                return null;
            }

            // Clear old validation issues for this control number
            var oldIssues = await _db.ValidationIssues
                .Where(v => v.ControlNumber == transactionSetControlNumber && v.Transaction == "837P")
                .ToListAsync(ct);

            _db.ValidationIssues.RemoveRange(oldIssues);

            // Re-run SNIP
            var snip = SnipValidator.Validate(msg837, options);

            if (!snip.IsValid)
            {
                var segErrors = snip.ErrorContext?.Errors
                                ?? Enumerable.Empty<EdiFabric.Core.Model.Edi.ErrorContexts.SegmentErrorContext>();

                foreach (var e in segErrors)
                {
                    _db.ValidationIssues.Add(new ValidationIssue
                    {
                        Transaction = "837P",
                        ControlNumber = transactionSetControlNumber,
                        Level = options.Level.ToString(),
                        SegmentId = e.LoopId,
                        Position = e.Position,
                        Code = e.Name,
                        Message = e?.Errors?.FirstOrDefault()?.Message ?? "Unknown error",
                        Severity = "Error",
                        RawContextJson = JsonSerializer.Serialize(e)
                    });
                }
            }

            // Update TransactionStatus
            var status = await _db.TransactionStatuses
                .FirstOrDefaultAsync(s => s.TransactionSetControlNumber == transactionSetControlNumber
                                          && s.TransactionType == "837P", ct);

            if (status != null)
            {
                status.IsValid = snip.IsValid;
                status.ErrorCount = snip.ErrorContext?.Errors?.Count ?? 0;
                status.Status = "Reprocessed";
            }

            await _db.SaveChangesAsync(ct);

            _log.LogInformation("Reprocessed 837P ControlNumber={CN}, IsValid={Valid}, Errors={Errors}",
                transactionSetControlNumber,
                snip.IsValid,
                snip.ErrorContext?.Errors?.Count ?? 0);

            return snip;
        }
    }
}
