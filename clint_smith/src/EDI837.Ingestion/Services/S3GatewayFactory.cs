using System.Threading.Tasks;
using Amazon.S3;
using EDI837.Ingestion.Gateways;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDI837.Ingestion.Services;

public interface IS3GatewayFactory
{
    Task<S3Gateway> CreateAsync(string bucketName);
}

public sealed class S3GatewayFactory : IS3GatewayFactory
{
    private readonly AppSettings _settings;
    private readonly ILogger<S3Gateway> _logger;
    private readonly IAmazonS3 _s3Client;

    public S3GatewayFactory(
        IOptions<AppSettings> options,
        ILogger<S3Gateway> logger,
        IAmazonS3 s3Client
    )
    {
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
    }

    public async Task<S3Gateway> CreateAsync(string bucketName)
    {
        var s3Gateway = new S3Gateway(_s3Client, _logger);
        await s3Gateway.EnsureBucketExistsAsync(bucketName);
        return s3Gateway;
    }
}
