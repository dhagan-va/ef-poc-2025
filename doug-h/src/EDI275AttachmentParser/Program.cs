using EDI275AttachmentParser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using Newtonsoft.Json;

namespace EDI275AttachmentParser
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Setup dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Create root command
            var rootCommand = new RootCommand("EDI 275 Attachment Parser - Parse EDI 275 files and extract attachments");

            // Add file option
            var fileOption = new Option<FileInfo>(
                name: "--file",
                description: "Path to the EDI 275 file to parse")
            {
                IsRequired = true
            };
            fileOption.AddAlias("-f");

            // Add output directory option
            var outputOption = new Option<DirectoryInfo>(
                name: "--output",
                description: "Directory to save extracted attachments",
                getDefaultValue: () => new DirectoryInfo("./attachments"));
            outputOption.AddAlias("-o");

            // Add license key option
            var licenseOption = new Option<string>(
                name: "--license",
                description: "EdiFabric license key (or set EDIFABRIC_LICENSE environment variable)");
            licenseOption.AddAlias("-l");

            // Add verbose option
            var verboseOption = new Option<bool>(
                name: "--verbose",
                description: "Enable verbose logging");
            verboseOption.AddAlias("-v");

            // Add JSON export option
            var jsonOption = new Option<bool>(
                name: "--json",
                description: "Export parsed data as JSON");
            jsonOption.AddAlias("-j");

            rootCommand.AddOption(fileOption);
            rootCommand.AddOption(outputOption);
            rootCommand.AddOption(licenseOption);
            rootCommand.AddOption(verboseOption);
            rootCommand.AddOption(jsonOption);

            rootCommand.SetHandler(async (file, output, license, verbose, exportJson) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                
                try
                {
                    // Set EdiFabric license key
                    var licenseKey = license ?? Environment.GetEnvironmentVariable("EDIFABRIC_LICENSE");
                    if (!string.IsNullOrEmpty(licenseKey))
                    {
                        // EdiFabric.Core.Model.Edi.SerialKey.Set(licenseKey);
                        logger.LogInformation("EdiFabric license key provided");
                    }
                    else
                    {
                        logger.LogWarning("No EdiFabric license provided. Using trial mode (may have limitations)");
                        logger.LogWarning("Set license with --license option or EDIFABRIC_LICENSE environment variable");
                        logger.LogWarning("Get trial license at: https://www.edifabric.com/trial.html");
                    }

                    var parser = serviceProvider.GetRequiredService<EDI275Parser>();

                    logger.LogInformation("Starting EDI 275 parsing...");
                    logger.LogInformation("Input file: {File}", file.FullName);
                    logger.LogInformation("Output directory: {Dir}", output.FullName);

                    // Parse the EDI file
                    var document = parser.ParseFile(file.FullName);

                    // Display summary
                    Console.WriteLine("\n" + new string('=', 60));
                    Console.WriteLine("EDI 275 PARSING SUMMARY");
                    Console.WriteLine(new string('=', 60));
                    Console.WriteLine($"Interchange Control Number: {document.InterchangeControlNumber}");
                    Console.WriteLine($"Sender ID: {document.SenderId}");
                    Console.WriteLine($"Receiver ID: {document.ReceiverId}");
                    Console.WriteLine($"Transaction Date: {document.TransactionDate}");
                    Console.WriteLine($"Patient Reports: {document.PatientReports.Count}");
                    Console.WriteLine($"Attachments Found: {document.Attachments.Count}");
                    Console.WriteLine(new string('=', 60));

                    // Display patient information
                    if (document.PatientReports.Count > 0)
                    {
                        Console.WriteLine("\nPATIENT INFORMATION:");
                        foreach (var patient in document.PatientReports)
                        {
                            Console.WriteLine($"  - {patient.PatientName} (ID: {patient.PatientId})");
                        }
                    }

                    // Display attachment information
                    if (document.Attachments.Count > 0)
                    {
                        Console.WriteLine("\nATTACHMENTS:");
                        for (int i = 0; i < document.Attachments.Count; i++)
                        {
                            var att = document.Attachments[i];
                            Console.WriteLine($"  [{i + 1}] Control#: {att.AttachmentControlNumber}");
                            Console.WriteLine($"      Type: {att.AttachmentTypeCode}");
                            Console.WriteLine($"      Description: {att.Description}");
                            Console.WriteLine($"      Size: {att.FileSize} bytes");
                            Console.WriteLine($"      File: {att.FileName}");
                        }

                        // Extract attachments
                        Console.WriteLine($"\nExtracting attachments to: {output.FullName}");
                        parser.ExtractAttachments(document, output.FullName);
                        Console.WriteLine("✓ Attachments extracted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nNo attachments found in the EDI file.");
                    }

                    // Export to JSON if requested
                    if (exportJson)
                    {
                        var jsonPath = Path.Combine(output.FullName, "parsed_data.json");
                        var json = JsonConvert.SerializeObject(document, Formatting.Indented);
                        File.WriteAllText(jsonPath, json);
                        Console.WriteLine($"\n✓ JSON export saved to: {jsonPath}");
                    }

                    Console.WriteLine(new string('=', 60));
                    logger.LogInformation("EDI 275 parsing completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error parsing EDI 275 file");
                    Console.WriteLine($"\n✗ ERROR: {ex.Message}");
                    if (verbose)
                    {
                        Console.WriteLine($"\nStack Trace:\n{ex.StackTrace}");
                    }
                    Environment.Exit(1);
                }
            }, fileOption, outputOption, licenseOption, verboseOption, jsonOption);

            return await rootCommand.InvokeAsync(args);
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Information);
            });

            services.AddTransient<EDI275Parser>();
            services.AddTransient<ManualEDI275Parser>();
        }
    }
}
