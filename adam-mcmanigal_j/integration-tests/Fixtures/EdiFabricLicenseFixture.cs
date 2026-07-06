using Edi837Ingestion.Configuration;
using Microsoft.Extensions.Configuration;

namespace integration.Fixtures;

/// <summary>
/// Applies the EdiFabric license once before any ingestion test parses an 837. The key is read from
/// the shared user-secrets (dev) or the <c>EdiFabric__SerialKey</c> environment variable (CI) and
/// fails fast if absent — parsing cannot run unlicensed. Mirrors the unit-test project's fixture; the
/// integration project carries the same <c>UserSecretsId</c>, so it resolves the same stored key.
/// </summary>
/// <remarks>
/// Configuration is built explicitly against this assembly rather than via
/// <see cref="AppConfiguration.Build"/>, because under <c>dotnet test</c> the entry assembly is the
/// test host (which carries no UserSecretsId), so the shared user-secrets would otherwise not be found.
/// EdiFabric's <c>SerialKey.Set</c> is global and validates over the network, so it is applied exactly
/// once through this single collection fixture rather than per test class.
/// </remarks>
public sealed class EdiFabricLicenseFixture
{
    public EdiFabricLicenseFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(EdiFabricLicenseFixture).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

        EdiFabricLicense.Apply(configuration.GetEdiFabricOptions());
    }
}
