using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using X12EDI.Core.Config;

namespace X12EDI.Core.FileProviders
{
    /// <summary>
    /// An implementation of <see cref="IFileProvider"/> that provides file access to an Amazon S3 bucket.
    /// </summary>
    public class S3FileProvider : IFileProvider
    {
        #region Private Fields

        private readonly string _bucketName;
        private readonly ILogger<S3FileProvider> _logger;
        private readonly IAmazonS3 _s3Client;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="S3FileProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger for recording information and errors.</param>
        /// <param name="s3Client">The configured Amazon S3 client.</param>
        /// <param name="s3Options">The S3 configuration options, including the bucket name.</param>
        public S3FileProvider(ILogger<S3FileProvider> logger, IAmazonS3 s3Client, S3Options s3Options)
        {
            _s3Client = s3Client;
            _bucketName = s3Options.BucketName;
            _logger = logger;
        }

        #endregion Public Constructors

        // --- IFileProvider Implementation ---

        #region Public Methods

        /// <summary>
        /// Enumerates a directory at the given path in the S3 bucket.
        /// </summary>
        /// <param name="subpath">The path that identifies the directory in the S3 bucket.</param>
        /// <returns>An <see cref="IDirectoryContents"/> object representing the contents of the directory.</returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            string prefix = string.IsNullOrEmpty(subpath) ? string.Empty : subpath.TrimStart('/');
            if (!prefix.EndsWith("/") && !string.IsNullOrEmpty(prefix))
            {
                prefix += "/"; // Append slash for directory search
            }

            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix,
                Delimiter = "/" // Used to get only top-level files/folders
            };

            try
            {
                // Blocking asynchronous call for synchronous contract compliance (common for IFileProvider)
                var response = _s3Client.ListObjectsV2Async(request).GetAwaiter().GetResult();

                var filesAndFolders = new List<IFileInfo>();

                // Map files (S3Objects)
                if (response.S3Objects != null)
                {
                    filesAndFolders.AddRange(response.S3Objects
                        // Exclude the folder object itself if it exists (e.g., prefix/)
                        .Where(o => o.Key != prefix)
                        .Select(o => new S3FileInfo(_s3Client, _bucketName, o)));
                }

                // Map folders (CommonPrefixes)
                if (response.CommonPrefixes != null)
                {
                    filesAndFolders.AddRange(response.CommonPrefixes
                        .Select(p => new S3DirectoryInfo(p)));
                }

                return new S3DirectoryContents(filesAndFolders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetDirectoryContents: {ex.Message}");
            }

            return NotFoundDirectoryContents.Singleton;
        }

        /// <summary>
        /// Locates a file at the given path in the S3 bucket.
        /// </summary>
        /// <param name="subpath">The path that identifies the file in the S3 bucket.</param>
        /// <returns>An <see cref="IFileInfo"/> object representing the file. The existence of the file is checked on-demand.</returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            // Note: We don't call S3 here. The metadata check (HEAD request)
            // is deferred to the S3FileInfo.Exists property.
            return new S3FileInfo(_s3Client, _bucketName, subpath);
        }

        /// <summary>
        /// Returns a <see cref="IChangeToken"/> that cannot be used to watch for changes.
        /// </summary>
        /// <param name="filter">A filter string used to determine which files or folders to watch.</param>
        /// <returns>A <see cref="NullChangeToken"/>, as S3 does not support native change notifications.</returns>
        public IChangeToken Watch(string filter)
        {
            // S3 does not have a native change notification system
            return NullChangeToken.Singleton;
        }

        #endregion Public Methods
    }
}