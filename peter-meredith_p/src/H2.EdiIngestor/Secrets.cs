namespace H2.EdiIngestor;

public class Secrets
{
    public Secrets() { }

    /// <summary>
    /// Database connection string. Bind from configuration or user-secrets.
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;
}
