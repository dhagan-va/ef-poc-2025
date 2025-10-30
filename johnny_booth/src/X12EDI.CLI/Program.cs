using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Extensions;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // Logging
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });

        // EDI service registration from .Core
        services.AddEDIServices(options =>
        {
            options.SerialKey = Environment.GetEnvironmentVariable("EDIKEY");
        });

        using var provider = services.BuildServiceProvider();

        // test logging
        var logger = provider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("CLI harness bootstrapped.");


        // Verify serial key
        var ediOptions = provider.GetRequiredService<EdiOptions>();
        logger.LogInformation($"EdiFabric serial key {(string.IsNullOrEmpty(ediOptions.SerialKey)?"not":"")} set.");


        // Resolve and run parser
        var parser = provider.GetRequiredService<IX12ParserService>();
        await parser.RunAsync(args, CancellationToken.None);
    }
}