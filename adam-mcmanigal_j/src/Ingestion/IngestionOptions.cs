namespace Edi837Ingestion.Ingestion;

/// <summary>
/// The resolved SQS runtime settings <see cref="IngestionService"/> needs: the work and dead-letter
/// queue URLs and the receive long-poll wait. These are <em>resolved</em> values — the URLs are looked
/// up from the configured queue names at startup — so they are distinct from the config-bound
/// <c>SqsOptions</c> (which holds the names). Bundling them keeps the service constructor to its genuine
/// dependencies and lets it be registered without a factory lambda.
/// </summary>
public sealed record IngestionOptions
{
    /// <summary>Resolved URL of the work queue that receives S3 <c>ObjectCreated</c> notifications.</summary>
    public required string QueueUrl { get; init; }

    /// <summary>Resolved URL of the dead-letter queue poison messages are moved to.</summary>
    public required string DeadLetterQueueUrl { get; init; }

    /// <summary>
    /// SQS receive long-poll wait, in seconds (0–20). Production uses 20 so an idle consumer waits for
    /// work rather than busy-spins; the Moto integration environment sets 0 so an empty receive returns
    /// immediately.
    /// </summary>
    public int ReceiveWaitTimeSeconds { get; init; } = 20;
}
