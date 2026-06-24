using Microsoft.EntityFrameworkCore;
using EdiFabric.Templates.Hipaa5010;
using EDI.Common.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EDI.Data
{
    public class EdiDbContext(DbContextOptions<EdiDbContext> options) : DbContext(options)
    {

        // Add DbSets for your entities here
        public DbSet<EdiDocument>? EdiDocuments { get; set; }
        public DbSet<EdiPartner>? EdiPartners { get; set; }
        public DbSet<EdiClaim>? EdiClaims { get; set; }
        public DbSet<EdiService>? EdiServices { get; set; }
        public DbSet<TS837D>? T837DTransactions { get; set; }
        public DbSet<TS837I>? T837ITransactions { get; set; }
        public DbSet<TS837P>? T837PTransactions { get; set; }
        public DbSet<TS835>? T835Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ignore pending model change warnings so SQL-based migrations (views) can be applied
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
