using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.X12004010;
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

    
        // Iniitialize configuration, logger and parser interfaces
        var provider = serviceCollection.BuildServiceProvider();
        _logger = provider.GetRequiredService<ILogger<Program>>();
        _readerService = provider.GetRequiredService<IEdiReaderService>();
        _parserService = provider.GetRequiredService<IEdiParserService>();
        _configuration = provider.GetRequiredService<IConfiguration>();
    }

    [Fact]
    public void EdiParser_ShouldParseSuccessfully()
    {
        // Set up path to the test file
        var sampleFilePath = Path.Combine("..", "..", "..", "..","..", "samples", "837File.edi");
        var absolutePath = Path.GetFullPath(sampleFilePath);
        Log.Logger.Information($"Using sample file: {absolutePath}");
       
        // Make sure file exists
        Assert.True(File.Exists(absolutePath), $"Sample file not found: {absolutePath}");
        
        // Parse the file
        var fileStream = _readerService.GetFileStream(sampleFilePath);
        var edi837Data = _parserService.ParseX12File(fileStream, "EdiFabric.Templates.X12");
        // Make sure parsed succesfully
        Assert.NotNull(edi837Data);
        Log.Logger.Information($"Read {edi837Data.Count()} TS837 items from {sampleFilePath}...");
        
        Log.Logger.Information($"\n✓ Test completed successfully");
        
    }
}