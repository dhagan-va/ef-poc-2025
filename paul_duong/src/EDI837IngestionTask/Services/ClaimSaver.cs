using EDI837IngestionTask.Models;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;


namespace EDI837IngestionTask.Services
{
    public static class ClaimSaver
    {

        /// <summary>
        /// Saves a list of 837P transactions to the database.
        /// check the duplicate records in file and db
        /// </summary>
        public static void Save837P(List<TS837P> trans)
        {
            using var ediDb = new HIPAA_5010_837P_Context();
            var seenKeys = new HashSet<(string ProviderNpi, string TransactionNumber)>();

            int total = 0;
            int skippedDuplicates = 0;
            int skippedInvalid = 0;
            int inserted = 0;

            foreach (var transaction in trans)
            {
                total++;
                var providerNpi = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactIdentifier_09)
                    .FirstOrDefault(npi => !string.IsNullOrWhiteSpace(npi));
                var transactionControlNumber = transaction.ST?.TransactionSetControlNumber_02;

                // Validate for non-null
                if (string.IsNullOrWhiteSpace(providerNpi) || string.IsNullOrWhiteSpace(transactionControlNumber))
                {
                    skippedInvalid++;
                    Console.WriteLine("Skipping claim with missing identifiers.");
                    continue;
                }

                var key = (providerNpi.Trim(), transactionControlNumber.Trim());
                // check dup in file
                if (!seenKeys.Add(key))
                {
                    skippedDuplicates++;
                    Console.WriteLine($"Duplicate Records in File: NPI={providerNpi}, ST02={transactionControlNumber}");
                    continue;
                }

                bool exists = ediDb.ClaimProcesses.AsNoTracking().Any(c => c.ProviderNPI == providerNpi.Trim() && c.TransactionControlNumber == transactionControlNumber.Trim());

                if (exists)
                {
                    skippedDuplicates++;
                    Console.WriteLine($"Duplicate Records in DB: NPI={providerNpi}, ST02={transactionControlNumber}");
                    continue;
                }

                var claim = new ClaimProcess
                {
                    ProviderNPI = providerNpi,
                    TransactionControlNumber = transactionControlNumber,
                    ProcessedAt = DateTime.UtcNow
                };

                ediDb.ClaimProcesses.Add(claim);
                ediDb.TS837P.Add(transaction);
                inserted++;

                if (inserted % 100 == 0)
                {
                    ediDb.SaveChanges();
                    Console.WriteLine($"batch inserted {inserted} records");
                }
            }
            ediDb.SaveChanges();

            Console.WriteLine($"Successfully ingested claim");
            Console.WriteLine($"TOTAL: {total}, INSERTED: {inserted}, DUP SKIP: {skippedDuplicates}, INVALID: {skippedInvalid}");

        }
    }
}