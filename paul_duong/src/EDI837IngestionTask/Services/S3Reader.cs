
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using EDI837IngestionTask.Models;

namespace EDI837IngestionTask.Services
{
    public static class S3Reader
    {
        private static IAmazonS3? s3Client;
        private static string bucketName = "";

        public static void SetClient(IAmazonS3 client)
        {
            s3Client = client;
        }

        /// <summary>
        /// Init provided variables
        /// </summary>
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

        /// <summary>
        /// get all files info from S3 and return a list of the file
        /// </summary>
        /// <returns>a list of the file</returns>
        public static async Task<List<S3FileInfo>> ListFilesAsync()
        {
            if (s3Client == null) throw new InvalidOperationException("S3Reader is not initialized!!!");
            try
            {
                var response = await s3Client.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucketName });
                return response.S3Objects.Select(o => new S3FileInfo(o.Key, o.ETag.Trim('"'), o.LastModified)).ToList();
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Files are failed to be retrieved:  StatusCode: {e.StatusCode} | ErrorCode: {e.ErrorCode}");
                return new List<S3FileInfo>();
            }


        }

        /// <summary>
        /// get file content from S3 and return the file content
        /// </summary>
        /// <returns>file content</returns>
        public static async Task<Stream> GetFileAsync(string key)
        {
            if (s3Client == null) throw new InvalidOperationException("S3Reader is not initialized!!!");

            var response = await s3Client.GetObjectAsync(bucketName, key);

            return response.ResponseStream;
        }

        /// <summary>
        /// store file content into a temp file and return the temp file path
        /// </summary>
        /// <returns>temp file path</returns>
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