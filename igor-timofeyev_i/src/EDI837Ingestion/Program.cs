using EDI837Ingestion.BusinessLayer;
using EDI837Ingestion.EF;
using EdiFabric.Templates.X12004010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // try to read appsettings.json if present; fallback to environment or hardcoded connection string below
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var conn = Environment.GetEnvironmentVariable("Default_Connection");
        //var conn = context.Configuration.GetConnectionString("DefaultConnection");
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
    #if DEBUG
        while (!System.Diagnostics.Debugger.IsAttached)
        {
            await Task.Delay(100);
        }
    #endif

    EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");
    var edi837Service = scope.ServiceProvider.GetRequiredService<IEdi837IngestionService>();
    await edi837Service.IngestEdi837();
}