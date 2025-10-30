using EdiFabric;
using Microsoft.Extensions.DependencyInjection;
using X12EDI.Abstractions.Services;

namespace X12EDI.Core.Extensions
{



    public static class ParserServiceCollectionExtensions
    {

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

            return services;
        }
    }

    public class EdiOptions
    {
        public string? SerialKey { get; set; }
    }
}
