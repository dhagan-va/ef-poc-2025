namespace EdiConsole.Sourcing;

/// <summary>
/// A file that a source listed and can open later.
/// </summary>
/// <param name="Name">The file name, e.g. 837p-sample.edi. Stored as IngestedFile.FileName.</param>
/// <param name="SourceUri">Where the file lives: a full path for a local folder, s3://bucket/key in Phase 2.</param>
public sealed record EdiFile(string Name, string SourceUri);
