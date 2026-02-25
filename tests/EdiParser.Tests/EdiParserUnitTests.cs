using Amazon.S3;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using EdiParser.Configuration;
using EdiParser.Entities;
using EdiParser.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EdiParser.Tests;


public class EdiParserUnitTests
{
    private readonly ILogger<Program> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEdiParserService _parserService;
    private readonly IEdiReaderService _readerService;
    private readonly IS3FileReaderService _s3FileReaderService;
    private readonly IEdiValidatorService _validatorService;
    private readonly IDatabaseService _databaseService;
    
    public EdiParserUnitTests()
    {
        // Load configuration from appsettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build();

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            
        // Set up DI container
        var serviceCollection = new ServiceCollection();
        // Register DI 
        serviceCollection.AddLogging(loggingBuilder =>
        {
            // Add Serilog as a logging provider
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddTransient<IEdiParserService, Services.EdiParserService>();
        serviceCollection.AddTransient<IEdiReaderService, Services.EdiReaderService>();
        serviceCollection.AddTransient<IEdiValidatorService, Services.EdiValidatorService>();
        serviceCollection.AddTransient<IS3FileReaderService, Services.S3FileReaderService>();
        serviceCollection.AddSingleton<IEdiDBContext, EdiDBContext>();
        serviceCollection.AddTransient<IDatabaseService, DatabaseService>();
        // Get S3 configuration information and initialize bucket
        var s3Configuration = configuration.GetRequiredSection("S3Configuration")
            .Get<S3Configuration>();
            
        // Initialize AWS client
        serviceCollection.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = s3Configuration?.ServiceUrl,
                ForcePathStyle = true,
            };
            return new AmazonS3Client(s3Configuration?.s3AccessKeyId, s3Configuration?.s3SecretAccessKey, config);
        });
    
        // Iniitialize configuration, logger and parser interfaces
        var provider = serviceCollection.BuildServiceProvider();
        _logger = provider.GetRequiredService<ILogger<Program>>();
        _readerService = provider.GetRequiredService<IEdiReaderService>();
        _s3FileReaderService = provider.GetRequiredService<IS3FileReaderService>();
        _parserService = provider.GetRequiredService<IEdiParserService>();
        _configuration = provider.GetRequiredService<IConfiguration>();
        _validatorService = provider.GetRequiredService<IEdiValidatorService>();
        _databaseService = provider.GetRequiredService<IDatabaseService>();
        
    }

    [Fact]
    public async Task EdiParser_ShouldParseSuccessfully()
    {
        // Set up path to the test file
        var sampleFilePath = Path.Combine("..", "..", "..", "..","..", "samples", "ClaimPayment.edi");
        var absolutePath = Path.GetFullPath(sampleFilePath);
        Log.Logger.Information($"Using sample file: {absolutePath}");
       
        // Make sure file exists
        Assert.True(File.Exists(absolutePath), $"Sample file not found: {absolutePath}");
        
        Log.Logger.Information($"Testing normal edi parsing");
        // Parse the file
        var fileStream = _readerService.GetFileStream(sampleFilePath);
        var edi837Transactions = _parserService.ParseX12File<EdiMessage>(fileStream, "EdiFabric.Templates.Hipaa");
        // Make sure parsed succesfully
        Assert.NotNull(edi837Transactions);
        Log.Logger.Information($"Read {edi837Transactions.Count()} TS837 items from {sampleFilePath}...");
        Log.Logger.Information($"Testing transaction validation");
        // Figure out claim type
        var claimType = 
            edi837Transactions.Any(x => x is TS837P) ? ClaimTypeEnum.Professional :
            edi837Transactions.Any(x => x is TS837I) ? ClaimTypeEnum.Institutional :
            edi837Transactions.Any(x => x is TS837D) ? ClaimTypeEnum.Dental : 
            ClaimTypeEnum.Unknown;
        Assert.NotEqual(ClaimTypeEnum.Unknown, claimType);
        Log.Logger.Information($"Testing SNIP1");
        ValidationLevel vl = ValidationLevel.SyntaxOnly_SNIP1;
        // Validate transactions
        var validatedTransactions = await _validatorService.ValidateItems(edi837Transactions, claimType, vl);
        Assert.NotNull(validatedTransactions);
        Log.Logger.Information($"Testing Save");
        var exception = await Record.ExceptionAsync(async () => await _databaseService.Save837Async(validatedTransactions, claimType));
        Assert.Null(exception);
        Log.Logger.Information($"\n✓ Test completed successfully");
        
    }
}