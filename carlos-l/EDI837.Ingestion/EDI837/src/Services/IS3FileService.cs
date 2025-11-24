namespace EDI837.src.Services
{
    public interface IS3FileService
    {
        public Task<bool> BucketExists(string bucketName);
        public Task<Stream> GetStreamByFileName(string bucketName, string fileName);
    }
}
