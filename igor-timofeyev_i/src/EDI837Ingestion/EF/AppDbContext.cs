using Microsoft.EntityFrameworkCore;
using EDI837Ingestion.EF.Entities;

namespace EDI837Ingestion.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<InterchangeControl> InterchangeControls { get; set; }
        public DbSet<FunctionalGroup> FunctionalGroups { get; set; }
        public DbSet<ClaimBatch> ClaimBatches { get; set; }
        public DbSet<BillingProvider> BillingProviders { get; set; }
        public DbSet<SubscriberPatient> SubscriberPatients { get; set; }
        public DbSet<MedicalClaim> MedicalClaims { get; set; }
        public DbSet<ClaimServiceLine> ClaimServiceLines { get; set; }
        public DbSet<ClaimDiagnosis> ClaimDiagnoses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many relationships mapping the hierarchy of your 837P file
            modelBuilder.Entity<InterchangeControl>()
                .HasMany(i => i.FunctionalGroups).WithOne().HasForeignKey(g => g.InterchangeControlId);

            modelBuilder.Entity<FunctionalGroup>()
                .HasMany(g => g.ClaimBatches).WithOne().HasForeignKey(b => b.FunctionalGroupId);

            modelBuilder.Entity<ClaimBatch>()
                .HasMany(b => b.MedicalClaims).WithOne().HasForeignKey(c => c.ClaimBatchId);

            modelBuilder.Entity<MedicalClaim>()
                .HasMany(c => c.ServiceLines).WithOne().HasForeignKey(s => s.MedicalClaimId);

            modelBuilder.Entity<MedicalClaim>()
                .HasMany(c => c.Diagnoses).WithOne().HasForeignKey(d => d.MedicalClaimId);

            // --- FIX FOR DECIMAL PRECISION WARNINGS ---

            // Configure MedicalClaim decimal columns
            modelBuilder.Entity<MedicalClaim>()
                .Property(c => c.TotalClaimChargeAmount)
                .HasPrecision(18, 2);

            // Configure ClaimServiceLine decimal columns
            modelBuilder.Entity<ClaimServiceLine>()
                .Property(s => s.LineChargeAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ClaimServiceLine>()
                .Property(s => s.UnitCount)
                .HasPrecision(18, 2);
        }
    }
}
