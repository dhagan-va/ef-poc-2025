using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using X12EDI837Ingestion.Application.Extensions;
using X12EDI837Ingestion.Application.Interfaces;
using X12EDI837Ingestion.Consumer.Application.Extensions;
using X12EDI837Ingestion.Consumer.Application.Interfaces;
using X12EDI837Ingestion.Consumer.Extensions;
using X12EDI837Ingestion.Domain;

public partial class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddApplication();
                services.AddInfrastructure(ctx.Configuration);
                services.AddEdiFabric(ctx.Configuration);
                services.AddSnipValidators(ctx.Configuration);
                services.AddConsumerServices(ctx.Configuration);
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

            var s3StorageService = sp.GetRequiredService<IS3StorageService>();
            var ingestionService = sp.GetRequiredService<IX12EDI837IngestionService>();

            var objectKeys = await s3StorageService.ListObjectKeysAsync();

            if (objectKeys.Count == 0)
            {
                logger.LogWarning("No files found in configured S3 bucket.");
                return 0;
            }

            logger.LogInformation(
                "Found {FileCount} file(s) in S3 bucket for ingestion.",
                objectKeys.Count);

            var stopwatch = Stopwatch.StartNew();

            foreach (var objectKey in objectKeys)
            {
                logger.LogInformation(
                    "Starting ingestion for S3 object {ObjectKey}",
                    objectKey);

                await using var stream = await s3StorageService.DownloadObjectAsync(objectKey);

                await ingestionService.ProcessIngestionAsync(stream, objectKey);

                logger.LogInformation(
                    "Completed ingestion for S3 object {ObjectKey}",
                    objectKey);
            }

            stopwatch.Stop();

            logger.LogInformation(
                "EDI X12 837P ingestion completed in {ElapsedMilliseconds} ms",
                stopwatch.ElapsedMilliseconds);

            return 0;
        }
        catch (Exception ex)
        {
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ingestion failed.");
            return 2;
        }
        finally
        {
            await host.StopAsync();
        }
    }

    private static async Task ApplyMigrationsAsync(IServiceProvider scopedProvider)
    {
        var db = scopedProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}