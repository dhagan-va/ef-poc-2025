using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace X12EDI.Core.FileProviders
{
    public class S3FileProvider : IFileProvider
    {
        #region Private Fields

        private readonly string _bucketName;
        private readonly IAmazonS3 _s3;

        #endregion Private Fields

        #region Public Constructors

        public S3FileProvider(IAmazonS3 s3, string bucketName)
        {
            _s3 = s3;
            _bucketName = bucketName;
        }

        // Constructor for client-side (self-contained)
        public S3FileProvider(string accessKey, string secretKey, string bucketName, string uRL, bool forcePathStyle = true)
        {
            var config = new AmazonS3Config
            {
                ServiceURL = uRL, // "https://localhost:5001", // Blazor endpoint
                ForcePathStyle = forcePathStyle                  // use /bucket/key style
            };

            _s3 = new AmazonS3Client(accessKey, secretKey, config);
            _bucketName = bucketName;
        }

        #endregion Public Constructors

        #region Public Methods

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // Normalize prefix (S3 uses keys, not real directories)
            string prefix = string.IsNullOrEmpty(subpath) ? string.Empty : subpath.TrimStart('/');

            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix,
                Delimiter = "/" // optional: treat "/" as directory separator
            };

            // Use Awaiter to avoid deadlock
            var response = _s3.ListObjectsV2Async(request)
                .GetAwaiter()
                .GetResult();

            if (response.S3Objects == null || response.S3Objects.Count == 0)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var files = response.S3Objects
                .Select(o => new S3FileInfo(_s3, _bucketName, o.Key))
                .Cast<IFileInfo>()
                .ToList();

            return new S3DirectoryContents(files);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return new S3FileInfo(_s3, _bucketName, subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        #endregion Public Methods
    }
}