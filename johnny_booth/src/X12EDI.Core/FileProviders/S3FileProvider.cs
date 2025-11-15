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

namespace X12EDI.Core.FileProviders
{
    public class S3FileProvider : IFileProvider
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        // 1. DI-FRIENDLY CONSTRUCTOR (Server-side/ASP.NET Core)
        // IAmazonS3 is expected to be registered and configured in Program.cs
        public S3FileProvider(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }

        // 2. CLIENT-SIDE/STANDALONE FACTORY METHOD
        // Use this for command-line apps or testing with a mock endpoint (like Moto.py)
        public static S3FileProvider CreateStandalone(
            string accessKey,
            string secretKey,
            string bucketName,
            string serviceURL,
            IServiceProvider serviceProvider,
            bool forcePathStyle = true)
        {
            var config = new AmazonS3Config
            {
                ServiceURL = serviceURL,
                ForcePathStyle = forcePathStyle,
                HttpClientFactory = new CustomS3ClientFactory(serviceProvider),
            };

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var s3Client = new AmazonS3Client(credentials, config);

            // Use the main constructor for initialization
            return new S3FileProvider(s3Client, bucketName);
        }

        // --- IFileProvider Implementation ---

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
            catch { /* Log error */ }

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
    }
}