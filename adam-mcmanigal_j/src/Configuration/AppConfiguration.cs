using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Edi837Ingestion.Configuration;

/// <summary>
/// Builds application configuration from appsettings.json plus user-secrets,
/// and exposes strongly-typed access to the bound option types. Shared by both
/// the src and tests entry points so the configuration wiring lives in one place.
/// </summary>
public static class AppConfiguration
{
    /// <summary>
    /// Builds an <see cref="IConfiguration"/> from the running app's
    /// appsettings.json, its user-secrets store, and environment variables.
    /// User-secrets are resolved against the entry assembly, so each project
    /// loads its own &lt;UserSecretsId&gt;. Precedence (lowest to highest):
    /// appsettings.json → user-secrets (dev only) → environment variables
    /// (CI/CD and deployment, e.g. EdiFabric__SerialKey).
    /// </summary>
    public static IConfiguration Build() =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddUserSecrets(Assembly.GetEntryAssembly()!, optional: true)
            .AddEnvironmentVariables()
            .Build();

    /// <summary>
    /// Binds the "EdiFabric" section to a strongly-typed options object.
    /// Throws if the serial key is missing so misconfiguration fails fast.
    /// </summary>
    public static EdiFabricOptions GetEdiFabricOptions(this IConfiguration configuration)
    {
        var options = configuration.GetSection(EdiFabricOptions.SectionName).Get<EdiFabricOptions>()
            ?? new EdiFabricOptions();

        if (string.IsNullOrWhiteSpace(options.SerialKey))
            throw new InvalidOperationException(
                "EdiFabric serial key is not configured. Set it with one of:" + Environment.NewLine +
                $"  - development:  dotnet user-secrets set \"{EdiFabricOptions.SectionName}:{nameof(EdiFabricOptions.SerialKey)}\" \"<key>\"" + Environment.NewLine +
                $"  - CI/deployment: set the environment variable {EdiFabricOptions.SectionName}__{nameof(EdiFabricOptions.SerialKey)}=<key>");

        return options;
    }

    /// <summary>
    /// Binds the "SqlServer" section to a strongly-typed options object. Throws if
    /// the non-secret topology (server/database/user) or the secret password is
    /// missing, so misconfiguration fails fast.
    /// </summary>
    public static SqlServerOptions GetSqlServerOptions(this IConfiguration configuration)
    {
        var options = configuration.GetSection(SqlServerOptions.SectionName).Get<SqlServerOptions>()
            ?? new SqlServerOptions();

        if (string.IsNullOrWhiteSpace(options.Server) ||
            string.IsNullOrWhiteSpace(options.Database) ||
            string.IsNullOrWhiteSpace(options.User))
            throw new InvalidOperationException(
                $"SQL Server connection is not fully configured. Set the \"{SqlServerOptions.SectionName}\" " +
                "section (Server, Database, User) in appsettings.json.");

        if (string.IsNullOrWhiteSpace(options.Password))
            throw new InvalidOperationException(
                "SQL Server password is not configured. Set it with one of:" + Environment.NewLine +
                $"  - development:  dotnet user-secrets set \"{SqlServerOptions.SectionName}:{nameof(SqlServerOptions.Password)}\" \"<password>\"" + Environment.NewLine +
                $"  - CI/deployment: set the environment variable {SqlServerOptions.SectionName}__{nameof(SqlServerOptions.Password)}=<password>");

        return options;
    }
}
