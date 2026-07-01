using System.Text.Json;
using EdiFabric.Core.Model.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// EF Core mapping for a per-variant 837 transaction-set table
/// (<typeparamref name="TEntity"/> closing <see cref="Edi837TransactionSet{TMessage}"/>): surrogate
/// key, a required FK + cascade to the parent <see cref="IngestedInterchange"/>, GS06/ST02 length
/// limits, and the typed EdiFabric POCO persisted as a single <c>nvarchar(max)</c> JSON column (a
/// <see cref="ValueConverter"/> plus a serialize-based <see cref="ValueComparer"/>, required for
/// change tracking of a reference-type property). Kept separate from the entity so the entity stays
/// persistence-ignorant; the context applies one closed instance per variant.
/// </summary>
internal sealed class Edi837TransactionSetConfiguration<TEntity, TMessage>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : Edi837TransactionSet<TMessage>
    where TMessage : EdiMessage, new()
{
    // Faithful, stable round-trip of the EdiFabric POCO (System.Text.Json, not XmlSerializer which
    // injects phantom empty segments) — fidelity matters for re-deriving the message from storage.
    private static readonly JsonSerializerOptions MessageJsonOptions = new();

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        var converter = new ValueConverter<TMessage, string>(
            poco => JsonSerializer.Serialize(poco, MessageJsonOptions),
            json => JsonSerializer.Deserialize<TMessage>(json, MessageJsonOptions)!);

        var comparer = new ValueComparer<TMessage>(
            (a, b) => JsonSerializer.Serialize(a, MessageJsonOptions)
                == JsonSerializer.Serialize(b, MessageJsonOptions),
            poco => JsonSerializer.Serialize(poco, MessageJsonOptions).GetHashCode(),
            poco => JsonSerializer.Deserialize<TMessage>(
                JsonSerializer.Serialize(poco, MessageJsonOptions), MessageJsonOptions)!);

        builder.HasKey(x => x.Id);

        // The transaction set cannot exist without its envelope; the non-nullable FK makes the
        // relationship required, and deleting the interchange cascades to its transaction sets.
        builder.HasOne(x => x.Interchange)
            .WithMany()
            .HasForeignKey(x => x.IngestedInterchangeId)
            .OnDelete(DeleteBehavior.Cascade);

        // X12 group (GS06) and transaction-set (ST02) control numbers are at most 9 chars.
        builder.Property(x => x.GroupControlNumber).HasMaxLength(9);
        builder.Property(x => x.TransactionSetControlNumber).HasMaxLength(9);

        // IsRequired because a row always carries its payload; EF can't infer this from the open
        // generic Message property, so the column would otherwise default to nullable.
        builder.Property(x => x.Message)
            .HasConversion(converter, comparer)
            .HasColumnType("nvarchar(max)")
            .IsRequired();
    }
}
