using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Config;
using X12EDI.Core.Extensions;
using X12EDI.Data.Extensions;

/// <summary>
/// Represents the main entry point for the X12 EDI Command Line Interface (CLI) application.
/// This class is responsible for configuring and bootstrapping the application, setting up dependency injection,
/// logging, and application-specific services.
/// </summary>
public class Program
{
    #region Private Methods

    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Builds the application configuration from various sources.
    /// 2. Configures the dependency injection container with necessary services, including logging, EDI services, S3 storage, and the database context.
    /// 3. Builds the service provider.
    /// 4. Resolves the <see cref="IFileIngestionService"/> and triggers the file ingestion process.
    /// </remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private static async Task Main()
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

    #endregion Private Methods
}