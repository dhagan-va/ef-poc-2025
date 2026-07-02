namespace Edi837Ingestion.Configuration;

/// <summary>
/// SQS settings for the S3 → queue <c>ObjectCreated</c> notification pipeline, bound from the
/// "Aws:Sqs" subsection of <see cref="AwsOptions"/>. Queue names and the redrive count mirror
/// <c>infra/init.sh</c>.
/// </summary>
public sealed class SqsOptions
{
    /// <summary>Main work queue that receives S3 <c>ObjectCreated</c> notifications.</summary>
    public string QueueName { get; set; } = string.Empty;

    /// <summary>Dead-letter queue that exhausted messages are redriven to.</summary>
    public string DeadLetterQueueName { get; set; } = string.Empty;

    /// <summary>Delivery attempts before a message is moved to the dead-letter queue (redrive <c>maxReceiveCount</c>).</summary>
    public int MaxReceiveCount { get; set; } = 3;
}
