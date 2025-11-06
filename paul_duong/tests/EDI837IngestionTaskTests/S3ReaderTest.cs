using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using EDI837IngestionTask.Models;
using EDI837IngestionTask.Services;
using Moq;

namespace EDI837IngestionTaskTests;

public class S3ReaderTest
{
    [Fact]
    public async Task S3Reader_ListFilesAsync_SHouldReturnFakeFiles_WhenMocked()
    {
        var mockS3 = new Mock<IAmazonS3>();

        mockS3.Setup(s3 => s3.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new ListObjectsV2Response
              {
                  S3Objects = new List<S3Object>
                  {
                    new S3Object{ Key = "test1.edi", ETag="\"etag1\"", LastModified=DateTime.UtcNow},
                    new S3Object{ Key = "test2.edi", ETag="\"etag2\"", LastModified=DateTime.UtcNow}
                  }
              });
        S3Reader.SetClient(mockS3.Object);
        var files = await S3Reader.ListFilesAsync();
        Assert.Equal(2, files.Count);
        Assert.Equal("test1.edi", files[0].Key);
        Assert.Equal("etag1", files[0].ETag);
    }

    [Fact]
    public async Task S3Reader_GetFileAsync_SHouldReturnStream_WhenMocked()
    {
        var mockS3 = new Mock<IAmazonS3>();
        var content = "ISA*00*FAKE~";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        mockS3.Setup(s3 => s3.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new GetObjectResponse { ResponseStream = stream });
        S3Reader.SetClient(mockS3.Object);
        var resultStream = await S3Reader.GetFileAsync("test.edi");

        using var sr = new StreamReader(resultStream);
        var text = sr.ReadToEnd();

        Assert.Contains("ISA*", text);
    }

    [Fact]
    public async Task S3Reader_DownloadFromS3Async_SHouldReturnPath_WhenMocked()
    {
        var mockS3 = new Mock<IAmazonS3>();
        var content = "ISA*00*FAKE~";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        mockS3.Setup(s3 => s3.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new GetObjectResponse { ResponseStream = stream });
        S3Reader.SetClient(mockS3.Object);

        var mockfile = new S3FileInfo("test.edi", "\"etag2\"", DateTime.UtcNow);

        var tempPath = await S3Reader.DownloadFromS3Async(mockfile);
        using var resultStream = File.OpenRead(tempPath);
        using var sr = new StreamReader(resultStream);
        var text = sr.ReadToEnd();

        Assert.Contains("ISA*", text);
    }

}