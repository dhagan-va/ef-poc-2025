using EDI837.Ingestion.Gateways;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDI837.Ingestion.Services;

public interface IS3GatewayFactory
{
    S3Gateway Create();
}

public sealed class S3GatewayFactory : IS3GatewayFactory
{
    private readonly AppSettings _settings;
    private readonly ILogger<S3Gateway> _logger;

    public S3GatewayFactory(IOptions<AppSettings> options, ILogger<S3Gateway> logger)
    {
        ArgumentNullException.ThrowIfNull(options);

        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    public S3Gateway Create()
    {
        var serviceUri = new Uri(_settings.S3.ServiceUrl);
        return new S3Gateway(serviceUri, _settings.S3.Bucket, _logger);
    }
}
