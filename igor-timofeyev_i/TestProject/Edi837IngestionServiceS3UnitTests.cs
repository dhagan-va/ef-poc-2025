using Amazon.S3;
using Amazon.S3.Model;
using EDI837Ingestion.BusinessLayer;
using EDI837Ingestion.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class Edi837IngestionServiceS3UnitTests
    {
        private static AppDbContext CreateInMemoryAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// POSITIVE TEST: Discovers files inside the S3 bucket configuration.
        /// </summary>
        [Fact]
        public async Task ProcessS3BucketFiles_ValidFiles_SuccessfullyDiscoversFiles()
        {
            // Arrange
            var mockS3Client = new Mock<IAmazonS3>();
            var service = new Edi837IngestionService(mockS3Client.Object, CreateInMemoryAppDbContext(), new ConfigurationBuilder().Build());

            var listResponse = new ListObjectsV2Response
            {
                IsTruncated = false,
                S3Objects = new List<S3Object> { new S3Object { Key = "claims/EDI837-sample2.edi" } }
            };

            mockS3Client.Setup(x => x.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default)).ReturnsAsync(listResponse);
            mockS3Client.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default)).ReturnsAsync(new GetObjectResponse { ResponseStream = new MemoryStream("ST~SE~"u8.ToArray()) });

            // Act
            await service.ProcessS3BucketFiles();

            // Assert
            mockS3Client.Verify(x => x.ListObjectsV2Async(It.Is<ListObjectsV2Request>(r => r.BucketName == "edi-claims-storage"), default), Times.Once);
        }

        /// <summary>
        /// NEGATIVE TEST: S3 Bucket contains no objects under the targeted prefix directory path.
        /// Expected: The do-while loop processes an empty collection smoothly without calling GetObjectAsync or parsing data.
        /// </summary>
        [Fact]
        public async Task ProcessS3BucketFiles_EmptyBucketRegistry_GracefulCompletion()
        {
            // Arrange
            var mockS3Client = new Mock<IAmazonS3>();
            var dbContext = CreateInMemoryAppDbContext();
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            var service = new Edi837IngestionService(mockS3Client.Object, dbContext, config);

            var emptyResponse = new ListObjectsV2Response
            {
                IsTruncated = false,
                S3Objects = new List<S3Object>() // Empty response from the cloud
            };
            mockS3Client
                .Setup(x => x.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
                .ReturnsAsync(emptyResponse);

            // Act
            await service.ProcessS3BucketFiles();

            // Assert
            // Guarantees that the foreach block is safely bypassed when the cloud bucket contains no data items
            mockS3Client.Verify(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default), Times.Never);
        }
    }
}
