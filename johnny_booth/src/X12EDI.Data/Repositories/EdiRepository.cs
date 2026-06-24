using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Repositories;
using X12EDI.Data.DBContext;
using X12EDI.Data.Entities;
using X12EDI.Data.Extensions;

namespace X12EDI.Data.Repositories
{
    /// <summary>
    /// Repository for handling EDI (Electronic Data Interchange) data operations.
    /// </summary>
    public class EdiRepository : IEdiRepository
    {
        #region Private Fields

        private readonly IEdiDbContext _dbContext;
        private readonly ILogger<EdiRepository> _logger;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context for EDI data.</param>
        /// <param name="logger">The logger for logging information and errors.</param>
        public EdiRepository(IEdiDbContext dbContext, ILogger<EdiRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Saves the contents of an EDI file, including its envelope, transactions, and errors, into the database.
        /// This method ensures that duplicate files and transactions are not persisted.
        /// </summary>
        /// <param name="identifier">The unique identifier for the file.</param>
        /// <param name="items">A collection of EDI objects (e.g., ISA, GS, EdiMessage) parsed from the file.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result is <c>true</c> if new data was saved; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> SaveFileAsync(string identifier, IEnumerable<object> items, CancellationToken cancellationToken)
        {
            ValidateArguments(identifier, items);

            cancellationToken.ThrowIfCancellationRequested();

            if (await FileAlreadyProcessedAsync(identifier, cancellationToken).ConfigureAwait(false))
            {
                _logger.LogInformation("File {Identifier} already processed. Skipping.", identifier);
                return false;
            }

            var file = new EdiFile
            {
                Identifier = identifier,
                IngestedAt = DateTime.UtcNow
            };

            var collected = ExtractEnvelopeAndMessages(file, items, cancellationToken);

            if (collected.Messages.Count == 0)
            {
                _logger.LogInformation("No transactions found to persist for file {Identifier}", identifier);
                return false;
            }

            var candidateChecksums = collected.Messages.Select(m => m.Checksum).ToList();
            var existingChecksums = await GetExistingChecksumsAsync(candidateChecksums, cancellationToken).ConfigureAwait(false);

            var newMessages = collected.Messages
                .Where(m => !existingChecksums.Contains(m.Checksum, StringComparer.OrdinalIgnoreCase))
                .ToList();

            LogSkippedDuplicates(collected.Messages, newMessages);

            if (newMessages.Count == 0)
            {
                _logger.LogInformation("All transactions already exist for file {Identifier}. Nothing to persist.", identifier);
                return false;
            }

            var tsEntities = BuildTransactionsAndTemplateEntities(file, newMessages);

            AttachEnvelopeToFile(file);

            // Persist template entities if any (TS837* templates). Implementation expects DB context helper(s) to exist.
            if (tsEntities.Count > 0)
            {
                _dbContext.AddRangeEntities(tsEntities);
            }

            _dbContext.EdiFiles.Add(file);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Persisted file {Identifier} with {TxCount} transactions and {ErrCount} errors",
                identifier, file.Transactions.Count, file.Errors.Count);

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Attaches the envelope to the file by setting the navigation property.
        /// </summary>
        /// <param name="file">The <see cref="EdiFile"/> entity.</param>
        private static void AttachEnvelopeToFile(EdiFile file)
        {
            if (file.Envelope != null)
            {
                file.Envelope.EdiFile = file;
            }
        }

        /// <summary>
        /// Attempts to retrieve the transaction set control number from an EDI message.
        /// </summary>
        /// <param name="message">The EDI message.</param>
        /// <returns>The control number if found; otherwise, <c>null</c>.</returns>
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

        /// <summary>
        /// Validates the arguments for the SaveFileAsync method.
        /// </summary>
        /// <param name="identifier">The file identifier.</param>
        /// <param name="items">The collection of EDI objects.</param>
        /// <exception cref="ArgumentException">Thrown if the identifier is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the items collection is null.</exception>
        private static void ValidateArguments(string identifier, IEnumerable<object> items)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Identifier must be provided", nameof(identifier));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
        }

