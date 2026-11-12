namespace EdiConsole.Ingestion;

/// <summary>
/// What happened to one file in this run. Unlike <see cref="Domain.Tracking.IngestionStatus"/>,
/// which is stored on the file's row, it also covers duplicates, which leave no row at all.
/// </summary>
public enum IngestionOutcome
{
    /// <summary>Stored, with its claims.</summary>
    Accepted,

    /// <summary>Stored, with its issues only.</summary>
    Rejected,

    /// <summary>Skipped: the same content was ingested before. Nothing stored, no EdiFabric operation spent.</summary>
    Duplicate,
}
