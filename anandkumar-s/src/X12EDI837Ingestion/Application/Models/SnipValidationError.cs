namespace X12EDI837Ingestion.Consumer.Application.Models;

public sealed class SnipValidationError
{
    public int SnipLevel { get; init; }
    public string ErrorCode { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? SegmentId { get; init; }
    public string? SegmentPosition { get; init; }
    public string? ElementPosition { get; init; }
}