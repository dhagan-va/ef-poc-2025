using EdiFabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Application.Extensions
{
    public static class EdiFabricExtensions
    {
        public static IServiceCollection AddEdiFabric(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            try
            {
                var licenseKey = configuration["EdiFabric:LicenseKey"];

                if (string.IsNullOrWhiteSpace(licenseKey))
                    throw new InvalidOperationException(
                        "EDIFabric license key is missing in configuration.");


                SerialKey.Set(licenseKey, true);


                return services;

            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Can't set token"))
                {
                    throw new Exception("Your trial has expired! To continue using EdiFabric SDK you must purchase a plan from https://www.edifabric.com/pricing.html");
                }
                else
                {
                    throw new Exception(ex.Message);
                }

            }

        }
    }
}
