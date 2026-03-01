using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using X12EDI837Ingestion.Application.Extensions;
using X12EDI837Ingestion.Application.Interfaces;
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // 1) Validate input early (before building DI container)
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: X12EDI837Ingestion.Cli <path-to-edi-file>");
            return 1;
        }

        var filePath = args[0];

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return 1;
        }

        // 2) Build Host + DI
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddApplication();
                services.AddInfrastructure(ctx.Configuration);
                services.AddEdiFabric(ctx.Configuration);
            })
            .Build();

       
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            await host.Services
                .GetRequiredService<IX12EDI837IngestionService>()
                .ProcessIngestionAsync(filePath);

            logger.LogInformation("Ingestion completed successfully for file: {FilePath}", filePath);
            return 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ingestion failed for file: {FilePath}", filePath);
            return 2;
        }
        finally
        {
           
            await host.StopAsync();
        }
    }
}