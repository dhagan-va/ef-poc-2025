using EdiFabric.Core.Model.Edi;

namespace Edi837Ingestion.Parsing;

/// <summary>
/// One parsed 837 transaction set (ST…SE — a set of claims) within an interchange, paired with the
/// inner-envelope control numbers it needs for tracking: the group control number (GS06) of its
/// enclosing functional group and its own transaction-set control number (ST02).
/// </summary>
/// <remarks>
/// <typeparamref name="TMessage"/> is the strongly-typed EdiFabric message — <c>TS837P</c>,
/// <c>TS837I</c>, or <c>TS837D</c> — so the variant is carried by the type rather than a runtime
/// tag, and <see cref="Message"/> needs no downcast. <see cref="ParsedInterchange"/> groups these
/// into one typed list per variant. The enclosing interchange envelope (sender/receiver/ISA13) lives
/// on <see cref="ParsedInterchange"/>, not here, since a single envelope is shared by every
/// transaction set in the file. GS06 comes from the functional-group envelope (not the message), so
/// it is captured here rather than read off <see cref="Message"/>.
/// </remarks>
public sealed record ParsedTransactionSet<TMessage>(
    string GroupControlNumber,
    string TransactionSetControlNumber,
    TMessage Message) where TMessage : EdiMessage;
