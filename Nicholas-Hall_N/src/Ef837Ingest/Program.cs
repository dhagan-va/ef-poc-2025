using Edi837Ingestion.Data;
using Edi837Ingestion.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var baseDir = AppContext.BaseDirectory;

var config = new ConfigurationBuilder()
    .SetBasePath(baseDir)
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>(optional: true)  
    .AddEnvironmentVariables()
    .Build();

var ediKey = config["EdiFabric:LicenseKey"];
if (!string.IsNullOrWhiteSpace(ediKey))
{
    EdiFabric.SerialKey.Set(ediKey);
}
else
{
    throw new Exception("EdiFabric License Key is missing. Add it to user secrets under 'EdiFabric:LicenseKey'.");
}

var services = new ServiceCollection();

// Logging
services.AddLogging(b => b.AddSimpleConsole().SetMinimumLevel(LogLevel.Information));

// Connection string check
var connStr = config.GetConnectionString("X12");
if (string.IsNullOrWhiteSpace(connStr))
{
    throw new InvalidOperationException(
        "Missing connection string 'X12'. Add it to appsettings.json or environment variables.");
}

// DbContext
services.AddDbContext<Hipaa5010Context>(opt => opt.UseSqlServer(connStr));

// Services
services.AddScoped<IEdiIngestionService, EdiIngestionService>();

var sp = services.BuildServiceProvider();

// Migrate DB
using (var scope = sp.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Hipaa5010Context>();
    await db.Database.MigrateAsync();
}

var inputPath = args.FirstOrDefault()
    ?? Path.GetFullPath(Path.Combine(baseDir, "Documents", "ClaimPaymentEVV.txt"));

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Missing EDI file: {inputPath}");
    return 2;
}

Console.WriteLine($"Parsing: {inputPath}");

using (var scope = sp.CreateScope())
{
    var svc = scope.ServiceProvider.GetRequiredService<IEdiIngestionService>();
    await using var fs = File.OpenRead(inputPath);
    var count = await svc.IngestAsync(fs);
    Console.WriteLine($"Saved {count} transaction(s).");
}

return 0;
