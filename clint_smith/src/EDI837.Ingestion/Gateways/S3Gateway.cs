using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace EDI837.Ingestion.Gateways
{
    /// <summary>
    /// S3 Gateway is a convenience class for interacting with S3.
    /// </summary>
    public sealed class S3Gateway : IDisposable
    {
        private readonly ILogger<S3Gateway> _logger;
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName = "edi-bucket";

        /// <summary>
        /// Constructor for S3 Gateway
        /// </summary>
        /// <param name="serviceURI"></param>
        /// <param name="bucketName"></param>
        /// <param name="logger"></param>
        public S3Gateway(Uri serviceURI, string bucketName, ILogger<S3Gateway> logger)
        {
            ArgumentNullException.ThrowIfNull(serviceURI);
            ArgumentNullException.ThrowIfNull(bucketName);

            _logger = logger;

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

        /// <summary>
        /// Asynchronous method which List files in the bucket given
        /// during class instantiation,
        /// </summary>
        /// <param name="prefix">An optional prefix for filtering files</param>
        /// <returns></returns>
        public async Task<List<S3Object>> ListFilesAsync(string prefix = "")
        {
            if (_s3Client == null)
            {
                _logger.LogError("S3 client not initialized.");
                return [];
            }

            if (string.IsNullOrEmpty(_bucketName))
            {
                _logger.LogError("Bucket name is missing.");
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
                _logger.LogError(ex, "S3 error listing objects: {ErrorCode}", ex.ErrorCode);
                return [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error listing objects.");
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves the contents of a file stored in the configured S3 bucket.
        /// </summary>
        /// <param name="key">The S3 object key (path) identifying the file to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the file content as a string.</returns>
        public async Task<string> GetFileContentAsync(string key)
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            using var reader = new StreamReader(response.ResponseStream);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a file from the configured S3 bucket as a readable stream.
        /// </summary>
        /// <param name="key">The S3 object key (path) identifying the file to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="Stream"/>
        /// providing access to the file’s contents.
        /// </returns>
        public async Task<Stream> GetFileStreamAsync(string key)
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            return response.ResponseStream;
        }

        /// <summary>
        /// Asynchronously uploads text content to the configured S3 bucket.
        /// </summary>
        /// <param name="key">The S3 object key (path) to assign to the uploaded file.</param>
        /// <param name="content">The text content to upload to S3.</param>
        /// <returns>A task representing the asynchronous upload operation.</returns>
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

        /// <summary>
        /// Asynchronously deletes a file from the configured S3 bucket.
        /// </summary>
        /// <param name="key">The S3 object key (path) of the file to delete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        /// <remarks>
        /// Logs S3-specific errors and rethrows unexpected exceptions to the caller.
        /// </remarks>
        public async Task DeleteFileAsync(string key)
        {
            try
            {
                await _s3Client.DeleteObjectAsync(
                    new DeleteObjectRequest { BucketName = _bucketName, Key = key }
                );

                _logger.LogInformation(
                    "Deleted file '{Key}' from bucket '{Bucket}'",
                    key,
                    _bucketName
                );
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, "S3 error deleting '{Key}': {ErrorCode}", key, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting '{Key}'", key);
                throw;
            }
        }

        /// <summary>
        /// Releases all resources used by the underlying S3 client.
        /// </summary>
        public void Dispose()
        {
            _s3Client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
