using X12EDI837Ingestion.Consumer.Application.Models;

namespace X12EDI837Ingestion.Consumer.Application.Validator;

public interface ISnipLevelValidator
{
    int SnipLevel { get; }

    Task<IReadOnlyList<SnipValidationError>> ValidateAsync(
        ParsedEdiDocument document,
        CancellationToken cancellationToken = default);
}