using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDI837.Ingestion.Services;

public class IngestionWorker(
    IS3GatewayFactory _gatewayFactory,
    TransactionSaver _transactionSaver,
    EdiParser _ediParser,
    IOptions<AppSettings> options,
    ILogger<IngestionWorker> _logger
) : BackgroundService
{
    private readonly string _bucketName =
        options.Value.S3.Bucket
        ?? throw new InvalidOperationException("S3 bucket missing in configuration.");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting EDI poller…");
        var gateway = await _gatewayFactory.CreateAsync(_bucketName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var files = await gateway.ListFilesAsync(_bucketName, "incoming/");

                if (files.Count > 0)
                {
                    _logger.LogInformation("Found {FileCount} file(s):", files.Count);
                    foreach (var file in files)
                    {
                        _logger.LogInformation("  {FileKey}", file.Key);

                        await using var stream = await gateway.GetFileStreamAsync(
                            _bucketName,
                            file.Key
                        );
                        var claims = _ediParser.ParseEdiStream(stream);
                        await _transactionSaver.SaveTransactionsAsync(claims);
                        await gateway.DeleteFileAsync(_bucketName, file.Key);
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
