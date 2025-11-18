using EDI.Data;
using EDI.Services;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

// Set required environment variable for EdiFabric
Environment.SetEnvironmentVariable("EDIFABRIC_SERIAL_KEY", "c417cb9dd9d54297a55c032a74c87996");
// Set environment variable for AWS service URL (moto server)
Environment.SetEnvironmentVariable("AWS_SERVICE_URL", "http://localhost:5000");
Environment.SetEnvironmentVariable("AWS_BUCKET", "edi-837-bucket");


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<EdiDbContext>(options =>
            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=EDI;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"EDIProcessor\";Command Timeout=0"));
        services.AddSingleton<IEdiValidationService, EdiValidationService>();
        services.AddScoped<IEdiProcessingService, EdiProcessingService>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var service = serviceProvider.GetRequiredService<IEdiProcessingService>();
    var dbContext = serviceProvider.GetRequiredService<EdiDbContext>();

    // Ensure database exists for ad-hoc runs
    dbContext.Database.EnsureCreated();

    // Process EDI files from S3 bucket
    var bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET");
    await service.ProcessEdiFilesFromS3Async(bucketName);

    Console.WriteLine("EDI ingestion from S3 completed.");

}
