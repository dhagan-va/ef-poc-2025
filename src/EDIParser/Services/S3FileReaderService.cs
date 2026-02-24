using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EdiParser.Services;

public interface IS3FileReaderService
{
    Task<Stream> GetS3FileStreamAsync(string bucketName, string fileName, CancellationToken cancellationToken = default);
}

public class S3FileReaderService: IS3FileReaderService, IDisposable
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _s3Client;
    private readonly bool _disposeClient = false;


    public S3FileReaderService(ILogger<Application> logger, IConfiguration configuration, IAmazonS3 s3Client)
    {
        _logger = logger;
        _configuration = configuration;
        _s3Client = s3Client;
    }

    /// <summary>
    /// Downloads an object from S3 into a MemoryStream.
    /// </summary>
    /// <param name="bucketName">S3 bucket name</param>
    /// <param name="fileName">S3 object key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A MemoryStream with the contents; Position==0. Caller must dispose.</returns>
    public async Task<Stream> GetS3FileStreamAsync(string bucketName, string fileName, CancellationToken cancellationToken = default)
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



    
    