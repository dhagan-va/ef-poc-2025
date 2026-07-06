using System.Reflection;
using DotNetEnv;
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
    /// Builds an <see cref="IConfiguration"/> from the running app's appsettings.json, its
    /// environment-specific override (appsettings.{Environment}.json), its user-secrets store, a
    /// local <c>.env</c> file, and environment variables. The environment name comes from
    /// <c>DOTNET_ENVIRONMENT</c> (or <c>ASPNETCORE_ENVIRONMENT</c>), defaulting to <c>Development</c>
    /// so <c>Production</c> is never entered by accident — it must be selected explicitly (e.g. in
    /// CI/CD or a deployment). The compose <c>app</c> service sets it to <c>Docker</c> for the
    /// in-container endpoints; the integration tests set it to <c>Testing</c>.
    /// User-secrets are resolved against the entry assembly, so each project loads its own
    /// &lt;UserSecretsId&gt;. Precedence (lowest to highest): appsettings.json →
    /// appsettings.{Environment}.json → user-secrets (dev only) → <c>.env</c> / environment variables.
    /// </summary>
    /// <remarks>
    /// The <c>.env</c> file (gitignored) is loaded into process environment variables so the same
    /// local secrets file feeds the app, the tests, and docker compose. It is searched for up the
    /// directory tree, and existing environment variables are never overwritten, so a real environment
    /// (CI or a container) still wins over the file. Absent (as in a published container), it is a no-op.
    /// </remarks>
    public static IConfiguration Build()
    {
        Env.NoClobber().TraversePath().Load();

        var environment =
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Development";

        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddUserSecrets(Assembly.GetEntryAssembly()!, optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

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

    /// <summary>
    /// Binds the "Aws" section (shared connection settings plus the nested S3 and SQS
    /// subsections) to a strongly-typed options object. Throws if the region or the S3/SQS
    /// resource names are missing, so misconfiguration fails fast.
    /// </summary>
    public static AwsOptions GetAwsOptions(this IConfiguration configuration)
    {
        var options = configuration.GetSection(AwsOptions.SectionName).Get<AwsOptions>()
            ?? new AwsOptions();

        if (string.IsNullOrWhiteSpace(options.Region))
            throw new InvalidOperationException(
                $"AWS region is not configured. Set \"{AwsOptions.SectionName}:{nameof(AwsOptions.Region)}\" in appsettings.json.");

        if (string.IsNullOrWhiteSpace(options.S3.BucketName))
            throw new InvalidOperationException(
                $"S3 bucket is not configured. Set \"{AwsOptions.SectionName}:{nameof(AwsOptions.S3)}:{nameof(S3Options.BucketName)}\" in appsettings.json.");

        if (string.IsNullOrWhiteSpace(options.Sqs.QueueName) ||
            string.IsNullOrWhiteSpace(options.Sqs.DeadLetterQueueName))
            throw new InvalidOperationException(
                $"SQS queues are not configured. Set \"{AwsOptions.SectionName}:{nameof(AwsOptions.Sqs)}:{nameof(SqsOptions.QueueName)}\" and " +
                $"\"{AwsOptions.SectionName}:{nameof(AwsOptions.Sqs)}:{nameof(SqsOptions.DeadLetterQueueName)}\" in appsettings.json.");

        return options;
    }
}
