using Amazon.S3;
using Amazon.S3.Model;
using EDI837.src.HelperObjects;
using EDI837.src.Models;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EDI837.src.Services
{
    public class S3FileService : IS3FileService
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ILogger _logger;

        public S3FileService(IAmazonS3 amazonS3, ILogger<S3FileService> logger) 
        { 
            _amazonS3 = amazonS3;
            _logger = logger;
        }

        /// <summary>
        /// Methods returns a boolean value determined by the existence of the bucket
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <returns>boolean value</returns>
        public async Task<bool> BucketExistsAsync(string bucketName)
        { 
            ArgumentException.ThrowIfNullOrEmpty(nameof(bucketName));

            try
            {
                // GetBucketLocationAsync and catch a 404.
                var response = await _amazonS3.GetBucketLocationAsync(bucketName);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }           
        }

        /// <summary>
        /// Method creates a Stream based on the file name and the location of the file.
        /// </summary>
        /// <param name="bucketName">AWS S3 Bucket Name.</param>
        /// <param name="fileName">Name of the file to be processed.</param>
        /// <returns>Readable Stream or a Null stream if the file does not exists.</returns>
        public async Task<Stream> GetClaimStreamByFileNameAsync(string bucketName, string fileName)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(bucketName));
            ArgumentException.ThrowIfNullOrEmpty(nameof(fileName));

            try
            {
                var response = await _amazonS3.GetObjectAsync(bucketName, fileName);

                if (response.ResponseStream != null)
                {
                    this._logger.LogInformation("The Claim was successfully extracted form the bucket.");
                    return response.ResponseStream;
                }

                this._logger.LogWarning("The Claim was not successfully extracted form the bucket.");
                return Stream.Null;
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }                          
        }

        /// <summary>
        /// Method gets a list of StramResult (File Name and Stream) based on a bucket name and start characters of the file names. 
        /// </summary>
        /// <param name="bucketName">Bucket Name</param>
        /// <param name="prefix">First characters of the Files. </param>
        /// <returns>StreamResult (File Name and Stream)</returns>
        public async Task<IEnumerable<StreamResult>> GetClaimStreamsByBucketNameAndPrefixAsync(string bucketName, string prefix)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(bucketName));
            ArgumentException.ThrowIfNullOrEmpty(nameof(prefix));
            List<StreamResult> streamResults = new List<StreamResult>();

            try
            {
                var response = await _amazonS3.ListObjectsV2Async(
                new ListObjectsV2Request { BucketName = bucketName, Prefix = prefix }
                );
            
                if (response != null)
                {
                    foreach (var s3Object in response.S3Objects)
                    {
                        var stream = await GetClaimStreamByFileNameAsync(s3Object.BucketName, s3Object.Key);
                        streamResults.Add(new StreamResult { FileName = s3Object.Key, FileStream = stream });
                    }                    
                }
                
                return streamResults;
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Method Deletes a file from a specific bucket.
        /// </summary>
        /// <param name="bucketName">Bucket Name</param>
        /// <param name="fileName">File Name</param>
        /// <returns>No Return</returns>
        public async Task DeleteFileAsync(string bucketName, string fileName)
        {
            try
            {
                await this._amazonS3.DeleteObjectAsync(
                    new DeleteObjectRequest { BucketName = bucketName, Key = fileName }
                );
                this._logger.LogInformation($"File {fileName} was deleted.");
            }
            catch (AmazonS3Exception ex)
            {
                this._logger.LogError($"Unable to Delete file {fileName}, {ex.Message} ");
                throw;
            }
        }
    }
}
