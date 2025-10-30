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
            options.SerialKey = "your-serial-key-here";
            // Add more config if needed
        });

        using var provider = services.BuildServiceProvider();

        // Optional: test logging
        var logger = provider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("CLI harness bootstrapped.");

        // Resolve and run your parser
        var parser = provider.GetRequiredService<IX12ParserService>();
        await parser.RunAsync(args, CancellationToken.None);
    }
}