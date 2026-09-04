using EdiFabric.Core.Model.Edi.ErrorContexts;

namespace PayerEDI.Data.Database.Tables;

public record EdiErrorTable
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid DocumentId { get; init; }
    public required string Name { get; init; }
    public string? ControlNumber { get; init; }
    public string? Edition { get; init; }
    public string? Release { get; init; }
    public int Index { get; init; }
    public int ValidatedSegmentsCount { get; init; }
    public string? Message { get; init; }
    public string[] Codes { get; init; } = [];
    public ICollection<EdiSegmentErrorTable> Errors { get; init; } = [];
}

public static class MessageErrorContextExtensions
{
    public static EdiErrorTable CreateEdiError(
        this MessageErrorContext errorContext,
        Guid documentId
    )
    {
        var result = new EdiErrorTable
        {
            DocumentId = documentId,
            Name = errorContext.Name ?? string.Empty,
            ControlNumber = errorContext.ControlNumber,
            Edition = errorContext.Edition,
            Release = errorContext.Release,
            Index = errorContext.Index,
            ValidatedSegmentsCount = errorContext.ValidatedSegmentsCount,
            Message = errorContext.Message,
            Codes = errorContext.Codes.Select(code => code.ToString()).ToArray(),
        };

        foreach (var error in errorContext.Errors)
        {
            result.Errors.Add(error.CreateEdiSegmentError(result.Id));
        }

        return result;
    }
}
