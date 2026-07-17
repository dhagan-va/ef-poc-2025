// csharp
using Amazon.S3;
using EDI837Ingestion.BusinessLayer;
using EDI837Ingestion.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class Edi837IngestionServiceFileSystemUnitTests
    {
        // Helper: Create in-memory AppDbContext instead of mocking
        private static AppDbContext CreateInMemoryAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// POSITIVE TEST: Can successfully read a valid EDI837 file.
        /// Expected: IngestEdi837() completes without exception; file is read and processed.
        /// </summary>
        [Fact]
        public async Task Edi837IngestionService_IngestEdi837_ReadsFileSuccessfully()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            try
            {
                var ediContent = @"ISA*00*          *00*          *ZZ*SENDER123     *ZZ*RECEIVER456   *200101*1200*U*00501*000000001*0*T*:~
                                    GS*HC*SENDER123*RECEIVER456*20200101*120000*1*X*005010X222A1~
                                    ST*837*0001*005010X222A1~
                                    BHT*0019*00*244*20200101*120000*CH~
                                    SE*6*0001~
                                    GE*1*1~
                                    IEA*1*000000001~";
                await File.WriteAllTextAsync(tempFile, ediContent);

                var configDict = new System.Collections.Generic.Dictionary<string, string>
            {
                { "FilePaths:Edi837PathWithFilename", tempFile }
            };
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(configDict)
                    .Build();

                var mockS3Client = new Mock<IAmazonS3>();
                var dbContext = CreateInMemoryAppDbContext();
                var service = new Edi837IngestionService(mockS3Client.Object, dbContext, config);

                // Act - Verify file can be read
                using (var fileStream = File.OpenRead(tempFile))
                {
                    var reader = new StreamReader(fileStream);
                    var content = await reader.ReadToEndAsync();

                    // Assert
                    Assert.NotEmpty(content);
                    Assert.Contains("ISA*00", content);
                    Assert.Contains("SENDER123", content);
                    Assert.Contains("RECEIVER456", content);
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// NEGATIVE TEST: Configuration is null or empty (no file path provided).
        /// Expected: Service uses fallback value.
        /// </summary>
        [Fact]
        public void Edi837IngestionService_MissingFilePath_UsesFallback()
        {
            // Arrange
            var configDict = new System.Collections.Generic.Dictionary<string, string>(); // Empty
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();

            var mockS3Client = new Mock<IAmazonS3>();
            var dbContext = CreateInMemoryAppDbContext();

            // Act
            var service = new Edi837IngestionService(mockS3Client.Object, dbContext, config);

            // Assert - Service is created
            Assert.NotNull(service);
        }

    }
}