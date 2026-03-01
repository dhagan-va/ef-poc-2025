using X12EDI837Ingestion.Domain.Entities;

namespace X12EDI837Ingestion.Infrastructure.Repositories
{

    public interface IX12EDI837IngestRepo
    {
        Task<long> InsertInterchangeAsync(
        InterchangeHeader interchange,
        CancellationToken cancellationToken = default);

    }

}
