using EDI837IngestionTask.Services;
using Microsoft.EntityFrameworkCore;
using EdiFabric.Templates.Hipaa5010;
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
            var connString = EnvSetup.GetDbConnection();
            optionsBuilder.UseSqlServer(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimProcess>()
                .HasIndex(c => new { c.ProviderNPI, c.TransactionControlNumber })
                .IsUnique();
        }
    }
}