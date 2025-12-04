using Amazon.S3;

namespace X12EDI.Core.Config
{
    /// <summary>
    /// Represents configuration options for connecting to an S3-compatible object store.
    /// </summary>
    public class S3Options
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the S3 access key.
        /// </summary>
        public string AccessKey { get; set; } = "test";

        /// <summary>
        /// Gets or sets the name of the S3 bucket.
        /// </summary>
        public string BucketName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to force path-style addressing.
        /// </summary>
        public bool ForcePathStyle { get; set; } = false;

        /// <summary>
        /// Gets or sets the AWS region for the S3 bucket.
        /// </summary>
        public string Region { get; set; } = "us-east-1";

        /// <summary>
        /// Gets or sets the S3 secret key.
        /// </summary>
        public string SecretKey { get; set; } = "test";

        /// <summary>
        /// Gets or sets the S3 service URL.
        /// </summary>
        public string ServiceURL { get; set; } = string.Empty;

        #endregion Public Properties
    }
}