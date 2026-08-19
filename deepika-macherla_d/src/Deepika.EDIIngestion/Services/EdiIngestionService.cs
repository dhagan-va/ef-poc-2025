using System;
using System.IO;
using System.Linq;
using Deepika.EDIIngestion.Data;
using Deepika.EDIIngestion.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Deepika.EDIIngestion.Services
{
    public class EdiIngestionService : IEdiIngestionService
    {
        private readonly AppDbContext _db;
        private readonly IEdiFabricParser _parser;
        private readonly IEdiFabricValidatorService _validator;
        private readonly IEdiStorageService _storage;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Microsoft.Extensions.Logging.ILogger<EdiIngestionService> _logger;

        public EdiIngestionService(AppDbContext db, IEdiFabricParser parser, IEdiFabricValidatorService validator, IEdiStorageService storage, IServiceScopeFactory scopeFactory, Microsoft.Extensions.Logging.ILogger<EdiIngestionService> logger)
        {
            _db = db;
            _parser = parser;
            _validator = validator;
            _storage = storage;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task ProcessS3BucketAsync(string bucket, int degreeOfParallelism = 4)
        {
            _logger.LogInformation("Processing S3 bucket: {Bucket}", bucket);

            var keys = (await _storage.ListObjectsAsync(bucket)).ToList();
            _logger.LogInformation("Found {Count} objects in bucket {Bucket}", keys.Count, bucket);

            if (!keys.Any())
            {
                _logger.LogInformation("No objects to process in bucket {Bucket}", bucket);
                return;
            }

            var semaphore = new System.Threading.SemaphoreSlim(degreeOfParallelism);
            var tasks = new List<System.Threading.Tasks.Task>();

            var index = 0;
            foreach (var key in keys)
            {
                var currentIndex = index++;
                await semaphore.WaitAsync();
                tasks.Add(System.Threading.Tasks.Task.Run(async () =>
                {
                    _logger.LogInformation("[#{Index}] Starting processing S3 object: {Key}", currentIndex, key);
                    try
                    {
                        var data = await _storage.ReadObjectAsync(bucket, key);
                        _logger.LogDebug("[#{Index}] Read {Bytes} bytes for {Key}", currentIndex, data?.Length ?? 0, key);

                        var tempPath = Path.GetTempFileName();
                        await System.IO.File.WriteAllBytesAsync(tempPath, data);
                        _logger.LogDebug("[#{Index}] Wrote temp file {TempPath} for {Key}", currentIndex, tempPath, key);

                        // Validate then parse similar to local file flow
                        if (_validator != null)
                        {
                            if (!_validator.ValidateFile(tempPath, out var vErrors))
                            {
                            var errMsg = string.Join("; ", (IEnumerable<string>?)vErrors ?? Enumerable.Empty<string>());
                                RecordError(key, new Exception(errMsg), "EdiFabricValidation", System.IO.File.ReadAllText(tempPath));
                                _logger.LogWarning("[#{Index}] Validation failed for S3 object {Key}: {Errors}", currentIndex, key, errMsg);
                                return;
                            }
                            _logger.LogDebug("[#{Index}] Validation passed for {Key}", currentIndex, key);
                        }

                        var interchanges = _parser.ParseFile(tempPath);
                        if (interchanges != null && interchanges.Any())
                        {
                            foreach (var interchange in interchanges)
                            {
                                _logger.LogDebug("[#{Index}] Parser returned interchange for {Key} (ISA: {Isa})", currentIndex, key, interchange.IsaControlNumber);
                                using var scope = _scopeFactory.CreateScope();
                                var scopedDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                                try
                                {
                                    if (string.IsNullOrWhiteSpace(interchange.SenderId) || string.IsNullOrWhiteSpace(interchange.ReceiverId))
                                    {
                                        throw new InvalidOperationException($"Missing SenderId or ReceiverId in S3 object {key}.");
                                    }

                                    scopedDb.Interchanges.Add(interchange);
                                    scopedDb.SaveChanges();
                                    _logger.LogInformation("[#{Index}] Saved interchange from S3 object {Key} (ISA: {Isa})", currentIndex, key, interchange.IsaControlNumber);
                                }
                                catch (Exception dbEx)
                                {
                                    RecordError(key, dbEx, "DatabaseSaveException");
                                    _logger.LogError(dbEx, "[#{Index}] Database save failed for {Key}", currentIndex, key);
                                }
                            }
                        }
                        else
                        {
                            RecordError(key, null, "ParserReturnedNull", System.IO.File.ReadAllText(tempPath));
                            _logger.LogWarning("[#{Index}] Parser returned null or no transactions for {Key}", currentIndex, key);
                        }
                    }
                    catch (Exception ex)
                    {
                        RecordError(key, ex, "S3ProcessingException");
                        _logger.LogError(ex, "[#{Index}] Exception processing S3 object {Key}", currentIndex, key);
                    }
                    finally
                    {
                        semaphore.Release();
                        _logger.LogInformation("[#{Index}] Finished processing S3 object: {Key}", currentIndex, key);
                    }
                }));
            }

            await System.Threading.Tasks.Task.WhenAll(tasks);
        }

        /// <summary>
        /// Process all .edi files in the provided folder. For each file, attempt to parse and save the interchange.
        /// On any parsing/storage error, record an ErrorLog and continue with the next file.
        /// </summary>
        public void ProcessFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                // Use logger instead of Console
                _logger.LogWarning("Folder not found: {FolderPath}", folderPath);
                return;
            }

            var files = Directory.GetFiles(folderPath).OrderBy(f => f);
            foreach (var file in files)
            {
                _logger.LogInformation("Processing: {File}", file);
                try
                {
                    // If an EdiFabric validator is available, perform validation first and skip files that fail validation.
                    if (_validator != null)
                    {
                        if (!_validator.ValidateFile(file, out var vErrors))
                        {
                            // Record validation failure and skip parsing/saving for this file.
                            RecordError(file, new Exception(string.Join("; ", vErrors)), "EdiFabricValidation", File.ReadAllText(file));
                            _logger.LogWarning("EdiFabric validation failed for {File}: {Errors}", file, string.Join("; ", vErrors));
                            continue;
                        }
                    }

                    var interchanges = _parser.ParseFile(file);
                    if (interchanges != null && interchanges.Any())
                    {
                        foreach (var interchange in interchanges)
                        {
                            try
                            {
                                // Simple validation: require SenderId and ReceiverId
                                if (string.IsNullOrWhiteSpace(interchange.SenderId) || string.IsNullOrWhiteSpace(interchange.ReceiverId))
                                {
                                    throw new InvalidOperationException($"Missing SenderId or ReceiverId in file {Path.GetFileName(file)}.");
                                }

                                _db.Interchanges.Add(interchange);
                                _db.SaveChanges();
                                _logger.LogInformation("Saved interchange from {File} (ISA: {Isa})", Path.GetFileName(file), interchange.IsaControlNumber);
                            }
                            catch (Exception dbEx)
                            {
                                RecordError(file, dbEx, "DatabaseSaveException");
                            }
                        }
                    }
                    else
                    {
                        // Parser returned null or no transactions - treat as parse failure
                        RecordError(file, null, "ParserReturnedNull", File.ReadAllText(file));
                    }
                }
                catch (Exception ex)
                {
                    RecordError(file, ex, "ParseException");
                }
            }
        }

        private void RecordError(string filePath, Exception? ex, string errorType, string? fileContents = null)
        {
            try
            {
                var snippet = fileContents ?? (File.Exists(filePath) ? File.ReadAllText(filePath) : null);
                if (snippet != null && snippet.Length > 2000) snippet = snippet.Substring(0, 2000);

                var log = new ErrorLog
                {
                    FileName = Path.GetFileName(filePath),
                    OccurredAt = DateTime.UtcNow,
                    ErrorMessage = ex?.Message ?? "Parser returned null or validation failed",
                    ErrorType = errorType,
                    StackTrace = ex?.StackTrace,
                    FileSnippet = snippet
                };

                _db.ErrorLogs.Add(log);
                _db.SaveChanges();

                _logger.LogError("Recorded error for {FileName}: {ErrorMessage}", log.FileName, log.ErrorMessage);
            }
            catch (Exception loggingEx)
            {
                // If error logging itself fails, write to logger and continue.
                _logger.LogError(loggingEx, "Failed to record error for file: {File} - {Message}", filePath, loggingEx.Message);
            }
        }
    }
}
