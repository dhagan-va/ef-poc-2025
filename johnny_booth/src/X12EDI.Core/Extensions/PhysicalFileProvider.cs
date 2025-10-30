using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace X12EDI.Core.Extensions
{
    public class PhysicalFileProvider : IFileProvider
    {
        private readonly string _directory;

        public PhysicalFileProvider(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Directory path must be provided.", nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
            }

            _directory = directory;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var fullPath = Path.Combine(_directory, subpath ?? string.Empty);

            if (!Directory.Exists(fullPath))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            return new PhysicalDirectoryContents(fullPath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var fullPath = Path.Combine(_directory, subpath ?? string.Empty);

            if (File.Exists(fullPath))
            {
                return new PhysicalFileInfo(new FileInfo(fullPath));
            }

            return new NotFoundFileInfo(subpath ?? string.Empty);
        }

        public IChangeToken Watch(string filter)
        {
            var fullPath = Path.Combine(_directory, filter ?? string.Empty);
            var fileInfo = new FileInfo(fullPath);

            return new PollingFileChangeToken(fileInfo);
        }

    }
}
