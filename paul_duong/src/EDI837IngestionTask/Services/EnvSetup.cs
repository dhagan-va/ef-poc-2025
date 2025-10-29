using DotNetEnv;


namespace EDI837IngestionTask.Services
{
    public static class EnvSetup
    {
        private static bool loaded = false;
        private static readonly string BaseDir = AppContext.BaseDirectory;
        private static readonly string projectDir = Path.GetFullPath(Path.Combine(BaseDir,"..","..","..","..",".."));

        private static void LoadEnv()
        {
            if (!loaded)
            {
                var envPath = Path.GetFullPath(Path.Combine(projectDir, "samples", ".env"));
                Console.WriteLine($"check path={envPath}");
                Console.WriteLine($"BaseDir path={BaseDir}");
                Console.WriteLine($"projectDir path={projectDir}");
                Env.Load(envPath);
                loaded = true;
            }
        }

        public static string GetDbConnection()
        {
            LoadEnv();
            return Environment.GetEnvironmentVariable("SQL_CONN") ?? "";
        }

        public static string GetSampleFile()
        {
            return Path.GetFullPath(Path.Combine(projectDir, "samples", "ClaimPayment.edi"));

        }

        public static string GetSampleEmptyFile()
        {
            return Path.GetFullPath(Path.Combine(projectDir, "samples", "ClaimPaymentEmpty.edi"));

        }

        /// <summary>
        /// Attempts to set the EDI Serial key
        /// </summary>
        /// <returns>True if successful, False otherwise</returns>
        public static bool SetEdiSerialKey()
        {
            LoadEnv();

            var ediKey = Environment.GetEnvironmentVariable("SERIAL_KEY");

            if (string.IsNullOrWhiteSpace(ediKey))
            {
                Console.WriteLine("Missing SERIAL_KEY in .env file!");
                return false;
            }

            EdiFabric.SerialKey.Set(ediKey);
            return true;
        }
    }



}