using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deepika.EDIIngestion.Services
{
    public interface IEdiStorageService
    {
        Task<IEnumerable<string>> ListObjectsAsync(string bucket);
        Task<byte[]> ReadObjectAsync(string bucket, string key);
    }
}
