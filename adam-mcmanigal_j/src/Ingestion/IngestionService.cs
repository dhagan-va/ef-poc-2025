using System.Net;
using Amazon.S3;
using Amazon.S3.Util;
using Amazon.SQS;
using Amazon.SQS.Model;
using Edi837Ingestion.Parsing;
using Edi837Ingestion.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edi837Ingestion.Ingestion;

/// <summary>
/// Consumes the S3 → SQS <c>ObjectCreated</c> notifications and ingests the raw 837 files they point
/// to: for each queued message it downloads the object from S3, parses it with
/// <see cref="Edi837Parser"/>, and persists the interchange ledger row plus its per-variant
/// transaction sets to <see cref="Edi837DbContext"/>, idempotently. A message is deleted from the
/// queue only after every S3 record it carries is handled.
/// <para>
/// Failures are split by whether a retry could ever succeed. A transient failure (an S3 hiccup, a
/// database blip) leaves the message on the queue, so SQS redelivers it and, past
/// <c>maxReceiveCount</c>, redrives it to the dead-letter queue. A deterministic failure — an
/// unparseable message body or a file that is not a valid 837 interchange — is <em>poison</em>: no
/// number of retries would change the outcome, so it is copied straight to the dead-letter queue and
/// acked off the work queue immediately, rather than reflowed <c>maxReceiveCount</c> times first.
/// Either way one bad file cannot block the queue.
/// </para>
/// </summary>
/// <remarks>
/// This is the passive, called unit of work: <see cref="IngestNextBatchAsync"/> drains one poll's
/// worth of messages and returns. The polling loop that drives it on an interval is a separate worker
/// concern, so this type stays deterministic and directly testable against Moto + SQL Server.
/// <para>
/// Idempotency has two layers: a cheap content-hash pre-check, and the unique index on
/// <see cref="IngestedInterchange.ContentHash"/> as the race-safe backstop — a duplicate insert
/// surfaces as a <see cref="DbUpdateException"/> over a unique-key violation and is treated as an
/// already-ingested file. Delivery is at-least-once, so re-seeing a file — a genuine resend or an SQS
/// redelivery of a partially handled message — is expected and safe.
/// </para>
/// </remarks>
public sealed class IngestionService(
    IAmazonSQS sqs,
    IAmazonS3 s3,
    Edi837Parser parser,
    IDbContextFactory<Edi837DbContext> contextFactory,
    string queueUrl,
    string deadLetterQueueUrl,
    ILogger<IngestionService> logger,
    int receiveWaitTimeSeconds = 20)
{
    // SQS caps a single receive at 10 messages. receiveWaitTimeSeconds long-polls so an idle queue does
    // not busy-spin (the 20s max in production); the Moto integration env sets it to 0 so an empty
    // receive returns immediately rather than long-polling.
    private const int MaxMessagesPerReceive = 10;

    // SQL Server error numbers for a unique-index / unique-constraint violation.
    private const int DuplicateKeyRow = 2601;
    private const int UniqueConstraint = 2627;

    /// <summary>
    /// Receives up to one SQS batch (10 messages) with long polling and ingests each. A message is
    /// deleted only after all S3 records it carries are ingested (or found already ingested); any
    /// failure leaves the message on the queue for redelivery. Returns the per-outcome counts so a
    /// caller's loop can decide whether to poll again immediately or back off.
    /// </summary>
    public async Task<BatchResult> IngestNextBatchAsync(CancellationToken cancellationToken = default)
    {
        var response = await sqs.ReceiveMessageAsync(
            new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = MaxMessagesPerReceive,
                WaitTimeSeconds = receiveWaitTimeSeconds,
            },
            cancellationToken);

        // AWS SDK v4 leaves unpopulated collections null rather than empty.
        var messages = response.Messages ?? [];
        var ingested = 0;
        var duplicates = 0;
        var deadLettered = 0;
        var failed = 0;

        foreach (var message in messages)
        {
            try
            {
                // A short-lived context per message: its change tracker starts empty and is discarded
                // on dispose, so a failed save can never leak Added entities into the next message, and
                // messages could be handled concurrently. This is why there is no ChangeTracker.Clear().
                await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

                var outcome = await HandleMessageAsync(db, message, cancellationToken);
                ingested += outcome.Ingested;
                duplicates += outcome.Duplicates;

                // Ack only once every record in the message has been handled without error.
                await sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
            }
            catch (PoisonMessageException poison)
            {
                // Deterministic failure: a retry would fail identically. Move it to the dead-letter
                // queue now instead of reflowing it maxReceiveCount times first.
                try
                {
                    await DeadLetterAsync(message, poison, cancellationToken);
                    deadLettered++;
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    // Even parking the poison message failed (e.g. an SQS blip). Leave it on the work
                    // queue so ordinary redelivery — and eventually maxReceiveCount redrive — still
                    // gets it off the queue.
                    logger.LogError(
                        exception,
                        "Failed to move poison SQS message {MessageId} to the dead-letter queue; leaving it on the work queue for redelivery.",
                        message.MessageId);
                    failed++;
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                // Transient failure. Leave the message on the queue: SQS redelivers it and, past
                // maxReceiveCount, redrives it to the dead-letter queue. Re-ingesting any records
                // already committed is safe — the content-hash ledger dedupes them on the retry.
                logger.LogError(
                    exception,
                    "Failed to handle SQS message {MessageId}; leaving it on the queue for redelivery.",
                    message.MessageId);
                failed++;
            }
        }

        if (messages.Count > 0)
            logger.LogDebug(
                "Batch complete: received {Received}, ingested {Ingested}, duplicates {Duplicates}, dead-lettered {DeadLettered}, failed {Failed}.",
                messages.Count, ingested, duplicates, deadLettered, failed);

        return new BatchResult(messages.Count, ingested, duplicates, deadLettered, failed);
    }

    /// <summary>
    /// Handles one SQS message: parses its body as an S3 event notification and ingests each object it
    /// references. A message with no records (e.g. an <c>s3:TestEvent</c>) is a no-op and is acked.
    /// </summary>
    private async Task<(int Ingested, int Duplicates)> HandleMessageAsync(
        Edi837DbContext db, Message message, CancellationToken cancellationToken)
    {
        S3EventNotification notification;
        try
        {
            notification = S3EventNotification.ParseJson(message.Body);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            // The body is not a parseable S3 event notification; no retry would change that.
            throw new PoisonMessageException(
                "SQS message body is not a parseable S3 event notification.", exception);
        }

        var records = notification.Records ?? [];
        if (records.Count == 0)
            logger.LogDebug(
                "SQS message {MessageId} carried no S3 records (e.g. an s3:TestEvent); acking with no work.",
                message.MessageId);

        var ingested = 0;
        var duplicates = 0;
        foreach (var record in records)
        {
            // S3 event object keys are URL-encoded (e.g. '/' as %2F, spaces as '+'), so decode before
            // using the key to fetch the object — otherwise the GetObject lookup misses.
            var key = WebUtility.UrlDecode(record.S3.Object.Key);
            var outcome = await IngestObjectAsync(db, record.S3.Bucket.Name, key, cancellationToken);

            if (outcome == IngestOutcome.Ingested)
                ingested++;
            else
                duplicates++;
        }

        return (ingested, duplicates);
    }

    /// <summary>
    /// Downloads one S3 object, parses it, and persists the interchange plus its transaction sets —
    /// idempotently. Returns whether the file was newly ingested or was already in the ledger.
    /// </summary>
    private async Task<IngestOutcome> IngestObjectAsync(
        Edi837DbContext db, string bucket, string key, CancellationToken cancellationToken)
    {
        using var response = await s3.GetObjectAsync(bucket, key, cancellationToken);

        ParsedInterchange parsed;
        try
        {
            parsed = parser.Parse(response.ResponseStream);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            // The object is not a readable X12 837 interchange; re-reading it would fail identically.
            throw new PoisonMessageException(
                $"Object s3://{bucket}/{key} could not be parsed as an X12 837 interchange.", exception);
        }

        // Cheap pre-check: skip the write the ledger already records. The unique index below is the
        // race-safe backstop for the window between this check and SaveChanges.
        if (await db.Interchanges.AnyAsync(i => i.ContentHash == parsed.ContentHash, cancellationToken))
        {
            logger.LogDebug(
                "Skipping already-ingested object s3://{Bucket}/{Key} (hash {ContentHash}).",
                bucket, key, parsed.ContentHash);
            return IngestOutcome.Duplicate;
        }

        IngestedInterchange interchange;
        try
        {
            interchange = IngestedInterchange.From(parsed, DateTime.UtcNow);
        }
        catch (ArgumentException exception)
        {
            // The file parsed but carried no transaction sets — nothing to ingest, and a retry can't
            // add any. Treat it as poison rather than reflowing it.
            throw new PoisonMessageException(
                $"Object s3://{bucket}/{key} parsed to an interchange with no transaction sets.", exception);
        }

        interchange.PayloadS3Key = key;
        db.Interchanges.Add(interchange);

        foreach (var transactionSet in parsed.ProfessionalTransactionSets)
            db.ProfessionalTransactionSets.Add(
                Edi837ProfessionalTransactionSet.From(transactionSet, interchange));
        foreach (var transactionSet in parsed.InstitutionalTransactionSets)
            db.InstitutionalTransactionSets.Add(
                Edi837InstitutionalTransactionSet.From(transactionSet, interchange));
        foreach (var transactionSet in parsed.DentalTransactionSets)
            db.DentalTransactionSets.Add(
                Edi837DentalTransactionSet.From(transactionSet, interchange));

        try
        {
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation(
                "Ingested interchange {ContentHash} from s3://{Bucket}/{Key}: {Professional}P/{Institutional}I/{Dental}D transaction sets.",
                parsed.ContentHash, bucket, key,
                parsed.ProfessionalTransactionSets.Count,
                parsed.InstitutionalTransactionSets.Count,
                parsed.DentalTransactionSets.Count);
            return IngestOutcome.Ingested;
        }
        catch (DbUpdateException exception) when (IsDuplicateContentHash(exception))
        {
            // Another delivery/consumer inserted the same file between the pre-check and here; the
            // unique content-hash index rejected this insert. Treat it as already ingested.
            logger.LogDebug(
                "Concurrent insert of interchange {ContentHash} lost the race; treating as an already-ingested duplicate.",
                parsed.ContentHash);
            return IngestOutcome.Duplicate;
        }
    }

    // A unique-key violation is only a *content-hash* duplicate when the offending index is the
    // content-hash index — SQL Server names it in the error message. Checking the name keeps any future
    // unique index from being silently misread as an already-ingested duplicate.
    private static bool IsDuplicateContentHash(DbUpdateException exception) =>
        exception.InnerException is SqlException { Number: DuplicateKeyRow or UniqueConstraint } sqlException
        && sqlException.Message.Contains(
            IngestedInterchangeConfiguration.ContentHashIndexName, StringComparison.Ordinal);

    /// <summary>
    /// Copies a poison message to the dead-letter queue and then acks it off the work queue. The copy is
    /// sent <em>before</em> the source message is deleted: if the send fails the source message is left
    /// untouched and ordinary redelivery still applies, so the message is never lost.
    /// </summary>
    private async Task DeadLetterAsync(
        Message message, PoisonMessageException reason, CancellationToken cancellationToken)
    {
        logger.LogError(
            reason,
            "SQS message {MessageId} is poison ({Reason}); moving it straight to the dead-letter queue without retrying.",
            message.MessageId, reason.Message);

        await sqs.SendMessageAsync(
            new SendMessageRequest { QueueUrl = deadLetterQueueUrl, MessageBody = message.Body },
            cancellationToken);
        await sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
    }

    private enum IngestOutcome
    {
        Ingested,
        Duplicate,
    }

    /// <summary>
    /// Marks a deterministic ("poison") ingestion failure — an unparseable message body or a file that
    /// is not a valid 837 interchange — that no retry could resolve, so the message is dead-lettered
    /// immediately rather than reflowed. Transient failures are represented by their original exception.
    /// </summary>
    private sealed class PoisonMessageException(string message, Exception innerException)
        : Exception(message, innerException);
}