        /// <summary>
        /// Builds the <see cref="EdiTransaction"/> entities and collects any strongly-typed template entities (e.g., TS837) for persistence.
        /// </summary>
        /// <param name="file">The parent <see cref="EdiFile"/> entity.</param>
        /// <param name="newMessages">The list of new messages to process.</param>
        /// <returns>A list of strongly-typed template entities to be persisted.</returns>
        private List<object> BuildTransactionsAndTemplateEntities(EdiFile file, List<CollectedMessage> newMessages)
        {
            var tsEntitiesToPersist = new List<object>();

            foreach (var msg in newMessages)
            {
                file.Transactions.Add(new EdiTransaction
                {
                    Raw = msg.Raw,
                    Checksum = msg.Checksum,
                    TransactionType = msg.Message.GetType().Name,
                    ControlNumber = TryGetControlNumber(msg.Message) ?? string.Empty,
                });

                // Generic detection: support any TS837* template (TS837P, TS837I, ...)
                if (msg.Message.GetType().Name.StartsWith("TS837", StringComparison.OrdinalIgnoreCase))
                {
                    tsEntitiesToPersist.Add(msg.Message);
                }
            }

            return tsEntitiesToPersist;
        }

        /// <summary>
        /// Extracts envelope data, messages, and errors from the collection of EDI items.
        /// </summary>
        /// <param name="file">The <see cref="EdiFile"/> entity to populate.</param>
        /// <param name="items">The collection of EDI objects.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An <see cref="ExtractResult"/> containing the collected messages.</returns>
        private ExtractResult ExtractEnvelopeAndMessages(EdiFile file, IEnumerable<object> items, CancellationToken cancellationToken)
        {
            var result = new ExtractResult();
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
                        var raw = message.ToXml();
                        var checksum = EDIDataExtensions.ComputeSha256(raw) ?? string.Empty;

                        if (seenChecksums.Contains(checksum))
                        {
                            _logger.LogWarning("Duplicate transaction in same file skipped (Checksum: {Checksum})", checksum);
                            continue;
                        }

                        seenChecksums.Add(checksum);
                        result.Messages.Add(new CollectedMessage(message, raw, checksum));
                        break;

                    case ReaderErrorContext error:
                        file.Errors.Add(new EdiError { Message = error.Message });
                        break;

                    default:
                        // unknown item: ignore
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if a file with the specified identifier has already been processed.
        /// </summary>
        /// <param name="identifier">The file identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><c>true</c> if the file has been processed; otherwise, <c>false</c>.</returns>
        private async Task<bool> FileAlreadyProcessedAsync(string identifier, CancellationToken cancellationToken)
        {
            return await _dbContext.EdiFiles
                .AnyAsync(a => a.Identifier == identifier, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a list of checksums that already exist in the database.
        /// </summary>
        /// <param name="candidateChecksums">A list of checksums to check.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of checksums that are already present in the database.</returns>
        private async Task<List<string>> GetExistingChecksumsAsync(List<string> candidateChecksums, CancellationToken cancellationToken)
        {
            if (candidateChecksums == null || candidateChecksums.Count == 0)
            {
                return new List<string>();
            }

            return await _dbContext.EdiTransactions
                .Where(t => t.Checksum != null && candidateChecksums.Contains(t.Checksum))
                .Select(t => t.Checksum!)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Logs a warning for each transaction that was skipped due to being a duplicate.
        /// </summary>
        /// <param name="all">The complete list of collected messages.</param>
        /// <param name="kept">The list of messages that will be persisted.</param>
        private void LogSkippedDuplicates(List<CollectedMessage> all, List<CollectedMessage> kept)
        {
            var skipped = all.Except(kept).ToList();
            if (skipped.Count == 0)
            {
                return;
            }

            foreach (var s in skipped)
            {
                _logger.LogWarning("Duplicate transaction skipped (Checksum: {Checksum})", s.Checksum);
            }
        }

        #endregion Private Methods

        /// <summary>
        /// Represents a collected EDI message with its raw data and checksum.
        /// </summary>
        /// <param name="Message">The parsed <see cref="EdiMessage"/> object.</param>
        /// <param name="Raw">The raw XML representation of the message.</param>
        /// <param name="Checksum">The SHA256 checksum of the raw data.</param>
        private sealed record CollectedMessage(EdiMessage Message, string Raw, string Checksum);

        #region Private Classes

        /// <summary>
        /// Holds the result of the message extraction process.
        /// </summary>
        private sealed class ExtractResult
        {
            #region Public Properties

            /// <summary>
            // Gets the list of collected messages.
            /// </summary>
            public List<CollectedMessage> Messages { get; } = new();

            #endregion Public Properties
        }

        #endregion Private Classes
    }
}