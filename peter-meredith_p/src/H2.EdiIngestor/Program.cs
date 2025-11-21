using H2.EdiIngestor;

using Microsoft.EntityFrameworkCore;

if (args.Length == 0 || string.IsNullOrEmpty(args[0]) || args[0].ToLower() == "--help")
{
    Console.WriteLine("dotnet run <837 filename>");
}

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

// Configure secrets and options
builder.Services.Configure<Secrets>(builder.Configuration.GetSection(nameof(Secrets)));

// Read and set EdiFabric serial key from configuration if present (do not hard-code secrets)
var serialKey = builder.Configuration["EdiFabric:SerialKey"];
if (!string.IsNullOrEmpty(serialKey))
{
    EdiFabric.SerialKey.Set(serialKey);
}

builder.Services.AddDbContext<EdiIngestorDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EdiIngestor"));
});

// Register parser and options using the options pattern
builder.Services.AddScoped<IX12N837Parser, X12N837Parser>();
builder.Services.Configure<EdiTestOptions>(options =>
{
    if (args == null || args.Length == 0)
        throw new InvalidOptionsException("A file argument is required");
    options.FileName = args[0];
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
