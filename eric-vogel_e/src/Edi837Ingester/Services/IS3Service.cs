
namespace Edi837Ingester.Services
{
    public interface IS3Service
    {
        void Dispose();
        Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default);
    }
}