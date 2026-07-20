using Microsoft.Data.SqlClient;

namespace Edi837Ingestion.Configuration;

/// <summary>
/// Strongly-typed binding for the SQL Server connection settings. The non-secret
/// topology (server, port, database, user) is bound from the "SqlServer" section
/// of appsettings.json; the <see cref="Password"/> is supplied via user-secrets
/// (dev) or an environment variable (CI/deployment), so it stays out of source
/// control. <see cref="ConnectionString"/> assembles the components into a
/// properly-escaped ADO.NET connection string.
/// </summary>
public sealed class SqlServerOptions
{
    /// <summary>Configuration section name this type binds to.</summary>
    public const string SectionName = "SqlServer";

    /// <summary>Host name or IP of the SQL Server instance.</summary>
    public string Server { get; set; } = "localhost";

    /// <summary>TCP port the instance listens on.</summary>
    public int Port { get; set; } = 1433;

    /// <summary>Target database (initial catalog).</summary>
    public string Database { get; set; } = string.Empty;

    /// <summary>SQL login user id.</summary>
    public string User { get; set; } = string.Empty;

    /// <summary>SQL login password. Secret — supplied via user-secrets or environment variable.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether to trust the server certificate without validation. True is typical
    /// for local/dev (self-signed certs); set false where a trusted cert is in place.
    /// </summary>
    public bool TrustServerCertificate { get; set; } = true;

    /// <summary>The assembled ADO.NET connection string for this configuration.</summary>
    public string ConnectionString => new SqlConnectionStringBuilder
    {
        DataSource = $"{Server},{Port}",
        InitialCatalog = Database,
        UserID = User,
        Password = Password,
        TrustServerCertificate = TrustServerCertificate,
    }.ConnectionString;
}
