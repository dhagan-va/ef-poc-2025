using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Framework.Readers;
using EdiFabric.Core.Model.Edi;
using DotNetEnv;


namespace EDI837.Ingestion
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting Ingestion Process...");

            // Attempt to set EDI token from .env file
            if (!SetEdiTokenKey())
            {
                Console.WriteLine("Failed to set token.");
                return;
            }

            var file_path = "../../samples/837-sample-file.edi";
            var claims = ParseEdiFile(file_path);
            SaveClaims(claims);
            Console.WriteLine("Finished ingestion process.");
        }

        /// <summary>
        /// Attempts to se the EDI Token key
        /// </summary>
        /// <returns>True if successful, False otherwise</returns>
        static bool SetEdiTokenKey()
        {
            // Load environment variables from .env file
            Env.Load("../../.env");

            var ediKey = Environment.GetEnvironmentVariable("EDI_TOKEN");

            if (string.IsNullOrWhiteSpace(ediKey))
            {
                Console.WriteLine("Missing EDI_TOKEN in .env file!");
                return false;
            }

            EdiFabric.SerialKey.Set(ediKey);
            return true;
        }

        /// <summary>
        /// Parses the given EDI file and extracts all valid 837P claims.
        /// Invalid claims are logged and skipped.
        /// </summary>
        /// <param name="path">Path to the .txt EDI file.</param>
        /// <returns>List of valid 837P claims.</returns>
        static List<TS837P> ParseEdiFile(string path)
        {
            var ediStream = File.OpenRead(path);

            List<IEdiItem> ediItems;
            using (var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa"))
                ediItems = [.. ediReader.ReadToEnd()];

            var claims = ediItems.OfType<TS837P>();

            var validClaims = new List<TS837P>();

            foreach (var claim in claims)
            {
                if (claim.HasErrors)
                {
                    var errors = claim.ErrorContext.Flatten();

                    Console.WriteLine($"Claim {claim.ST?.TransactionSetControlNumber_02 ?? "(unknown)"} has {errors.Count()} errors:");
                    foreach (var err in errors)
                        Console.WriteLine($"  {err}");
                }
                else
                {
                    validClaims.Add(claim);
                }
            }

            Console.WriteLine($"Parsed {validClaims.Count} valid claims successfully.");

            return validClaims;

        }

        /// <summary>
        /// Saves a list of claims to the Database
        /// </summary>
        /// <param name="claims">A list of 837P claims</param>
        static void SaveClaims(List<TS837P> claims)
        {
            return;
        }
    }


}

