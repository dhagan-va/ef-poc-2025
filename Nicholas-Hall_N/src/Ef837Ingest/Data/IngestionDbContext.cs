using Edi837Ingestion.Domain.Entities;
using Ef837Ingest.Data;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingestion.Data;

public class IngestionDbContext(DbContextOptions<IngestionDbContext> options) : DbContext(options)
{
    public DbSet<Interchange> Interchanges => Set<Interchange>();
    public DbSet<FunctionalGroup> FunctionalGroups => Set<FunctionalGroup>();
    public DbSet<TransactionSet> TransactionSets => Set<TransactionSet>();
    public DbSet<ClaimStub> ClaimStubs => Set<ClaimStub>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Interchange
        modelBuilder.Entity<Interchange>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ControlNumber).HasMaxLength(50);
            e.HasIndex(x => x.ControlNumber);
            e.Property(x => x.Isa01).HasMaxLength(10);
            e.Property(x => x.Isa02).HasMaxLength(20);
            e.Property(x => x.Isa03).HasMaxLength(10);
            e.Property(x => x.Isa04).HasMaxLength(20);
            e.Property(x => x.Isa05).HasMaxLength(10);
            e.Property(x => x.Isa06).HasMaxLength(30);
            e.Property(x => x.Isa07).HasMaxLength(10);
            e.Property(x => x.Isa08).HasMaxLength(30);
            e.Property(x => x.Isa09Date).HasMaxLength(8);   // YYMMDD or CCYYMMDD formats appear as text
            e.Property(x => x.Isa10Time).HasMaxLength(8);   // HHMM or HHMMSS
            e.HasMany(x => x.FunctionalGroups)
             .WithOne(x => x.Interchange)
             .HasForeignKey(x => x.InterchangeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // FunctionalGroup
        modelBuilder.Entity<FunctionalGroup>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ControlNumber).HasMaxLength(50);
            e.HasIndex(x => x.ControlNumber);
            e.Property(x => x.Gs01).HasMaxLength(10);
            e.Property(x => x.Gs02).HasMaxLength(30);
            e.Property(x => x.Gs03).HasMaxLength(30);
            e.Property(x => x.Gs04Date).HasMaxLength(8);
            e.Property(x => x.Gs05Time).HasMaxLength(8);
            e.Property(x => x.TransactionTypeCode).HasMaxLength(10);
            e.Property(x => x.VersionAndRelease).HasMaxLength(20);
            e.HasMany(x => x.TransactionSets)
             .WithOne(x => x.FunctionalGroup)
             .HasForeignKey(x => x.FunctionalGroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // TransactionSet
        modelBuilder.Entity<TransactionSet>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.St01).HasMaxLength(10);
            e.Property(x => x.ControlNumber).HasMaxLength(50);
            e.HasIndex(x => new { x.FunctionalGroupId, x.ControlNumber }).IsUnique(false);
            e.Property(x => x.Bht02).HasMaxLength(10);
            e.Property(x => x.Bht06).HasMaxLength(10);
            e.Property(x => x.RawJson).HasColumnType("nvarchar(max)");
            e.HasMany(x => x.ClaimStubs)
             .WithOne(x => x.TransactionSet)
             .HasForeignKey(x => x.TransactionSetId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ClaimStub
        modelBuilder.Entity<ClaimStub>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PatientControlNumber).HasMaxLength(50);
            e.Property(x => x.TotalClaimCharge).HasPrecision(18, 2);
        });
    }
}
