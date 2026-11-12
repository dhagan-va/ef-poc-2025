using EdiConsole.Domain.Tracking;

namespace EdiConsole.Persistence;

/// <summary>
/// Stores ingestion results. Callers never see EF Core.
/// </summary>
public interface IIngestionStore
{
    /// <summary>True if a file with this content (SHA-256, hex) was ingested before.</summary>
    Task<bool> IsAlreadyIngestedAsync(string sha256, CancellationToken cancellationToken = default);

    /// <summary>Saves the file with its claims and issues: all of it, or nothing.</summary>
    Task SaveAsync(IngestedFile file, CancellationToken cancellationToken = default);
}
