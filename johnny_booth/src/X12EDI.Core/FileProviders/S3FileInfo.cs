using Amazon.S3;
using Microsoft.Extensions.FileProviders;

public class S3FileInfo : IFileInfo
{
    #region Private Fields

    private readonly string _bucket;
    private readonly string _key;
    private readonly IAmazonS3 _s3;

    #endregion Private Fields

    #region Public Constructors

    public S3FileInfo(IAmazonS3 s3, string bucket, string key)
    {
        _s3 = s3;
        _bucket = bucket;
        _key = key;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool Exists => true; // You can add a metadata check if needed

    public bool IsDirectory => false;

    public DateTimeOffset LastModified => DateTimeOffset.UtcNow;

    public long Length
    {
        get
        {
            var meta = _s3.GetObjectMetadataAsync(_bucket, _key).Result;
            return meta.ContentLength;
        }
    }

    public string Name => Path.GetFileName(_key);
    public string? PhysicalPath => null;

    #endregion Public Properties

    #region Public Methods

    // Or use metadata
    public Stream CreateReadStream()
    {
        var response = _s3.GetObjectAsync(_bucket, _key).Result;
        return response.ResponseStream;
    }

    #endregion Public Methods
}