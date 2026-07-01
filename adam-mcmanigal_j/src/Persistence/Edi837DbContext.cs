using Microsoft.EntityFrameworkCore;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// EF Core context for 837 ingestion. <see cref="IngestedInterchange"/> is the idempotency ledger —
/// one row per ingested interchange (837 file), keyed by a unique SHA-256 content hash so a re-sent
/// file is rejected by the database; the raw file is archived in S3
/// (<see cref="IngestedInterchange.PayloadS3Key"/>). The per-variant transaction-set tables
/// (<see cref="Edi837ProfessionalTransactionSet"/>, <see cref="Edi837InstitutionalTransactionSet"/>,
/// <see cref="Edi837DentalTransactionSet"/>) hold a structured, queryable copy of the parsed
/// transaction sets: each row carries its envelope FK + GS06/ST02 and the EdiFabric POCO as a single
/// JSON column, rather than shredding the deep claim graph into hundreds of relational tables.
/// </summary>
public sealed class Edi837DbContext(DbContextOptions<Edi837DbContext> options) : DbContext(options)
{
    public DbSet<IngestedInterchange> Interchanges => Set<IngestedInterchange>();

    public DbSet<Edi837ProfessionalTransactionSet> ProfessionalTransactionSets => Set<Edi837ProfessionalTransactionSet>();

    public DbSet<Edi837InstitutionalTransactionSet> InstitutionalTransactionSets => Set<Edi837InstitutionalTransactionSet>();

    public DbSet<Edi837DentalTransactionSet> DentalTransactionSets => Set<Edi837DentalTransactionSet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IngestedInterchange>(entity =>
        {
            entity.HasKey(x => x.Id);

            // Idempotent ingestion: the SHA-256 of the raw interchange bytes is the dedup key.
            // A re-sent file produces the same hash and is rejected by this unique constraint.
            entity.HasIndex(x => x.ContentHash).IsUnique();
            entity.Property(x => x.ContentHash).HasMaxLength(64); // 32-byte SHA-256 as hex

            // X12 ISA sender/receiver IDs are 15 chars; the control number is at most 9.
            entity.Property(x => x.SenderId).HasMaxLength(15);
            entity.Property(x => x.ReceiverId).HasMaxLength(15);
            entity.Property(x => x.InterchangeControlNumber).HasMaxLength(9);
        });

        // Each variant configures its own table via IEntityTypeConfiguration (implemented by the
        // shared Edi837TransactionSet<,> base); the context just applies it.
        modelBuilder.ApplyConfiguration(new Edi837ProfessionalTransactionSet());
        modelBuilder.ApplyConfiguration(new Edi837InstitutionalTransactionSet());
        modelBuilder.ApplyConfiguration(new Edi837DentalTransactionSet());
    }
}
