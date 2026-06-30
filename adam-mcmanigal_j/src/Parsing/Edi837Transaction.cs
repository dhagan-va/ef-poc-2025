using EdiFabric.Core.Model.Edi;

namespace Edi837Ingestion.Parsing;

/// <summary>
/// One parsed 837 transaction (a claim set) within an interchange, paired with the identity
/// it needs for tracking: its <see cref="Variant"/> (837P/I/D), the group control number
/// (GS06) of its enclosing functional group, and its own transaction-set control number (ST02).
/// </summary>
/// <remarks>
/// <see cref="Transaction"/> is the EdiFabric message held as its base <see cref="EdiMessage"/>
/// type, so all three 837 variants share one shape; downcast to <c>TS837P</c>/<c>TS837I</c>/
/// <c>TS837D</c> (matching <see cref="Variant"/>) to read claim data. The enclosing interchange
/// envelope (sender/receiver/ISA13) lives on <see cref="ParsedInterchange"/>, not here, since a
/// single envelope is shared by every transaction in the file.
/// </remarks>
public sealed record Edi837Transaction(
    Edi837Variant Variant,
    string GroupControlNumber,
    string TransactionSetControlNumber,
    EdiMessage Transaction);
