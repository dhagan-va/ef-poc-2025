using Microsoft.EntityFrameworkCore;
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

        private static readonly string _baseDir = AppContext.BaseDirectory;
        private static readonly string _projectDir = Path.GetFullPath(Path.Combine(_baseDir, "..", "..", ".."));
        public static readonly string SamplesDir = Path.GetFullPath(Path.Combine(_projectDir, "..", "..", "samples"));


        /// <summary>
        /// load appsettings.json
        /// </summary>
        private static void EnsureConfigLoad()
        {
            if (_config == null)
            {
                Console.WriteLine("Loading Configuration from appsettings.json...");
                var appSettingsPath = Path.Combine(_projectDir, "appsettings.json");
                if (!File.Exists(appSettingsPath))
                {
                    throw new FileNotFoundException($"Cannot find appsettings.json at {appSettingsPath}");
                }
                var builder = new ConfigurationBuilder()
                    .SetBasePath(_projectDir)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _config = builder.Build();
                Console.WriteLine("Completed Loading Configuration from appsettings.json...");
            }
        }

        /// <summary>
        /// Get DB connection setup from appsettings.json
        /// </summary>
        /// <returns>DB Connection setup info</returns>
        public static string GetDbConnection()
        {
            EnsureConfigLoad();
            return _config!.GetConnectionString("HIPAA_5010_837P") ?? "";
        }

        /// <summary>
        /// Get sample file - SampleClaimPayment837PEdi.edi path
        /// </summary>
        /// <returns>sample file path</returns>
        public static string GetSampleFile()
        {
            return Path.GetFullPath(Path.Combine(SamplesDir, "SampleClaimPayment837PEdi.edi"));

        }

        /// <summary>
        /// Get sample file - SampleClaim837PEdi4.edi path
        /// </summary>
        /// <returns>sample file path</returns>
        public static string GetSampleFile1()
        {
            return Path.GetFullPath(Path.Combine(SamplesDir, "SampleClaim837PEdi4.edi"));

        }

        /// <summary>
        /// Get sample file - TestClaimPaymentEmpty.edi path
        /// </summary>
        /// <returns>sample file path</returns>
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

        /// <summary>
        /// Retrieve setups from appsettings.json and initalize the variables
        /// </summary>
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

        /// <summary>
        /// Retrieve setups from appsettings.json and initalize the variables for S3
        /// </summary>
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

        /// <summary>
        /// Setup DB config
        /// </summary>
        public static HIPAA_5010_837P_Context CreateContext()
        {
            var options = new DbContextOptionsBuilder<HIPAA_5010_837P_Context>()
            .UseSqlServer(GetDbConnection())
            .Options;
            return new HIPAA_5010_837P_Context(options);
        }
    }



}