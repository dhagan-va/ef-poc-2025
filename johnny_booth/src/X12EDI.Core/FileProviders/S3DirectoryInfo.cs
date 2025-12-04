using Microsoft.Extensions.FileProviders;

namespace X12EDI.Core.FileProviders
{

    /// <summary>
    /// Represents a directory in an S3 bucket, identified by a common prefix.
    /// </summary>
    public class S3DirectoryInfo : IFileInfo
    {
        #region Private Fields

        private readonly string _prefix;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="S3DirectoryInfo"/> class.
        /// </summary>
        /// <param name="prefix">The common prefix representing the directory.</param>
        public S3DirectoryInfo(string prefix)
        {
            _prefix = prefix;
        }

        #endregion Public Constructors

        #region Public Properties

        // --- IFileInfo Properties ---
        /// <summary>
        /// Gets a value indicating whether the directory exists. Always returns true for S3 prefixes.
        /// </summary>
        public bool Exists => true; // If we found a prefix, it exists

        /// <summary>
        /// Gets a value indicating that this is a directory. Always returns true.
        /// </summary>
        public bool IsDirectory => true;

        /// <summary>
        /// Gets the last modified date, which is not applicable for S3 directories.
        /// </summary>
        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        /// <summary>
        /// Gets the length, which is not applicable for directories.
        /// </summary>
        public long Length => -1; // Directories have no length

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        public string Name => Path.GetFileName(_prefix.TrimEnd('/'));

        /// <summary>
        /// Gets the physical path, which is null for S3 directories.
        /// </summary>
        public string? PhysicalPath => null;

        #endregion Public Properties

        #region Public Methods

        // --- IFileInfo Method ---
        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> as creating a stream for a directory is not supported.
        /// </summary>
        /// <returns>This method does not return a value.</returns>
        public Stream CreateReadStream()
        {
            throw new InvalidOperationException("Cannot create a stream for a directory.");
        }

        #endregion Public Methods
    }
}