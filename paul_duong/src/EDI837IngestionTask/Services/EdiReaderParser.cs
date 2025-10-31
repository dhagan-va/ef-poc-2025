using EdiFabric.Framework.Readers;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;

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
            using var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa");

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

            return transactionsList;

        }

    }

}