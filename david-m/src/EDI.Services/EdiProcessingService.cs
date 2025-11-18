using EDI.Data;
using EdiFabric;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
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
        private static bool _serialKeyInitialized;
        private static readonly object _serialKeyLock = new();

        public EdiProcessingService(EdiDbContext dbContext, IEdiValidationService validationService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
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
                                ?? "c417cb9dd9d54297a55c032a74c87996";
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
            // Implement business logic for processing EDI documents
            // This could include validation, transformation, routing, etc.
            await Task.CompletedTask; // Placeholder
        }

        public async Task ProcessT837DAsync(string ediContent)
        {
            var transactions = Parse837D(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T837DTransactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessT837IAsync(string ediContent)
        {
            var transactions = Parse837I(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T837ITransactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessT837PAsync(string ediContent)
        {
            var transactions = Parse837P(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T837PTransactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessT835Async(string ediContent)
        {
            var transactions = Parse835(ediContent);
            foreach (var transaction in transactions)
            {
                _validationService.Validate(transaction);
                _dbContext.T835Transactions?.Add(transaction);
            }
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<TS837D> Parse837D(string ediContent)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediContent)))
            {
                var ediItems = new X12Reader(memoryStream, "EdiFabric.Templates.Hipaa").ReadToEnd().ToList();
                return ediItems.OfType<TS837D>();
            }
        }

        private IEnumerable<TS837I> Parse837I(string ediContent)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediContent)))
            {
                var ediItems = new X12Reader(memoryStream, "EdiFabric.Templates.Hipaa").ReadToEnd().ToList();
                return ediItems.OfType<TS837I>();
            }
        }

        private IEnumerable<TS837P> Parse837P(string ediContent)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediContent)))
            {
                var ediItems = new X12Reader(memoryStream, "EdiFabric.Templates.Hipaa").ReadToEnd().ToList();
                return ediItems.OfType<TS837P>();
            }
        }

        private IEnumerable<TS835> Parse835(string ediContent)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediContent)))
            {
                var ediItems = new X12Reader(memoryStream, "EdiFabric.Templates.Hipaa").ReadToEnd().ToList();
                return ediItems.OfType<TS835>();
            }
        }

        public async Task<List<string>> GetEdiFilesFromS3Async(string bucketName)
        {
            var s3Client = GetS3Client();
            var request = new ListObjectsV2Request { BucketName = bucketName };
            var response = await s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Select(o => o.Key).ToList();
        }

        public async Task ProcessEdiFilesFromS3Async(string bucketName)
        {
            var files = await GetEdiFilesFromS3Async(bucketName);
            var s3Client = GetS3Client();

            foreach (var file in files)
            {
                // Skip if already in success or failed folder
                if (file.StartsWith("success/") || file.StartsWith("failed/")) continue;

                var request = new GetObjectRequest { BucketName = bucketName, Key = file };
                using var response = await s3Client.GetObjectAsync(request);
                using var reader = new StreamReader(response.ResponseStream);
                var ediContent = await reader.ReadToEndAsync();

                try
                {
                    await ProcessEdiAsync(ediContent);
                    await MoveObjectAsync(s3Client, bucketName, file, $"success/{Path.GetFileName(file)}");
                }
                catch
                {
                    await MoveObjectAsync(s3Client, bucketName, file, $"failed/{Path.GetFileName(file)}");
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

        private AmazonS3Client GetS3Client()
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
