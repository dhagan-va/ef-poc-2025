namespace EdiConsole.Domain.Tracking;

/// <summary>
/// One problem found in a file. The columns mirror EdiFabric's structured error objects
/// (MessageErrorContext → SegmentErrorContext → DataElementErrorContext).
/// </summary>
public class ValidationIssue
{
    public long Id { get; set; }
    public long IngestedFileId { get; set; }
    public string? TransactionSetControlNumber { get; set; }  // ST02; null for file-level problems (e.g. empty file)
    public string? Segment { get; set; }                      // e.g. NM1
    public int? SegmentPosition { get; set; }                 // position within the transaction
    public string? LoopId { get; set; }                       // e.g. 2010BA
    public int? ElementPosition { get; set; }                 // e.g. 9 for NM109
    public required string Code { get; set; }                 // EdiFabric's error code, or ours (e.g. UnsupportedTransaction)
    public required string Message { get; set; }
}
