using Ef837Ingest.Edi.Validator;

namespace Edi837Ingestion.Services
{
    public interface IEdiReprocessService
    {
        Task<SnipResult?> Reprocess837PAsync(string transactionSetControlNumber, SnipOptions? options = null, CancellationToken ct = default);
    }
}