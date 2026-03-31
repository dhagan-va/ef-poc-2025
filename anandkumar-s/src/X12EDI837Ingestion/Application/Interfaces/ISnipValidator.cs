using X12EDI837Ingestion.Consumer.Application.Models;

namespace X12EDI837Ingestion.Consumer.Application.Interfaces;

public interface ISnipValidator
{
    Task<SnipValidationResult> ValidateAsync(
        Stream stream,
        string sourceName,
        CancellationToken cancellationToken = default);
}