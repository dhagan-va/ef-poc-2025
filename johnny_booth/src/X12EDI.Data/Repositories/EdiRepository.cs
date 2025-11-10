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
            var file = new EdiFile
            {
                Identifier = identifier,
                IngestedAt = DateTime.UtcNow
            };

            var fileAlreadyProcessed = await _dbContext.EdiFiles.AnyAsync((a) => a.Identifier == identifier, cancellationToken);
            var hasNewTransactions = false;

            foreach (var item in items)
            {
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
                        var checksum = EDIDataExtensions.ComputeSha256(raw);
                        // Check for existing transaction with same checksum
                        var exists = await _dbContext.EdiTransactions
                            .AnyAsync(t => t.Checksum == checksum, cancellationToken);

                        if (exists)
                        {
                            _logger.LogWarning("Duplicate transaction skipped (Checksum: {Checksum})", checksum);
                            continue;
                        }
                        file.Transactions.Add(new EdiTransaction
                        {
                            Raw = raw,
                            Checksum = checksum,
                            TransactionType = message.GetType().Name,
                            ControlNumber = TryGetControlNumber(message) ?? string.Empty,
                        });
                        hasNewTransactions = true;
                        break;

                    case ReaderErrorContext error:
                        file.Errors.Add(new EdiError { Message = error.Message });
                        break;
                }
            }

            if (!fileAlreadyProcessed && hasNewTransactions)
            {
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
            return false;
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

        #endregion Private Methods
    }
}