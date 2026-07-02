using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.SQS;
using Amazon.SQS.Model;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Edi837Ingestion.Configuration;
using Xunit;

namespace integration.Fixtures;

/// <summary>
/// Spins up a Moto container (mock S3 + SQS) via Testcontainers and provisions the same
/// resources <c>infra/init.sh</c> creates for the docker-compose environment: the S3 bucket, the
/// SQS work queue with a dead-letter queue redrive policy, and an S3 → SQS <c>ObjectCreated</c>
/// notification. All resource names, the region, and the credentials come from the integration
/// project's own <c>appsettings.json</c> via <see cref="AppConfiguration.GetAwsOptions"/>; only the
/// endpoint is supplied at runtime, since Testcontainers publishes the container on a random port.
/// </summary>
/// <remarks>
/// The container is ephemeral: a fresh Moto is started per run and reaped by Testcontainers when the
/// run ends. The provisioning is nonetheless idempotent, which keeps it robust regardless of the
/// container's starting state.
/// </remarks>
public sealed class MotoFixture : IAsyncLifetime
{
    private const int MotoPort = 5000;

    private readonly IContainer _moto = new ContainerBuilder("motoserver/moto")
        .WithPortBinding(MotoPort, assignRandomHostPort: true)
        .WithWaitStrategy(Wait.ForUnixContainer()
            .UntilHttpRequestIsSucceeded(request => request.ForPort(MotoPort).ForPath("/moto-api/")))
        .Build();

    /// <summary>The AWS options bound from the integration project's appsettings.json.</summary>
    public AwsOptions Options { get; } = AppConfiguration.Build().GetAwsOptions();

    /// <summary>Base endpoint (host + mapped port) the AWS clients target.</summary>
    public string ServiceUrl { get; private set; } = string.Empty;

    public IAmazonS3 S3 { get; private set; } = null!;

    public IAmazonSQS Sqs { get; private set; } = null!;

    /// <summary>Queue URL of the provisioned main queue.</summary>
    public string QueueUrl { get; private set; } = string.Empty;

    /// <summary>Queue URL of the provisioned dead-letter queue.</summary>
    public string DeadLetterQueueUrl { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _moto.StartAsync();

        // The endpoint is the one value that can't come from config: Testcontainers maps the
        // container's port to a random host port, so it is resolved from the running container.
        ServiceUrl = new UriBuilder(Uri.UriSchemeHttp, _moto.Hostname, _moto.GetMappedPublicPort(MotoPort)).Uri.ToString();
        var credentials = new BasicAWSCredentials(Options.AccessKey, Options.SecretKey);

        S3 = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = ServiceUrl,
            ForcePathStyle = Options.S3.ForcePathStyle,
            AuthenticationRegion = Options.Region,
        });
        Sqs = new AmazonSQSClient(credentials, new AmazonSQSConfig
        {
            ServiceURL = ServiceUrl,
            AuthenticationRegion = Options.Region,
        });

        await ProvisionAsync();
    }

    public async Task DisposeAsync()
    {
        S3?.Dispose();
        Sqs?.Dispose();
        await _moto.DisposeAsync();
    }

    /// <summary>
    /// The C# equivalent of <c>infra/init.sh</c>, driven by <see cref="Options"/>. Every step is
    /// idempotent: the bucket is only created if absent, and re-creating an SQS queue with the same
    /// name and attributes simply returns the existing queue.
    /// </summary>
    private async Task ProvisionAsync()
    {
        var bucket = Options.S3.BucketName;
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(S3, bucket))
            await S3.PutBucketAsync(bucket);

        DeadLetterQueueUrl = (await Sqs.CreateQueueAsync(Options.Sqs.DeadLetterQueueName)).QueueUrl;
        var deadLetterArn = await GetQueueArnAsync(DeadLetterQueueUrl);

        var redrivePolicy =
            $"{{\"deadLetterTargetArn\":\"{deadLetterArn}\",\"maxReceiveCount\":\"{Options.Sqs.MaxReceiveCount}\"}}";
        QueueUrl = (await Sqs.CreateQueueAsync(new CreateQueueRequest
        {
            QueueName = Options.Sqs.QueueName,
            Attributes = new Dictionary<string, string> { ["RedrivePolicy"] = redrivePolicy },
        })).QueueUrl;
        var queueArn = await GetQueueArnAsync(QueueUrl);

        await S3.PutBucketNotificationAsync(new PutBucketNotificationRequest
        {
            BucketName = bucket,
            QueueConfigurations =
            [
                new QueueConfiguration
                {
                    Queue = queueArn,
                    Events = [EventType.ObjectCreatedAll],
                },
            ],
        });
    }

    private async Task<string> GetQueueArnAsync(string queueUrl)
    {
        var attributes = await Sqs.GetQueueAttributesAsync(new GetQueueAttributesRequest
        {
            QueueUrl = queueUrl,
            AttributeNames = ["QueueArn"],
        });
        return attributes.QueueARN;
    }
}
