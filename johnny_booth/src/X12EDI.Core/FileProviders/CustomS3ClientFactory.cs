using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

public class CustomS3ClientFactory : Amazon.Runtime.HttpClientFactory
{
    #region Private Fields

    private readonly IServiceProvider _serviceProvider;

    #endregion Private Fields

    #region Public Constructors

    public CustomS3ClientFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #endregion Public Constructors

    #region Public Methods

    public override HttpClient CreateHttpClient(IClientConfig clientConfig)
    {
        // Resolve the custom logging handler from the DI container
        var handler = _serviceProvider.GetRequiredService<S3LoggingHandler>();

        // Return a new HttpClient instance using the custom handler
        return new HttpClient(handler);
    }

    #endregion Public Methods
}