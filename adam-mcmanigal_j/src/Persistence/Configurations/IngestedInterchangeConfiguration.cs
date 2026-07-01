using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// EF Core mapping for the <see cref="IngestedInterchange"/> idempotency ledger: surrogate key, the
/// unique SHA-256 content-hash index that makes ingestion idempotent (a re-sent file hashes the same
/// and is rejected by the database), and X12 length limits on the envelope identity columns.
/// </summary>
internal sealed class IngestedInterchangeConfiguration : IEntityTypeConfiguration<IngestedInterchange>
{
    public void Configure(EntityTypeBuilder<IngestedInterchange> builder)
    {
        builder.HasKey(x => x.Id);

        // Idempotent ingestion: the SHA-256 of the raw interchange bytes is the dedup key.
        // A re-sent file produces the same hash and is rejected by this unique constraint.
        builder.HasIndex(x => x.ContentHash).IsUnique();
        builder.Property(x => x.ContentHash).HasMaxLength(64); // 32-byte SHA-256 as hex

        // X12 ISA sender/receiver IDs are 15 chars; the control number is at most 9.
        builder.Property(x => x.SenderId).HasMaxLength(15);
        builder.Property(x => x.ReceiverId).HasMaxLength(15);
        builder.Property(x => x.InterchangeControlNumber).HasMaxLength(9);
    }
}
