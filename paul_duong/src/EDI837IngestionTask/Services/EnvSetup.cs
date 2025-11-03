using Microsoft.Extensions.Configuration;

namespace EDI837IngestionTask.Services
{
    public static class EnvSetup
    {
        private static IConfigurationRoot? _config;

        public static int PollingSeconds { get; private set; } = 30;
        public static int BatchSize { get; private set; } = 100;
        public static int MaxConcurrency { get; private set; } = 5;
        public static int validLevel { get; private set; } = 1;

        private static readonly string BaseDir = AppContext.BaseDirectory;
        private static readonly string ProjectDir = Path.GetFullPath(Path.Combine(BaseDir, "..", "..", ".."));
        public static readonly string SamplesDir = Path.GetFullPath(Path.Combine(ProjectDir, "..", "..", "samples"));


        private static void EnsureConfigLoad()
        {
            if (_config == null)
            {
                Console.WriteLine("Loading Configuration from appsettings.json...");
                var appSettingsPath = Path.Combine(ProjectDir, "appsettings.json");
                if (!File.Exists(appSettingsPath))
                {
                    throw new FileNotFoundException($"Cannot find appsettings.json at {appSettingsPath}");
                }
                var builder = new ConfigurationBuilder()
                    .SetBasePath(ProjectDir)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _config = builder.Build();
                Console.WriteLine("Completed Loading Configuration from appsettings.json...");
            }
        }

        public static string GetDbConnection()
        {
            EnsureConfigLoad();
            return _config!.GetConnectionString("HIPAA_5010_837P") ?? "";
        }

        public static string GetSampleFile()
        {
            return Path.GetFullPath(Path.Combine(SamplesDir, "ClaimPayment.edi"));

        }

        public static string GetSampleEmptyFile()
        {
            return Path.GetFullPath(Path.Combine(SamplesDir, "TestClaimPaymentEmpty.edi"));

        }

        /// <summary>
        /// Attempts to set the EDI Serial key
        /// </summary>
        /// <returns>True if successful, False otherwise</returns>
        public static bool SetEdiSerialKey()
        {
            EnsureConfigLoad();
            if (_config != null)
            {
                var ediKey = _config["EdiConfig:SerialKey"];

                if (string.IsNullOrWhiteSpace(ediKey))
                {
                    Console.WriteLine("Missing SERIAL_KEY in .env file!");
                    return false;
                }

                EdiFabric.SerialKey.Set(ediKey);
                return true;
            }
            return false;


        }

        public static void GeneralInitalize()
        {
            EnsureConfigLoad();
            if (_config != null)
            {
                PollingSeconds = _config.GetValue<int>("Ingestion:PollingSeconds", 30);

                BatchSize = _config.GetValue<int>("Ingestion:BatchSize", 100);

                MaxConcurrency = _config.GetValue<int>("Ingestion:MaxConcurrency", 5);

                validLevel = _config.GetValue<int>("Ingestion:ValidationLevel", 5);
            }
        }

        public static void S3Initalize()
        {
            EnsureConfigLoad();
            if (_config != null)
            {
                var aws = _config.GetSection("S3");
                var bucket = aws["BucketName"] ?? "test-bucket";
                var endpoint = aws["Endpoint"] ?? "http://127.0.0.1:4566";
                var region = aws["Region"] ?? "us-east-1";
                var accessKey = aws["AccessKey"] ?? "fake-access-key";
                var secretKey = aws["SecretKey"] ?? "fake-secret-key";

                S3Reader.Init(bucket, endpoint, region, accessKey, secretKey);

                Console.WriteLine($"Environment For S3 Mock initialized. Polling every {PollingSeconds}s.");

            }

        }
    }



}