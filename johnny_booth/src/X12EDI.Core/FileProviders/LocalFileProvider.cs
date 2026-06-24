using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace X12EDI.Core.FileProviders
{
    /// <summary>
    /// An implementation of <see cref="IFileProvider"/> that provides file access to a physical file system.
    /// </summary>
    public class LocalFileProvider : IFileProvider
    {
        #region Private Fields

        private readonly string _directory;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileProvider"/> class.
        /// </summary>
        /// <param name="directory">The root directory for this provider.</param>
        /// <exception cref="ArgumentException">Thrown if the directory path is null, empty, or whitespace.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown if the specified directory does not exist.</exception>
        public LocalFileProvider(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Directory path must be provided.", nameof(directory));
            }

            _directory = Path.GetFullPath(directory);

            if (!Directory.Exists(_directory))
            {
                throw new DirectoryNotFoundException($"The directory '{_directory}' does not exist.");
            }
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Enumerates a directory at the given path.
        /// </summary>
        /// <param name="subpath">The path relative to the root directory.</param>
        /// <returns>The contents of the directory. Returns <see cref="NotFoundDirectoryContents"/> if the directory does not exist.</returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var fullPath = Path.Combine(_directory, subpath ?? string.Empty);

            if (!Directory.Exists(fullPath))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            return new PhysicalDirectoryContents(fullPath);
        }

        /// <summary>
        /// Locates a file at the given path.
        /// </summary>
        /// <param name="subpath">The path relative to the root directory.</param>
        /// <returns>The file information. Returns a <see cref="NotFoundFileInfo"/> if the file does not exist.</returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            var fullPath = Path.Combine(_directory, subpath ?? string.Empty);

            if (File.Exists(fullPath))
            {
                return new PhysicalFileInfo(new FileInfo(fullPath));
            }

            return new NotFoundFileInfo(subpath ?? string.Empty);
        }

        /// <summary>
        /// Creates an <see cref="IChangeToken"/> for the specified filter.
        /// </summary>
        /// <param name="filter">A filter string for files and directories to watch. This can include wildcards.</param>
        /// <returns>An <see cref="IChangeToken"/> that is notified when a file matching the filter is added, modified, or deleted.</returns>
        public IChangeToken Watch(string filter)
        {
            var fullPath = Path.Combine(_directory, filter ?? string.Empty);
            var fileInfo = new FileInfo(fullPath);

            return new PollingFileChangeToken(fileInfo);
        }

        #endregion Public Methods
    }
}