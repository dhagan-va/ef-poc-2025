using Edi837Ingestion.Parsing;
using EdiFabric.Core.Model.Edi;

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
/// Abstract and generic so the columns are declared once; each concrete variant
/// (<see cref="Edi837ProfessionalTransactionSet"/>, <see cref="Edi837InstitutionalTransactionSet"/>,
/// <see cref="Edi837DentalTransactionSet"/>) closes <typeparamref name="TMessage"/> and becomes its
/// own table. This is plain C# reuse, not an EF mapping hierarchy: the concrete types have distinct
/// closed base types and the open generic base is never mapped, so EF treats each as an independent
/// table with no inheritance discriminator.
/// <para>
/// This type is a persistence-ignorant POCO. The EF mapping lives separately in
/// <see cref="Edi837TransactionSetConfiguration{TEntity,TMessage}"/>, applied by the context.
/// </para>
/// </remarks>
public abstract class Edi837TransactionSet<TMessage>
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
    /// Populates this row from a parsed <paramref name="transactionSet"/>, owned by
    /// <paramref name="interchange"/>: copies the GS06/ST02 control numbers and the typed payload and
    /// links the parent (whose FK EF fixes up on save). Called by each variant's <c>From</c> factory.
    /// </summary>
    protected void CopyFrom(
        ParsedTransactionSet<TMessage> transactionSet, IngestedInterchange interchange)
    {
        ArgumentNullException.ThrowIfNull(transactionSet);
        ArgumentNullException.ThrowIfNull(interchange);

        Interchange = interchange;
        GroupControlNumber = transactionSet.GroupControlNumber;
        TransactionSetControlNumber = transactionSet.TransactionSetControlNumber;
        Message = transactionSet.Message;
    }
}
