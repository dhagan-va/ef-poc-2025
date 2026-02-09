using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.X12004010;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EdiParser.Tests;

public class EdiParserUnitTests
{
    [Fact]
    public void EdiParser_ShouldParseSuccessfully()
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
        serviceCollection.AddTransient<IEdiParser, EdiParser>();

    
        // Iniitialize configuration, logger and parser interfaces
        var provider = serviceCollection.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<Program>>();
        var parser = provider.GetRequiredService<IEdiParser>();

        
        // Set up path to the test file
        var sampleFilePath = Path.Combine("..", "..", "..", "..","..", "samples", "837File.edi");
        var absolutePath = Path.GetFullPath(sampleFilePath);
        Log.Logger.Information($"Using sample file: {absolutePath}");
       
        // Make sure file exists
        Assert.True(File.Exists(absolutePath), $"Sample file not found: {absolutePath}");
        
        // Parse the file
        List<IEdiItem> ediItems = parser.ParseX12File(sampleFilePath, "EdiFabric.Templates.X12");
        var edi837Data = ediItems.OfType<TS837>();
        // Make sure parsed succesfully
        Assert.NotNull(edi837Data);
        Log.Logger.Information($"Read {edi837Data.Count()} TS837 items from {sampleFilePath}...");
        
        Log.Logger.Information($"\n✓ Test completed successfully");
        
    }
}