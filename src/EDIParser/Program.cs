using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Amazon.S3;
using EdiParser.Configuration;
using EdiParser.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


namespace EdiParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Register the global unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // Load configuration from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();


            // Set up DI container
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(loggingBuilder =>
            {
                // Add Serilog as a logging provider
                loggingBuilder.AddSerilog(Log.Logger, dispose: true);
            });


            // Register ApplicationDbContext as a service
            serviceCollection.AddDbContext<EdiDBContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Register your main application class
            serviceCollection.AddSingleton<Application>();
            // Register EdiDBContext with DI
            serviceCollection.AddSingleton<IEdiDBContext, EdiDBContext>();
            // Register DB service
            serviceCollection.AddTransient<IDatabaseService, DatabaseService>();
            // Add IConfiguration to DI
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            // Add EdiParser registration to DI
            serviceCollection.AddTransient<IEdiParserService, Services.EdiParserService>();
            // Add EdiReaderService registration to DI
            serviceCollection.AddTransient<IEdiReaderService, Services.EdiReaderService>();
            // Add S3ReaderService registration to DI
            serviceCollection.AddTransient<IS3FileReaderService, Services.S3FileReaderService>();
            // Add EdiValidatorService registration to DI
            serviceCollection.AddTransient<IEdiValidatorService, Services.EdiValidatorService>();

            // Build the provider
            var provider = serviceCollection.BuildServiceProvider();

            // Run the app
            var app = provider.GetRequiredService<Application>();
            var logger = provider.GetRequiredService<ILogger<Program>>();
            
            // Get S3 configuration information and initialize bucket
            var s3Configuration = configuration.GetRequiredSection("S3Configuration")
                .Get<S3Configuration>();
            string? s3Bucket = s3Configuration?.Bucket;

            // Initialize AWS client
            serviceCollection.AddSingleton<IAmazonS3>(sp =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = s3Configuration?.ServiceUrl,
                    ForcePathStyle = true,
                };
                return new AmazonS3Client(s3Configuration?.s3AccessKeyId, s3Configuration?.s3SecretAccessKey, config);
            });
            
            // Get runing mode from configuration
            bool s3Mode = false;
            string? s3ModeStr = configuration["IsS3Mode"];
            if (string.IsNullOrEmpty(s3ModeStr))
            { 
                s3Mode = Boolean.Parse(s3ModeStr!);
                if (!String.IsNullOrEmpty(s3ModeStr) && s3Mode)
                {
                    logger.LogInformation($"Running in S3 mode");
                }
                else
                {
                    logger.LogInformation($"Running in edi mode");
                }
            }

            ValidationLevel SNIPLevel = ValidationLevel.SyntaxOnly_SNIP1;
            string? snipLevelStr = configuration["SNIPValidationLevel"];
            if (string.IsNullOrEmpty(snipLevelStr))
            {
                int snipLevelInt = int.Parse(snipLevelStr!);
                if (snipLevelInt > 0 && snipLevelInt < 5)
                {
                    var result = snipLevelInt switch
                    {
                        1 => ValidationLevel.SyntaxOnly_SNIP1,
                        2 => ValidationLevel.LimitsAndCodes_SNIP2,
                        3 => ValidationLevel.Balancing_SNIP3,
                        4 => ValidationLevel.InterSegment_SNIP4,
                        _ => ValidationLevel.SyntaxOnly_SNIP1
                        
                    };
                    
                    logger.LogInformation($"Validating snip level {SNIPLevel}");
                }
            }
            
            // Run app
            await app.Run(SNIPLevel, s3Mode);

            logger.LogInformation("Console app shutting down...");
            

        }
        
        // The global exception handler method
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine("\n\n#############################################");
            Console.WriteLine("A global unhandled exception occurred!");
            Console.WriteLine($"Is terminating: {e.IsTerminating}"); // e.IsTerminating is usually true
            Console.WriteLine($"Exception Type: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine("Logging the error and preparing for application termination.");
            // Log the exception to a file or a logging service (e.g., NLog, Serilog) here
            Console.WriteLine("#############################################\n\n");
        }
    }
    
}