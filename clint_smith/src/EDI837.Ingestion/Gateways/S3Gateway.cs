using Amazon.S3;
using Amazon.S3.Model;

namespace EDI837.Ingestion.Gateways
{
    public sealed class S3Gateway : IDisposable
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName = "edi-bucket";

        public S3Gateway(Uri serviceURI, string bucketName)
        {
            ArgumentNullException.ThrowIfNull(serviceURI);
            ArgumentNullException.ThrowIfNull(bucketName);

            var config = new AmazonS3Config
            {
                ServiceURL = serviceURI.ToString(),
                ForcePathStyle = true,
            };

            _s3Client = new AmazonS3Client("test", "test", config);

            try
            {
                _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = bucketName }).Wait();
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == "BucketAlreadyOwnedByYou")
            {
                // ignore if it already exists
            }
        }

        public async Task<List<S3Object>> ListFilesAsync(string prefix = "")
        {
            if (_s3Client == null)
            {
                Console.WriteLine("Error: S3 client not initialized.");
                return [];
            }

            if (string.IsNullOrEmpty(_bucketName))
            {
                Console.WriteLine("Error: Bucket name is missing.");
                return [];
            }

            try
            {
                var response = await _s3Client.ListObjectsV2Async(
                    new ListObjectsV2Request { BucketName = _bucketName, Prefix = prefix }
                );

                return response.S3Objects ?? [];
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 error listing objects: {ex.ErrorCode} - {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error listing objects: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetFileContentAsync(string key)
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            using var reader = new StreamReader(response.ResponseStream);
            return await reader.ReadToEndAsync();
        }

        public async Task<Stream> GetFileStreamAsync(string key)
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            return response.ResponseStream;
        }

        public async Task UploadFileAsync(string key, string content)
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            await _s3Client.PutObjectAsync(
                new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = stream,
                }
            );
        }

        public async Task DeleteFileAsync(string key)
        {
            try
            {
                await _s3Client.DeleteObjectAsync(
                    new DeleteObjectRequest { BucketName = _bucketName, Key = key }
                );

                Console.WriteLine($"Deleted file '{key}' from bucket '{_bucketName}'.");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"S3 error deleting '{key}': {ex.ErrorCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error deleting '{key}': {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            _s3Client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
