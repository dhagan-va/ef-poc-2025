using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using X12EDI837Ingestion.Producer.Configuration;
using X12EDI837Ingestion.Producer.Interfaces;
using X12EDI837Ingestion.Producer.Services;

public class ProducerServiceTests
{
    private readonly Mock<IS3StorageService> _s3Mock = new();
    private readonly Mock<ILogger<ProducerService>> _loggerMock = new();

    private ProducerService CreateService(string folderPath, int workerCount = 2)
    {
        var options = Options.Create(new FileInformation
        {
            FilePath = folderPath,
            WorkerCount = workerCount,
            ChannelCapacity = 10
        });

        return new ProducerService(
            _s3Mock.Object,
            options,
            _loggerMock.Object);
    }

    [Fact]
    public async Task UploadFolderAsync_Should_Call_S3_For_Each_File()
    {
        // Arrange
        var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        var file1 = Path.Combine(tempDir.FullName, "test1.edi");
        var file2 = Path.Combine(tempDir.FullName, "test2.edi");

        File.WriteAllText(file1, "dummy");
        File.WriteAllText(file2, "dummy");

        var service = CreateService(tempDir.FullName);

        // Act
        await service.UploadFolderAsync();

        // Assert
        _s3Mock.Verify(s => s.UploadFileAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task UploadFolderAsync_Should_Not_Call_S3_When_No_Files()
    {
        // Arrange
        var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        var service = CreateService(tempDir.FullName);

        // Act
        await service.UploadFolderAsync();

        // Assert
        _s3Mock.Verify(s => s.UploadFileAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UploadUsingWorkerPoolAsync_Should_Process_All_Files()
    {
        // Arrange
        var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        var files = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            var file = Path.Combine(tempDir.FullName, $"file{i}.edi");
            File.WriteAllText(file, "dummy");
            files.Add(file);
        }

        var service = CreateService(tempDir.FullName, workerCount: 2);

        // Act
        await service.UploadUsingWorkerPoolAsync();

        // Assert
        _s3Mock.Verify(s => s.UploadFileAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Exactly(5));
    }
    [Fact]
    public async Task UploadUsingWorkerPoolAsync_Should_Handle_Empty_Folder()
    {
        // Arrange
        var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        var service = CreateService(tempDir.FullName, workerCount: 2);

        // Act
        await service.UploadUsingWorkerPoolAsync();

        // Assert
        _s3Mock.Verify(s => s.UploadFileAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }
}