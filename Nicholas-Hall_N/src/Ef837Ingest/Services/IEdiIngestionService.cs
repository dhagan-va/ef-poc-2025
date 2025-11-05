namespace Edi837Ingestion.Edi
{
    public interface IEdiIngestionService
    {
        Task<int> IngestAsync(Stream ediStream, CancellationToken ct = default);
    }
}
