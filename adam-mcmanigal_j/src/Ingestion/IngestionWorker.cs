using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edi837Ingestion.Ingestion;

/// <summary>
/// The long-running host that drives <see cref="IngestionService"/>. Its <see cref="ExecuteAsync"/>
/// loops <see cref="IngestionService.IngestNextBatchAsync"/> until the host stops, polling the SQS work
/// queue for S3 <c>ObjectCreated</c> notifications and ingesting the 837 files they point to. The
/// service owns one batch of work; this worker owns the process lifetime, the poll cadence, and the
/// top-level error recovery.
/// </summary>
/// <remarks>
/// <see cref="IngestionService.IngestNextBatchAsync"/> already isolates per-message failures (transient
/// ones are left for redelivery, poison ones are dead-lettered), so an exception reaching this loop
/// means the receive itself failed — e.g. SQS is briefly unreachable. The worker logs and backs off
/// rather than propagating, because a faulted <see cref="BackgroundService"/> would stop the host.
/// </remarks>
public sealed class IngestionWorker(
    IngestionService ingestionService,
    ILogger<IngestionWorker> logger) : BackgroundService
{
    // A brief pause after an empty receive so an idle queue does not hot-loop. With SQS long polling
    // the receive itself already waits up to WaitTimeSeconds, so this only bites if long polling is
    // disabled (WaitTimeSeconds = 0).
    private static readonly TimeSpan IdleBackoff = TimeSpan.FromSeconds(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Ingestion worker started; polling for 837 files.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await ingestionService.IngestNextBatchAsync(stoppingToken);

                // A non-empty receive may mean more is waiting, so poll straight back; an empty receive
                // means an idle queue, so pause briefly before polling again.
                if (result.Received == 0)
                    await Task.Delay(IdleBackoff, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break; // graceful shutdown
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Ingestion poll failed at the queue level; backing off {Backoff} before retrying.",
                    IdleBackoff);
                await Task.Delay(IdleBackoff, stoppingToken);
            }
        }

        logger.LogInformation("Ingestion worker stopping.");
    }
}
