using Amazon.S3;
using EdiFabric.Templates.Hipaa5010;

namespace EDI837.src.Services
{
    public class S3FileService : IS3FileService
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ILogger _logger;

        public S3FileService(IAmazonS3 amazonS3, ILogger<S3FileService> logger) 
        { 
            _amazonS3 = amazonS3;
            _logger = logger;
        }

        public async Task<bool> BucketExists(string bucketName)
        { 
            ArgumentException.ThrowIfNullOrEmpty(nameof(bucketName));

            try
            {
                // GetBucketLocationAsync and catch a 404.
                var response = await _amazonS3.GetBucketLocationAsync(bucketName);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }           
        }
    }
}
