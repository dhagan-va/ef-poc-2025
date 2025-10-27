using EdiFabric.Templates.Hipaa5010;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using EDI837.Ingestion.Services;
using EDI837.Ingestion.Gateways;
using Microsoft.Data.SqlClient;


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
                var connString = Environment.GetEnvironmentVariable("STAGING_CONN_STRING");
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimStaging>()
                .HasIndex(c => new { c.ProviderNPI, c.TransactionControlNumber })
                .IsUnique()
                .HasFilter("[ProviderNPI] IS NOT NULL AND [TransactionControlNumber] IS NOT NULL");
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
                string sqlConnString = EnvSetup.Get("SQL_CONN_STRING") ?? "";
                var connString = Environment.GetEnvironmentVariable(sqlConnString);
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connString);
            }
        }
    }

        
    internal class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Starting Ingestion Process...");

            // Attempt to set EDI token from .env file
            if (!EnvSetup.SetEdiTokenKey())
            {
                Console.WriteLine("Failed to set token.");
                return;
            }

            string s3Url = EnvSetup.Get("S3_SERVICE_URL") ?? "http://localhost:5000";
            var bucketName = EnvSetup.Get("S3_BUCKET") ?? "edi-bucket";
            var s3Gateway = new S3Gateway(s3Url, bucketName);

            // TODO update to read file and inject text into parser
            // TODO or maybe just put the s3 gateway into the parser to read the file there?
            var files = await s3Gateway.ListFilesAsync("incoming/");
            foreach (var file in files)
            {
                Console.WriteLine(file.Key);
            }

            var file_path = "../../samples/837-sample-file.edi";
            var claims = EdiParser.ParseEdiFile(file_path);
            ClaimSaver.SaveTransactions(claims);
            Console.WriteLine("Finished ingestion process.");
        }
    }
}
