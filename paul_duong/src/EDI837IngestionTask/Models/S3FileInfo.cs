namespace EDI837IngestionTask.Models
{
    public class S3FileInfo
    {
        public string Key { get; set; } = string.Empty;
        public string ETag { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }

        public S3FileInfo(string key, string etag, DateTime? lastModified)
        {
            Key = key;
            ETag = etag;
            LastModified = lastModified ?? DateTime.UtcNow;
        }
    }
}