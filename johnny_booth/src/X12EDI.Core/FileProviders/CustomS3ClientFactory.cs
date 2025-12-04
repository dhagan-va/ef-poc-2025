using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace X12EDI.Core.FileProviders
{

    /// <summary>
    /// A custom factory for creating <see cref="HttpClient"/> instances used by the Amazon S3 client,
    /// with a custom logging handler injected.
    /// </summary>
    public class CustomS3ClientFactory : HttpClientFactory
    {
        #region Private Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomS3ClientFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
        public CustomS3ClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Creates an <see cref="HttpClient"/> with a custom <see cref="S3LoggingHandler"/>.
        /// </summary>
        /// <param name="clientConfig">The client configuration.</param>
        /// <returns>A new <see cref="HttpClient"/> instance.</returns>
        public override HttpClient CreateHttpClient(IClientConfig clientConfig)
        {
            // Resolve the custom logging handler from the DI container
            var handler = _serviceProvider.GetRequiredService<S3LoggingHandler>();

            // Return a new HttpClient instance using the custom handler
            return new HttpClient(handler);
        }

        #endregion Public Methods
    }
}