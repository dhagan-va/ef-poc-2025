using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Framework.Readers;
using EdiFabric.Core.Model.Edi;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;


namespace EDI837.Ingestion
{
    /// <summary>
    /// ClaimStagingContext creates a table for pre-staging claims,
    /// allowing XML Blob storage and easy duplication prevention.
    /// </summary>
    public class ClaimStagingContext : DbContext
    {
        public DbSet<ClaimStaging> ClaimStagings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                Env.Load("../../.env");
                var connString = Environment.GetEnvironmentVariable("SQL_CONN_STRING");
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimStaging>()
                .HasIndex(c => new { c.ProviderNPI, c.PatientControlNumber })
                .IsUnique()
                .HasFilter("[ProviderNPI] IS NOT NULL AND [PatientControlNumber] IS NOT NULL");
        }
    }

    /// <summary>
    /// The Main X12 Hierarchy DB Context
    /// </summary>
    public class HIPAA_5010_837P_Context : DbContext
    {
        public DbSet<TS837P> TS837P { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                Env.Load("../../.env");
                var connString = Environment.GetEnvironmentVariable("SQL_CONN_STRING");
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connString);
            }
        }
    }

        
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting Ingestion Process...");

            // Attempt to set EDI token from .env file
            if (!SetEdiTokenKey())
            {
                Console.WriteLine("Failed to set token.");
                return;
            }

            var file_path = "../../samples/837-sample-file.edi";
            var claims = ParseEdiFile(file_path);
            SaveClaims(claims);
            Console.WriteLine("Finished ingestion process.");
        }

        /// <summary>
        /// Attempts to se the EDI Token key
        /// </summary>
        /// <returns>True if successful, False otherwise</returns>
        static bool SetEdiTokenKey()
        {
            // Load environment variables from .env file
            Env.Load("../../.env");

            var ediKey = Environment.GetEnvironmentVariable("EDI_TOKEN");

            if (string.IsNullOrWhiteSpace(ediKey))
            {
                Console.WriteLine("Missing EDI_TOKEN in .env file!");
                return false;
            }

            EdiFabric.SerialKey.Set(ediKey);
            return true;
        }

        /// <summary>
        /// Parses the given EDI file and extracts all valid 837P claims.
        /// Invalid claims are logged and skipped.
        /// </summary>
        /// <param name="path">Path to the .txt EDI file.</param>
        /// <returns>List of valid 837P claims.</returns>
        static List<TS837P> ParseEdiFile(string path)
        {
            var ediStream = File.OpenRead(path);

            List<IEdiItem> ediItems;
            using (var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa"))
            {
                ediItems = [.. ediReader.ReadToEnd()];
            }

            var claims = ediItems.OfType<TS837P>();

            var validClaims = new List<TS837P>();

            foreach (var claim in claims)
            {
                if (claim.HasErrors)
                {
                    var errors = claim.ErrorContext.Flatten();

                    Console.WriteLine($"Claim {claim.ST?.TransactionSetControlNumber_02 ?? "(unknown)"} has {errors.Count()} errors:");
                    foreach (var err in errors)
                    {
                        Console.WriteLine($"  {err}");
                    }
                }
                else
                {
                    validClaims.Add(claim);
                }
            }

            Console.WriteLine($"Parsed {validClaims.Count} valid claims successfully.");

            return validClaims;

        }

        /// <summary>
        /// Saves a list of claims to the Database
        /// </summary>
        /// <param name="claims">A list of 837P claims</param>
        static void SaveClaims(List<TS837P> claims)
        {
            using var ediDb = new HIPAA_5010_837P_Context();
            using var stagingDb = new ClaimStagingContext();

            // foreach claim
            foreach (var claim in claims)
            {
                var providerNpi = claim.Loop2000A?
                    .Select(a => a.AllNM1?.Loop2010AA?.NM1_BillingProviderName?.ResponseContactIdentifier_09)
                    .FirstOrDefault(npi => !string.IsNullOrWhiteSpace(npi));

                var patientControlNumber = claim.Loop2000A?
                    .SelectMany(a => a.Loop2000B ?? [])
                    .SelectMany(b => b.Loop2300 ?? [])
                    .Select(l2300 => l2300.CLM_ClaimInformation?.PatientControlNumber_01)
                    .FirstOrDefault(clm => !string.IsNullOrWhiteSpace(clm));


                // validate not null
                if (string.IsNullOrWhiteSpace(providerNpi) || string.IsNullOrWhiteSpace(patientControlNumber))
                {
                    Console.WriteLine("Skipping claim with missing identifiers.");
                    Console.WriteLine($"NPI: {providerNpi}, CLM01: {patientControlNumber}");
                    continue;
                }
                
                var stagingRecord = new ClaimStaging
                {
                    ProviderNPI = providerNpi,
                    PatientControlNumber = patientControlNumber,
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
                    Console.WriteLine($"[DB Error] Failed to insert claim (NPI={providerNpi}, CLM01={patientControlNumber}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                    stagingDb.Entry(stagingRecord).State = EntityState.Detached;
                    continue;
                }

                // Only serialize and insert full claim if DB insert succeeded
                var xml = SerializeToXml(claim);
                stagingRecord.ClaimXml = xml.ToString();
                stagingDb.SaveChanges();

                // Add to main DB now that we know its not a duplicate
                ediDb.TS837P.Add(claim);
                ediDb.SaveChanges();

                Console.WriteLine($"Successfully ingested claim: NPI={providerNpi}, CLM01={patientControlNumber}");
            }
            

        }

        /// <summary>
        /// XML Serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        static string SerializeToXml<T>(T instance)
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
