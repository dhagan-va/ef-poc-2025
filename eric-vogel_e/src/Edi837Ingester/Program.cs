// See https://aka.ms/new-console-template for more information

using Edi837Ingester.Data;
using Edi837Ingester.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");

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