using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Edi837Ingester.Services
{
    /// <summary>
    /// Simple S3 gateway that downloads an object from S3 by bucket/key.
    /// - Constructor accepts an injected <see cref="IAmazonS3"/> for testability.
    /// - Provides an async download method and a synchronous wrapper to preserve the original API.
    /// - Caller is responsible for disposing the returned <see cref="Stream"/>.
    /// </summary>
    public class S3Service : IDisposable, IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly bool _disposeClient;

        public S3Service(IAmazonS3? s3Client = null)
        {
            if (s3Client is not null)
            {
                _s3Client = s3Client;
                _disposeClient = false;
            }
            else
            {
                // Use AWS region environment variables if present, otherwise rely on SDK defaults
                var regionName = Environment.GetEnvironmentVariable("AWS_REGION")
                                 ?? Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");

                _s3Client = !string.IsNullOrEmpty(regionName)
                    ? new AmazonS3Client(RegionEndpoint.GetBySystemName(regionName))
                    : new AmazonS3Client();

                _disposeClient = true;
            }
        }

        /// <summary>
        /// Downloads an object from S3 into a MemoryStream.
        /// </summary>
        /// <param name="bucketName">S3 bucket name</param>
        /// <param name="fileName">S3 object key</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A MemoryStream with the contents; Position==0. Caller must dispose.</returns>
        public async Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("Bucket name must be provided", nameof(bucketName));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name (key) must be provided", nameof(fileName));

            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                using var response = await _s3Client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);

                // Copy response stream into a memory stream so we can return a seekable stream
                var ms = new MemoryStream();
                await response.ResponseStream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
                ms.Position = 0;
                return ms;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new FileNotFoundException($"S3 object not found: bucket='{bucketName}', key='{fileName}'", ex);
            }
        }

        public void Dispose()
        {
            if (_disposeClient)
            {
                _s3Client?.Dispose();
            }
        }
    }
}