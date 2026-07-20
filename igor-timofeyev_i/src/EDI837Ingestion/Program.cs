using Amazon.Extensions.NETCore.Setup; // Required for .AddAWSService<T>()
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using EDI837Ingestion.BusinessLayer;
using EDI837Ingestion.EF;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Templates.X12004010;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

bool.TryParse(Environment.GetEnvironmentVariable("AWS__UseLocalMoto"), out bool parsed);
bool useLocalMoto = parsed;
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) => {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) => {
        var conn = Environment.GetEnvironmentVariable("Default_Connection");
        if (string.IsNullOrEmpty(conn))
        {
            conn = context.Configuration["ConnectionStrings:DefaultConnection"];
        }
        if (string.IsNullOrEmpty(conn))
        {
            conn = "Server=Igor-Surface\\SQLEXPRESS;Database=PayerEDI;Trusted_Connection=True;TrustServerCertificate=True;";
        }
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conn));
        services.AddScoped<IEdi837IngestionService, Edi837IngestionService>();

        if (useLocalMoto)
        {
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var credentials = new BasicAWSCredentials("mock_key", "mock_secret");
                var s3Config = new AmazonS3Config
                {
                    ServiceURL = Environment.GetEnvironmentVariable("AWS__MotoServiceUrl") ?? "http://localhost:5000",
                    ForcePathStyle = true // Critical for Moto
                };
                return new AmazonS3Client(credentials, s3Config);
            });
        }
        else
        {
            // Production AWS Configuration - Resolves correctly via AWSSDK.Extensions.NETCore.Setup
            services.AddAWSService<IAmazonS3>();
        }
    })
    .Build();

    using (var scope = host.Services.CreateScope())
    {
        EdiFabric.SerialKey.Set(Environment.GetEnvironmentVariable("EdiFabric_SerialKey"));

        // --- EXECUTE PIPELINE ---
        var edi837Service = scope.ServiceProvider.GetRequiredService<IEdi837IngestionService>();
        await edi837Service.IngestEdi837(useLocalMoto);
    }
