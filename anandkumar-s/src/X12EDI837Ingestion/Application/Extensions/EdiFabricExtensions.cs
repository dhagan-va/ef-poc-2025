using EdiFabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X12EDI837Ingestion.Consumer.Application.Interfaces;
using X12EDI837Ingestion.Consumer.Application.Validator;

namespace X12EDI837Ingestion.Application.Extensions
{
    public static class EdiFabricExtensions
    {
        public static IServiceCollection AddEdiFabric(
                      this IServiceCollection services,
                      IConfiguration configuration)
        {

            Console.WriteLine($"ENV direct: {Environment.GetEnvironmentVariable("EdiFabric__LicenseKey")}");
            Console.WriteLine($"CONFIG value: {configuration["EdiFabric:LicenseKey"]}");
            var licenseKey = configuration["EdiFabric:LicenseKey"];

           

            if (string.IsNullOrWhiteSpace(licenseKey))
                throw new InvalidOperationException(
                    "EdiFabric license key is missing in configuration.");

            try
            {
                SerialKey.Set(licenseKey, true);
            }
            catch (Exception ex) when (ex.Message.StartsWith("Can't set token"))
            {
                throw new InvalidOperationException(
                    "Your EdiFabric trial has expired. Purchase a license from https://www.edifabric.com/pricing.html",
                    ex);
            }

            return services;
        }

        
    }
}
