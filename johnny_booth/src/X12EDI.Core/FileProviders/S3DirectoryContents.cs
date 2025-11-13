using Microsoft.Extensions.FileProviders;
using System.Collections;

namespace X12EDI.Core.FileProviders
{
    public class S3DirectoryContents : IDirectoryContents
    {
        #region Private Fields

        private readonly IEnumerable<IFileInfo> _entries;

        #endregion Private Fields

        #region Public Constructors

        public S3DirectoryContents(IEnumerable<IFileInfo> entries)
        {
            _entries = entries ?? new List<IFileInfo>();
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Exists => true;

        #endregion Public Properties

        #region Public Methods

        public IEnumerator<IFileInfo> GetEnumerator() => _entries.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _entries.GetEnumerator();

        #endregion Public Methods
    }
}