using Amazon.S3;
using Microsoft.Extensions.FileProviders;

public class S3FileInfo : IFileInfo
{
    #region Private Fields

    private readonly string _bucketName;
    private readonly string _key;
    private readonly IAmazonS3 _s3;

    #endregion Private Fields

    #region Public Constructors

    public S3FileInfo(IAmazonS3 s3, string bucketName, string key)
    {
        _s3 = s3;
        _bucketName = bucketName;
        _key = key;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool Exists => true;
    public bool IsDirectory => false;

    public DateTimeOffset LastModified =>
        _s3.GetObjectMetadataAsync(_bucketName, _key)
           .GetAwaiter().GetResult().LastModified ?? DateTime.MinValue;

    public long Length => _s3.GetObjectMetadataAsync(_bucketName, _key)
                             .GetAwaiter().GetResult().ContentLength;

    public string Name => Path.GetFileName(_key);
    public string? PhysicalPath => null;

    #endregion Public Properties

    #region Public Methods

    public Stream CreateReadStream()
    {
        var response = _s3.GetObjectAsync(_bucketName, _key)
                          .GetAwaiter().GetResult();
        return response.ResponseStream;
    }

    #endregion Public Methods
}