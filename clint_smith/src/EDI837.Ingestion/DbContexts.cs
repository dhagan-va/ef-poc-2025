using DotNetEnv;
using EDI837.Ingestion.Gateways;
using EDI837.Ingestion.Services;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EDI837.Ingestion
{
    /// <summary>
    /// ClaimStagingContext creates a table for pre-staging claims,
    /// allowing XML Blob storage and easy duplication prevention.
    /// </summary>
    public class ClaimStagingContext : DbContext
    {
        private readonly string _connectionString = String.Empty;

        public ClaimStagingContext() { }

        public ClaimStagingContext(DbContextOptions<ClaimStagingContext> options)
            : base(options) { }

        public ClaimStagingContext(AppSettings settings)
        {
            if (settings != null)
            {
                _connectionString = settings.Database.ConnectionStrings.ClaimStaging;
            }
        }

        public DbSet<ClaimStaging> ClaimStagings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentNullException.ThrowIfNull(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder
                .Entity<ClaimStaging>()
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
        private readonly string _connectionString = String.Empty;

        public HIPAA_5010_837P_Context() { }

        public HIPAA_5010_837P_Context(DbContextOptions<HIPAA_5010_837P_Context> options)
            : base(options) { }

        public HIPAA_5010_837P_Context(AppSettings settings)
        {
            if (settings != null)
            {
                _connectionString = settings.Database.ConnectionStrings.HIPAA_5010_837P;
            }
        }

        public DbSet<TS837P> TS837P { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentNullException.ThrowIfNull(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);
            }
        }
    }
}
