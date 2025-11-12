using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDI837.Ingestion.Services
{
    public class IngestionWorker(
        IS3GatewayFactory gatewayFactory,
        TransactionSaver transactionSaver,
        EdiParser ediParser,
        IOptions<AppSettings> options,
        ILogger<IngestionWorker> logger
    ) : BackgroundService
    {
        private readonly IS3GatewayFactory _gatewayFactory = gatewayFactory;
        private readonly TransactionSaver _transactionSaver = transactionSaver;
        private readonly ILogger<IngestionWorker> _logger = logger;
        private readonly EdiParser _ediParser = ediParser;
        private readonly AppSettings _settings = options.Value;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting EDI poller…");
            var _s3BucketName = _settings.S3.Bucket;
            var s3Gateway = _gatewayFactory.Create();
            await s3Gateway.EnsureBucketExistsAsync(_s3BucketName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var files = await s3Gateway.ListFilesAsync(_s3BucketName, "incoming/");

                    if (files.Count > 0)
                    {
                        _logger.LogInformation("Found {FileCount} file(s):", files.Count);
                        foreach (var file in files)
                        {
                            _logger.LogInformation("  {FileKey}", file.Key);

                            await using var stream = await s3Gateway.GetFileStreamAsync(
                                _s3BucketName,
                                file.Key
                            );
                            var claims = _ediParser.ParseEdiStream(stream);
                            await _transactionSaver.SaveTransactionsAsync(claims);

                            // For now just delete the file but I'm sure we'd want
                            // to verify that the claims were saved correctly in
                            // a real-world scenario
                            await s3Gateway.DeleteFileAsync(_s3BucketName, file.Key);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No new files found.");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // expected on Ctrl+C; just break
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while polling S3");
                    throw;
                }
            }

            _logger.LogInformation("EDI poller stopped.");
        }
    }
}
