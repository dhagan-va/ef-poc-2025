using Amazon.S3;
using EDI837.Ingestion.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EDI837.Ingestion
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string source = args.Length > 0 ? args[0].ToUpperInvariant() : "S3"; // default to s3

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile(
                            "appsettings.Development.json",
                            optional: true,
                            reloadOnChange: true
                        )
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.Configure<AppSettings>(context.Configuration);
                        services.AddSingleton(sp =>
                            sp.GetRequiredService<IOptions<AppSettings>>().Value
                        );
                        services.AddDbContext<HIPAA_5010_837P_Context>(
                            (sp, options) =>
                            {
                                var settings = sp.GetRequiredService<AppSettings>();
                                options.UseSqlServer(
                                    settings.Database.ConnectionStrings.HIPAA_5010_837P
                                );
                            }
                        );
                        services.AddDbContext<ClaimStagingContext>(
                            (sp, options) =>
                            {
                                var settings = sp.GetRequiredService<AppSettings>();
                                options.UseSqlServer(
                                    settings.Database.ConnectionStrings.ClaimStaging
                                );
                            }
                        );
                        services.AddTransient<TransactionSaver>();
                        services.AddTransient<EdiParser>();

                        if (source == "S3")
                        {
                            services.AddSingleton<IAmazonS3>(sp =>
                            {
                                var appSettings = sp.GetRequiredService<
                                    IOptions<AppSettings>
                                >().Value;
                                var config = new AmazonS3Config
                                {
                                    ServiceURL = appSettings.S3.ServiceUrl,
                                    ForcePathStyle = true,
                                };
                                return new AmazonS3Client("test", "test", config);
                            });

                            services.AddSingleton<IS3GatewayFactory, S3GatewayFactory>();
                            services.AddHostedService<IngestionWorker>();
                        }
                    }
                )
                .Build();

            var settings = host.Services.GetRequiredService<AppSettings>();
            EdiFabric.SerialKey.Set(settings.EDI.Token);

            if (source == "LOCAL")
            {
                using var scope = host.Services.CreateScope();
                var parser = scope.ServiceProvider.GetRequiredService<EdiParser>();
                var saver = scope.ServiceProvider.GetRequiredService<TransactionSaver>();

                var filePath = "../../samples/837-sample-file.edi";
                var claims = parser.ParseEdiFileFromPath(filePath);
                await saver.SaveTransactionsAsync(claims);
            }
            else
            {
                await host.RunAsync();
            }
        }
    }
}
