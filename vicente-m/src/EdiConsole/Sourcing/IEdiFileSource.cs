namespace EdiConsole.Sourcing;

/// <summary>
/// Where EDI files come from: a local folder in Phase 1, an S3 bucket in Phase 2.
/// Knows nothing about EDI or the database.
/// </summary>
public interface IEdiFileSource
{
    /// <summary>Lists the files available for ingestion.</summary>
    IAsyncEnumerable<EdiFile> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Opens a file for reading. The caller disposes the stream.</summary>
    Task<Stream> OpenReadAsync(EdiFile file, CancellationToken cancellationToken = default);
}
