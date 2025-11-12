using Amazon.S3;
using Amazon.S3.Model;
using EDI837.Ingestion.Gateways;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EDI837.Ingestion.Tests;

public class S3GatewayTests : TestBase
{
    private readonly Mock<IAmazonS3> _mockS3 = new(MockBehavior.Strict);
    private readonly S3Gateway _gateway;
    private readonly string _bucket;

    public S3GatewayTests()
    {
        _gateway = new S3Gateway(_mockS3.Object, NullLogger<S3Gateway>.Instance);
        _bucket = Configuration["S3:Bucket"]
            ?? throw new InvalidOperationException("Missing S3 bucket in test settings.");
    }

    [Fact]
    public async Task ListFilesAsync_ReturnsExpectedObjects()
    {
        var expectedObjects = new List<S3Object>
        {
            new() { Key = "incoming/test1.edi" },
            new() { Key = "incoming/test2.edi" }
        };

        _mockS3
            .Setup(s =>
                s.ListObjectsV2Async(
                    It.Is<ListObjectsV2Request>(r =>
                        r.BucketName == _bucket && r.Prefix == "incoming/"
                    ),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new ListObjectsV2Response { S3Objects = expectedObjects });

        var result = await _gateway.ListFilesAsync(_bucket, "incoming/");

        Assert.Equal(2, result.Count);
        Assert.Equal("incoming/test1.edi", result[0].Key);
        Assert.Equal("incoming/test2.edi", result[1].Key);
        _mockS3.VerifyAll();
    }

    [Fact]
    public async Task GetFileContentAsync_ReturnsContent()
    {
        const string key = "incoming/test.edi";
        const string payload = "EDI DATA";
        var response = new GetObjectResponse
        {
            ResponseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload))
        };

        _mockS3
            .Setup(s => s.GetObjectAsync(_bucket, key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _gateway.GetFileContentAsync(_bucket, key);

        Assert.Equal(payload, result);
        _mockS3.VerifyAll();
    }

    [Fact]
    public async Task GetFileStreamAsync_ReturnsUnderlyingStream()
    {
        const string key = "incoming/test.edi";
        var stream = new MemoryStream([1, 2, 3]);
        var response = new GetObjectResponse { ResponseStream = stream };

        _mockS3
            .Setup(s => s.GetObjectAsync(_bucket, key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _gateway.GetFileStreamAsync(_bucket, key);

        Assert.Same(stream, result);
        _mockS3.VerifyAll();
    }

    [Fact]
    public async Task UploadFileAsync_PutsObject()
    {
        const string key = "incoming/new.edi";
        const string content = "EDI CONTENT";

        _mockS3
            .Setup(s =>
                s.PutObjectAsync(
                    It.Is<PutObjectRequest>(r => r.BucketName == _bucket && r.Key == key),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new PutObjectResponse());

        await _gateway.UploadFileAsync(_bucket, key, content);

        _mockS3.VerifyAll();
    }

    [Fact]
    public async Task DeleteFileAsync_RemovesObject()
    {
        const string key = "incoming/old.edi";

        _mockS3
            .Setup(s =>
                s.DeleteObjectAsync(
                    It.Is<DeleteObjectRequest>(r =>
                        r.BucketName == _bucket && r.Key == key
                    ),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new DeleteObjectResponse());

        await _gateway.DeleteFileAsync(_bucket, key);

        _mockS3.VerifyAll();
    }
}
