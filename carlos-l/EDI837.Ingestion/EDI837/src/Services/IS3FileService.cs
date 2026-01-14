using EDI837.src.HelperObjects;

namespace EDI837.src.Services
{
    public interface IS3FileService
    {
        public Task<bool> BucketExistsAsync(string bucketName);
        public Task<Stream> GetClaimStreamByFileNameAsync(string bucketName, string fileName);
        public Task<IEnumerable<StreamResult>> GetClaimStreamsByBucketNameAndPrefixAsync(string bucketName, string prefix);
        public Task DeleteFileAsync(string bucketName, string fileName);
    }
}
