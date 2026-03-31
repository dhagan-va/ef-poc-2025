using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X12EDI837Ingestion.Consumer.Application.Interfaces;
using X12EDI837Ingestion.Consumer.Application.Services;
using X12EDI837Ingestion.Consumer.Application.Validator;

namespace X12EDI837Ingestion.Consumer.Application.Extensions
{
    public static class SnipValidatorExtension
    {
        public static IServiceCollection AddSnipValidators(
                      this IServiceCollection services,
                      IConfiguration configuration)
        {
            // Reader
            services.AddTransient<IEdiX12DocumentReader, EdiDocumentReader>();

            // Orchestrator
            services.AddTransient<ISnipValidator, SnipValidator>();

            // SNIP Levels
            services.AddTransient<ISnipLevelValidator, Snip1Validator>();
            services.AddTransient<ISnipLevelValidator, Snip2Validator>();

            return services;
        }
    }
}
