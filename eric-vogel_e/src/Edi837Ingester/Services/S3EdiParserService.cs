using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edi837Ingester.Services
{
    public class S3EdiParserService(IS3Service s3Service, IEdiParserService ediParserService) : IS3EdiParserService
    {
        /// <summary>
        /// Read and parse the EDI 837 file from S3 bucket/key.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="fileName">The filename in the S3 bucket.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task ParseFromS3Async(string bucketName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("Bucket name must be provided", nameof(bucketName));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name (key) must be provided", nameof(fileName));
            using var stream = await s3Service.DownloadFileAsync(bucketName, fileName);
            await ediParserService.Parse(stream);
        }
    }
}
