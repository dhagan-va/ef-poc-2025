using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;


namespace EDI837.Ingestion.Services
{



    
    public class TransactionSaver
    {
        private readonly AppSettings _settings;
        public TransactionSaver(AppSettings settings) => _settings = settings;

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
        public async Task SaveTransactionsAsync(List<TS837P> transactions)
        {
            await using var ediDb = new HIPAA_5010_837P_Context(_settings);
            await using var stagingDb = new ClaimStagingContext(_settings);

            foreach (var transaction in transactions)
            {
                // Extract attributes used for detecting uniqueness
                var providerNpi = transaction.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactIdentifier_09)
                    .FirstOrDefault(npi => !string.IsNullOrWhiteSpace(npi));
                var transactionControlNumber = transaction.ST?.TransactionSetControlNumber_02;

                // Validate for non-null
                if (string.IsNullOrWhiteSpace(providerNpi) || string.IsNullOrWhiteSpace(transactionControlNumber))
                {
                    Console.WriteLine("Skipping claim with missing identifiers.");
                    Console.WriteLine($"NPI: {providerNpi}, ST02: {transactionControlNumber}");
                    continue;
                }

                // Add to Staging (ClaimStaging is a little bit of a misnomer)
                // TODO Rename ClaimStaging to TransactionStaging (involves new migrations and renaming DB also)
                var stagingRecord = new ClaimStaging
                {
                    ProviderNPI = providerNpi,
                    TransactionControlNumber = transactionControlNumber,
                    ReceivedAt = DateTime.UtcNow,
                    ClaimXml = ""
                };

                await stagingDb.ClaimStagings.AddAsync(stagingRecord);

                try
                {
                    await stagingDb.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"[DB Error] Failed to insert claim (NPI={providerNpi}, ST02={transactionControlNumber}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                    stagingDb.Entry(stagingRecord).State = EntityState.Detached;
                    continue;
                }

                // Only serialize and save XML if DB insert succeeded
                var xml = SerializeToXml(transaction);
                stagingRecord.ClaimXml = xml.ToString();
                await stagingDb.SaveChangesAsync();

                // Add to X12 Hierarchy Tables now that we know its not a duplicate
                await ediDb.TS837P.AddAsync(transaction);
                await ediDb.SaveChangesAsync();

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