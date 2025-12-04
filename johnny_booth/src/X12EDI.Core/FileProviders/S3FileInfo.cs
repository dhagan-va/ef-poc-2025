using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;
using System.Threading;

namespace X12EDI.Core.FileProviders
{
    /// <summary>
    /// Represents a file in an Amazon S3 bucket, implementing the <see cref="IFileInfo"/> interface.
    /// </summary>
    public class S3FileInfo : IFileInfo
    {
        #region Private Fields

        private readonly string _bucketName;
        private readonly string _key;
        private readonly IAmazonS3 _s3Client;
        private GetObjectMetadataResponse? _metadata = default;
        private bool _metadataInitialized = false;

        #endregion Private Fields

        // Cached metadata

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="S3FileInfo"/> class for a file whose metadata is not yet fetched.
        /// </summary>
        /// <param name="s3Client">The configured Amazon S3 client.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="key">The key (path) of the file in the bucket.</param>
        public S3FileInfo(IAmazonS3 s3Client, string bucketName, string key)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
            _key = key.TrimStart('/');
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="S3FileInfo"/> class from an existing <see cref="S3Object"/>.
        /// </summary>
        /// <param name="s3Client">The configured Amazon S3 client.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Object">The S3 object containing metadata.</param>
        public S3FileInfo(IAmazonS3 s3Client, string bucketName, S3Object s3Object)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
            _key = s3Object.Key;

            // Populate properties directly from S3Object for efficiency
            Length = s3Object.Size ?? -1;
            LastModified = s3Object.LastModified.HasValue
                ? s3Object.LastModified.Value.ToUniversalTime()
                : default;
            _metadataInitialized = true;
        }

        #endregion Public Constructors

        #region Public Properties

        // --- IFileInfo Properties ---
        /// <summary>
        /// Gets a value indicating whether the file exists in the S3 bucket.
        /// This property triggers a metadata fetch if not already initialized.
        /// </summary>
        public bool Exists
        {
            get
            {
                if (!_metadataInitialized)
                {
                    // This is a synchronous call, which is not ideal.
                    // Consider initializing with FetchMetadataAsync for better performance.
                    FetchMetadataAsync(CancellationToken.None).GetAwaiter().GetResult();
                }
                return _metadata != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is a directory.
        /// </summary>
        public bool IsDirectory => _key.EndsWith('/');

        /// <summary>
        /// Gets the last modified date and time of the file.
        /// </summary>
        public DateTimeOffset LastModified { get; private set; }

        /// <summary>
        /// Gets the length of the file in bytes.
        /// </summary>
        public long Length { get; private set; } = -1;

        /// <summary>
        /// Gets the name of the file or directory.
        /// </summary>
        public string Name => Path.GetFileName(_key.TrimEnd('/'));

        /// <summary>
        /// Gets the physical path of the file, which is always null for S3 files.
        /// </summary>
        public string PhysicalPath => default!;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates a readable stream for the file's content.
        /// </summary>
        /// <returns>A <see cref="Stream"/> for reading the file.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        public Stream CreateReadStream()
        {
            if (!Exists)
            {
                throw new FileNotFoundException($"File not found in S3: {_key}");
            }

            var request = new GetObjectRequest { BucketName = _bucketName, Key = _key };

            // Blocking call to get the stream (GET request)
            var response = _s3Client.GetObjectAsync(request).GetAwaiter().GetResult();
            return response.ResponseStream;
        }

        /// <summary>
        /// Asynchronously fetches the file's metadata from S3.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task FetchMetadataAsync(CancellationToken cancellationToken)
        {
            if (_metadataInitialized)
            {
                return;
            }

            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = _key
                };
                _metadata = await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
                Length = _metadata.ContentLength;
                LastModified = _metadata.LastModified.HasValue
                    ? _metadata.LastModified.Value.ToUniversalTime()
                    : default;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _metadata = null;
            }
            finally
            {
                _metadataInitialized = true;
            }
        }

        #endregion Public Methods
    }
}