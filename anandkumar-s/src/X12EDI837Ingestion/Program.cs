using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using X12EDI837Ingestion.Application.Extensions;
using Microsoft.Extensions.Logging;
using EdiFabric.Core.Model.Edi.X12;
using Microsoft.EntityFrameworkCore;
using X12EDI837Ingestion.Domain;
using Microsoft.Extensions.Configuration;
using X12EDI837Ingestion.Infrastructure.Repositories;
using static X12EDI837Ingestion.Infrastructure.Repositories.X12EDI837IngestRepo;


public class Program
{
    public static async Task<int> Main(string[] args)
    {

        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: EdiIngestor.Cli <path-to-edi-file>");
            return -1;
        }
        if (!File.Exists(args[0]))
        {
            Console.Error.WriteLine($"File not found: {args[0]}");
            return -1;
        }
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddApplication();
                services.AddInfrastructure(ctx.Configuration);
                services.AddEdiFabric(ctx.Configuration);
            })
            .Build();



        //await host.Services
        //          .GetRequiredService<IIngestionService>()
        //          .ProcessAsync(args);

        return 0;

    }
}
 