using Amazon.Runtime;
using Amazon.S3;
using Edi837Ingestion.Data;
using Edi837Ingestion.Edi;
using Edi837Ingestion.Services;
using Ef837Ingest.Edi;                 // your IFileSource / FileSource
using Ef837Ingest.Edi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var baseDir = AppContext.BaseDirectory;

// Build configuration
var config = new ConfigurationBuilder()
    .SetBasePath(baseDir)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

// EdiFabric license
var ediKey = config["EdiFabric:LicenseKey"];
if (string.IsNullOrWhiteSpace(ediKey))
    throw new Exception("Missing EdiFabric:LicenseKey in user secrets or environment.");
EdiFabric.SerialKey.Set(ediKey);

// mode flag
var isWatchMode = args.Any(a => string.Equals(a, "--watch", StringComparison.OrdinalIgnoreCase));

var reprocessArgIndex = Array.FindIndex(args, a => string.Equals(a, "--reprocess", StringComparison.OrdinalIgnoreCase));
string? reprocessControlNumber = null;
if (reprocessArgIndex >= 0 && reprocessArgIndex + 1 < args.Length)
{
    reprocessControlNumber = args[reprocessArgIndex + 1];
}

// Host
using var host = Host.CreateDefaultBuilder()
    .ConfigureLogging(lb =>
    {
        lb.ClearProviders();
        lb.AddSimpleConsole();
        lb.SetMinimumLevel(LogLevel.Information);
    })
    .ConfigureServices(services =>
    {
        // Db
        var connStr = config.GetConnectionString("X12")
            ?? throw new InvalidOperationException("Missing connection string 'X12'.");
        services.AddDbContext<Hipaa5010Context>(opt => opt.UseSqlServer(connStr));

        var serviceUrl = config["S3:ServiceURL"] ?? "http://localhost:4566";
        services.AddSingleton<IAmazonS3>(_ =>
            new AmazonS3Client(
                new BasicAWSCredentials(
                    Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? "test",
                    Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "test"),
                new AmazonS3Config
                {
                    ServiceURL = serviceUrl,
                    ForcePathStyle = true,
                    UseHttp = serviceUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                }));

        // App services
        services.AddScoped<IEdiIngestionService, EdiIngestionService>(); 
        services.AddScoped<IEdiReprocessService, EdiReprocessService>();


        // File source + queue
        services.AddSingleton<IFileSource, FileSource>();  
        services.AddSingleton<IngestionQueue>();

        services.Configure<S3Options>(config.GetSection("S3"));

        // Background poller only in --watch mode
        if (isWatchMode)
        {
            services.AddHostedService<IngestionWorker>();
        }
    })
    .Build();

// Ensure DB is migrated
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Hipaa5010Context>();
    await db.Database.MigrateAsync();
}

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Hipaa5010Context>();
    await db.Database.MigrateAsync();

    var s3Opts = scope.ServiceProvider.GetRequiredService<IOptions<S3Options>>().Value;
    Console.WriteLine($"S3 Options -> Bucket='{s3Opts.Bucket}', Inbound='{s3Opts.InboundPrefix}', Archive='{s3Opts.ArchivePrefix}', Poll={s3Opts.PollSeconds}s");
}

if (isWatchMode)
{
    Console.WriteLine("Starting S3 watch mode (polling + auto-ingest). Drop files into your bucket/prefix to process.");
    await host.RunAsync();
    return;
}

if (!string.IsNullOrWhiteSpace(reprocessControlNumber))
{
    using var scope = host.Services.CreateScope();
    var reprocess = scope.ServiceProvider.GetRequiredService<EdiReprocessService>();

    Console.WriteLine($"Reprocessing 837P ControlNumber={reprocessControlNumber}...");
    var result = await reprocess.Reprocess837PAsync(reprocessControlNumber);

    if (result is null)
    {
        Console.WriteLine("No transaction found.");
        Environment.ExitCode = 1;
        return;
    }

    Console.WriteLine($"Reprocess complete. IsValid={result.IsValid}, Level={result.Level}");
    return;
}


// One-shot mode (read arg path, else newest from S3, else local default)
await using var input = await ResolveInputAsync(args, host.Services, config, baseDir);
if (input == Stream.Null)
{
    Environment.ExitCode = 2;
    return;
}

using (var scope = host.Services.CreateScope())
{
    var svc = scope.ServiceProvider.GetRequiredService<IEdiIngestionService>();
    var count = await svc.IngestAsync(input);
    Console.WriteLine($"Saved {count} transaction(s).");
}

return;

// ---------- helpers ----------
static async Task<Stream> ResolveInputAsync(string[] args, IServiceProvider sp, IConfiguration config, string baseDir)
{
    var argPath = args.FirstOrDefault(a => !a.StartsWith("--"));
    if (!string.IsNullOrWhiteSpace(argPath) && File.Exists(argPath))
    {
        Console.WriteLine($"Parsing (local arg): {argPath}");
        return File.OpenRead(argPath);
    }

    // newest from S3
    var bucket = config["S3:Bucket"];
    var prefix = config["S3:InboundPrefix"];
    var serviceUrl = config["S3:ServiceURL"] ?? "http://localhost:4566";

    if (!string.IsNullOrWhiteSpace(bucket) && !string.IsNullOrWhiteSpace(prefix))
    {
        try
        {
            var s3 = sp.GetRequiredService<IAmazonS3>();
            var list = await s3.ListObjectsV2Async(new Amazon.S3.Model.ListObjectsV2Request
            {
                BucketName = bucket,
                Prefix = prefix
            });

            var objects = list?.S3Objects ?? new List<Amazon.S3.Model.S3Object>();
            var newest = objects
                .Where(o => !o.Key.EndsWith("/", StringComparison.Ordinal))
                .OrderByDescending(o => o.LastModified)
                .FirstOrDefault();

            if (newest != null)
            {
                Console.WriteLine($"Parsing (s3): s3://{bucket}/{newest.Key} via {serviceUrl}");
                var obj = await s3.GetObjectAsync(bucket, newest.Key);
                var ms = new MemoryStream();
                await obj.ResponseStream.CopyToAsync(ms);
                ms.Position = 0;
                return ms;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"S3 read failed: {ex.Message}. Falling back to local file.");
        }
    }

    // local default
    var localDefault = Path.GetFullPath(Path.Combine(baseDir, "Documents", "ClaimPaymentEVV.txt"));
    if (File.Exists(localDefault))
    {
        Console.WriteLine($"Parsing (local default): {localDefault}");
        return File.OpenRead(localDefault);
    }

    Console.Error.WriteLine($"No input found. Checked S3 and '{localDefault}'.");
    return Stream.Null;
}

