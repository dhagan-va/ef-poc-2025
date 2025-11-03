using EdiFabric.Framework.Readers;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Core.Model.Edi.ErrorContexts;

namespace EDI837IngestionTask.Services
{
    public static class EdiReaderParser
    {
        /// <summary>
        /// Read Claim and Parses it
        /// </summary>
        public static List<TS837P> ReadAndParse(string filePath)
        {
            using var ediStream = File.OpenRead(filePath);

            var settings = new X12ReaderSettings
            {
                ContinueOnError = true
            };

            using var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa", settings);

            var ediItems = ediReader.ReadToEnd().ToList();
            var transactions = ediItems.OfType<TS837P>();

            var transactionsList = new List<TS837P>();

            foreach (var transaction in transactions)
            {
                if (transaction.HasErrors)
                {
                    //  partially parsed
                    var errors = transaction.ErrorContext.Flatten();
                    foreach (var err in errors)
                    {
                        Console.WriteLine($"ERROR: {err}");
                    }

                }
                else
                {
                    transactionsList.Add(transaction);
                }
            }

            return ValidateSnip(transactionsList, EnvSetup.validLevel);

        }

        private static List<TS837P> ValidateSnip(IEnumerable<TS837P> transactions, int level = 3)
        {
            Console.WriteLine($"Start Running SNIP Validation - Level {level}");
            var transactionsList = new List<TS837P>();

            var validationSettings = new ValidationSettings
            {
                ValidationLevel = level switch
                {
                    1 => ValidationLevel.SyntaxOnly_SNIP1,
                    2 => ValidationLevel.LimitsAndCodes_SNIP2,
                    3 => ValidationLevel.Balancing_SNIP3,
                    4 => ValidationLevel.InterSegment_SNIP4,
                    _ => ValidationLevel.SyntaxOnly_SNIP1
                }
            };

            int passed = 0, failed = 0;

            foreach (var tran in transactions)
            {
                bool isValid = tran.IsValid(out MessageErrorContext errorContext, validationSettings);
                if (!isValid)
                {
                    failed++;
                    Console.WriteLine($"Validation failed for transaction - transactionControlNumber: {tran.ST?.TransactionSetControlNumber_02}");

                    foreach (var err in errorContext.Flatten())
                    {
                        Console.WriteLine($"ERROR: {err}");
                    }
                }
                else
                {
                    passed++;
                    transactionsList.Add(tran);
                    Console.WriteLine($"Validation passed for transaction - transactionControlNumber: {tran.ST?.TransactionSetControlNumber_02}");
                }
            }

            Console.WriteLine($"SNIP Validation Summary: {passed} passed, {failed} failed.");

            return transactionsList;

        }

    }

}