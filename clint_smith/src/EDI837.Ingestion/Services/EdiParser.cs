using EDI837.Ingestion.Gateways;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace EDI837.Ingestion.Services
{
    public class EdiParser
    {
        private readonly ILogger<EdiParser> _logger;

        public EdiParser(ILogger<EdiParser> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parse the Edi file from a given local path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<TS837P> ParseEdiFileFromPath(string path)
        {
            using var ediStream = File.OpenRead(path);
            return ParseEdiStream(ediStream);
        }

        /// <summary>
        /// Synchronously parses the given EDI file and extracts all valid 837P claims.
        /// Invalid claims are logged and skipped.
        /// </summary>
        /// <param name="ediStream">A Stream object</param>
        /// <returns>List of valid 837P transactions.</returns>
        public IEnumerable<TS837P> ParseEdiStream(Stream ediStream)
        {
            List<IEdiItem> ediItems;
            using (var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa"))
            {
                ediItems = [.. ediReader.ReadToEnd()];
            }

            var transactions = ediItems.OfType<TS837P>();

            var validTransactions = new List<TS837P>();

            foreach (var transaction in transactions)
            {
                transaction.IsValid(out var errorContext);

                if (transaction.HasErrors)
                {
                    var errors = transaction.ErrorContext.Flatten().ToList();

                    _logger.LogWarning(
                        "Claim {TransactionControlNumber} has {ErrorCount} errors",
                        transaction.ST?.TransactionSetControlNumber_02 ?? "(unknown)",
                        errors.Count
                    );
                    foreach (var err in errors)
                    {
                        _logger.LogWarning("  {ErrorDetail}", err);
                    }
                }
                else
                {
                    validTransactions.Add(transaction);
                }
            }

            _logger.LogInformation(
                "Parsed {ValidTransactionCount} valid claims successfully.",
                validTransactions.Count
            );

            return validTransactions;
        }
    }
}
