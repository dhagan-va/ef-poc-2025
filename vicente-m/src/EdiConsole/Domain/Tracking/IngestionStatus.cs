namespace EdiConsole.Domain.Tracking;

/// <summary>
/// Outcome of processing one file. Stored as text (e.g. 'Rejected').
/// </summary>
public enum IngestionStatus
{
    /// <summary>All transactions passed validation and their claims were stored.</summary>
    Accepted,

    /// <summary>Validation failed, the content isn't a supported 837P, or the file is empty. Only issues are stored.</summary>
    Rejected,
}
