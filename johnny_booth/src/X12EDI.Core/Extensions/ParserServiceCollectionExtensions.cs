using Amazon.Runtime;
using Amazon.S3;
using EdiFabric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Config;
using X12EDI.Core.FileProviders;
using X12EDI.Core.Services;
using PhysicalFileProvider = X12EDI.Core.FileProviders.PhysicalFileProvider;

namespace X12EDI.Core.Extensions
{
    public static class ParserServiceCollectionExtensions
    {
        #region Public Methods

        /// <summary>Adds the edi services.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns>
        ///   <br />
        /// </returns>
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