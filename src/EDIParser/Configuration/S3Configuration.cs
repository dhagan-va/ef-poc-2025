namespace EdiParser.Configuration;

public class S3Configuration
{
    public string? ServiceUrl { get; set; }
    public string? Bucket { get; set; }
    public string? s3AccessKeyId { get; set; }
    public string? s3SecretAccessKey { get; set; }
}
    