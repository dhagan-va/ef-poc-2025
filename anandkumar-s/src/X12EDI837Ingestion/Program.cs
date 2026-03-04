using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using X12EDI837Ingestion.Application.Extensions;
using X12EDI837Ingestion.Application.Interfaces;
using X12EDI837Ingestion.Domain;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // 1) Validate input early
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

        try
        {
            
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;

            var config = sp.GetRequiredService<IConfiguration>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var startupLogger = loggerFactory.CreateLogger("Startup");

            var runMigrations = config.GetValue<bool>("DB:RunMigrations");

            if (runMigrations)
            {
                startupLogger.LogWarning("DB:RunMigrations=true -> applying migrations...");
                await ApplyMigrationsAsync(sp);
            }
            else
            {
                startupLogger.LogInformation("DB migrations disabled (DB:RunMigrations=false).");
            }

            var ingestionService = sp.GetRequiredService<IX12EDI837IngestionService>();
            await ingestionService.ProcessIngestionAsync(filePath);

            startupLogger.LogInformation("Ingestion completed successfully for file: {FilePath}", filePath);
            return 0;
        }
        catch (Exception ex)
        {
            // Use a logger even if ingestion scope failed
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ingestion failed for file: {FilePath}", filePath);
            return 2;
        }
        finally
        {
            // Optional (Dispose() will also stop it). Keeping is fine.
            await host.StopAsync();
        }
    }

    private static async Task ApplyMigrationsAsync(IServiceProvider scopedProvider)
    {
        var db = scopedProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}