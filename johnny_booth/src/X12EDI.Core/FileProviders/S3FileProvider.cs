using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Amazon;
using EdiFabric.Templates.Hipaa5010;
using X12EDI.Core.Config;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace X12EDI.Core.FileProviders
{
    public class S3FileProvider : IFileProvider
    {
        #region Private Fields

        private readonly string _bucketName;
        private readonly ILogger<S3FileProvider> _logger;
        private readonly IAmazonS3 _s3Client;

        #endregion Private Fields

        #region Public Constructors

        public S3FileProvider(ILogger<S3FileProvider> logger, IAmazonS3 s3Client, S3Options s3Options)
        {
            _s3Client = s3Client;
            _bucketName = s3Options.BucketName;
            _logger = logger;
        }

        #endregion Public Constructors

        // --- IFileProvider Implementation ---

        #region Public Methods

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

        public IFileInfo GetFileInfo(string subpath)
        {
            // Note: We don't call S3 here. The metadata check (HEAD request)
            // is deferred to the S3FileInfo.Exists property.
            return new S3FileInfo(_s3Client, _bucketName, subpath);
        }

        public IChangeToken Watch(string filter)
        {
            // S3 does not have a native change notification system
            return NullChangeToken.Singleton;
        }

        #endregion Public Methods
    }
}