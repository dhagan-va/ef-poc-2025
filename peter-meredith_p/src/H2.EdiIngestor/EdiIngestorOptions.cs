namespace H2.EdiIngestor;

/// <summary>
/// Options used for test runs / single-file ingestion scenarios.
/// Bind this from configuration or configure at startup.
/// </summary>
public class EdiTestOptions
{
    /// <summary>
    /// Path to the 837 file to ingest.
    /// </summary>
    public string FileName { get; set; } = string.Empty;
}
