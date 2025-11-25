using System.Xml.Serialization;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EDI837.Ingestion.Services
{
    public class TransactionSaver
    {
        private readonly ILogger<TransactionSaver> _logger;
        private readonly HIPAA_5010_837P_Context _ediDb;
        private readonly ClaimStagingContext _stagingDb;
        private readonly AppSettings _settings;

        public TransactionSaver(
            ILogger<TransactionSaver> logger,
            HIPAA_5010_837P_Context ediDb,
            ClaimStagingContext stagingDb,
            AppSettings settings
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ediDb = ediDb ?? throw new ArgumentNullException(nameof(ediDb));
            _stagingDb = stagingDb ?? throw new ArgumentNullException(nameof(stagingDb));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

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
        public async Task SaveTransactionsAsync(IEnumerable<TS837P> transactions)
        {
            ArgumentNullException.ThrowIfNull(transactions);

            foreach (var transaction in transactions)
            {
                // Extract attributes used for detecting uniqueness
                var providerNpi = transaction
                    .Loop2000A?.Select(a =>
                        a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactIdentifier_09
                    )
                    .FirstOrDefault(npi => !string.IsNullOrWhiteSpace(npi));
                var transactionControlNumber = transaction.ST?.TransactionSetControlNumber_02;

                // Validate for non-null
                if (
                    string.IsNullOrWhiteSpace(providerNpi)
                    || string.IsNullOrWhiteSpace(transactionControlNumber)
                )
                {
                    _logger.LogInformation(
                        "Skipping claim with missing identifiers. NPI={ProviderNpi}, ST02={TransactionControlNumber}",
                        providerNpi,
                        transactionControlNumber
                    );
                    continue;
                }

                // Add to Staging (ClaimStaging is a little bit of a misnomer)
                // TODO Rename ClaimStaging to TransactionStaging (involves new migrations and renaming DB also)
                var stagingRecord = new ClaimStaging
                {
                    ProviderNPI = providerNpi,
                    TransactionControlNumber = transactionControlNumber,
                    ReceivedAt = DateTime.UtcNow,
                    ClaimXml = "",
                };

                await _stagingDb.ClaimStagings.AddAsync(stagingRecord);

                try
                {
                    await _stagingDb.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                    when (dbEx.InnerException is SqlException sql && sql.Number == 2601)
                {
                    _logger.LogWarning(
                        "Duplicate claim detected (NPI={ProviderNpi}, ST02={TransactionControlNumber}). "
                            + "Will not write to database.",
                        providerNpi,
                        transactionControlNumber
                    );
                    _stagingDb.Entry(stagingRecord).State = EntityState.Detached;
                    continue;
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(
                        dbEx,
                        "Failed to insert claim (NPI={ProviderNpi}, ST02={TransactionControlNumber})",
                        providerNpi,
                        transactionControlNumber
                    );
                    _stagingDb.Entry(stagingRecord).State = EntityState.Detached;
                    continue;
                }

                // Only serialize and save XML if DB insert succeeded
                var xml = SerializeToXml(transaction);
                stagingRecord.ClaimXml = xml.ToString();
                await _stagingDb.SaveChangesAsync();

                // Add to X12 Hierarchy Tables now that we know its not a duplicate
                await _ediDb.TS837P.AddAsync(transaction);
                await _ediDb.SaveChangesAsync();

                _logger.LogInformation(
                    "Successfully ingested claim. NPI={ProviderNpi}, ST02={TransactionControlNumber}",
                    providerNpi,
                    transactionControlNumber
                );
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
