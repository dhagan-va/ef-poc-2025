using EdiFabric.Framework.Readers;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;

namespace EDI837.Ingestion.Services
{
    public static class EdiParser
    {
        /// <summary>
        /// Parses the given EDI file and extracts all valid 837P claims.
        /// Invalid claims are logged and skipped.
        /// </summary>
        /// <param name="path">Path to the .txt EDI file.</param>
        /// <returns>List of valid 837P transactions.</returns>
        public static List<TS837P> ParseEdiFile(string path)
        {
            var ediStream = File.OpenRead(path);

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
                    var errors = transaction.ErrorContext.Flatten();

                    Console.WriteLine($"Claim {transaction.ST?.TransactionSetControlNumber_02 ?? "(unknown)"} has {errors.Count()} errors:");
                    foreach (var err in errors)
                    {
                        Console.WriteLine($"  {err}");
                    }
                }
                else
                {
                    validTransactions.Add(transaction);
                }
            }

            Console.WriteLine($"Parsed {validTransactions.Count} valid claims successfully.");

            return validTransactions;

        }
    }
}