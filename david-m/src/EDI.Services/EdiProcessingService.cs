using EDI.Data;
using EdiFabric;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text;
using System.IO;

namespace EDI.Services
{
    public class EdiProcessingService : IEdiProcessingService
    {
        private readonly EdiDbContext _dbContext;
        private readonly IEdiValidationService _validationService;
        private readonly ILogger<EdiProcessingService> _logger;
        private readonly AmazonS3Client _s3Client;
        private static bool _serialKeyInitialized;
        private static readonly object _serialKeyLock = new();

        public EdiProcessingService(EdiDbContext dbContext, IEdiValidationService validationService, ILogger<EdiProcessingService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _s3Client = CreateS3Client();
            EnsureSerialKey();
        }

        private static void EnsureSerialKey()
        {
            if (_serialKeyInitialized)
            {
                return;
            }

            lock (_serialKeyLock)
            {
                if (_serialKeyInitialized)
                {
                    return;
                }

                var serialKey = Environment.GetEnvironmentVariable("EDIFABRIC_SERIAL_KEY")
                                ?? throw new InvalidOperationException("EDIFABRIC_SERIAL_KEY environment variable is required");
                SerialKey.Set(serialKey, true);
                _serialKeyInitialized = true;
            }
        }

        public async Task ProcessEdiAsync(string ediContent)
        {
            // Identify the document type using EdiFabric
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediContent));
            var messages = new X12Reader(memoryStream, "EdiFabric.Templates.Hipaa").ReadToEnd().ToList();

            object? firstTransaction = null;
            foreach (var msg in messages)
            {
                if (msg is TS837P tp) { firstTransaction = tp; break; }
                if (msg is TS837D td) { firstTransaction = td; break; }
                if (msg is TS837I ti) { firstTransaction = ti; break; }
                if (msg is TS835 t5) { firstTransaction = t5; break; }
            }

            if (firstTransaction == null)
            {
                throw new InvalidOperationException("No supported transaction found in EDI file.");
            }

            // Process based on transaction type
            if (firstTransaction is TS837P)
            {
                await ProcessT837PAsync(ediContent);
            }
            else if (firstTransaction is TS837D)
            {
                await ProcessT837DAsync(ediContent);
            }
            else if (firstTransaction is TS837I)
            {
                await ProcessT837IAsync(ediContent);
            }
            else if (firstTransaction is TS835)
            {
                await ProcessT835Async(ediContent);
            }
            else
            {
                throw new NotSupportedException($"Unsupported transaction type: {firstTransaction.GetType().Name}");
            }
        }

        public async Task ProcessEdiDocumentAsync(string ediContent)
        {
            // Process the EDI document content
            // This could include additional business logic like document validation, transformation, routing, etc.
            await ProcessEdiAsync(ediContent);
        }

        public async Task ProcessT837DAsync(string ediContent)
        {
            var transactions = ParseEdi<TS837D>(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T837DTransactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessT837IAsync(string ediContent)
        {
            var transactions = ParseEdi<TS837I>(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T837ITransactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessT837PAsync(string ediContent)
        {
            var transactions = ParseEdi<TS837P>(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T837PTransactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessT835Async(string ediContent)
        {
            var transactions = ParseEdi<TS835>(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T835Transactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<TTransaction> ParseEdi<TTransaction>(string ediContent)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediContent));
            var ediItems = new X12Reader(memoryStream, "EdiFabric.Templates.Hipaa").ReadToEnd().ToList();
            return ediItems.OfType<TTransaction>();
        }

        public async Task<List<string>> GetEdiFilesFromS3Async(string bucketName)
        {
            var request = new ListObjectsV2Request { BucketName = bucketName };
            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Select(o => o.Key).ToList();
        }

        public async Task ProcessEdiFilesFromS3Async(string bucketName)
        {
            var files = await GetEdiFilesFromS3Async(bucketName);

            foreach (var file in files)
            {
                // Skip if already in success or failed folder
                if (file.StartsWith("success/") || file.StartsWith("failed/")) continue;

                var request = new GetObjectRequest { BucketName = bucketName, Key = file };
                using var response = await _s3Client.GetObjectAsync(request);
                using var reader = new StreamReader(response.ResponseStream);
                var ediContent = await reader.ReadToEndAsync();

                try
                {
                    await ProcessEdiAsync(ediContent);
                    await MoveObjectAsync(_s3Client, bucketName, file, $"success/{Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process EDI file {File}", file);
                    await MoveObjectAsync(_s3Client, bucketName, file, $"failed/{Path.GetFileName(file)}");
                }
            }
        }

        private async Task MoveObjectAsync(IAmazonS3 s3Client, string bucketName, string sourceKey, string destinationKey)
        {
            var copyRequest = new CopyObjectRequest
            {
                SourceBucket = bucketName,
                SourceKey = sourceKey,
                DestinationBucket = bucketName,
                DestinationKey = destinationKey
            };
            await s3Client.CopyObjectAsync(copyRequest);
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = sourceKey
            };
            await s3Client.DeleteObjectAsync(deleteRequest);
        }

        private static AmazonS3Client CreateS3Client()
        {
            var config = new AmazonS3Config
            {
                ServiceURL = Environment.GetEnvironmentVariable("AWS_SERVICE_URL") ?? "http://localhost:5000",
                ForcePathStyle = true
            };
            return new AmazonS3Client("fake", "fake", config);
        }
    }
}
