using DotNetEnv;
using System.Reflection;



namespace EDI837.Ingestion.Services
{
    public static class EnvSetup
    {
        private static bool loaded = false;

        // Load .env once
        private static void LoadEnv()
        {
            if (!loaded)
            {
                // Find directory where the compiled assembly lives
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

                // Walk upward until we find the .env file
                var envPath = Path.Combine(basePath, "../../.env");

                if (!File.Exists(envPath))
                {
                    Console.WriteLine($".env not found at {Path.GetFullPath(envPath)}");
                }
                else
                {
                    Env.Load(envPath);
                    Console.WriteLine($"Loaded .env from {Path.GetFullPath(envPath)}");
                }
                loaded = true;
            }
        }

        // Get any environment variable by name
        public static string? Get(string key)
        {
            LoadEnv();
            return Environment.GetEnvironmentVariable(key);
        }
    
        /// <summary>
        /// Attempts to se the EDI Token key
        /// </summary>
        /// <returns>True if successful, False otherwise</returns>
        public static bool SetEdiTokenKey()
        {
            LoadEnv();

            var ediKey = Environment.GetEnvironmentVariable("EDI_TOKEN");

            if (string.IsNullOrWhiteSpace(ediKey))
            {
                Console.WriteLine("Missing EDI_TOKEN in .env file!");
                return false;
            }

            EdiFabric.SerialKey.Set(ediKey);
            return true;
        }
    }
}