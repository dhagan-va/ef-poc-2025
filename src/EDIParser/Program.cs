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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

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

            // Add IConfiguration to DI
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            // Add EdiParser registration to DI
            serviceCollection.AddTransient<IEdiParserService, Services.EdiParserService>();
            // Add EdiReaderService registration to DI
            serviceCollection.AddTransient<IEdiReaderService, Services.EdiReaderService>();

            
            var s3Configuration = configuration.GetRequiredSection("S3Configuration")
                .Get<S3Configuration>();
            string? s3Bucket = s3Configuration?.Bucket;
            
            serviceCollection.AddSingleton<IAmazonS3>(sp =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = s3Configuration?.ServiceUrl,
                    ForcePathStyle = true,
                };
                return new AmazonS3Client(s3Configuration?.s3AccessKeyId, s3Configuration?.s3SecretAccessKey, config);
            });
            

            // Build the provider
            var provider = serviceCollection.BuildServiceProvider();

            // Run the app
            var app = provider.GetRequiredService<Application>();
            var logger = provider.GetRequiredService<ILogger<Program>>();
            
            
            // Default validation level
            ValidationLevel? validationLevel = null;

            // Parse validation level from command line arguments --validation or --validation-level <value>
            // Accepts enum names (case-insensitive) or integer values; maps user-friendly 1..4 -> SNIP1..SNIP4
            for (int i = 0; i < args.Length; i++)
            {
                if ((args[i].Equals("--validation", StringComparison.OrdinalIgnoreCase) ||
                     args[i].Equals("--validation-level", StringComparison.OrdinalIgnoreCase)) && i + 1 < args.Length)
                {
                    var val = args[i + 1];

                    // Try parse enum name first (case-insensitive)
                    if (int.TryParse(val, out var intVal) && intVal >= 1 && intVal <= 4)
                    {
                        validationLevel = (ValidationLevel)(intVal - 1);
                        logger.LogInformation($"Using validation level: {validationLevel}");
                    }
                    else
                    {
                        logger.LogWarning($"Warning: Unknown validation level '{val}', you will need to enter the validation level manually later.");
                    }

                    break;
                }
            }

            ValidationLevel? validationLevelLocal = validationLevel;

            // Check mode
            bool s3Mode = false;
            for (int i = 0; i < args.Length; i++)
            {       
                if (args[i] == "--s3")
                {
                    s3Mode = true;
                    logger.LogInformation($"Running in S3 mode");
                    break;
                }
            }
            await app.Run(validationLevelLocal, s3Mode);

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