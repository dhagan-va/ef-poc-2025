using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingestion.Parsing;

/// <summary>
/// The result of parsing one X12 interchange (ISA…IEA — i.e. one 837 file): the envelope
/// identity and content hash that become an <c>IngestedInterchange</c> ledger row, plus the
/// ready-to-use parsed transaction sets it contained. An immutable snapshot of "what the file
/// said"; the clock (received-at) and S3 archive pointer are supplied later at persist time.
/// </summary>
/// <remarks>
/// <see cref="ContentHash"/> is the SHA-256 (hex) of the raw interchange bytes — the
/// idempotency key. One interchange per file is modelled (one file = one ledger row); the
/// parser tracks the envelope from the ISA/GS segments it reads. Init-only members so the
/// parser builds it with named initializers — clearer and transposition-proof given the
/// number of same-typed envelope fields.
/// </remarks>
public sealed record ParsedInterchange
{
    /// <summary>SHA-256 (hex) of the raw interchange bytes — the idempotency key.</summary>
    public required string ContentHash { get; init; }

    /// <summary>ISA05 — interchange sender ID qualifier (e.g. ZZ, 30); qualifies <see cref="SenderId"/>.</summary>
    public required string SenderIdQualifier { get; init; }

    /// <summary>ISA06 — interchange sender ID (unique only within <see cref="SenderIdQualifier"/>).</summary>
    public required string SenderId { get; init; }

    /// <summary>ISA07 — interchange receiver ID qualifier; qualifies <see cref="ReceiverId"/>.</summary>
    public required string ReceiverIdQualifier { get; init; }

    /// <summary>ISA08 — interchange receiver ID.</summary>
    public required string ReceiverId { get; init; }

    /// <summary>ISA09 — interchange date (YYMMDD) as stamped by the sender (distinct from received-at).</summary>
    public required string InterchangeDate { get; init; }

    /// <summary>ISA10 — interchange time (HHMM) as stamped by the sender.</summary>
    public required string InterchangeTime { get; init; }

    /// <summary>ISA12 — interchange control version number (e.g. 00501).</summary>
    public required string InterchangeControlVersionNumber { get; init; }

    /// <summary>ISA13 — interchange control number.</summary>
    public required string InterchangeControlNumber { get; init; }

    /// <summary>ISA15 — usage indicator: <c>P</c>(roduction) or <c>T</c>(est).</summary>
    public required string UsageIndicator { get; init; }

    /// <summary>The 837 <b>professional</b> (837P) transaction sets contained in this interchange.</summary>
    public required IReadOnlyList<ParsedTransactionSet<TS837P>> ProfessionalTransactionSets { get; init; }

    /// <summary>The 837 <b>institutional</b> (837I) transaction sets contained in this interchange.</summary>
    public required IReadOnlyList<ParsedTransactionSet<TS837I>> InstitutionalTransactionSets { get; init; }

    /// <summary>The 837 <b>dental</b> (837D) transaction sets contained in this interchange.</summary>
    public required IReadOnlyList<ParsedTransactionSet<TS837D>> DentalTransactionSets { get; init; }
}
