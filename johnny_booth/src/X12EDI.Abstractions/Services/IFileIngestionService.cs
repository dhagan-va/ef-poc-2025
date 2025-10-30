
namespace X12EDI.Core.Services
{
    public interface IFileIngestionService
    {
        Task IngestAllAsync(CancellationToken cancellationToken = default);
        Task IngestAsync(string subpath, CancellationToken cancellationToken = default);
    }
}