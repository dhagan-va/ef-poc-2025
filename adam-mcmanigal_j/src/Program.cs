using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using Edi837Ingestion.Configuration;
using Edi837Ingestion.Ingestion;
using Edi837Ingestion.Parsing;
using Edi837Ingestion.Persistence;
using Edi837Ingestion.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configuration = AppConfiguration.Build();
var aws = configuration.GetAwsOptions();
var sqlServer = configuration.GetSqlServerOptions();
var ediFabric = configuration.GetEdiFabricOptions();

// Apply the EdiFabric license once at startup, before any parsing.
EdiFabricLicense.Apply(ediFabric);

// Create the AWS clients before the host so the SQS queue names from config can be resolved to their
// URLs up front — IngestionService is constructed with the resolved URLs. The same instances are then
// registered as singletons, so the host owns their disposal.
var s3 = CreateS3Client(aws);
var sqs = CreateSqsClient(aws);
var queueUrl = (await sqs.GetQueueUrlAsync(aws.Sqs.QueueName)).QueueUrl;
var deadLetterQueueUrl = (await sqs.GetQueueUrlAsync(aws.Sqs.DeadLetterQueueName)).QueueUrl;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(s3);
builder.Services.AddSingleton(sqs);
builder.Services.AddSingleton<Edi837Parser>();

// The SNIP validator is configured once from the EdiFabric options; a file that fails validation at
// this level is dead-lettered by the ingestion service rather than persisted.
builder.Services.AddSingleton(new Edi837Validator(ediFabric.ValidationLevel));

// A context factory, not a scoped context: IngestionService opens one short-lived context per SQS
// message, which is what lets it (and the IngestionWorker that drives it) be a singleton.
builder.Services.AddDbContextFactory<Edi837DbContext>(options =>
    options.UseSqlServer(sqlServer.ConnectionString));

// The resolved queue URLs and poll wait travel together as one options object, so IngestionService
// takes only its genuine dependencies and needs no construction factory.
builder.Services.AddSingleton(new IngestionOptions
{
    QueueUrl = queueUrl,
    DeadLetterQueueUrl = deadLetterQueueUrl,
    ReceiveWaitTimeSeconds = aws.Sqs.ReceiveWaitTimeSeconds,
});
builder.Services.AddSingleton<IngestionService>();

builder.Services.AddHostedService<IngestionWorker>();

var host = builder.Build();
await host.RunAsync();

static IAmazonS3 CreateS3Client(AwsOptions aws)
{
    var config = new AmazonS3Config
    {
        AuthenticationRegion = aws.Region,
        ForcePathStyle = aws.S3.ForcePathStyle,
    };
    ConfigureEndpoint(config, aws);
    return HasStaticCredentials(aws)
        ? new AmazonS3Client(new BasicAWSCredentials(aws.AccessKey, aws.SecretKey), config)
        : new AmazonS3Client(config);
}

static IAmazonSQS CreateSqsClient(AwsOptions aws)
{
    var config = new AmazonSQSConfig { AuthenticationRegion = aws.Region };
    ConfigureEndpoint(config, aws);
    return HasStaticCredentials(aws)
        ? new AmazonSQSClient(new BasicAWSCredentials(aws.AccessKey, aws.SecretKey), config)
        : new AmazonSQSClient(config);
}

// Local mocks (moto/LocalStack) set ServiceUrl; against real AWS it is empty and the region resolves
// the endpoint. Credentials work the same way: static keys for the mock, or — when they are left
// blank — the default credential chain (environment, IAM role, shared profile).
static void ConfigureEndpoint(ClientConfig config, AwsOptions aws)
{
    if (!string.IsNullOrWhiteSpace(aws.ServiceUrl))
        config.ServiceURL = aws.ServiceUrl;
    else
        config.RegionEndpoint = RegionEndpoint.GetBySystemName(aws.Region);
}

static bool HasStaticCredentials(AwsOptions aws) =>
    !string.IsNullOrWhiteSpace(aws.AccessKey) && !string.IsNullOrWhiteSpace(aws.SecretKey);
