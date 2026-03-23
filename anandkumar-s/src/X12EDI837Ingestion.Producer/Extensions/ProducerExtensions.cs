using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using X12EDI837Ingestion.Producer.Configuration;
using X12EDI837Ingestion.Producer.Interfaces;
using X12EDI837Ingestion.Producer.Services;
using X12EDI837Ingestion.Producer.Validators;

namespace X12EDI837Ingestion.Producer.Extensions;

public static class ProducerExtensions
{
    public static IServiceCollection AddProducerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<S3Information>()
            .Bind(configuration.GetSection("S3Information"))
            .ValidateOnStart();

        services.AddOptions<FileInformation>()
            .Bind(configuration.GetSection("FileInformation"))
            .ValidateOnStart();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var s3Options = sp.GetRequiredService<IOptions<S3Information>>().Value;

            var credentials = new BasicAWSCredentials(
                s3Options.AccessKey,
                s3Options.SecretKey);

            var config = new AmazonS3Config
            {
                ServiceURL = s3Options.Url,
                ForcePathStyle = s3Options.ForcePathStyle,
                AuthenticationRegion = s3Options.Region
            };

            return new AmazonS3Client(credentials, config);
        });

        services.AddSingleton<IS3StorageService, S3StorageService>();
        services.AddTransient<IProducerService, ProducerService>();
        services.AddSingleton<IValidateOptions<S3Information>, S3OptionsValidator>();

        return services;
    }

    
}