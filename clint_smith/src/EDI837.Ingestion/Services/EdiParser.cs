using EDI837.Ingestion.Gateways;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace EDI837.Ingestion.Services
{
    public static class EdiParser
    {
        /// <summary>
        /// Parse the Edi file from a given local path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<TS837P> ParseEdiFileFromPath(string path)
        {
            using var ediStream = File.OpenRead(path);
            return ParseEdiStream(ediStream);
        }

        /// <summary>
        /// Synchronously parses the given EDI file and extracts all valid 837P claims.
        /// Invalid claims are logged and skipped.
        /// </summary>
        /// <param name="ediStream">A Stream object</param>
        /// <returns>List of valid 837P transactions.</returns>
        public static IEnumerable<TS837P> ParseEdiStream(Stream ediStream)
        {
            List<IEdiItem> ediItems;
            using (var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa"))
            {
                ediItems = [.. ediReader.ReadToEnd()];
            }

            var transactions = ediItems.OfType<TS837P>();

            var validTransactions = new List<TS837P>();

            foreach (var transaction in transactions)
            {
                transaction.IsValid(out var errorContext);

                if (transaction.HasErrors)
                {
                    var errors = transaction.ErrorContext.Flatten().ToList();

                    Console.WriteLine(
                        $"Claim {transaction.ST?.TransactionSetControlNumber_02 ?? "(unknown)"} has {errors.Count} errors:"
                    );
                    foreach (var err in errors)
                    {
                        Console.WriteLine($"  {err}");
                    }
                }
                else
                {
                    validTransactions.Add(transaction);
                }
            }

            Console.WriteLine($"Parsed {validTransactions.Count} valid claims successfully.");

            return validTransactions;
        }

        public static async Task RunAsync(
            S3Gateway s3Gateway,
            TransactionSaver transactionSaver,
            CancellationToken cancellationToken
        )
        {
            ArgumentNullException.ThrowIfNull(s3Gateway);
            ArgumentNullException.ThrowIfNull(transactionSaver);

            if (s3Gateway == null)
            {
                Console.WriteLine("S3Gateway not initialized. Exiting poller.");
                return;
            }

            Console.WriteLine("Starting EDI poller... Press Ctrl-C to stop.");

            // poll forever until Ctrl-C (cancellation) is requested
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var files = await s3Gateway.ListFilesAsync("incoming/");

                    if (files.Count > 0)
                    {
                        Console.WriteLine($"Found {files.Count} file(s):");
                        foreach (var file in files)
                        {
                            Console.WriteLine($"  {file.Key}");

                            // Example placeholder for your real processing
                            var stream = await s3Gateway.GetFileStreamAsync(file.Key);
                            var claims = ParseEdiStream(stream);
                            await transactionSaver.SaveTransactionsAsync(claims);

                            // For now just delete the file but I'm sure we'd want
                            // to verify that the claims were saved correctly in
                            // a real-world scenario
                            await s3Gateway.DeleteFileAsync(file.Key);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No new files found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while polling S3: {ex.Message}");
                    throw;
                }

                // Wait 10 seconds before next poll
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }

            Console.WriteLine("EDI poller stopped.");
        }
    }
}
