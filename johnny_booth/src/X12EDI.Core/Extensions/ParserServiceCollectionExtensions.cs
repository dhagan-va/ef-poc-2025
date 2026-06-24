using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Config;
using X12EDI.Core.FileProviders;
using X12EDI.Core.Services;
using PhysicalFileProvider = X12EDI.Core.FileProviders.LocalFileProvider;

namespace X12EDI.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for registering EDI and S3 related services in the dependency injection container.
    /// </summary>
    public static class ParserServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds EDI processing services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configure">An action to configure the <see cref="EdiOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddEDIServices(this IServiceCollection services, Action<EdiOptions> configure)

        {
            var options = new EdiOptions();

            configure(options);

            if (!string.IsNullOrEmpty(options.SerialKey))
            {
                EdiFabric.SerialKey.Set(options.SerialKey);
            }

            services.AddSingleton(options);

            services.Configure(configure);
            services.AddScoped<IX12ParserService, X12ParserService>();

            if (!string.IsNullOrEmpty(options.FolderPath))
            {
                services.AddSingleton<IFileProvider>(
                    new PhysicalFileProvider(options.FolderPath));
            }
            services.AddScoped<IFileIngestionService, FileIngestionService>();

            return services;
        }

        /// <summary>
        /// Adds and configures services for interacting with an S3-compatible object store.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configure">An action to configure the <see cref="S3Options"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentException">Thrown if <see cref="S3Options.BucketName"/> is not provided.</exception>
        public static IServiceCollection AddS3(
            this IServiceCollection services,
            Action<S3Options> configure)
        {
            // 1. Validate and store the options
            var options = new S3Options();

            // call the callback to get actual configuration options
            configure(options);

            if (string.IsNullOrEmpty(options.BucketName))
            {
                throw new ArgumentException("S3Options.BucketName is required.");
            }

            // 2. Register IAmazonS3 as a Singleton (Expensive to create)
            services.AddSingleton<IAmazonS3>(serviceProvider =>
            {
                // Create configuration object
                var config = new AmazonS3Config
                {
                    RegionEndpoint = string.IsNullOrEmpty(options.Region) ? Amazon.RegionEndpoint.USEast1 : Amazon.RegionEndpoint.GetBySystemName(options.Region),
                    ServiceURL = options.ServiceURL,
                    ForcePathStyle = options.ForcePathStyle,
                    UseHttp = new Uri(options.ServiceURL).Scheme == "http",
                };

                // Create and return the AmazonS3Client
                if (!string.IsNullOrEmpty(options.AccessKey) && !string.IsNullOrEmpty(options.SecretKey))
                {
                    var credentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);

                    return new AmazonS3Client(credentials, config);
                }

                // Fallback: Use default credential chain (IAM roles, profiles, etc.)
                return new AmazonS3Client(config);
            });

            // 3. Register the IFileProvider using the configured IAmazonS3 client
            services.AddTransient<IFileProvider, S3FileProvider>();

            services.AddSingleton<S3Options>(options);

            // Add the S3 logging handler to be able to see request URLs
            services.AddTransient<S3LoggingHandler>();

            return services;
        }

        #endregion Public Methods
    }
}