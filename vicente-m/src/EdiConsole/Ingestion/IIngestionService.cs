using EdiConsole.Sourcing;

namespace EdiConsole.Ingestion;

/// <summary>
/// The facade: runs the whole workflow for a file (dedupe, parse, validate, map, save)
/// and applies the policy: a file is accepted only if everything in it is valid.
/// </summary>
public interface IIngestionService
{
    Task<IngestionResult> IngestAsync(EdiFile file, CancellationToken cancellationToken = default);
}
