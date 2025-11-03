using EdiFabric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Services;

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

        #endregion Public Methods
    }

    public class EdiOptions
    {
        #region Public Properties

        public bool ContinueOnError { get; set; } = true;
        public string? FolderPath { get; set; }
        public string? SerialKey { get; set; }

        #endregion Public Properties
    }
}