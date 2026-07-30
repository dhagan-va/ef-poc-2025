using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Deepika.EDIIngestion.Data;
using Deepika.EDIIngestion.Services;
using Serilog;

namespace Deepika.EDIIngestion
{
    #if !TEST_PROJECT
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var conn = config.GetConnectionString("DefaultConnection") ?? "Server=localhost;Database=DeepikaEdiDb;Trusted_Connection=True;TrustServerCertificate=True;";

            // Initialize EdiFabric license once at startup:
            var license = Environment.GetEnvironmentVariable("TRIAL_EDIFABRIC_LICENSE")
                          ?? Environment.GetEnvironmentVariable("EDIFABRIC_LICENSE");
            if (!string.IsNullOrWhiteSpace(license))
            {
                try
                {
                    EdiFabric.SerialKey.Set(license);
                    Log.Information("EdiFabric license set from environment.");
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Failed to set EdiFabric license");
                }
            }

            // Ensure local development always includes TrustServerCertificate to avoid SSL trust failures
            if (!string.IsNullOrWhiteSpace(conn) && !conn.Contains("TrustServerCertificate", StringComparison.OrdinalIgnoreCase))
            {
                if (!conn.EndsWith(";")) conn += ";";
                conn += "TrustServerCertificate=True;";
            }

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));

            // Configure Serilog for file logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/edi-ingestion-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddLogging(builder => builder.AddSerilog());

            services.AddSingleton<IEdiFabricValidatorService, EdiFabricValidatorService>();
            services.AddTransient<IEdiFabricParser, EdiFabricParser>();
            services.AddTransient<IEdiIngestionService, EdiIngestionService>();
            var provider = services.BuildServiceProvider(); // noop: update context

            Log.Information("Using connection: {Connection}", conn);

            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                var ingestion = scope.ServiceProvider.GetRequiredService<IEdiIngestionService>();
                ingestion.ProcessFolder("samples");
            }
        }
    }
    #endif
}
