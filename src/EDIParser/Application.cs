using EdiFabric.Core.Model.Edi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EdiParser;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    private readonly EdiDBContext _context;
    private readonly IEdiParser _parser;

    public Application(ILogger<Application> logger, IConfiguration configuration, EdiDBContext context, IEdiParser parser)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _parser = parser;
        
    }

    public void Run()
    {
        _logger.LogInformation("Starting EDI parser...");
        
        // Read a setting
        string? configTestFilesPath = _configuration["TestFilesPath"];
        if (String.IsNullOrEmpty(configTestFilesPath))
        {
            _logger.LogError("Failed to find TestFilesPath");
            return;
        }
        else
            _logger.LogInformation($"Testing file in '{configTestFilesPath}' is used");
            
        string fullPath = Path.GetFullPath(configTestFilesPath);

        string? ediFabricLicenseKey = _configuration["EdiFabricLicenseKey"];
        if (String.IsNullOrEmpty(ediFabricLicenseKey))
        {
            _logger.LogError("Failed to find EDIFabricLicenseKey");
            return;
        }
        
        try
        {
            List<IEdiItem> ediItems = _parser.ParseX12File(fullPath, "EdiFabric.Templates.X12");
            var edi837Data = ediItems.OfType<TS837>();
            _logger.LogInformation($"Read {edi837Data .Count()} TS837 items from {fullPath}...");

            Save837P(edi837Data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message + (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }
    
    public void Save837P(IEnumerable<TS837> ediData)
    {
        _logger.LogInformation($"Comitting {ediData.Count()} edi data...");
        try
        {
            _context.TS837.AddRange(ediData);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to save edi data: " + e.Message + (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
        
        
    }
}