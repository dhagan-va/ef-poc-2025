using Amazon.S3;

namespace X12EDI.Core.Config
{
    public class S3Options
    {
        #region Public Properties

        public string AccessKey { get; set; } = "test";

        public string BucketName { get; set; } = string.Empty;

        public bool ForcePathStyle { get; set; } = false;
        public string Region { get; set; } = "us-east-1";
        public string SecretKey { get; set; } = "test";
        public string ServiceURL { get; set; } = string.Empty;

        #endregion Public Properties
    }
}