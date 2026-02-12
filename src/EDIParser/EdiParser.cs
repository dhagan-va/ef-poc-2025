using System.ComponentModel;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EdiParser;

public interface IEdiParser
{
    List<IEdiItem> ParseX12File(string fullPath, string ediFabricTemplateName);
    Task<IEnumerable<IEdiItem>> ParseX12FileAsync(string fullPath, string ediFabricTemplateName);
}

public class EdiParser : IEdiParser
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    
    public EdiParser(ILogger<Application> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
        string? ediFabricLicenseKey = _configuration["EdiFabricLicenseKey"];
        if (String.IsNullOrEmpty(ediFabricLicenseKey))
        {
            _logger.LogError("Failed to find EDIFabricLicenseKey");
            return;
        }
        
        EdiFabric.SerialKey.Set(ediFabricLicenseKey);
        _logger.LogInformation($"Days to key expiration {EdiFabric.SerialKey.DaysToExpiration}...");
    }
    
    
    /// <summary>
    /// Parse action
    /// </summary>
    /// <param name="fullPath">Complete path to file to open</param>
    /// <param name="ediFabricTemplateName">Edi Fabric Template name (like EdiFabric.Templates.X12)</param>
    public List<IEdiItem> ParseX12File(string fullPath, string ediFabricTemplateName)
    {
        //  Open to a stream 
        Stream fileStream;
        try
        {
            _logger.LogInformation($"Reading {ediFabricTemplateName} file {fullPath}");
            fileStream = File.OpenRead(fullPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Error reading {ediFabricTemplateName} file {fullPath}: {e.Message}");
           throw new Exception(e.Message, e);
        }

        // Parse file
        try
        {
            _logger.LogInformation($"Parsing EDI file {fullPath}...");
            List<IEdiItem> ediItems;
            using (var reader = new X12Reader(fileStream, ediFabricTemplateName,
                       new X12ReaderSettings { ContinueOnError = true }))
            {
                ediItems = reader.ReadToEnd().ToList();
            }
            _logger.LogInformation($"Read EDI file {fullPath} successfully...");
            return ediItems;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error parsing X12 File {fullPath}: {e.Message}");
            throw new Exception(e.Message, e);
        }
    }
    
    /// <summary>
    /// Parse action
    /// </summary>
    /// <param name="fullPath">Complete path to file to open</param>
    /// <param name="ediFabricTemplateName">Edi Fabric Template name (like EdiFabric.Templates.X12)</param>
    public async Task<IEnumerable<IEdiItem>> ParseX12FileAsync(string fullPath, string ediFabricTemplateName)
    {
        //  Open to a stream 
        Stream fileStream;
        try
        {
            _logger.LogInformation($"Reading {ediFabricTemplateName} file {fullPath}");
            fileStream = File.OpenRead(fullPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Error reading {ediFabricTemplateName} file {fullPath}: {e.Message}");
            throw new Exception(e.Message, e);
        }

        // Parse file
        try
        {
            _logger.LogInformation($"Parsing EDI file {fullPath}...");
            IEnumerable<IEdiItem> ediItems;
            using (var reader = new X12Reader(fileStream, ediFabricTemplateName,
                       new X12ReaderSettings { ContinueOnError = true }))
            {
                ediItems = await reader.ReadToEndAsync();
            }

            _logger.LogInformation($"Read EDI file {fullPath} successfully...");
            return ediItems;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error reading X12 File {fullPath}: {e.Message}");
            throw new Exception(e.Message, e);
        }
    }
}

