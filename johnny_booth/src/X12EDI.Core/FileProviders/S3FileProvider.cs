using Amazon.S3;
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

        #endregion Public Constructors

        #region Public Methods

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // Optional: list objects in S3 bucket
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return new S3FileInfo(_s3, _bucketName, subpath);
        }

        public IChangeToken Watch(string filter)
        {
            // Optional: implement if you want change tracking
            return NullChangeToken.Singleton;
        }

        #endregion Public Methods
    }
}