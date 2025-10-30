
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Azure;
using EDI837IngestionTask.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EDI837IngestionTask.Services
{
    public static class S3Reader
    {
        private static IAmazonS3? s3Client;
        private static string bucketName = "";

        public static void Init(
            string bucket,
            string endpoint,
            string region,
            string accessKey,
            string secretKey)
        {
            bucketName = bucket;
            var config = new AmazonS3Config
            {
                ServiceURL = endpoint,
                ForcePathStyle = true,
                AuthenticationRegion = region
            };
            s3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), config);

            Console.WriteLine($"Connected to S3 mock: {endpoint}, bucket: {bucket}");
        }

        public static async Task<List<S3FileInfo>> ListFilesAsync()
        {
            if (s3Client == null) throw new InvalidOperationException("S3Reader is not initialized!!!");

            var response = await s3Client.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucketName });

            return response.S3Objects.Select(o => new S3FileInfo(o.Key,o.ETag.Trim('"'),o.LastModified)).ToList();
        }

        public static async Task<Stream> GetFileAsync(string key)
        {
            if (s3Client == null) throw new InvalidOperationException("S3Reader is not initialized!!!");

            var response = await s3Client.GetObjectAsync(bucketName, key);

            return response.ResponseStream;
        }

        public static async Task<string> DownloadFromS3Async(S3FileInfo file)
        {
            using var stream = await GetFileAsync(file.Key);
            string tempPath = Path.Combine(Path.GetTempPath(), file.Key.Replace("/", "_"));
            using (var fs = File.Create(tempPath))
                await stream.CopyToAsync(fs);
            return tempPath;
        }
    }

}