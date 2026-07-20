using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837Ingestion.BusinessLayer.FilesProviders
{
    public class S3EdiSourceProvider : IEdiSourceProvider
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _config;

        public S3EdiSourceProvider(IAmazonS3 s3Client, IConfiguration config)
        {
            _s3Client = s3Client;
            _config = config;
        }

        public async Task<IEnumerable<string>> GetEdiPayloadsAsync()
        {
            Console.WriteLine("Downloading EDI files from S3...");

            //set up S3 bucket and seed it with sample EDI files for local testing
            await S3BucketSetup();
            //extract files from S3 bucket
            var ediPayloads = await ExtractS3BucketFiles();
           
            return ediPayloads;
        }

        private async Task S3BucketSetup()
        {
            Console.WriteLine("[Mock Setup] Initializing local Moto S3 Bucket...");
            await _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = "edi-claims-storage" });

            // Sample raw EDI 837 payload text to seed your local Moto environment
            string sampleEdi = string.Empty;
            string filePath = Environment.GetEnvironmentVariable("Edi837_Path") ?? _config["FilePaths:Edi837Path"] ?? "C:\\Projects\\VA\\EDI 837\\igor-timofeyev_i\\samples";
            string sampleFile = Path.Combine(filePath, "EDI837-sample.edi");

            if (!string.IsNullOrEmpty(sampleFile) && File.Exists(sampleFile))
            {
                sampleEdi = await File.ReadAllTextAsync(sampleFile);
            }
            byte[] ediBytes = Encoding.UTF8.GetBytes(sampleEdi);


            //upload sample 1 to s3 bucket
            using var memoryStream = new MemoryStream(ediBytes);
            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = "edi-claims-storage",
                Key = "claims/EDI837-sample_1.edi",
                InputStream = memoryStream
            });
            Console.WriteLine("[Mock Setup] Sample 1 EDI 837 data successfully pushed to local Moto storage.");

            //upload sample 2 to s3 bucket
            using var memoryStream2 = new MemoryStream(ediBytes);
            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = "edi-claims-storage",
                Key = "claims/EDI837-sample_2.edi",
                InputStream = memoryStream2
            });
            Console.WriteLine("[Mock Setup] Sample 2 EDI 837 data successfully pushed to local Moto storage.");
        }

        public async Task<IEnumerable<string>> ExtractS3BucketFiles()
        {
            var payloads = new List<string>();
            string bucketName = "edi-claims-storage";
            string prefix = "claims/";

            Console.WriteLine($"Querying S3 bucket '{bucketName}' for files under prefix '{prefix}'...");

            // Fetch the metadata index of all objects inside the "claims/" folder
            var listRequest = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = prefix
            };

            ListObjectsV2Response listResponse;

            do
            {
                listResponse = await _s3Client.ListObjectsV2Async(listRequest);

                foreach (S3Object s3Object in listResponse.S3Objects)
                {
                    if (s3Object.Key.EndsWith("/")) continue;

                    var getRequest = new GetObjectRequest { BucketName = listRequest.BucketName, Key = s3Object.Key };
                    using var response = await _s3Client.GetObjectAsync(getRequest);
                    using var reader = new StreamReader(response.ResponseStream, Encoding.UTF8);

                    payloads.Add(await reader.ReadToEndAsync());
                }

                listRequest.ContinuationToken = listResponse.NextContinuationToken;
            } while (listResponse.IsTruncated ?? true);

            Console.WriteLine("\n[Pipeline Complete] All S3 EDI claim payloads processed successfully.");

            return payloads;
        }
    }
}
