using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using X12EDI837Ingestion.Producer.Configuration;
//using X12EDI837Ingestion.Producer.Extensions;
using X12EDI837Ingestion.Producer.Interfaces;

namespace X12EDI837Ingestion.Producer.Services;

public sealed class S3StorageService : IS3StorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Information _s3Info;
    private readonly ILogger<S3StorageService> _logger;

    public S3StorageService(
        IAmazonS3 s3Client,
        IOptions<S3Information> options,
        ILogger<S3StorageService> logger)
    {
        _s3Client = s3Client;
        _s3Info = options.Value;
        _logger = logger;
    }

    public async Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default)
    {
        var bucketName = _s3Info.Bucket;

        var response = await _s3Client.ListBucketsAsync(cancellationToken);

        var bucketExists = response.Buckets
            .Any(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

        if (bucketExists)
        {
            _logger.LogInformation(
                "S3 bucket '{Bucket}' already exists.",
                bucketName);

            return;
        }

        _logger.LogInformation(
            "Bucket '{Bucket}' does not exist. Creating it...",
            bucketName);

        var request = new PutBucketRequest
        {
            BucketName = bucketName
        };

        await _s3Client.PutBucketAsync(request, cancellationToken);

        _logger.LogInformation(
            "Bucket '{Bucket}' created successfully.",
            bucketName);
    }

    public async Task UploadFileAsync(
        string key,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        await using var stream = File.OpenRead(filePath);

        var request = new PutObjectRequest
        {
            BucketName = _s3Info.Bucket,
            Key = key,
            InputStream = stream,
            ContentType = "text/plain"
        };

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);

        _logger.LogInformation(
            "Uploaded '{File}' to bucket '{Bucket}' as key '{Key}'. Status: {Status}",
            filePath,
            _s3Info.Bucket,
            key,
            response.HttpStatusCode);
    }

    public async Task<IReadOnlyList<string>> ListObjectKeysAsync(
        string? prefix = null,
        CancellationToken cancellationToken = default)
    {
        var keys = new List<string>();
        string? continuationToken = null;

        do
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _s3Info.Bucket,
                Prefix = prefix,
                ContinuationToken = continuationToken
            };

            var response = await _s3Client.ListObjectsV2Async(request, cancellationToken);

            keys.AddRange(response.S3Objects.Select(x => x.Key));

            continuationToken = response.NextContinuationToken;

        } while (continuationToken != null);

        _logger.LogInformation(
            "Found {Count} object(s) in bucket '{Bucket}'.",
            keys.Count,
            _s3Info.Bucket);

        return keys;
    }

    public async Task<Stream> DownloadObjectAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var request = new GetObjectRequest
        {
            BucketName = _s3Info.Bucket,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(request, cancellationToken);

        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);

        memoryStream.Position = 0;

        _logger.LogInformation(
            "Downloaded object '{Key}' from bucket '{Bucket}'.",
            key,
            _s3Info.Bucket);

        return memoryStream;
    }
}