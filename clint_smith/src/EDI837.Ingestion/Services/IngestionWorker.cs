using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDI837.Ingestion.Services
{
    public class IngestionWorker(
        S3GatewayFactory gatewayFactory,
        TransactionSaver transactionSaver,
        EdiParser ediParser,
        ILogger<IngestionWorker> logger
    ) : BackgroundService
    {
        private readonly S3GatewayFactory _gatewayFactory = gatewayFactory;
        private readonly TransactionSaver _transactionSaver = transactionSaver;
        private readonly ILogger<IngestionWorker> _logger = logger;
        private readonly EdiParser _ediParser = ediParser;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting EDI poller…");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var s3Gateway = _gatewayFactory.Create();
                    var files = await s3Gateway.ListFilesAsync("incoming/");

                    if (files.Count > 0)
                    {
                        _logger.LogInformation("Found {FileCount} file(s):", files.Count);
                        foreach (var file in files)
                        {
                            _logger.LogInformation("  {FileKey}", file.Key);

                            await using var stream = await s3Gateway.GetFileStreamAsync(file.Key);
                            var claims = _ediParser.ParseEdiStream(stream);
                            await _transactionSaver.SaveTransactionsAsync(claims);

                            // For now just delete the file but I'm sure we'd want
                            // to verify that the claims were saved correctly in
                            // a real-world scenario
                            await s3Gateway.DeleteFileAsync(file.Key);
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

                _logger.LogInformation("EDI poller stopped.");
            }
        }
    }
}
