using Edi837Ingestion.Configuration;
using Edi837Ingestion.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Xunit;

namespace integration.Fixtures;

/// <summary>
/// Spins up a SQL Server container via the Testcontainers MSSQL module and applies the app's EF Core
/// migrations against it, so tests run against the real schema on a real engine. The database name,
/// login, and password come from the integration project's own <c>appsettings.json</c> via
/// <see cref="AppConfiguration.GetSqlServerOptions"/>; only the endpoint (host + port) is supplied at
/// runtime, since Testcontainers publishes the container on a random port. This mirrors
/// <c>MotoFixture</c>: configuration drives identity, the container supplies the dynamic endpoint.
/// </summary>
/// <remarks>
/// The container is ephemeral: a fresh SQL Server is started per run and reaped by Testcontainers
/// when the run ends. Because the container owns its lifecycle, the SA password is set from the same
/// options value used to build the connection string, keeping the two in sync.
/// </remarks>
public sealed class SqlServerFixture : IAsyncLifetime
{
    private const int SqlServerPort = 1433;

    private readonly SqlServerOptions _options = AppConfiguration.Build().GetSqlServerOptions();
    private readonly MsSqlContainer _container;

    public SqlServerFixture()
    {
        // The container's SA password is the same value the connection string is built from.
        // Image matches the docker-compose db service (SQL Server 2025).
        _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest")
            .WithPassword(_options.Password)
            .Build();
    }

    /// <summary>Connection string to the migrated database on the running container.</summary>
    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        // The endpoint is the one value that can't come from config: Testcontainers maps 1433 to a
        // random host port. Point the configured topology at it and reuse the options' connection-
        // string builder (which carries the target database, login, and TrustServerCertificate).
        _options.Server = _container.Hostname;
        _options.Port = _container.GetMappedPublicPort(SqlServerPort);
        ConnectionString = _options.ConnectionString;

        // Migrate() creates the target database (it does not yet exist on a fresh container) and
        // applies every migration, leaving the full 837-ingestion schema in place.
        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();

    /// <summary>Builds a fresh context bound to the migrated container database.</summary>
    public Edi837DbContext CreateContext() =>
        new(new DbContextOptionsBuilder<Edi837DbContext>()
            .UseSqlServer(ConnectionString)
            .Options);
}
