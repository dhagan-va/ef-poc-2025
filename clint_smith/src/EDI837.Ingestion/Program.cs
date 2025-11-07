using EDI837.Ingestion.Gateways;
using EDI837.Ingestion.Services;
using Microsoft.Extensions.Configuration;

namespace EDI837.Ingestion
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string source = args.Length > 0 ? args[0].ToUpperInvariant() : "S3"; // default to s3
            Console.WriteLine(
                $"Starting Ingestion Process using source: {source.ToUpperInvariant()}"
            );

            var settings = GetAppSettings();
            EdiFabric.SerialKey.Set(settings.EDI.Token);

            var transactionSaver = new TransactionSaver(settings);

            if (source == "LOCAL")
            {
                var file_path = "../../samples/837-sample-file.edi";
                var claims = EdiParser.ParseEdiFileFromPath(file_path);
                await transactionSaver.SaveTransactionsAsync(claims);
            }
            else if (source == "S3")
            {
                Uri s3Uri = new Uri(settings.S3.ServiceUrl);
                var bucketName = settings.S3.Bucket;
                using var s3Gateway = new S3Gateway(s3Uri, bucketName);

                using var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    await EdiParser.RunAsync(s3Gateway, transactionSaver, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Ingestion canceled by user.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Environment.ExitCode = 1;
                    throw;
                }
                finally
                {
                    Console.WriteLine("Cleanup complete. Exiting.");
                }
            }
            else
            {
                Console.WriteLine($"Unknown source: {source}. Use 'local' or 's3'.");
            }

            Console.WriteLine("Finished ingestion process.");
        }

        static AppSettings GetAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Bind to strongly typed class
            var settings = new AppSettings();
            config.Bind(settings);

            return settings;
        }
    }
}
