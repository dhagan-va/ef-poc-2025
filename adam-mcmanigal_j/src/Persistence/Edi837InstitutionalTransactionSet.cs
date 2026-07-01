using Edi837Ingestion.Parsing;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// One persisted 837 <b>institutional</b> (837I, 005010X223A2) transaction set — an
/// <see cref="Edi837TransactionSet{TSelf,TMessage}"/> closed over <see cref="TS837I"/>, mapped to its own table.
/// </summary>
public sealed class Edi837InstitutionalTransactionSet : Edi837TransactionSet<Edi837InstitutionalTransactionSet, TS837I>
{
    /// <summary>Projects a parsed 837I transaction set into a row owned by <paramref name="interchange"/>.</summary>
    public static Edi837InstitutionalTransactionSet From(
        ParsedTransactionSet<TS837I> transactionSet, IngestedInterchange interchange) =>
        Create(transactionSet, interchange);
}
