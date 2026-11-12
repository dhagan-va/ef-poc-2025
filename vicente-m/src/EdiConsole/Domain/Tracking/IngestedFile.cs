using EdiConsole.Domain.Claims;

namespace EdiConsole.Domain.Tracking;

/// <summary>
/// One row per file processed: what came in, how it was validated, and the outcome.
/// Aggregate root: the file owns the claims and issues that came from it, and they are saved with it as one unit.
/// </summary>
public class IngestedFile
{
    public long Id { get; set; }
    public required string FileName { get; set; }
    public required string Sha256 { get; set; }             // hex; unique → the same file is never ingested twice
    public DateTimeOffset ReceivedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public IngestionStatus Status { get; set; }

    public byte SnipLevelApplied { get; set; }              // 1–4: the EdiFabric ValidationLevel used
    public required string EdiFabricVersion { get; set; }   // e.g. 11.1.0

    public string? InterchangeControlNumber { get; set; }   // ISA13 (null for an empty file)
    public string? UsageIndicator { get; set; }             // ISA15: T = test, P = production

    public int ClaimCount { get; set; }
    public int ErrorCount { get; set; }

    public List<Claim> Claims { get; } = [];               // empty when the file is rejected (all-or-nothing)
    public List<ValidationIssue> Issues { get; } = [];     // empty when the file is accepted
}
