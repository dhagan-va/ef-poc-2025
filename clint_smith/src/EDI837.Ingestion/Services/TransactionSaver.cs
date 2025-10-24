using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;


namespace EDI837.Ingestion.Services
{
    public static class ClaimSaver
    {
        /// <summary>
        /// Saves a list of 837P transactions to the database.
        /// </summary>
        /// <remarks>
        /// This method first writes key identifying details to the 
        /// <see cref="ClaimStagingContext"/> to prevent duplicate 
        /// transactions from being inserted. 
        /// Unique transactions are then serialized as XML blobs and 
        /// inserted into the X12 hierarchy tables.
        /// </remarks>
        /// <param name="transactions">A list of 837P transactions to be saved.</param>
        /// <returns>The number of unique transactions successfully saved.</returns>
        public static void SaveTransactions(List<TS837P> transactions)
        {
            using var ediDb = new HIPAA_5010_837P_Context();
            using var stagingDb = new ClaimStagingContext();

            foreach (var transaction in transactions)
            {
                var providerNpi = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactIdentifier_09)
                    .FirstOrDefault(npi => !string.IsNullOrWhiteSpace(npi));

                var transactionControlNumber = transaction.ST?.TransactionSetControlNumber_02;

                if (string.IsNullOrWhiteSpace(providerNpi) || string.IsNullOrWhiteSpace(transactionControlNumber))
                {
                    Console.WriteLine("Skipping claim with missing identifiers.");
                    Console.WriteLine($"NPI: {providerNpi}, ST02: {transactionControlNumber}");
                    continue;
                }

                var stagingRecord = new ClaimStaging
                {
                    ProviderNPI = providerNpi,
                    TransactionControlNumber = transactionControlNumber,
                    ReceivedAt = DateTime.UtcNow,
                    ClaimXml = ""
                };

                stagingDb.ClaimStagings.Add(stagingRecord);

                // Attempt saving claim in staging DB
                try
                {
                    stagingDb.SaveChanges();
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"[DB Error] Failed to insert claim (NPI={providerNpi}, ST02={transactionControlNumber}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                    stagingDb.Entry(stagingRecord).State = EntityState.Detached;
                    continue;
                }

                // Only serialize and insert full claim if DB insert succeeded
                var xml = SerializeToXml(transaction);
                stagingRecord.ClaimXml = xml.ToString();
                stagingDb.SaveChanges();

                // Add to main DB now that we know its not a duplicate
                ediDb.TS837P.Add(transaction);
                ediDb.SaveChanges();

                Console.WriteLine($"Successfully ingested claim: NPI={providerNpi}, ST02={transactionControlNumber}");
            }


        }

         /// <summary>
        /// XML Serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string SerializeToXml<T>(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var serializer = new XmlSerializer(instance.GetType());
            using var ms = new MemoryStream();
            serializer.Serialize(ms, instance);
            ms.Position = 0;
            using var reader = new StreamReader(ms);
            return reader.ReadToEnd();
        }
    }
}