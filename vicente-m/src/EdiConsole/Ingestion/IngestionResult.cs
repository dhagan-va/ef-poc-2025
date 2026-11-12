namespace EdiConsole.Ingestion;

/// <summary>
/// The summary of one file's ingestion, for logs and the console.
/// </summary>
public sealed record IngestionResult(string FileName, IngestionOutcome Outcome, int ClaimCount, int ErrorCount);
