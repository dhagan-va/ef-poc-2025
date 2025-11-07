using Amazon.S3;
using Amazon.S3.Model;
using Edi837Ingestion.Edi;
using Ef837Ingest.Edi;
using Ef837Ingest.Edi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class IngestionWorker : BackgroundService
{
    private readonly ILogger<IngestionWorker> _log;
    private readonly IngestionQueue _queue;
    private readonly IFileSource _files;
    private readonly IEdiIngestionService _svc;
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;
    private readonly string _inPrefix;
    private readonly string _archivePrefix;
    private readonly TimeSpan _poll;

    // Track newest LastModified we've seen to avoid re-enqueueing
    private DateTimeOffset _lastSeen = DateTimeOffset.MinValue;

    public IngestionWorker(
        ILogger<IngestionWorker> log,
        IngestionQueue queue,
        IFileSource files,
        IEdiIngestionService svc,
        IAmazonS3 s3,
        IOptions<S3Options> opt)
    {
        _log = log;
        _queue = queue;
        _files = files;
        _svc = svc;
        _s3 = s3;

        var cfg = opt.Value;
        _bucket = cfg.Bucket;
        _inPrefix = string.IsNullOrWhiteSpace(cfg.InboundPrefix) ? "incoming/" : cfg.InboundPrefix;
        _archivePrefix = string.IsNullOrWhiteSpace(cfg.ArchivePrefix) ? "archive/" : cfg.ArchivePrefix;
        _poll = TimeSpan.FromSeconds(Math.Max(1, cfg.PollSeconds));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_bucket))
        {
            _log.LogError("IngestionWorker: S3 bucket is not configured. Check S3:Bucket in configuration.");
            return;
        }

        _log.LogInformation("IngestionWorker started. Bucket={Bucket} Inbound={Inbound} Archive={Archive} Poll={Poll}s",
            _bucket, _inPrefix, _archivePrefix, _poll.TotalSeconds);

        await EnsureBucketAsync(stoppingToken);

        var queueLoop = Task.Run(() => DrainQueueAsync(stoppingToken), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EnqueueNewObjectsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Polling error");
            }

            try
            {
                await Task.Delay(_poll, stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        await queueLoop;
    }

    private async Task EnsureBucketAsync(CancellationToken ct)
    {
        try
        {
            await _s3.GetACLAsync(_bucket, ct); // cheap existence probe
        }
        catch
        {
            try
            {
                await _s3.PutBucketAsync(_bucket, ct);
                _log.LogInformation("Created bucket {Bucket}", _bucket);
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Bucket check/create failed (continuing): {Bucket}", _bucket);
            }
        }
    }

    private async Task EnqueueNewObjectsAsync(CancellationToken ct)
    {
        string? token = null;

        do
        {
            ListObjectsV2Response? resp;
            try
            {
                resp = await _s3.ListObjectsV2Async(new ListObjectsV2Request
                {
                    BucketName = _bucket!,
                    Prefix = _inPrefix,
                    ContinuationToken = token
                }, ct);
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "ListObjectsV2 failed. Will retry next poll.");
                return; // bail this poll cycle
            }

            // Treat missing collection as empty (LocalStack can return null)
            var objs = (resp?.S3Objects ?? new List<S3Object>())
                       .Where(o => !string.IsNullOrEmpty(o.Key) &&
                                   !o.Key.EndsWith("/", StringComparison.Ordinal))
                       .OrderBy(o => o.LastModified)
                       .ToList();

            foreach (var o in objs)
            {
                // only enqueue new keys beyond our last seen timestamp
                if (_lastSeen != DateTimeOffset.MinValue && o.LastModified <= _lastSeen) continue;

                var uri = $"s3://{_bucket}/{o.Key}";
                await _queue.EnqueueAsync(uri, ct);
                _log.LogInformation("Enqueued {Uri}", uri);

                var last = (DateTimeOffset)o.LastModified;
                if (last > _lastSeen) _lastSeen = last;
            }

            token = string.IsNullOrEmpty(resp?.NextContinuationToken) ? null : resp!.NextContinuationToken;
        }
        while (token is not null && !ct.IsCancellationRequested);
    }

    private async Task DrainQueueAsync(CancellationToken ct)
    {
        await foreach (var s3Uri in _queue.Reader.ReadAllAsync(ct))
        {
            try
            {
                await using var stream = await _files.OpenAsync(s3Uri, ct);
                var count = await _svc.IngestAsync(stream, ct);
                _log.LogInformation("Ingested {Count} transactions from {Uri}", count, s3Uri);

                // Archive after success
                var (bucket, key) = ParseS3(s3Uri);
                var fileName = Path.GetFileName(key);
                var archiveKey = $"{_archivePrefix}{fileName}";

                await _s3.CopyObjectAsync(new CopyObjectRequest
                {
                    SourceBucket = bucket,
                    SourceKey = key,
                    DestinationBucket = bucket,
                    DestinationKey = archiveKey
                }, ct);

                await _s3.DeleteObjectAsync(bucket, key, ct);

                _log.LogInformation("Archived to s3://{Bucket}/{Key}", bucket, archiveKey);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to ingest {Uri}", s3Uri);
                // optional: move to a "dead-letter" prefix
                // await SafeMoveAsync(s3Uri, "deadletter/", ct);
            }
        }
    }

    private static (string bucket, string key) ParseS3(string s3Uri)
    {
        // s3://bucket/key...
        if (!s3Uri.StartsWith("s3://", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Not an S3 URI: {s3Uri}");

        var rest = s3Uri.Substring("s3://".Length);
        var slash = rest.IndexOf('/');
        if (slash < 0) return (rest, "");
        return (rest[..slash], rest[(slash + 1)..]);
    }
}