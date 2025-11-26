// See https://aka.ms/new-console-template for more information

using Amazon.S3;
using DotNetEnv;
using Edi837Ingester.Configuration;
using Edi837Ingester.Data;
using Edi837Ingester.Data.Repositories;
using Edi837Ingester.Services;
using EdiFabric.Core.Model.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

string? s3bucket = null;

// Load environment variables from .env file early so Host configuration can pick them up
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine("✓ Loaded configuration from .env file");
}

string? editSerialKey = null;
string? s3AccessKeyId = null;
string? s3SecretAccessKey = null;

editSerialKey = Env.GetString("TRIAL_EDIFABRIC_LICENSE");
s3AccessKeyId = Env.GetString("S3_ACCESS_KEY_ID");
s3SecretAccessKey = Env.GetString("S3_SECRET_ACCESS_KEY");

if (string.IsNullOrWhiteSpace(editSerialKey))
{
    Console.WriteLine("EDI Fabric serial key is not set. Please set the TRIAL_EDIFABRIC_LICENSE environment variable or use --license command.");
    return;
}

if(string.IsNullOrWhiteSpace(s3AccessKeyId) || string.IsNullOrWhiteSpace(s3SecretAccessKey))
{
    Console.WriteLine("S3 credentials are not set. Please set the S3_ACCESS_KEY_ID and S3_SECRET_ACCESS_KEY environment variables.");
    return;
}

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Get connection string from appsettings.json (automatically loaded by CreateDefaultBuilder)
        string? connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)); // Use your provider

        // Register other services/classes that will use the DbContext
        services.AddTransient<IEdiParserService, EdiParserService>();
        services.AddTransient<IEdiRepository, EdiRepository>();
        services.AddTransient<IS3Service, S3Service>();
        services.AddTransient<IS3EdiParserService, S3EdiParserService>();

        var s3Configuration = context.Configuration.GetRequiredSection("S3")
            .Get<S3Configuration>();
        s3bucket = s3Configuration?.Bucket;

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = s3Configuration?.ServiceUrl,
                ForcePathStyle = true,
            };
            return new AmazonS3Client(s3AccessKeyId, s3SecretAccessKey, config);
        });
    })
    .Build();


// Check if --license argument is provided
for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--license" && i + 1 < args.Length)
    {
        editSerialKey = args[i + 1];
        break;
    }
}

EdiFabric.SerialKey.Set(editSerialKey);

// Default validation level
ValidationLevel validationLevel = ValidationLevel.SyntaxOnly_SNIP1;

// Parse validation level from command line arguments --validation or --validation-level <value>
// Accepts enum names (case-insensitive) or integer values
for (int i = 0; i < args.Length; i++)
{
    if ((args[i].Equals("--validation", StringComparison.OrdinalIgnoreCase) ||
         args[i].Equals("--validation-level", StringComparison.OrdinalIgnoreCase)) && i + 1 < args.Length)
    {
        var val = args[i + 1];
        if (Enum.TryParse<ValidationLevel>(val, ignoreCase: true, out var parsed))
        {
            validationLevel = parsed;
            Console.WriteLine($"Using validation level: {validationLevel}");
        }
        else if (int.TryParse(val, out var intVal) && Enum.IsDefined(typeof(ValidationLevel), intVal))
        {
            validationLevel = (ValidationLevel)intVal;
            Console.WriteLine($"Using validation level (numeric): {validationLevel}");
        }
        else
        {
            Console.WriteLine($"Warning: Unknown validation level '{val}'. Falling back to default: {validationLevel}");
        }
        break;
    }
}

var validationLevelLocal = validationLevel; // capture for closures if needed

// get S3 path from command line arguments
string? s3Path = null;
for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--s3" && i + 1 < args.Length)
    {
        s3Path = args[i + 1];
        break;
    }
}

string? path = null;

// Check if --file argument is provided
for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--file" && i + 1 < args.Length)
    {
        path = args[i + 1];
        break;
    }
}

if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(s3Path))
{
    Console.WriteLine("Would you like to load an EDI from S3? (y/n)");
    var response = Console.ReadLine();
    if (response != null && response.ToLower() == "y")
    {
        Console.WriteLine("Enter the S3 path to the EDI 837 file:");
        s3Path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(s3Path))
        {
            Console.WriteLine("S3 path cannot be empty or whitespace. Exiting.");
            return;
        }
    }
    else if (response != null && response.ToLower() == "n")
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Enter the path to the EDI 837 file:");
            path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Path cannot be empty or whitespace. Exiting.");
                return;
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid response. Exiting.");
        return;
    }
}

using (var scope = host.Services.CreateScope())
{
    try
    {
        if (!string.IsNullOrWhiteSpace(s3Path))
        {
            Console.WriteLine("Downloading EDI 837 file from S3...");
            var s3Service = scope.ServiceProvider.GetRequiredService<IS3EdiParserService>();
            await s3Service.ParseFromS3Async(s3bucket!, s3Path, validationLevelLocal);
        }
        else if (!string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine($"Parsing EDI 837 file: {path}");
            var parser = scope.ServiceProvider.GetRequiredService<IEdiParserService>();
            await parser.Parse(path, validationLevelLocal);
        }
    } catch(Exception ex)
    {
        Console.WriteLine($"An error occurred while parsing the EDI 837 file: {ex.Message}");
        return;
    }
}

Console.WriteLine("Done parsing the EDI 837 file.");