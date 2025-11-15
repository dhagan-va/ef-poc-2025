using Microsoft.Extensions.Logging;

public class S3LoggingHandler : DelegatingHandler
{
    #region Private Fields

    private readonly ILogger<S3LoggingHandler> _logger;

    #endregion Private Fields

    #region Public Constructors

    public S3LoggingHandler(ILogger<S3LoggingHandler> logger)
        : base(new HttpClientHandler())
    {
        _logger = logger;
    }

    #endregion Public Constructors

    #region Protected Methods

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // CRITICAL LOGGING:
        _logger.LogCritical("S3 Outbound Request URL: {Url}", request.RequestUri);
        _logger.LogCritical("S3 Outbound Host Header: {Host}", request.Headers.Host);

        // Let the request proceed to the Blazor API on your specific port (e.g., 7008)
        return await base.SendAsync(request, cancellationToken);
    }

    #endregion Protected Methods
}