using Edi837Ingestion.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// Design-time factory used by EF Core tooling (<c>dotnet ef migrations</c>). Builds the
/// connection from the same configuration the app uses. Migration scaffolding does not open
/// a connection, so when the SQL password secret is absent on this machine a placeholder is
/// substituted — the real value is supplied at runtime where the app builds the context.
/// </summary>
public sealed class Edi837DbContextFactory : IDesignTimeDbContextFactory<Edi837DbContext>
{
    public Edi837DbContext CreateDbContext(string[] args)
    {
        var configuration = AppConfiguration.Build();

        var sqlServer = configuration.GetSection(SqlServerOptions.SectionName).Get<SqlServerOptions>()
            ?? new SqlServerOptions();

        // Design-time only: `migrations add` builds the model without connecting.
        if (string.IsNullOrWhiteSpace(sqlServer.Password))
            sqlServer.Password = "design-time-placeholder";

        var options = new DbContextOptionsBuilder<Edi837DbContext>()
            .UseSqlServer(sqlServer.ConnectionString)
            .Options;

        return new Edi837DbContext(options);
    }
}
