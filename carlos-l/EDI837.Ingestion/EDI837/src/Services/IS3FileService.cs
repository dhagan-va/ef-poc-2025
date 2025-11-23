namespace EDI837.src.Services
{
    public interface IS3FileService
    {
        public Task<bool> BucketExists(string bucketName);
    }
}
