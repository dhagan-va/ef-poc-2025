namespace Edi837Ingestion.Ingestion;

/// <summary>
/// The per-outcome tally for one call to <see cref="IngestionService.IngestNextBatchAsync"/>: how
/// many SQS messages were received, and across the S3 records they carried, how many interchanges were
/// newly ingested, skipped as already-ingested duplicates, moved to the dead-letter queue as poison, or
/// failed transiently. A caller's polling loop can use <see cref="Received"/> to decide whether to poll
/// again immediately (a full batch may mean more is waiting) or back off (an empty receive means an idle
/// queue).
/// </summary>
/// <param name="Received">Number of SQS messages returned by the receive.</param>
/// <param name="Ingested">Interchanges written to the ledger for the first time.</param>
/// <param name="Duplicates">Interchanges skipped because the content hash was already recorded.</param>
/// <param name="DeadLettered">
/// Messages moved straight to the dead-letter queue because they are poison (unparseable body or file),
/// so redelivery could never succeed.
/// </param>
/// <param name="Failed">Messages left on the queue for redelivery after a transient error while handling them.</param>
public sealed record BatchResult(int Received, int Ingested, int Duplicates, int DeadLettered, int Failed);
