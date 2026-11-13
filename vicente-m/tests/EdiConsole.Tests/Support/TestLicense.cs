using EdiConsole.Configuration;
using EdiConsole.Mapping;
using EdiFabric;
using Microsoft.Extensions.Configuration;

namespace EdiConsole.Tests.Support;

/// <summary>
/// Sets EdiFabric's process-wide license once per test run, from the same sources the app uses:
/// the console app's User Secrets locally, or the <c>EdiFabric__SerialKey</c> environment variable elsewhere
/// (the variable wins, as in the app). The key is never committed.
/// </summary>
internal static class TestLicense
{
    private static readonly Lazy<bool> Applied = new(Apply);

    public static void EnsureSet() => _ = Applied.Value;

    private static bool Apply()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<ClaimMapper>(optional: true)    // the EdiConsole assembly carries the UserSecretsId
            .AddEnvironmentVariables()
            .Build();

        var serialKey = configuration[$"{EdiFabricOptions.SectionName}:{nameof(EdiFabricOptions.SerialKey)}"];
        if (string.IsNullOrWhiteSpace(serialKey))
        {
            throw new InvalidOperationException(
                "EdiFabric:SerialKey is missing. Locally run (in src/EdiConsole): dotnet user-secrets set \"EdiFabric:SerialKey\" \"<your key>\". " +
                "Elsewhere set the EdiFabric__SerialKey environment variable.");
        }

        License.SetSerial(serialKey);
        return true;
    }
}
