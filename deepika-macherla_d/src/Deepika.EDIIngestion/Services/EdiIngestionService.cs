using System;
using System.IO;
using System.Linq;
using Deepika.EDIIngestion.Data;
using Deepika.EDIIngestion.Models;
using Microsoft.Extensions.Logging;

namespace Deepika.EDIIngestion.Services
{
    public class EdiIngestionService : IEdiIngestionService
    {
        private readonly AppDbContext _db;
        private readonly IEdiFabricParser _parser;
        private readonly IEdiFabricValidatorService _validator;
        private readonly Microsoft.Extensions.Logging.ILogger<EdiIngestionService> _logger;

        public EdiIngestionService(AppDbContext db, IEdiFabricParser parser, IEdiFabricValidatorService validator, Microsoft.Extensions.Logging.ILogger<EdiIngestionService> logger)
        {
            _db = db;
            _parser = parser;
            _validator = validator;
            _logger = logger;
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

                    var interchange = _parser.ParseFile(file);
                    if (interchange != null)
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
                    else
                    {
                        // Parser returned null - treat as parse failure
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
