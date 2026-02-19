using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EdiParser.Services;

public interface IEdiReaderService
{
    Stream GetFileStream(string fullPath);
    Task<Stream> GetFileStreamAsync(string fullPath);
    Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default);
}

public class EdiReaderService : IEdiReaderService, IDisposable
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _s3Client;
    private readonly bool _disposeClient = false;


    public EdiReaderService(ILogger<Application> logger, IConfiguration configuration, IAmazonS3 s3Client)
    {
        _logger = logger;
        _configuration = configuration;
        _s3Client = s3Client;
    }

    /// <summary>
    /// Method opens the file for reading
    /// </summary>
    /// <param name="fullPath">Path to the file</param>
    /// <returns>Stream of the file</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Stream> GetFileStreamAsync(string fullPath)
    {
        try
        {
            _logger.LogInformation($"Reading file {fullPath}");
            await Task.Run(() =>
            {
                FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                return stream;
            });
            //fileStream = File.OpenRead(fullPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Error reading file {fullPath}: {e.Message}");
            throw new Exception(e.Message, e);
        }
        // Should not execute
        FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return stream;
    }

    
    /// <summary>
    /// Method opens the file for reading
    /// </summary>
    /// <param name="fullPath">Path to the file</param>
    /// <returns>Stream of the file</returns>
    /// <exception cref="Exception"></exception>
    public Stream GetFileStream(string fullPath)
    {
        //  Open to a stream 
        try
        {
            _logger.LogInformation($"Opening file stream for {fullPath}");
            FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            return fs;
            
            //fileStream = File.OpenRead(fullPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Error reading file {fullPath}: {e.Message}");
            throw new Exception(e.Message, e);
        }
        
    }
    
    /// <summary>
    /// Downloads an object from S3 into a MemoryStream.
    /// </summary>
    /// <param name="bucketName">S3 bucket name</param>
    /// <param name="fileName">S3 object key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A MemoryStream with the contents; Position==0. Caller must dispose.</returns>
    public async Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default)
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