using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.S3;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PayerEDI.Data.Database;
using PayerEDI.Data.Database.Repositories;
using PayerEDI.Data.Helpers;
using PayerEDI.Data.Models;
using PayerEDI.Data.Services;
using PayerEDI.Processor.Console.Command;
using PayerEDI.Processor.Console.Services;
using Serilog;

DotNetEnv.Env.Load(); // look for a .env file and if it exists load environment variables from that.  Good for local dev

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
};

await Parser
    .Default.ParseArguments<CliOptions>(args)
    .WithNotParsed(errors =>
    {
        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }
    })
    .WithParsedAsync(async options =>
    {
        using var app = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(
                (hostingContext, config) =>
                {
                    config
                        .AddJsonFile(
                            "appsettings.all.example.json",
                            optional: true,
                            reloadOnChange: false
                        )
                        .AddJsonFile(
                            $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.example.json",
                            optional: true,
                            reloadOnChange: false
                        );
                    config.AddEnvironmentVariables("EDI_PROCESSOR_");
                }
            )
            .ConfigureServices(
                (context, services) =>
                {
                    services.AddSingleton<IAmazonS3, AmazonS3Client>(); // will use the default constructor and follow the credential resolution rules of env vars and then ~/.aws
                    // Register concrete loader types so selecting one loader does not resolve both implementations.
                    // The interface remains the consumption contract; AddSingleton<T>() registers T as its own service type.
                    services.AddSingleton<LocalSystemFileLoader>();
                    services.AddSingleton<S3FileLoader>();
                    services.AddSingleton<IEdiProcessor, EdiFabricEdiProcessor>();

                    if (!options.Save)
                    {
                        return;
                    }

                    var connectionString =
                        context.Configuration.GetConnectionString("Default")
                        ?? throw new InvalidOperationException(
                            "The default database connection string is required when --save is enabled. Set EDI_PROCESSOR_CONNECTIONSTRINGS__DEFAULT."
                        );

                    // AddDbContext registers the EF Core context as scoped by default so each processing
                    // scope gets one unit-of-work context; DbContext is not thread-safe and should not
                    // be shared across concurrent work or retained for the application's lifetime.
                    services.AddDbContext<PayerEdiDbContext>(dbOptions =>
                        dbOptions.UseSqlServer(connectionString)
                    );
                    // These services consume the scoped EF Core DbContext, so they must also be scoped.
                    services.AddScoped<IDocumentTableRepository, DocumentTableRepository>();
                    services.AddScoped<IPatientRepository, PatientRepository>();
                    services.AddScoped<IPersistenceService, PersistenceService>();
                }
            )
            .UseSerilog(
                (context, _, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext();
                }
            )
            .Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        var ediFabricKey =
            app.Services.GetRequiredService<IConfiguration>()["Key:Edifabric"]
            ?? throw new InvalidOperationException("EDI Fabric Key is required");
        var tokenLoadedVia = EdiFabricHelper.ConfigureEdiFabric(ediFabricKey);

        logger.LogDebug("EdiFabric token configuration: {TokenLoadedVia}", tokenLoadedVia);

        IEdiFileLoader ediFileLoader = options.GetEdiFileLocation() switch
        {
            EdiFileLocation.FileSystem => app.Services.GetRequiredService<LocalSystemFileLoader>(),
            EdiFileLocation.S3 => app.Services.GetRequiredService<S3FileLoader>(),
            _ => throw new InvalidOperationException("Unsupported EDI file location."),
        };

        await using var ediStream = await ediFileLoader.OpenStreamAsync(options.EdiFile);
        var transactions = app
            .Services.GetRequiredService<IEdiProcessor>()
            .ProcessEdi(ediStream); // Process the EDI file

        logger.LogInformation("Transactions found in {file}", options.EdiFile);

        foreach (var transaction in transactions) // EDI File can have multiple messages, so need to process each
        {
            if (options.Save)
            {
                // Use one scope per claim so its scoped DbContext and change tracker are disposed
                // promptly instead of retaining every entity from the entire EDI file.
                await using var scope = app.Services.CreateAsyncScope();
                var persistenceService =
                    scope.ServiceProvider.GetRequiredService<IPersistenceService>();

                switch (transaction)
                {
                    case ProcessedProfessionalClaim professional:
                        await persistenceService.Save(
                            professional.EdiMessage,
                            professional.Claim
                        );
                        break;
                    case ProcessedDentalClaim dental:
                        await persistenceService.Save(dental.EdiMessage, dental.Claim);
                        break;
                    case ProcessedAttachmentTransaction attachment:
                        await persistenceService.Save(
                            attachment.EdiMessage,
                            attachment.Mapping,
                            attachment.Mapping.Transaction.TransactedAt
                        );
                        break;
                    default:
                        logger.LogWarning("Unknown transaction: {Transaction}", transaction);
                        break;
                }
            }

            switch (transaction)
            {
                case ProcessedProfessionalClaim professional:
                    if (logger.IsEnabled(LogLevel.Debug))
                    {
                        logger.LogDebug(
                            "TS837P:\n{ProClaim:l}",
                            JsonSerializer.Serialize(professional.Claim, jsonOptions)
                        );
                    }

                    break;
                case ProcessedDentalClaim dental:
                    if (logger.IsEnabled(LogLevel.Debug))
                    {
                        logger.LogDebug(
                            "TS837D:\n{DentalClaim:l}",
                            JsonSerializer.Serialize(dental.Claim, jsonOptions)
                        );
                    }

                    break;
                case ProcessedAttachmentTransaction attachment:
                    logger.LogDebug(
                        "TS275 attachment metadata: subjects={SubjectCount}, attachments={AttachmentCount}, errors={ErrorCount}",
                        attachment.Mapping.Transaction.Subjects.Count,
                        attachment.Mapping.Transaction.Attachments.Count,
                        attachment.Mapping.Errors.Count
                    );
                    break;
                default:
                    logger.LogWarning("Unknown transaction: {Transaction}", transaction);
                    break;
            }

            switch (transaction)
            {
                case ProcessedProfessionalClaim professional
                    when logger.IsEnabled(LogLevel.Trace):
                    logger.LogTrace(
                        "TS837P XML:\n{ProClaimXml:l}",
                        professional.EdiMessage.ToXml()
                    );
                    break;
                case ProcessedDentalClaim dental when logger.IsEnabled(LogLevel.Trace):
                    logger.LogTrace(
                        "TS837D XML:\n{DentalClaimXml:l}",
                        dental.EdiMessage.ToXml()
                    );
                    break;
                case ProcessedAttachmentTransaction attachment
                    when logger.IsEnabled(LogLevel.Trace):
                    logger.LogTrace(
                        "TS275 XML:\n{Attachment:l}.",
                        attachment.EdiMessage.ToXml()
                    );
                    break;
            }
        }
    });
