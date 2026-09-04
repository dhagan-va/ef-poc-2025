using EdiFabric.Core.Model.Edi.ErrorContexts;

namespace PayerEDI.Data.Database.Tables;

public record EdiSegmentErrorTable
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid EdiErrorId { get; init; }
    public required string Message { get; init; }
    public required string Name { get; init; }
    public int Position { get; init; }
    public string? LoopId { get; init; }
    public string? Value { get; init; }
    public string? SpecRef { get; init; }
    public string[] Codes { get; init; } = [];
}

public static class SegmentErrorContextExtensions
{
    public static EdiSegmentErrorTable CreateEdiSegmentError(
        this SegmentErrorContext errorContext,
        Guid ediErrorId
    ) =>
        new()
        {
            EdiErrorId = ediErrorId,
            Message = errorContext.Message ?? string.Empty,
            Name = errorContext.Name ?? string.Empty,
            Position = errorContext.Position,
            LoopId = errorContext.LoopId,
            Value = errorContext.Value,
            SpecRef = errorContext.SpecRef,
            Codes = errorContext.Codes.Select(code => code.ToString()).ToArray(),
        };
}
