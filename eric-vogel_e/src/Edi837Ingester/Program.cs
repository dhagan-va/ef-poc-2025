// See https://aka.ms/new-console-template for more information

using DotNetEnv;
using Edi837Ingester.Data;
using Edi837Ingester.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Load environment variables from .env file
// Load .env file if it exists
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine("✓ Loaded configuration from .env file");
}

var editSerialKey = Env.GetString("TRIAL_EDIFABRIC_LICENSE");

if(string.IsNullOrWhiteSpace(editSerialKey))
{
    Console.WriteLine("EDI Fabric serial key is not set. Please set the TRIAL_EDIFABRIC_LICENSE environment variable.");
    return;
}

EdiFabric.SerialKey.Set(editSerialKey);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Get connection string from appsettings.json (automatically loaded by CreateDefaultBuilder)
        string? connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)); // Use your provider

        // Register other services/classes that will use the DbContext
        services.AddTransient<IEdiParser, EdiParser>();
        services.AddTransient<IEdiSaverService, EdiSaverService>();
    })
    .Build();

Console.WriteLine("Enter the path to the EDI 837 file:");
var path = Console.ReadLine();

if(string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("Path cannot be empty or whitespace. Exiting.");
    return;
}

Console.WriteLine($"Parsing EDI 837 file: {path}");

using (var scope = host.Services.CreateScope())
{
    try
    {
        var parser = scope.ServiceProvider.GetRequiredService<IEdiParser>();
        await parser.Parse(path);
    } catch(Exception ex)
    {
        Console.WriteLine($"An error occurred while parsing the EDI 837 file: {ex.Message}");
        return;
    }
}

Console.WriteLine("Done parsing the EDI 837 file.");