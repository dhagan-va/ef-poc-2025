namespace Edi837Ingestion.Configuration;

/// <summary>
/// Strongly-typed binding for the AWS settings shared by the S3 archive and the SQS
/// notification pipeline. Bound from the "Aws" section of appsettings.json.
/// <para>
/// Local development targets the moto mock (docker-compose service <c>moto</c>, published on
/// <c>localhost:5001</c>) via <see cref="ServiceUrl"/>, using the dummy <c>test</c>/<c>test</c>
/// credentials moto accepts. Against real AWS, leave <see cref="ServiceUrl"/> empty so the SDK
/// resolves the regional endpoint from <see cref="Region"/>, and leave the credentials empty so
/// the default credential chain (environment, IAM role, shared profile) supplies them.
/// </para>
/// </summary>
public sealed class AwsOptions
{
    /// <summary>Configuration section name this type binds to.</summary>
    public const string SectionName = "Aws";

    /// <summary>AWS region for all clients (e.g. <c>us-east-1</c>).</summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// Endpoint override for S3-compatible mocks (moto/LocalStack). Empty against real AWS,
    /// where the SDK resolves the regional endpoint from <see cref="Region"/>.
    /// </summary>
    public string ServiceUrl { get; set; } = string.Empty;

    /// <summary>Access key id. Dummy value for moto; prefer the default credential chain in real AWS (leave empty).</summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>Secret access key. Dummy value for moto; prefer the default credential chain in real AWS (leave empty).</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>S3 archive settings.</summary>
    public S3Options S3 { get; set; } = new();

    /// <summary>SQS notification-pipeline settings.</summary>
    public SqsOptions Sqs { get; set; } = new();
}
