using Edi837Ingestion.Data;
using Edi837Ingestion.Edi;              
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var envBase = AppContext.BaseDirectory;
var appSettingsPath = Path.Combine(envBase, "appsettings.json");
var config = new ConfigurationBuilder()
    .AddJsonFile(appSettingsPath, optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.AddDbContext<IngestionDbContext>(opt =>
    opt.UseSqlServer(config.GetConnectionString("Default")));
services.AddLogging();
services.AddScoped<EdiIngestionService>();

var sp = services.BuildServiceProvider();

using (var scope = sp.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IngestionDbContext>();
    await db.Database.MigrateAsync();
}

var inputPath = args.FirstOrDefault()
    ?? Path.GetFullPath(Path.Combine(envBase, "..", "..", "..", "samples", "sample-837p.txt"));

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Sample file not found: {inputPath}");
    return 2;
}

Console.WriteLine($"Parsing: {inputPath}");

using (var scope = sp.CreateScope())
{
    var svc = scope.ServiceProvider.GetRequiredService<EdiIngestionService>();
    await using var fs = File.OpenRead(inputPath);
    var count = await svc.Ingest837Async(fs);
    Console.WriteLine($"Ingested {count} transaction(s).");
}

return 0;
