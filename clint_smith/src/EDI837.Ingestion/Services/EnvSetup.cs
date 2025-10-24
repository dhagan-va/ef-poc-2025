using DotNetEnv;


namespace EDI837.Ingestion.Services
{
    public static class EnvSetup
    {
        /// <summary>
        /// Attempts to se the EDI Token key
        /// </summary>
        /// <returns>True if successful, False otherwise</returns>
        public static bool SetEdiTokenKey()
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
    }
}