using Microsoft.EntityFrameworkCore;
using EDI837Ingestion.EF.Entities;

namespace EDI837Ingestion.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<RawEdiFile> RawEdiFiles { get; set; }
        public DbSet<Interchange> Interchanges { get; set; }
        public DbSet<FunctionalGroup> FunctionalGroups { get; set; }
        public DbSet<TransactionSet> TransactionSets { get; set; }
        public DbSet<Payer> Payers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimLineItem> ClaimLineItems { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<ValidationIssue> ValidationIssues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // common column settings
            builder.Entity<Claim>().Property(c => c.TotalChargeAmount).HasColumnType("decimal(18,2)");
            builder.Entity<ClaimLineItem>().Property(li => li.UnitCharge).HasColumnType("decimal(18,2)");
            builder.Entity<ClaimLineItem>().Property(li => li.LineTotal).HasColumnType("decimal(18,2)");
            builder.Entity<ClaimLineItem>().Property(li => li.Units).HasColumnType("decimal(18,2)");

            // indexes and constraints
            builder.Entity<Payer>().HasIndex(p => p.PayerIdentifier);
            builder.Entity<Provider>().HasIndex(p => p.Npi).IsUnique(false);
            builder.Entity<TransactionSet>().HasIndex(t => t.TransactionControlNumber);
            builder.Entity<Interchange>().HasIndex(i => new { i.IsaControlNumber, i.SenderId, i.ReceiverId }).IsUnique(false);
            builder.Entity<RawEdiFile>().HasIndex(r => r.FileName);

            // relationships
            builder.Entity<FunctionalGroup>()
                .HasOne(fg => fg.Interchange)
                .WithMany(i => i.FunctionalGroups)
                .HasForeignKey(fg => fg.InterchangeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TransactionSet>()
                .HasOne(ts => ts.FunctionalGroup)
                .WithMany(fg => fg.TransactionSets)
                .HasForeignKey(ts => ts.FunctionalGroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TransactionSet>()
                .HasOne(ts => ts.RawEdiFile)
                .WithMany()
                .HasForeignKey(ts => ts.RawEdiFileId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Claim>()
                .HasOne(c => c.TransactionSet)
                .WithMany(ts => ts.Claims)
                .HasForeignKey(c => c.TransactionSetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Claim>()
                .HasOne(c => c.Payer)
                .WithMany(p => p.Claims)
                .HasForeignKey(c => c.PayerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Claim>()
                .HasOne(c => c.Patient)
                .WithMany()
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ClaimLineItem>()
                .HasOne(li => li.Claim)
                .WithMany(c => c.LineItems)
                .HasForeignKey(li => li.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Diagnosis>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.Diagnoses)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ValidationIssue>()
                .HasOne(v => v.TransactionSet)
                .WithMany(ts => ts.ValidationIssues)
                .HasForeignKey(v => v.TransactionSetId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}
