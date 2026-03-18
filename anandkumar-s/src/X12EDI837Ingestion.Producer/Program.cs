using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using X12EDI837Ingestion.Producer.Validators;
using X12EDI837Ingestion.Producer.Interfaces;
using X12EDI837Ingestion.Producer.Configuration;
using X12EDI837Ingestion.Producer.Services;
using X12EDI837Ingestion.Producer.Extensions;

namespace X12EDI837Ingestion.Producer;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions<S3Information>()
                        .Bind(context.Configuration.GetSection("S3Information"))
                        .ValidateOnStart();
                    var configuration = context.Configuration;

                    services.AddProducerServices(context.Configuration);

                    Console.WriteLine("S3Information section exists: " +
                    context.Configuration.GetSection("S3Information").Exists());
                    services.AddTransient<IProducerService, ProducerService>();
                    
                })
                .Build();

            var s3Options = host.Services
                    .GetRequiredService<IOptions<S3Information>>()
                    .Value;
            var credentials = new BasicAWSCredentials(
                s3Options.AccessKey,
                s3Options.SecretKey);

            var config = new AmazonS3Config
            {
                ServiceURL = s3Options.Url,
                ForcePathStyle = s3Options.ForcePathStyle
            };

           

            using var s3Client = new AmazonS3Client(credentials, config);

            if (!await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, s3Options.Bucket))
            {
                await s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = s3Options.Bucket
                });

                Console.WriteLine($"Bucket '{s3Options.Bucket}' created.");
            }

            //Upload the documents from the sample folder to Moto server running on localhost:5000

            var producerService = host.Services.GetRequiredService<IProducerService>();
            CancellationToken cancellationToken = CancellationToken.None;
            await producerService.UploadFolderAsync(cancellationToken);

            //Use the bellow aws cli command to verify the files are uploaded to the moto server
            //aws --endpoint-url=http://localhost:5000 s3 ls s3://x12-edi-837-bucket/incoming/ --recursive

            return 0;
        }
        catch (OptionsValidationException ex)
        {
            Console.WriteLine("S3 configuration validation failed:");

            foreach (var failure in ex.Failures)
            {
                Console.WriteLine($"- {failure}");
            }
            return -1;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine("AmazonS3Exception:");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StatusCode: {ex.StatusCode}");
            Console.WriteLine($"ErrorCode: {ex.ErrorCode}");
            Console.WriteLine($"RequestId: {ex.RequestId}");
            Console.WriteLine($"AmazonId2: {ex.AmazonId2}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                Console.WriteLine($"Inner StackTrace: {ex.InnerException.StackTrace}");
            }

            return -1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return -1;
        }
       
    }
}