using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using X12EDI.Abstractions.Repositories;
using X12EDI.Data.DBContext;
using X12EDI.Data.Entities;
using X12EDI.Data.Extensions;

namespace X12EDI.Data.Repositories
{
    public class EdiRepository : IEdiRepository
    {
        #region Private Fields

        private readonly EdiDbContext _dbContext;
        private readonly ILogger<EdiRepository> _logger;

        #endregion Private Fields

        #region Public Constructors

        public EdiRepository(EdiDbContext dbContext, ILogger<EdiRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>Saves the file asynchronous.</summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="items">The items.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<bool> SaveFileAsync(string identifier, IEnumerable<object> items, CancellationToken cancellationToken)
        {
            var file = new EdiFile
            {
                Identifier = identifier,
                IngestedAt = DateTime.UtcNow
            };

            var exists = _dbContext.EdiFiles.Where((a) => a.Identifier == identifier).Any();

            foreach (var item in items)
            {
                switch (item)
                {
                    case ISA isa:
                        if (file.Envelope == null)
                        {
                            file.Envelope = new EdiEnvelope
                            {
                                InterchangeControlNumber = isa.InterchangeControlNumber_13,
                                SenderId = isa.InterchangeSenderID_6,
                                ReceiverId = isa.InterchangeReceiverID_8,
                                Timestamp = DateTime.UtcNow
                            };
                        }
                        else
                        {
                            file.Envelope.InterchangeControlNumber = isa.InterchangeControlNumber_13;
                            file.Envelope.SenderId = isa.InterchangeSenderID_6;
                            file.Envelope.ReceiverId = isa.InterchangeReceiverID_8;
                            file.Envelope.Timestamp = DateTime.UtcNow;
                        }
                        break;
                    case GS gs:
                        if (file.Envelope == null)
                        {
                            file.Envelope = new EdiEnvelope
                            {
                                GroupControlNumber = gs.GroupControlNumber_6,
                                CodeIdentifyingInformationType = gs.CodeIdentifyingInformationType_1
                            };
                        }
                        else
                        {
                            file.Envelope.GroupControlNumber = gs.GroupControlNumber_6;
                            file.Envelope.CodeIdentifyingInformationType = gs.CodeIdentifyingInformationType_1;
                        }
                        break;
                    case EdiMessage message:
                        var raw = message.ToXml();
                        var checksum = EDIDataExtensions.ComputeSha256(raw);
                        // Check for existing transaction with same checksum
                        exists = await _dbContext.EdiTransactions
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
                        break;

                    case ReaderErrorContext error:
                        file.Errors.Add(new EdiError { Message = error.Message });
                        break;
                }
            }

            if (exists == false)
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