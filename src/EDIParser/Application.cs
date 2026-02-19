using EdiFabric.Core.Model.Edi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using EdiParser.Configuration;
using EdiParser.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EdiParser;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    private readonly EdiDBContext _context;
    private readonly IEdiReaderService _readerService;
    private readonly IEdiParserService _parserService;

    public Application(ILogger<Application> logger, IConfiguration configuration, EdiDBContext context, IEdiReaderService readerService, IEdiParserService parserService)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _readerService = readerService;
        _parserService = parserService;
        
    }

    public async Task Run(ValidationLevel? vl, bool s3Mode)
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
            Stream fileStream;
            string[] splitFilePath = configTestFilesPath.Split("/");
            string fileName = splitFilePath[splitFilePath.Length - 1];
            
            if (s3Mode)
            {
                var s3Configuration = _configuration.GetRequiredSection("S3Configuration").Get<S3Configuration>();
                string? s3Bucket = s3Configuration?.Bucket;
                using var cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                
                _logger.LogInformation($"Downloading {fileName} from S3 bucket {s3Bucket}...");
                fileStream = await _readerService.DownloadFileAsync(
                    String.IsNullOrEmpty(s3Bucket) ? "test-bucket" : s3Bucket,
                    fileName,
                    token);
            }
            else
            {
                _logger.LogInformation($"Reading {fileName} from  {configTestFilesPath}...");
                fileStream =  _readerService.GetFileStream(fullPath);
            }
            
           
            var edi837Data =  await _parserService.ParseX12FileAsync(fileStream, "EdiFabric.Templates.Hipaa");
            _logger.LogInformation($"Read {edi837Data.Count()} TS837 items from {fullPath}...");
            // Save asynchronously
            await Save837Async(edi837Data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message + (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }

    /// <summary>
    /// Synchronous version of the save
    /// </summary>
    /// <param name="ediData"></param>
    public void Save837(List<TS837P> ediData)
    {
        _logger.LogInformation($"Committing {ediData.Count} edi data...");
        try
        {
            _context.TS837P.AddRange(ediData);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to save edi data: " + e.Message +
                             (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }

    /// <summary>
    /// Asynchronous version of the method
    /// </summary>
    /// <param name="ediData"></param>
    public async Task Save837Async(IEnumerable<TS837P> ediData)
    {
        await Task.Run(() => _logger.LogInformation($"Comitting {ediData.Count()} edi data..."));
        try
        {
            await _context.TS837P.AddRangeAsync(ediData);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to save edi data: " + e.Message +
                             (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }


    
}