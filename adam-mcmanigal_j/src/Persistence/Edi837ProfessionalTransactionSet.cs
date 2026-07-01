using Edi837Ingestion.Parsing;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// One persisted 837 <b>professional</b> (837P, 005010X222A1) transaction set — an
/// <see cref="Edi837TransactionSet{TSelf,TMessage}"/> closed over <see cref="TS837P"/>, mapped to its own table.
/// </summary>
public sealed class Edi837ProfessionalTransactionSet : Edi837TransactionSet<Edi837ProfessionalTransactionSet, TS837P>
{
    /// <summary>Projects a parsed 837P transaction set into a row owned by <paramref name="interchange"/>.</summary>
    public static Edi837ProfessionalTransactionSet From(
        ParsedTransactionSet<TS837P> transactionSet, IngestedInterchange interchange) =>
        Create(transactionSet, interchange);
}
