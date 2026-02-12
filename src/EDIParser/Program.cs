using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            serviceCollection.AddTransient<IEdiParser, EdiParser>();

            // Build the provider
            var provider = serviceCollection.BuildServiceProvider();

            // Run the app
            var app = provider.GetRequiredService<Application>();
            var logger = provider.GetRequiredService<ILogger<Program>>();
            await app.Run();

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