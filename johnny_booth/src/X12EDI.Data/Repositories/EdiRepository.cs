using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Repositories;
using X12EDI.Data.DBContext;
using X12EDI.Data.Entities;
using X12EDI.Data.Extensions;

namespace X12EDI.Data.Repositories
{
    public class EdiRepository : IEdiRepository
    {
        #region Private Fields

        private readonly IEdiDbContext _dbContext;
        private readonly ILogger<EdiRepository> _logger;

        #endregion Private Fields

        #region Public Constructors

        public EdiRepository(IEdiDbContext dbContext, ILogger<EdiRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>Saves the file asynchronously.</summary>
        /// <param name="identifier">The file unique identifier.</param>
        /// <param name="items">The items.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<bool> SaveFileAsync(string identifier, IEnumerable<object> items, CancellationToken cancellationToken)
        {
            // Fail early if identifier is missing.
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Identifier must be provided", nameof(identifier));
            }

            // Respect cancellation request.
            cancellationToken.ThrowIfCancellationRequested();

            // Early check to avoid expensive processing if file was already handled.
            if (await AlreadyProcessedAsync(identifier, cancellationToken))
            {
                _logger.LogInformation("File {Identifier} already processed. Skipping.", identifier);
                return false;
            }

            var file = new EdiFile
            {
                Identifier = identifier,
                IngestedAt = DateTime.UtcNow
            };

            // Collect messages first to minimize round-trips to the DB.
            var collectedMessages = new List<(EdiMessage Message, string Raw, string Checksum)>();
            var seenChecksums = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                switch (item)
                {
                    case ISA isa:
                        if (file.Envelope == null)
                        {
                            file.Envelope = new EdiEnvelope(isa);
                        }
                        else
                        {
                            file.Envelope.MapISA(isa);
                        }
                        break;

                    case GS gs:
                        if (file.Envelope == null)
                        {
                            file.Envelope = new EdiEnvelope(gs);
                        }
                        else
                        {
                            file.Envelope.MapGS(gs);
                        }
                        break;

                    case EdiMessage message:
                        // Compute raw and checksum once.
                        var raw = message.ToXml();
                        var checksum = EDIDataExtensions.ComputeSha256(raw) ?? string.Empty;

                        // Skip duplicate inside the same incoming file.
                        if (seenChecksums.Contains(checksum))
                        {
                            _logger.LogWarning("Duplicate transaction in same file skipped (Checksum: {Checksum})", checksum);
                            continue;
                        }

                        seenChecksums.Add(checksum);
                        collectedMessages.Add((message, raw, checksum));

                        break;

                    case ReaderErrorContext error:
                        file.Errors.Add(new EdiError { Message = error.Message });
                        break;
                }
            }

            if (collectedMessages.Count == 0)
            {
                _logger.LogInformation("No transactions found to persist for file {Identifier}", identifier);
                return false;
            }

            // Query DB once for existing checksums to avoid N separate .AnyAsync calls.
            var candidateChecksums = collectedMessages.Select(x => x.Checksum).ToList();

            var existingChecksums = await _dbContext.EdiTransactions
                .Where(t => t.Checksum != null && candidateChecksums.Contains(t.Checksum))
                .Select(t => t.Checksum)
                .ToListAsync(cancellationToken);

            // Add only those messages not already in DB.
            var newTransactions = collectedMessages
                .Where(m => !existingChecksums.Contains(m.Checksum, StringComparer.OrdinalIgnoreCase))
                .ToList();

            // Log duplicates skipped due to DB presence
            var skippedCount = collectedMessages.Count - newTransactions.Count;
            if (skippedCount > 0)
            {
                foreach (var skipped in collectedMessages.Where(m => existingChecksums.Contains(m.Checksum, StringComparer.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning("Duplicate transaction skipped (Checksum: {Checksum})", skipped.Checksum);
                }
            }

            if (newTransactions.Count == 0)
            {
                _logger.LogInformation("All transactions already exist for file {Identifier}. Nothing to persist.", identifier);
                return false;
            }

            // Create transaction entities and attach them to the file entity
            var tsEntitiesToPersist = new List<object>();
            foreach (var (message, raw, checksum) in newTransactions)
            {
                file.Transactions.Add(new EdiTransaction
                {
                    Raw = raw,
                    Checksum = checksum,
                    TransactionType = message.GetType().Name,
                    ControlNumber = TryGetControlNumber(message) ?? string.Empty,
                });

                // Generic detection: support any TS837* template (TS837P, TS837I, ...)
                if (message.GetType().Name.StartsWith("TS837", StringComparison.OrdinalIgnoreCase))
                {
                    tsEntitiesToPersist.Add(message);
                }
            }

            // Batch-add any strongly-typed TS837* template entities
            if (tsEntitiesToPersist.Count > 0)
            {
                _dbContext.AddRangeEntities(tsEntitiesToPersist);
            }

            if (file.Envelope != null)
            {
                file.Envelope.EdiFile = file;
            }

            _dbContext.EdiFiles.Add(file);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Persisted file {Identifier} with {TxCount} transactions and {ErrCount} errors",
                identifier, file.Transactions.Count, file.Errors.Count);

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private static string? TryGetControlNumber(EdiMessage message)
        {
            var stProp = message.GetType().GetProperty("ST");
            if (stProp?.GetValue(message) is object stSegment)
            {
                var ctrlProp = stSegment.GetType().GetProperty("TransactionSetControlNumber_02");
                return ctrlProp?.GetValue(stSegment)?.ToString();
            }
            return null;
        }

        private Task<bool> AlreadyProcessedAsync(string identifier, CancellationToken cancellationToken)
        {
            return _dbContext.EdiFiles.AnyAsync(a => a.Identifier == identifier);
        }

        #endregion Private Methods
    }
}