using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Producer.Interfaces
{
    public interface IS3StorageService
    {
        Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default);
        Task UploadFileAsync(string key, string filePath, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<string>> ListObjectKeysAsync(string? prefix = null, CancellationToken cancellationToken = default);
        Task<Stream> DownloadObjectAsync(string key, CancellationToken cancellationToken = default);
    }
}
