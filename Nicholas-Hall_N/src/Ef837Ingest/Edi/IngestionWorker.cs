using Amazon.S3;
using Amazon.S3.Model;
using Edi837Ingestion.Edi;
using Ef837Ingest.Edi;
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
    private readonly string _prefix;
    private readonly TimeSpan _poll;

    public class S3Options
    {
        public string Bucket { get; set; } = "";
        public string InboundPrefix { get; set; } = "inbox/";
        public int PollSeconds { get; set; } = 5;
    }

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
        _bucket = opt.Value.Bucket;
        _prefix = opt.Value.InboundPrefix;
        _poll = TimeSpan.FromSeconds(Math.Max(1, opt.Value.PollSeconds));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("IngestionWorker started. Bucket={Bucket} Prefix={Prefix}", _bucket, _prefix);

        // polling loop + queue loop
        var queueLoop = Task.Run(() => DrainQueueAsync(stoppingToken), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var list = await _s3.ListObjectsV2Async(new ListObjectsV2Request
                {
                    BucketName = _bucket,
                    Prefix = _prefix
                }, stoppingToken);

                foreach (var o in list.S3Objects)
                {
                    var uri = $"s3://{_bucket}/{o.Key}";
                    await _queue.EnqueueAsync(uri, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Polling error");
            }

            await Task.Delay(_poll, stoppingToken);
        }

        await queueLoop;
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
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to ingest {Uri}", s3Uri);
            }
        }
    }
}
