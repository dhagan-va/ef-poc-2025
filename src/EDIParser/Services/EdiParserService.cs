using System.ComponentModel;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using EdiParser.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EdiParser.Services;

public interface IEdiParserService
{
    List<T> ParseX12File<T>(Stream fileStream, string ediFabricTemplateName) where T : EdiMessage;
    Task<List<T>> ParseX12FileAsync<T>(Stream fileStream,  string ediFabricTemplateName) where T : EdiMessage;
}

public class EdiParserService : IEdiParserService
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    
    public EdiParserService(ILogger<Application> logger, IConfiguration configuration)
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
    /// <param name="fileStream">File stream of the file</param>
    /// <param name="ediFabricTemplateName">Edi Fabric Template name (like EdiFabric.Templates.X12)</param>
    public List<T> ParseX12File<T>(Stream fileStream, string ediFabricTemplateName) where T : EdiMessage
    {
        // Parse file
        try
        {
            _logger.LogInformation($"Parsing EDI file using {ediFabricTemplateName} template...");
            using (var reader = new X12Reader(fileStream, ediFabricTemplateName,
                       new X12ReaderSettings { ContinueOnError = true }))
            {
                IEnumerable<IEdiItem> ediItems = reader.ReadToEnd();
                var parsedEdiItems = ediItems.OfType<T>().ToList();
                _logger.LogInformation($"Read EDI file successfully...");
                return parsedEdiItems;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error parsing X12 File: {e.Message}");
            throw new Exception(e.Message, e);
        }
        finally
        {
            fileStream.Close();
        }
    }

    /// <summary>
    /// Parse action
    /// </summary>
    /// <param name="fileStream">File Stream of the file</param>
    /// <param name="ediFabricTemplateName">Edi Fabric Template name (like EdiFabric.Templates.X12)</param>
    public async Task<List<T>> ParseX12FileAsync<T>(Stream fileStream, string ediFabricTemplateName) where T : EdiMessage
    {
        // Parse file
        try
        {
            _logger.LogInformation($"Parsing EDI file using {ediFabricTemplateName} template...");
            
            using (var reader = new X12Reader(fileStream, ediFabricTemplateName,
                       new X12ReaderSettings { ContinueOnError = true }))
            {
                IEnumerable<IEdiItem> ediItems = await reader.ReadToEndAsync(); 
                var transactions  = ediItems.OfType<T>().ToList();
                _logger.LogInformation($"Parsed EDI file successfully...");
                
                if (!transactions.Any())
                {
                    _logger.LogWarning("No EDI transactions found in file");
                    throw new Exception("No EDI transactions found in file");
                }
                return transactions;
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error parsing X12 File: {e.Message}");
            throw new Exception(e.Message, e);
        }
        finally
        {
            fileStream.Close();
        }
    }
}

