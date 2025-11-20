using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Config;
using X12EDI.Core.Extensions;
using X12EDI.Data.Extensions;

public class Program
{
    static async Task Main()
    {
        // 1. Build configuration (JSON + environment variables + command line)
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
            .Build();


        var services = new ServiceCollection();

        // 2. Make IConfiguration available via DI
        services.AddSingleton<IConfiguration>(configuration);

        // 3. Logging
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });

        // 4. EDI service registration from .Core, using config
        services.AddEDIServices(options =>
        {
            // pull from config first, fall back to environment variable
            options.SerialKey = string.IsNullOrEmpty(configuration["EdiOptions:SerialKey"]) ? Environment.GetEnvironmentVariable("EDIKEY") : configuration["EdiOptions:SerialKey"];

            options.FolderPath = configuration["EdiOptions:FolderPath"];
        });

        // Register the S3 services, pointing the IAmazonS3 client to the local Blazor API.
        services.AddS3(options =>
        {
            configuration.GetSection("S3Options").Bind(options);
            options.ForcePathStyle = true;
        });

        // 5. Database context registration from .Data
        services.AddX12EdiData(configuration);

        // Build and use IServiceProvider
        using var provider = services.BuildServiceProvider();

        // test logging
        var logger = provider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("CLI harness bootstrapped.");

        // Verify serial key
        var ediOptions = provider.GetRequiredService<EdiOptions>();
        logger.LogInformation($"EdiFabric serial key {(string.IsNullOrEmpty(ediOptions.SerialKey) ? "not" : "is")} set.");

        // Resolve and run
        var fileIngestionService = provider.GetRequiredService<IFileIngestionService>();
        await fileIngestionService.IngestAllAsync(CancellationToken.None);
    }
}