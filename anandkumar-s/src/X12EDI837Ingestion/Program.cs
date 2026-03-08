using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using X12EDI837Ingestion.Application.Extensions;
using X12EDI837Ingestion.Application.Interfaces;
using X12EDI837Ingestion.Domain;

public class Program
{
    public static async Task<int> Main(string[] args)
    {

        #region "Basic DOTNET and PM Console commands for this application"
        // To build the application:
        // dotnet build -c Release
        // To run the application with a file:
        // dotnet run --project src/X12EDI837Ingestion/X12EDI837Ingestion.csproj -- "path/to/your/edi/file.edi"
        //Create migration script from PM Console:
        // dotnet ef migrations add InitialCreate -p src/X12EDI837Ingestion -s src/X12EDI837Ingestion.Cli
        //Or using PM Console: Add-Migration InitialCreate -Context AppDbContext
        //Microsoft recommends IDesignTimeDbContextFactory for the console application
        // Add-Migration InitialCreate -Project X12EDI837Ingestion.Infrastructure -StartupProject X12EDI837Ingestion.Cli
        // Since it 's a console app, we can also run the migration command directly without needing to create a migration script first:
        // dotnet ef database update -p src/X12EDI837Ingestion -s src/X12EDI837Ingestion.Cli
        // Or using PM Console:
        // To apply migrations to the database:
        // dotnet ef database update -p src/X12EDI837Ingestion.Infrastructure -s src/X12EDI837Ingestion.Cli
        //Or using PM Console: Update-Database -Context AppDbContext
        // Update-Database -Project X12EDI837Ingestion
        // To remove database: dotnet ef database drop --force --project src/X12EDI837Ingestion
        #endregion

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

 
        
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddApplication();
                services.AddInfrastructure(ctx.Configuration);
                services.AddEdiFabric(ctx.Configuration);
                services.AddLogging(log => log.ClearProviders().AddConsole());
            })
            .Build();

       
        try
        {
            
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;

            var config = sp.GetRequiredService<IConfiguration>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Startup");

            logger.LogInformation(
            "Starting ingestion for file {FileName}",
                    Path.GetFileName(filePath));

            var runMigrations = config.GetValue<bool>("DB:RunMigrations");

            if (runMigrations)
            {
                logger.LogWarning("DB:RunMigrations=true -> applying migrations...");
                await ApplyMigrationsAsync(sp);
            }
            else
            {
                logger.LogInformation("DB migrations disabled (DB:RunMigrations=false).");
            }

            var ingestionService = sp.GetRequiredService<IX12EDI837IngestionService>();

            var stopwatch = Stopwatch.StartNew();
            await ingestionService.ProcessIngestionAsync(filePath);
            stopwatch.Stop();

            logger.LogInformation(
                "EDI X12 837P ingestion completed in {ElapsedMilliseconds} ms",
                stopwatch.ElapsedMilliseconds);

            logger.LogInformation("Ingestion completed successfully for file: {FilePath}", filePath);
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