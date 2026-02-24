using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EdiParser.Services;

public interface IEdiReaderService
{
    Stream GetFileStream(string fullPath);
    Task<Stream> GetFileStreamAsync(string fullPath);
}

public class EdiReaderService : IEdiReaderService
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    

    public EdiReaderService(ILogger<Application> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Error reading file {fullPath}: {e.Message}");
            throw new Exception(e.Message, e);
        }
    }
}