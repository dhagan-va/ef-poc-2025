using EDI.Data;
using EDI.Services;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;

LoadEnvFile();

const string defaultConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=EDI;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"EDIProcessor\";Command Timeout=0";
const string defaultAwsServiceUrl = "http://localhost:5000";
const string defaultBucketName = "edi-837-bucket";
const string defaultEdiFabricSerialKey = "c417cb9dd9d54297a55c032a74c87996";

// Ensure required environment variables are set (fall back to defaults for local development)
var ediFabricSerialKey = Environment.GetEnvironmentVariable("EDIFABRIC_SERIAL_KEY") ?? defaultEdiFabricSerialKey;
Environment.SetEnvironmentVariable("EDIFABRIC_SERIAL_KEY", ediFabricSerialKey);

var awsServiceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL") ?? defaultAwsServiceUrl;
Environment.SetEnvironmentVariable("AWS_SERVICE_URL", awsServiceUrl);

var awsBucket = Environment.GetEnvironmentVariable("AWS_BUCKET") ?? defaultBucketName;
Environment.SetEnvironmentVariable("AWS_BUCKET", awsBucket);

var dbConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? defaultConnectionString;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<EdiDbContext>(options =>
            options.UseSqlServer(dbConnectionString));
        services.AddSingleton<IEdiValidationService, EdiValidationService>();
        services.AddScoped<IEdiProcessingService, EdiProcessingService>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var service = serviceProvider.GetRequiredService<IEdiProcessingService>();
    var dbContext = serviceProvider.GetRequiredService<EdiDbContext>();

    // Ensure database exists for ad-hoc runs
    dbContext.Database.EnsureCreated();

    // Process EDI files from S3 bucket
    var bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET");
    await service.ProcessEdiFilesFromS3Async(bucketName);

    Console.WriteLine("EDI ingestion from S3 completed.");

}

static void LoadEnvFile()
{
    var envPath = FindEnvPath();
    if (envPath is null)
    {
        return;
    }

    foreach (var line in File.ReadAllLines(envPath))
    {
        var trimmed = line.Trim();
        if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
        {
            continue;
        }

        var separatorIndex = trimmed.IndexOf('=', StringComparison.Ordinal);
        if (separatorIndex <= 0)
        {
            continue;
        }

        var key = trimmed[..separatorIndex].Trim();
        var value = trimmed[(separatorIndex + 1)..].Trim().Trim('"');

        if (!string.IsNullOrWhiteSpace(key))
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}

static string? FindEnvPath()
{
    var current = AppContext.BaseDirectory;
    while (!string.IsNullOrEmpty(current))
    {
        var candidate = Path.Combine(current, ".env");
        if (File.Exists(candidate))
        {
            return candidate;
        }

        current = Directory.GetParent(current)?.FullName;
    }

    return null;
}
