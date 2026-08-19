using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace Deepika.EDIIngestion.Services
{
    public class S3EdiStorageService : IEdiStorageService
    {
        private readonly AmazonS3Client _client;

        public S3EdiStorageService(string? serviceURL = null)
        {
            // Allow service URL to be provided via constructor or AWS_S3_ENDPOINT env var (used by moto/local tests)
            var resolvedUrl = serviceURL ?? Environment.GetEnvironmentVariable("AWS_S3_ENDPOINT");
            var config = new AmazonS3Config();
            if (!string.IsNullOrWhiteSpace(resolvedUrl))
            {
                // When using a local endpoint (moto) we prefer explicit ServiceURL and path-style addressing.
                config.ServiceURL = resolvedUrl;
                config.ForcePathStyle = true;

                // If the service URL uses plain http, tell the SDK to use http (moto server defaults to http).
                if (resolvedUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    config.UseHttp = true;
                }
            }
            else
            {
                // Ensure a RegionEndpoint is set when no ServiceURL is provided. This prevents
                // the AWSSDK error: "No RegionEndpoint or ServiceURL configured" when creating the client.
                config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
            }

            // Credentials: read from env vars (AWS_ACCESS_KEY_ID/AWS_SECRET_ACCESS_KEY) or fall back to default test creds for moto
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? "test";
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "test";

            var creds = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            _client = new AmazonS3Client(creds, config);
        }

        public async Task<IEnumerable<string>> ListObjectsAsync(string bucket)
        {
            var keys = new List<string>();
            string? token = null;
            do
            {
                var resp = await _client.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucket, ContinuationToken = token });
                foreach (var s in resp.S3Objects) keys.Add(s.Key);
                token = resp.IsTruncated ? resp.NextContinuationToken : null;
            } while (token != null);

            return keys;
        }

        public async Task<byte[]> ReadObjectAsync(string bucket, string key)
        {
            using var resp = await _client.GetObjectAsync(bucket, key);
            using var ms = new MemoryStream();
            await resp.ResponseStream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
