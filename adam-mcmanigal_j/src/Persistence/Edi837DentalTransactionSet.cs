using Edi837Ingestion.Parsing;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// One persisted 837 <b>dental</b> (837D, 005010X224A2) transaction set — an
/// <see cref="Edi837TransactionSet{TSelf,TMessage}"/> closed over <see cref="TS837D"/>, mapped to its own table.
/// </summary>
public sealed class Edi837DentalTransactionSet : Edi837TransactionSet<Edi837DentalTransactionSet, TS837D>
{
    /// <summary>Projects a parsed 837D transaction set into a row owned by <paramref name="interchange"/>.</summary>
    public static Edi837DentalTransactionSet From(
        ParsedTransactionSet<TS837D> transactionSet, IngestedInterchange interchange) =>
        Create(transactionSet, interchange);
}
