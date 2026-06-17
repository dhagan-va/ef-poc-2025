
using System.ComponentModel.DataAnnotations;

namespace X12EDI837Ingestion.Consumer.Configuration
{

    public sealed class S3Information
    {
        [Required]
        public string Url { get; set; } = string.Empty;

        [Required]
        public string Bucket { get; set; } = string.Empty;

        [Required]
        public string AccessKey { get; set; } = string.Empty;

        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        public bool ForcePathStyle { get; set; } = true;
    }
}
