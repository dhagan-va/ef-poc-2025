using EdiFabric.Framework.Readers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using X12EDI837Ingestion.Domain;
using X12EDI837Ingestion.Infrastructure.Repositories;

namespace X12EDI837Ingestion.Application.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            //services.AddScoped<IIngestionService, IngestionService>();
            //services.AddScoped<IEdiReader, EdiFabricReader>();
            //services.AddScoped<ITransactionRouter, TransactionRouter>();

            //services.AddScoped<Ts837Handler>();
            //services.AddScoped<Ts835Handler>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SqlServer")));

            services.AddScoped<IX12EDI837IngestRepo, X12EDI837IngestRepo>();

           
            return services;
        }
    }
}
