using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EDI837Ingestion.EF;
using EDI837Ingestion.BusinessLayer;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // try to read appsettings.json if present; fallback to environment or hardcoded connection string below
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var conn = context.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(conn))
        {
            conn = context.Configuration["ConnectionStrings:DefaultConnection"];
        }

        // If appsettings.json was not created yet, fall back to the provided connection string so the project still runs.
        if (string.IsNullOrEmpty(conn))
        {
            conn = "Server=Igor-Surface\\SQLEXPRESS;Database=PayerEDI;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conn));
        services.AddScoped<IEdi837IngestionService, Edi837IngestionService>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    //var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // simple sanity check: write how many payers exist
    //var count = db.Payers.Count();
    //Console.WriteLine($"Payers in DB: {count}");
    EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");
    var edi837Service = scope.ServiceProvider.GetRequiredService<IEdi837IngestionService>();
    await edi837Service.IngestEdi837();
}

// run the host if you need background services; otherwise exit
// await host.RunAsync();
