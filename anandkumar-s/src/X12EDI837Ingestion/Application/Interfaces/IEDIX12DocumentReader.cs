
using X12EDI837Ingestion.Consumer.Application.Models;

namespace X12EDI837Ingestion.Consumer.Application.Services;

public interface IEdiX12DocumentReader
{
    Task<ParsedEdiDocument> ReadAsync(
        Stream stream,
        string sourceName,
        CancellationToken cancellationToken = default);
}