using EdiFabric.Core.Model.Edi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using EdiParser.Configuration;
using EdiParser.Entities;
using EdiParser.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EdiParser;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEdiReaderService _readerService;
    private readonly IEdiParserService _parserService;
    private readonly IEdiValidatorService _validatorService;
    private readonly IDatabaseService _databaseService;

    public Application(ILogger<Application> logger, IConfiguration configuration, IEdiReaderService readerService, 
        IEdiParserService parserService, IEdiValidatorService validatorService, IDatabaseService databaseService)
    {
        _logger = logger;
        _configuration = configuration;
        _readerService = readerService;
        _parserService = parserService;
        _validatorService = validatorService;
        _databaseService = databaseService;
    }

    public async Task Run(ValidationLevel vl, bool s3Mode)
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
            
           
            var edi837Transactions =  await _parserService.ParseX12FileAsync<EdiMessage>(fileStream, "EdiFabric.Templates.Hipaa");
            _logger.LogInformation($"Read {edi837Transactions.Count()} TS837 items from {fullPath}...");
            // Figure out claim type
            var claimType = 
                edi837Transactions.Any(x => x is TS837P) ? ClaimTypeEnum.Professional :
                edi837Transactions.Any(x => x is TS837I) ? ClaimTypeEnum.Institutional :
                edi837Transactions.Any(x => x is TS837D) ? ClaimTypeEnum.Dental : 
                ClaimTypeEnum.Unknown;
            // Validate transactions
            var validatedTransactions = await _validatorService.ValidateItems(edi837Transactions, claimType, vl);
            // Save asynchronously
            await _databaseService.Save837Async(validatedTransactions, claimType);
            
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message + (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }
}