using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;
using System.IO;

// ... (namespace and usings for Amazon.S3, Microsoft.Extensions.FileProviders)

public class S3FileInfo : IFileInfo
{
    #region Private Fields

    private readonly string _bucketName;
    private readonly string _key;
    private readonly IAmazonS3 _s3Client;
    private GetObjectMetadataResponse? _metadata = default;

    #endregion Private Fields

    // Cached metadata

    #region Public Constructors

    // Constructor used by GetFileInfo
    public S3FileInfo(IAmazonS3 s3Client, string bucketName, string key)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
        _key = key.TrimStart('/');
    }

    // Constructor used by GetDirectoryContents (when S3Object is known)
    public S3FileInfo(IAmazonS3 s3Client, string bucketName, S3Object s3Object)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
        _key = s3Object.Key;

        // Populate properties directly from S3Object for efficiency
        Length = s3Object.Size != null ? s3Object.Size.Value : -1;
        LastModified = s3Object.LastModified.HasValue ? s3Object.LastModified.Value : DateTime.MinValue;
    }

    #endregion Public Constructors

    #region Public Properties

    // --- IFileInfo Properties ---
    public bool Exists
    {
        get
        {
            if (_metadata != null)
            {
                return true;
            }

            if (Length > 0)
            {
                return true; // Already populated from ListObjectsV2
            }

            var request = new GetObjectMetadataRequest { BucketName = _bucketName, Key = _key };
            try
            {
                // Blocking call for metadata (HEAD request)
                _metadata = _s3Client.GetObjectMetadataAsync(request).GetAwaiter().GetResult();
                Length = _metadata.ContentLength;
                LastModified = _metadata.LastModified.HasValue ? _metadata.LastModified.Value : DateTime.MinValue;
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            catch { return false; }
        }
    }

    public bool IsDirectory => false;

    public DateTimeOffset LastModified { get; private set; }

    // Lazy-loaded properties
    public long Length { get; private set; } = -1;

    public string Name => Path.GetFileName(_key);
    public string PhysicalPath => string.Empty;

    #endregion Public Properties

    #region Public Methods

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

    #endregion Public Methods
}