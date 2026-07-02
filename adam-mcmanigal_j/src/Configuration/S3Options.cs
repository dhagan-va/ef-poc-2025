namespace Edi837Ingestion.Configuration;

/// <summary>
/// S3 settings for the raw-file archive, bound from the "Aws:S3" subsection of
/// <see cref="AwsOptions"/>. The bucket mirrors <c>infra/init.sh</c>; the archived raw 837
/// files are referenced from the ledger via <c>IngestedInterchange.PayloadS3Key</c>.
/// </summary>
public sealed class S3Options
{
    /// <summary>Bucket that stores the archived raw 837 files.</summary>
    public string BucketName { get; set; } = string.Empty;

    /// <summary>
    /// Use path-style addressing (bucket in the URL path rather than the host name). Required by
    /// moto/LocalStack and other S3-compatible endpoints; harmless against real AWS.
    /// </summary>
    public bool ForcePathStyle { get; set; } = true;
}
