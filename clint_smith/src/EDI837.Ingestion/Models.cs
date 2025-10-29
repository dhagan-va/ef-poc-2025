namespace EDI837.Ingestion
{
    public class ClaimStaging
    {
        public int Id { get; set; }

        public string? ProviderNPI { get; set; } // optional — see below
        public string? TransactionControlNumber { get; set; } // ST02

        public string ClaimXml { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
    }

    public class AppSettings
    {
        public DatabaseSettings Database { get; set; } = new();
        public EdiSettings EDI { get; set; } = new();
        public S3Settings S3 { get; set; } = new();
    }

    public class DatabaseSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new();
    }

    public class ConnectionStrings
    {
        public string HIPAA_5010_837P { get; set; } = String.Empty;
        public string ClaimStaging { get; set; } = String.Empty;
    }

    public class EdiSettings
    {
        public string Token { get; set; } = String.Empty;
    }

    public class S3Settings
    {
        public string ServiceUrl { get; set; } = String.Empty;
        public string Bucket { get; set; } = String.Empty;
    }
}
