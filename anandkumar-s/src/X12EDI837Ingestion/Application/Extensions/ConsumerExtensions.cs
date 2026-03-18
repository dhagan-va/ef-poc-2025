using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using X12EDI837Ingestion.Consumer.Configuration;
using X12EDI837Ingestion.Consumer.Application.Interfaces;
//using X12EDI837Ingestion.Consumer.Application.Services;
using X12EDI837Ingestion.Consumer.Validators;
using X12EDI837Ingestion.Consumer.Application.Consumer.Services;

namespace X12EDI837Ingestion.Consumer.Extensions;
    
public static class ConsumerExtensions
{
    public static IServiceCollection AddConsumerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<S3Information>()
            .Bind(configuration.GetSection("S3Information"))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<S3Information>, S3OptionsValidator>();

        services.AddSingleton<IAmazonS3>(serviceProvider =>
        {
            var s3Information = serviceProvider
                .GetRequiredService<IOptions<S3Information>>()
                .Value;

            var credentials = new BasicAWSCredentials(
                s3Information.AccessKey,
                s3Information.SecretKey);

            var s3Config = new AmazonS3Config
            {
                ServiceURL = s3Information.Url,
                ForcePathStyle = s3Information.ForcePathStyle
            };

            return new AmazonS3Client(credentials, s3Config);
        });

        services.AddScoped<IS3StorageService, S3StorageService>();

        return services;
    }
}