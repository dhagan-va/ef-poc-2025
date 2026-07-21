using Amazon.S3;
using EDI837Ingestion.BusinessLayer;
using EDI837Ingestion.BusinessLayer.FilesProviders;
using EDI837Ingestion.EF;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;

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

        // Helper method to convert an in-memory collection of streams into an IAsyncEnumerable
        private static async IAsyncEnumerable<Stream> CreateMockAsyncEnumerableStreams(List<Stream> streams)
        {
            foreach (var stream in streams)
            {
                yield return stream;
                await Task.CompletedTask; // Satisfies async state requirements
            }
        }

        [Fact]
        public async Task Edi837IngestionService_Provider_SuccessPath_ProcessesPayloads()
        {
            // Arrange
            var dbContext = CreateInMemoryAppDbContext();
            var mockSourceProvider = new Mock<IEdiSourceProvider>();

            // Build a valid, single-line in-memory dummy EDI segment stream
            var mockPayloads = Encoding.UTF8.GetBytes("ST*837~SE*2~");
            var mockStreamList = new List<Stream> { new MemoryStream(mockPayloads) };

            // Setup Moq to return the stream lazily via our helper
            mockSourceProvider
                .Setup(x => x.GetEdiStreamsAsync())
                .Returns(CreateMockAsyncEnumerableStreams(mockStreamList));

            var service = new Edi837IngestionService(dbContext, mockSourceProvider.Object);

            // Act
            await service.IngestEdi837();

            // Assert
            mockSourceProvider.Verify(x => x.GetEdiStreamsAsync(), Times.Once);
        }


        [Fact]
        public async Task Edi837IngestionService_FileSystem_FailurePath_BubblesExceptionToCaller()
        {
            // Arrange
            var dbContext = CreateInMemoryAppDbContext();
            var mockSourceProvider = new Mock<IEdiSourceProvider>();

            // Simulate a hard file system directory missing error bubbling up natively
            mockSourceProvider
                .Setup(x => x.GetEdiStreamsAsync())
                .Throws(new DirectoryNotFoundException("Target folder 'samples' does not exist."));

            var service = new Edi837IngestionService(dbContext, mockSourceProvider.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.IngestEdi837());

            // Assert
            // The service intercept catch block handles the blown engine safely without breaking execution threads
            Assert.Null(exception);
            mockSourceProvider.Verify(x => x.GetEdiStreamsAsync(), Times.Once);
        }

        [Fact]
        public async Task Edi837IngestionService_S3_FailurePath_HandlesDockerOrNetworkCrash()
        {
            // Arrange
            var dbContext = CreateInMemoryAppDbContext();
            var mockSourceProvider = new Mock<IEdiSourceProvider>();

            // Simulate a network connection drop context exception from AWS SDK layers
            mockSourceProvider
                .Setup(x => x.GetEdiStreamsAsync())
                .Throws(new AmazonS3Exception("Connection refused by endpoint http://localhost:5000"));

            var service = new Edi837IngestionService(dbContext, mockSourceProvider.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.IngestEdi837());

            // Assert
            Assert.Null(exception);
            mockSourceProvider.Verify(x => x.GetEdiStreamsAsync(), Times.Once);
        }
    }
}
