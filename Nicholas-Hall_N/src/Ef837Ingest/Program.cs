using Amazon.Runtime;
using Amazon.S3;
using Edi837Ingestion.Data;
using Edi837Ingestion.Edi;
using Edi837Ingestion.Services;
using Ef837Ingest.Edi;                 // your IFileSource / FileSource
using Ef837Ingest.Edi.Models;
using Ef837Ingest.Data.Entities;       // <-- add for ClaimEntity / Ts837pEntity
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

// mode flags
var isWatchMode = args.Any(a => string.Equals(a, "--watch", StringComparison.OrdinalIgnoreCase));
var isDemoMode = args.Any(a => string.Equals(a, "--demo", StringComparison.OrdinalIgnoreCase));

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
        services.AddScoped<IEdiReprocessService, EdiReprocessService>();  // <-- register via interface

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

// ----- modes -----
if (isWatchMode)
{
    Console.WriteLine("Starting S3 watch mode (polling + auto-ingest). Drop files into your bucket/prefix to process.");
    await host.RunAsync();
    return;
}

if (!string.IsNullOrWhiteSpace(reprocessControlNumber))
{
    using var scope = host.Services.CreateScope();
    var reprocess = scope.ServiceProvider.GetRequiredService<IEdiReprocessService>();  // <-- use interface

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

// NEW: demo mode
if (isDemoMode)
{
    await RunDemoAsync(host.Services);
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

// ===== Demo helpers =====

static async Task RunDemoAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<Hipaa5010Context>();

    Console.WriteLine("=== EDI 837P Demo ===");
    Console.WriteLine("Showing the 10 most recent claims:\n");

    var claims = await db.Claims
        .OrderByDescending(c => c.Id)
        .Take(10)
        .ToListAsync();

    if (!claims.Any())
    {
        Console.WriteLine("No claims found. Run the app normally first to ingest a file.");
        return;
    }

    Console.WriteLine(" ID  | ST02 | Patient              | DOS        | Amount    | NPI");
    Console.WriteLine("-----+------+----------------------+------------+-----------+----------------");

    foreach (var c in claims)
    {
        var name = $"{c.PatientLastName}, {c.PatientFirstName}".Trim(',', ' ');
        if (name.Length > 22) name = name[..22];

        var dos = c.ServiceFromDate?.ToString("yyyy-MM-dd") ?? "n/a";
        var amt = c.TotalClaimAmount?.ToString("0.00") ?? "n/a";

        Console.WriteLine(
            $"{c.Id,4} | {c.TransactionSetControlNumber,-4} | {name,-22} | {dos,-10} | {amt,9} | {c.BillingProviderNpi}");
    }

    Console.WriteLine();
    Console.Write("Enter a TransactionSetControlNumber (ST02) to view details (or just Enter to exit): ");
    var st02 = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(st02))
        return;

    await ShowClaimDetailsAsync(db, st02.Trim());
}

static async Task ShowClaimDetailsAsync(Hipaa5010Context db, string controlNumber)
{
    var claim = await db.Claims
        .FirstOrDefaultAsync(c => c.TransactionSetControlNumber == controlNumber);

    if (claim == null)
    {
        Console.WriteLine($"No claim found for ST02='{controlNumber}'.");
        return;
    }

    var issues = await db.ValidationIssues
        .Where(v => v.Transaction == "837P" && v.ControlNumber == controlNumber)
        .OrderBy(v => v.Id)
        .ToListAsync();

    var raw = await db.TS837P
        .FirstOrDefaultAsync(t => t.ControlNumber == controlNumber);

    Console.WriteLine();
    Console.WriteLine("=== Claim Header ===");
    Console.WriteLine($"ST02 (Control #): {claim.TransactionSetControlNumber}");
    Console.WriteLine($"Patient Control #: {claim.PatientControlNumber}");
    Console.WriteLine($"Patient Name     : {claim.PatientLastName}, {claim.PatientFirstName}");
    Console.WriteLine($"Service From Date: {claim.ServiceFromDate:yyyy-MM-dd}");
    Console.WriteLine($"Total Amount     : {claim.TotalClaimAmount:0.00}");
    Console.WriteLine($"Billing NPI      : {claim.BillingProviderNpi}");
    Console.WriteLine();

    Console.WriteLine("=== SNIP / Validation Issues ===");
    if (!issues.Any())
    {
        Console.WriteLine("No validation issues recorded for this transaction.");
    }
    else
    {
        foreach (var i in issues)
        {
            Console.WriteLine($"- Segment={i.SegmentId}, Pos={i.Position}, Code={i.Code}");
            Console.WriteLine($"  Message: {i.Message}");
        }
    }

    if (raw != null)
    {
        Console.WriteLine();
        Console.Write("Show raw TS837P JSON? (y/N): ");
        var key = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(key) && key.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine();
            Console.WriteLine("=== Raw TS837P JSON ===");
            Console.WriteLine(raw.RawJson);
        }
    }
}
