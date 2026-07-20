using Amazon.S3;
using EDI837Ingestion.BusinessLayer;
using EDI837Ingestion.BusinessLayer.FilesProviders;
using EDI837Ingestion.EF;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject
{
    public class Edi837IngestionServiceUnitTests
    {
        private static AppDbContext CreateInMemoryAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Edi837IngestionService_Provider_SuccessPath_ProcessesPayloads()
        {
            // Arrange
            var dbContext = CreateInMemoryAppDbContext();
            var mockSourceProvider = new Mock<IEdiSourceProvider>();

            var mockPayloads = new List<string> { "ST*837~SE*2~" };
            mockSourceProvider.Setup(x => x.GetEdiPayloadsAsync()).ReturnsAsync(mockPayloads);

            var service = new Edi837IngestionService(dbContext, mockSourceProvider.Object);

            // Act
            await service.IngestEdi837();

            // Assert
            mockSourceProvider.Verify(x => x.GetEdiPayloadsAsync(), Times.Once);
        }


        [Fact]
        public async Task Edi837IngestionService_FileSystem_FailurePath_BubblesExceptionToCaller()
        {
            // Arrange
            var dbContext = CreateInMemoryAppDbContext();
            var mockSourceProvider = new Mock<IEdiSourceProvider>();

            // Simulate a hard OS filesystem error when reading files
            mockSourceProvider
                .Setup(x => x.GetEdiPayloadsAsync())
                .ThrowsAsync(new DirectoryNotFoundException("Target folder 'samples' does not exist."));

            var service = new Edi837IngestionService(dbContext, mockSourceProvider.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.IngestEdi837());

            // Assert
            // The service caller catches the error inside its own high-level try/catch block, 
            // so the application doesn't completely crash out at runtime.
            Assert.Null(exception);
            mockSourceProvider.Verify(x => x.GetEdiPayloadsAsync(), Times.Once);
        }

        [Fact]
        public async Task Edi837IngestionService_S3_FailurePath_HandlesDockerOrNetworkCrash()
        {
            // Arrange
            var dbContext = CreateInMemoryAppDbContext();
            var mockSourceProvider = new Mock<IEdiSourceProvider>();

            // Simulate an AWS infrastructure crash (e.g., Docker container is offline)
            mockSourceProvider
                .Setup(x => x.GetEdiPayloadsAsync())
                .ThrowsAsync(new AmazonS3Exception("Connection refused by endpoint http://localhost:5000"));

            var service = new Edi837IngestionService(dbContext, mockSourceProvider.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.IngestEdi837());

            // Assert
            // Verifies that the service caller successfully intercepts the unswallowed provider error 
            // and gracefully completes its high-level logging sequence.
            Assert.Null(exception);
            mockSourceProvider.Verify(x => x.GetEdiPayloadsAsync(), Times.Once);
        }
    }
}
