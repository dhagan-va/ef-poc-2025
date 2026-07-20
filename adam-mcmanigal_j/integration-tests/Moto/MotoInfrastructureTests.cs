using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.SQS.Model;
using integration.Fixtures;
using Xunit;

namespace integration.Moto;

/// <summary>
/// Smoke tests asserting the <see cref="MotoFixture"/> provisions the resources described by the
/// integration project's Aws configuration — the config-driven equivalent of confirming
/// <c>infra/init.sh</c> ran. These require Docker (Testcontainers starts the Moto image).
/// </summary>
[Collection("Moto")]
public sealed class MotoInfrastructureTests(MotoFixture moto)
{
    [Fact]
    public async Task Bucket_IsProvisioned()
    {
        Assert.True(await AmazonS3Util.DoesS3BucketExistV2Async(moto.S3, moto.Options.S3.BucketName));
    }

    [Fact]
    public async Task Queues_AreProvisioned()
    {
        var queue = await moto.Sqs.GetQueueUrlAsync(moto.Options.Sqs.QueueName);
        var deadLetter = await moto.Sqs.GetQueueUrlAsync(moto.Options.Sqs.DeadLetterQueueName);

        Assert.Equal(moto.QueueUrl, queue.QueueUrl);
        Assert.Equal(moto.DeadLetterQueueUrl, deadLetter.QueueUrl);
    }

    [Fact]
    public async Task WorkQueue_RedrivesToDeadLetterQueue()
    {
        var attributes = await moto.Sqs.GetQueueAttributesAsync(new GetQueueAttributesRequest
        {
            QueueUrl = moto.QueueUrl,
            AttributeNames = ["RedrivePolicy"],
        });

        Assert.Contains(moto.Options.Sqs.DeadLetterQueueName, attributes.Attributes["RedrivePolicy"]);
    }

    [Fact]
    public async Task Bucket_StoresAndReadsBackAnObject()
    {
        const string key = "smoke/hello.txt";

        await moto.S3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = moto.Options.S3.BucketName,
            Key = key,
            ContentBody = "hello",
        });

        using var response = await moto.S3.GetObjectAsync(moto.Options.S3.BucketName, key);
        using var reader = new StreamReader(response.ResponseStream);

        Assert.Equal("hello", await reader.ReadToEndAsync());
    }
}
