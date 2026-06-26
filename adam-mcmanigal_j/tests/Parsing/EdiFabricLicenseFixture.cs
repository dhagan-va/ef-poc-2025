using Edi837Ingestion.Configuration;
using Microsoft.Extensions.Configuration;

namespace test.Parsing;

/// <summary>
/// xUnit class fixture that applies the EdiFabric license once before parser tests run.
/// The key is read from the tests' user-secrets (dev) or the EdiFabric__SerialKey
/// environment variable (CI) and fails fast if absent — parsing cannot run unlicensed.
/// </summary>
/// <remarks>
/// Configuration is built explicitly against this assembly rather than via
/// <see cref="AppConfiguration.Build"/>, because under <c>dotnet test</c> the entry
/// assembly is the test host (which carries no UserSecretsId), so the tests' user-secrets
/// would otherwise not be found.
/// </remarks>
public sealed class EdiFabricLicenseFixture
{
    public EdiFabricLicenseFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(EdiFabricLicenseFixture).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Reuses the app's validation (throws a clear InvalidOperationException if missing)
        // and the same license-application path the app uses at startup.
        EdiFabricLicense.Apply(configuration.GetEdiFabricOptions());
    }
}
