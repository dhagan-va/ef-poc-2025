using System.Text.Json;
using Edi837Ingestion.Data;
using Ef837Ingest.Data.Entities;
using EdiFabric.Templates.Hipaa5010;
using Ef837Ingest.Edi.Validator;
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
            string controlNumber,
            SnipOptions? options = null,
            CancellationToken ct = default)
        {
            // 1) Load stored TS837P
            var raw = await _db.TS837P
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ControlNumber == controlNumber, ct);

            if (raw == null)
            {
                _log.LogWarning("Reprocess: No TS837P found for ControlNumber={CN}", controlNumber);
                return null;
            }

            TS837P? msg;
            try
            {
                msg = JsonSerializer.Deserialize<TS837P>(raw.RawJson);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to deserialize TS837P JSON for ControlNumber={CN}", controlNumber);
                return null;
            }

            if (msg == null)
            {
                _log.LogWarning("Reprocess: Deserialized TS837P was null for ControlNumber={CN}", controlNumber);
                return null;
            }

            options ??= new SnipOptions
            {
                Level = SnipLevel.Snip4,
                FailOnError = false,   
                ThrowOnNullMessage = true
            };

            // 2) Run SNIP validation
            var result = SnipValidator.Validate(msg, options);

            // 3) Update TransactionStatus
            var txStatus = await _db.TransactionStatuses
                .FirstOrDefaultAsync(t =>
                        t.TransactionType == "837P" &&
                        t.TransactionSetControlNumber == controlNumber,
                    ct);

            if (txStatus != null)
            {
                txStatus.IsValid = result.IsValid;
                txStatus.ErrorCount = result.ErrorContext?.Errors?.Count ?? 0;
                txStatus.Status = result.IsValid ? "Validated" : "FailedValidation";
            }


            var existingIssues = _db.ValidationIssues
                .Where(v => v.Transaction == "837P" && v.ControlNumber == controlNumber);
            _db.ValidationIssues.RemoveRange(existingIssues);

            var segErrors = result.ErrorContext?.Errors
                            ?? Enumerable.Empty<EdiFabric.Core.Model.Edi.ErrorContexts.SegmentErrorContext>();

            foreach (var e in segErrors)
            {
                _db.ValidationIssues.Add(new ValidationIssue
                {
                    Transaction = "837P",
                    ControlNumber = controlNumber,
                    Level = options.Level.ToString(),
                    SegmentId = e.LoopId,
                    Position = e.Position,
                    Code = e.Name,
                    Message = e?.Errors?.FirstOrDefault()?.Message ?? "Unknown error",
                    Severity = "Error",
                    RawContextJson = JsonSerializer.Serialize(e)
                });
            }

            await _db.SaveChangesAsync(ct);

            _log.LogInformation("Reprocess of 837P ControlNumber={CN} complete. IsValid={Valid}, Errors={Errors}.",
                controlNumber, result.IsValid, txStatus?.ErrorCount ?? 0);

            return result;
        }
    }
}
