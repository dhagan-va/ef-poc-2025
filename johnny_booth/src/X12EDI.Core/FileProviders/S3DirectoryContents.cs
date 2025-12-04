using Microsoft.Extensions.FileProviders;
using System.Collections;

namespace X12EDI.Core.FileProviders
{
    /// <summary>
    /// Represents the contents of a directory in an S3 bucket.
    /// </summary>
    public class S3DirectoryContents : IDirectoryContents
    {
        #region Private Fields

        private readonly IEnumerable<IFileInfo> _entries;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="S3DirectoryContents"/> class.
        /// </summary>
        /// <param name="entries">The collection of files and directories.</param>
        public S3DirectoryContents(IEnumerable<IFileInfo> entries)
        {
            _entries = entries ?? [];
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the directory exists. Always returns true for S3.
        /// </summary>
        public bool Exists => true;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets an enumerator for the files and directories.
        /// </summary>
        /// <returns>An enumerator for the files and directories.</returns>
        public IEnumerator<IFileInfo> GetEnumerator() => _entries.GetEnumerator();

        /// <summary>
        /// Gets an enumerator for the files and directories.
        /// </summary>
        /// <returns>An enumerator for the files and directories.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _entries.GetEnumerator();

        #endregion Public Methods
    }
}