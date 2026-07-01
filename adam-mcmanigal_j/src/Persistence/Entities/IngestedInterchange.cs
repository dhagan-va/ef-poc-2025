using Edi837Ingestion.Parsing;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// The idempotency-ledger record for one ingested X12 interchange (ISA…IEA — i.e. one 837
/// file). <see cref="ContentHash"/> is the dedup guard: a SHA-256 over the raw interchange
/// bytes, made unique so a re-sent file is rejected by the database rather than re-ingested.
/// The interchange envelope identity (sender/receiver/control number) is kept for observability
/// and conflict detection, and the raw file itself is archived in S3 (referenced by
/// <see cref="PayloadS3Key"/>). The parsed transaction sets are stored in the per-variant
/// tables (<see cref="Edi837ProfessionalTransactionSet"/>, <see cref="Edi837InstitutionalTransactionSet"/>,
/// <see cref="Edi837DentalTransactionSet"/>), each row carrying its GS06/ST02 control numbers and
/// the EdiFabric message as a JSON column; those rows hang off this ledger record via FK.
/// </summary>
public sealed class IngestedInterchange
{
    /// <summary>Surrogate key.</summary>
    public int Id { get; set; }

    /// <summary>SHA-256 (hex) of the raw interchange bytes — the idempotency key.</summary>
    public string ContentHash { get; set; } = string.Empty;

    /// <summary>ISA06 — interchange sender ID.</summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>ISA08 — interchange receiver ID.</summary>
    public string ReceiverId { get; set; } = string.Empty;

    /// <summary>ISA13 — interchange control number.</summary>
    public string InterchangeControlNumber { get; set; } = string.Empty;

    /// <summary>When this interchange was ingested (UTC).</summary>
    public DateTime ReceivedAt { get; set; }

    /// <summary>S3 object key for the archived raw file; null until the file is stored.</summary>
    public string? PayloadS3Key { get; set; }

    /// <summary>
    /// Projects a parsed interchange into a ledger record. The envelope identity and
    /// <see cref="ParsedInterchange.ContentHash"/> come straight off
    /// <paramref name="interchange"/>; <paramref name="receivedAtUtc"/> is the ingestion
    /// clock supplied by the caller. The interchange must contain at least one transaction set.
    /// </summary>
    public static IngestedInterchange From(ParsedInterchange interchange, DateTime receivedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(interchange);
        ArgumentException.ThrowIfNullOrEmpty(interchange.ContentHash);
        if (interchange.ProfessionalTransactionSets.Count == 0
            && interchange.InstitutionalTransactionSets.Count == 0
            && interchange.DentalTransactionSets.Count == 0)
            throw new ArgumentException(
                "An interchange must contain at least one transaction set.", nameof(interchange));

        return new IngestedInterchange
        {
            ContentHash = interchange.ContentHash,
            SenderId = interchange.SenderId,
            ReceiverId = interchange.ReceiverId,
            InterchangeControlNumber = interchange.InterchangeControlNumber,
            ReceivedAt = receivedAtUtc,
        };
    }
}
