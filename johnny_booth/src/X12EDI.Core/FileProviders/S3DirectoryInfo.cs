using Microsoft.Extensions.FileProviders;

public class S3DirectoryInfo : IFileInfo
{
    #region Private Fields

    private readonly string _prefix;

    #endregion Private Fields

    #region Public Constructors

    public S3DirectoryInfo(string prefix)
    {
        _prefix = prefix;
    }

    #endregion Public Constructors

    #region Public Properties

    // --- IFileInfo Properties ---
    public bool Exists => true; // If we found a prefix, it exists

    public bool IsDirectory => true;
    public DateTimeOffset LastModified => DateTimeOffset.MinValue;
    public long Length => -1; // Directories have no length
    public string Name => Path.GetFileName(_prefix.TrimEnd('/'));
    public string? PhysicalPath => null;

    #endregion Public Properties

    #region Public Methods

    // --- IFileInfo Method ---
    public Stream CreateReadStream()
    {
        throw new InvalidOperationException("Cannot create a stream for a directory.");
    }

    #endregion Public Methods
}