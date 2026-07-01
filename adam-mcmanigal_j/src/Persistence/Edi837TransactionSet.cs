using System.Text.Json;
using Edi837Ingestion.Parsing;
using EdiFabric.Core.Model.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// Shared shape for a persisted 837 transaction set (ST…SE) of any variant: a surrogate key, the
/// FK/navigation back to the parent <see cref="IngestedInterchange"/> envelope, the inner-envelope
/// control numbers (GS06/ST02) identifying the functional group and transaction set, and the
/// strongly-typed EdiFabric message itself (<typeparamref name="TMessage"/>), kept intact and
/// persisted as a single JSON column rather than shredded into relational segments. The per-variant
/// tables are a structured, queryable copy that complements the raw file archived in S3; the content
/// hash on the parent remains the idempotency guard.
/// </summary>
/// <remarks>
/// Abstract and generic so the columns and mapping are declared once; each concrete variant
/// (<see cref="Edi837ProfessionalTransactionSet"/>, <see cref="Edi837InstitutionalTransactionSet"/>,
/// <see cref="Edi837DentalTransactionSet"/>) closes <typeparamref name="TSelf"/>/<typeparamref name="TMessage"/>
/// and becomes its own table. This is plain C# reuse, not an EF mapping hierarchy: the concrete types
/// have distinct closed base types and the open generic base is never mapped, so EF treats each as an
/// independent table with no inheritance discriminator.
/// <para>
/// The base also <em>configures itself</em>: it implements <see cref="IEntityTypeConfiguration{TSelf}"/>
/// (closed over the concrete type via the <typeparamref name="TSelf"/> self type-parameter), so each
/// variant's EF mapping lives with the entity instead of in the context. The context applies it with
/// <c>modelBuilder.ApplyConfiguration(new Edi837…TransactionSet())</c>.
/// </para>
/// </remarks>
public abstract class Edi837TransactionSet<TSelf, TMessage> : IEntityTypeConfiguration<TSelf>
    where TSelf : Edi837TransactionSet<TSelf, TMessage>, new()
    where TMessage : EdiMessage, new()
{
    /// <summary>Surrogate key.</summary>
    public int Id { get; set; }

    /// <summary>FK to the parent <see cref="IngestedInterchange"/> (the ISA envelope / 837 file).</summary>
    public int IngestedInterchangeId { get; set; }

    /// <summary>
    /// Navigation to the parent interchange envelope. Nullable because it is only populated when
    /// explicitly loaded (e.g. via <c>Include</c>); the relationship itself is required, enforced by
    /// the non-nullable <see cref="IngestedInterchangeId"/> foreign key.
    /// </summary>
    public IngestedInterchange? Interchange { get; set; }

    /// <summary>GS06 — the group control number of the enclosing functional group.</summary>
    public string GroupControlNumber { get; set; } = string.Empty;

    /// <summary>ST02 — this transaction set's control number.</summary>
    public string TransactionSetControlNumber { get; set; } = string.Empty;

    /// <summary>The strongly-typed EdiFabric message, persisted as JSON.</summary>
    public TMessage Message { get; set; } = new();

    /// <summary>
    /// Builds a transaction-set row of the concrete type <typeparamref name="TSelf"/> from a parsed
    /// <paramref name="transactionSet"/>, owned by <paramref name="interchange"/>: copies the GS06/ST02
    /// control numbers and the typed payload and links the parent (whose FK EF fixes up on save).
    /// </summary>
    protected static TSelf Create(
        ParsedTransactionSet<TMessage> transactionSet, IngestedInterchange interchange)
    {
        ArgumentNullException.ThrowIfNull(transactionSet);
        ArgumentNullException.ThrowIfNull(interchange);

        return new TSelf
        {
            Interchange = interchange,
            GroupControlNumber = transactionSet.GroupControlNumber,
            TransactionSetControlNumber = transactionSet.TransactionSetControlNumber,
            Message = transactionSet.Message,
        };
    }

    // Faithful, stable round-trip of the EdiFabric POCO (System.Text.Json, not XmlSerializer which
    // injects phantom empty segments) — fidelity matters for re-deriving the message from storage.
    private static readonly JsonSerializerOptions MessageJsonOptions = new();

    /// <summary>
    /// Self-mapping for the per-variant transaction-set table: surrogate key, a required FK + cascade
    /// to the parent <see cref="IngestedInterchange"/>, GS06/ST02 length limits, and the typed
    /// EdiFabric POCO persisted as a single <c>nvarchar(max)</c> JSON column (a
    /// <see cref="ValueConverter"/> plus a serialize-based <see cref="ValueComparer"/>, required for
    /// change tracking of a reference-type property).
    /// </summary>
    public void Configure(EntityTypeBuilder<TSelf> builder)
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
