using EdiFabric.Templates.Hipaa5010;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using EDI837IngestionTask.Services;
using EDI837IngestionTask.Models;

namespace EDI837IngestionTask
{

    /// <summary>
    /// DB Context
    /// </summary>
    public class HIPAA_5010_837P_Context : DbContext
    {
        public DbSet<TS837P> TS837P { get; set; }
        public DbSet<ClaimProcess> ClaimProcesses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connString = EnvSetup.GetDbConnection();
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimProcess>()
                .HasIndex(c => new { c.ProviderNPI, c.TransactionControlNumber })
                .IsUnique();
        }
    }


    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting Ingestion Process...");

            // Attempt to set EDI Serial key from .env file
            if (!EnvSetup.SetEdiSerialKey())
            {
                Console.WriteLine("Invalid Serial key. Skip remaining process!!!");
                return;
            }

            var filePath = EnvSetup.GetSampleFile();
            var claims = EdiReaderParser.ReadAndParse(filePath);
            ClaimSaver.Save837P(claims);
            Console.WriteLine("Completed Ingestion Process!!!");
        }
    }

}