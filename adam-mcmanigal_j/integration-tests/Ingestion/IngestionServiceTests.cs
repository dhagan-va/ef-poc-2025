using Amazon.S3.Model;
using Amazon.SQS.Model;
using Edi837Ingestion.Ingestion;
using Edi837Ingestion.Parsing;
using integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace integration.Ingestion;

/// <summary>
/// End-to-end tests for <see cref="IngestionService"/> against the real dependencies: a Moto-backed S3
/// bucket + SQS queue and a migrated SQL Server, all via Testcontainers. Each test uploads an object to
/// S3 and lets Moto's provisioned <c>ObjectCreated</c> notification enqueue the SQS message — the true
/// S3 → SQS → service path — then drives batches and asserts on the accumulated <see cref="BatchResult"/>
/// and the persisted ledger.
/// </summary>
/// <remarks>
/// Moto is not a perfect SQS: it delivers the notification asynchronously, and emits a one-off
/// <c>s3:TestEvent</c> (zero records) when the bucket notification is first configured — exactly as real
/// S3 does. The integration env also disables long-polling (<c>ReceiveWaitTimeSeconds = 0</c>), so a
/// receive returns immediately even before the notification has landed. So the tests accumulate batches
/// until the object's business outcome (ingested or deduplicated) appears, which absorbs the TestEvent
/// no-op and the delivery delay, rather than asserting on a single receive.
/// </remarks>
[Collection("Ingestion")]
public sealed class IngestionServiceTests(MotoFixture moto, SqlServerFixture sql)
{
    // The bundled professional sample, read once. Each test derives a byte-unique copy so their content
    // hashes never collide in the ledger shared across the collection (see UniqueSample).
    private static readonly string SampleTemplate =
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "samples", "837-sample-file.edi"));

    private static int _controlNumber = 200_000_000;

    [Fact]
    public async Task IngestNextBatchAsync_IngestsQueuedInterchange_AndPersistsLedger()
    {
        await ClearQueueAsync(moto.QueueUrl);
        var content = UniqueSample();
        var key = $"incoming/{Guid.NewGuid():N}.edi";
        await PutObjectAsync(key, content);

        var result = await ProcessUntilAsync(r => r.Ingested >= 1);

        Assert.Equal(1, result.Ingested);
        Assert.Equal(0, result.Duplicates);
        Assert.Equal(0, result.Failed);

        // The ledger row and its professional transaction set are persisted, keyed to the S3 object.
        await using var verify = sql.CreateContext();
        var interchange = await verify.Interchanges.SingleAsync(i => i.PayloadS3Key == key);
        Assert.Equal("1234567", interchange.SenderId);
        Assert.True(
            await verify.ProfessionalTransactionSets.AnyAsync(t => t.IngestedInterchangeId == interchange.Id));
    }

    [Fact]
    public async Task IngestNextBatchAsync_SkipsResendOfAlreadyIngestedInterchange()
    {
        await ClearQueueAsync(moto.QueueUrl);

        // The same file bytes, uploaded twice under different keys, hash identically.
        var content = UniqueSample();

        var firstKey = $"incoming/{Guid.NewGuid():N}.edi";
        await PutObjectAsync(firstKey, content);
        await ProcessUntilAsync(r => r.Ingested >= 1);

        var secondKey = $"incoming/{Guid.NewGuid():N}.edi";
        await PutObjectAsync(secondKey, content);
        var result = await ProcessUntilAsync(r => r.Duplicates >= 1);

        Assert.Equal(0, result.Ingested);
        Assert.Equal(1, result.Duplicates);
        Assert.Equal(0, result.Failed);

        // The resend created no second ledger row; only the original interchange remains.
        await using var verify = sql.CreateContext();
        Assert.Equal(1, await verify.Interchanges.CountAsync(i => i.PayloadS3Key == firstKey || i.PayloadS3Key == secondKey));
    }

    [Fact]
    public async Task IngestNextBatchAsync_MovesUnparseableObjectToDeadLetterQueue()
    {
        await ClearQueueAsync(moto.QueueUrl);
        await ClearQueueAsync(moto.DeadLetterQueueUrl);

        // Content that is not a valid X12 837 interchange: the parser yields no transaction sets, so
        // ingestion is a deterministic (poison) failure that must not be retried.
        var key = $"incoming/{Guid.NewGuid():N}.edi";
        await PutObjectAsync(key, "this is not a valid x12 837 edi interchange");

        var result = await ProcessUntilAsync(r => r.DeadLettered >= 1);

        Assert.Equal(1, result.DeadLettered);
        Assert.Equal(0, result.Ingested);
        Assert.Equal(0, result.Duplicates);
        Assert.Equal(0, result.Failed);

        // No ledger row was written for the poison file.
        await using var verify = sql.CreateContext();
        Assert.False(await verify.Interchanges.AnyAsync(i => i.PayloadS3Key == key));

        // The message was parked on the dead-letter queue rather than reflowed onto the work queue.
        var deadLettered = await moto.Sqs.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = moto.DeadLetterQueueUrl,
            MaxNumberOfMessages = 10,
            WaitTimeSeconds = 0,
        });
        Assert.NotEmpty(deadLettered.Messages ?? []);
    }

    // Drives batches, accumulating outcomes, until the predicate holds — tolerating Moto's async
    // delivery, its ignored long-poll, and the one-off s3:TestEvent — then returns the running totals.
    private async Task<BatchResult> ProcessUntilAsync(Func<BatchResult, bool> satisfied)
    {
        var received = 0;
        var ingested = 0;
        var duplicates = 0;
        var deadLettered = 0;
        var failed = 0;

        // Moto delivers the ObjectCreated notification a few seconds after the upload, so allow a
        // generous budget (~15s) of fast, short-poll receives.
        for (var attempt = 0; attempt < 150; attempt++)
        {
            var batch = await CreateService().IngestNextBatchAsync();
            received += batch.Received;
            ingested += batch.Ingested;
            duplicates += batch.Duplicates;
            deadLettered += batch.DeadLettered;
            failed += batch.Failed;

            var total = new BatchResult(received, ingested, duplicates, deadLettered, failed);
            if (satisfied(total))
                return total;

            await Task.Delay(100);
        }

        throw new TimeoutException("The expected ingestion outcome did not occur before the timeout.");
    }

    // The service creates a context per message from the fixture (which is an IDbContextFactory),
    // mirroring the app's runtime wiring.
    private IngestionService CreateService() =>
        new(moto.Sqs, moto.S3, new Edi837Parser(), sql, moto.QueueUrl, moto.DeadLetterQueueUrl,
            NullLogger<IngestionService>.Instance, moto.Options.Sqs.ReceiveWaitTimeSeconds);

    private Task PutObjectAsync(string key, string content) =>
        moto.S3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = moto.Options.S3.BucketName,
            Key = key,
            ContentBody = content,
        });

    // Removes any residual messages from a previous test so each starts from a known-empty queue.
    private async Task ClearQueueAsync(string queueUrl)
    {
        while (true)
        {
            var response = await moto.Sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 0,
            });

            var messages = response.Messages ?? [];
            if (messages.Count == 0)
                return;

            foreach (var message in messages)
                await moto.Sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
        }
    }

    // A byte-unique copy of the sample: the interchange control number (ISA13 and the matching IEA02)
    // is swapped so every test's file hashes differently and stays independent in the shared ledger.
    private static string UniqueSample() =>
        SampleTemplate.Replace("000000101", Interlocked.Increment(ref _controlNumber).ToString("D9"));
}
