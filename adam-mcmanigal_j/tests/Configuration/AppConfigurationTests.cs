using Edi837Ingestion.Configuration;
using Xunit;

namespace test.Configuration;

/// <summary>
/// Covers <see cref="AppConfiguration.Build"/>'s layering pipeline — specifically
/// that environment variables are the highest-precedence source. Per-options
/// binding/validation lives in <see cref="EdiFabricOptionsTests"/> and
/// <see cref="SqlServerOptionsTests"/>.
/// </summary>
public class AppConfigurationTests
{
    private const string SerialKeyEnvVar = "EdiFabric__SerialKey"; // "__" maps to the ":" separator

    [Fact]
    public void Build_LoadsSerialKey_FromEnvironmentVariable()
    {
        // Environment variables are the highest-precedence source, so this also
        // proves an env var wins over appsettings.json / user-secrets.
        var original = Environment.GetEnvironmentVariable(SerialKeyEnvVar);
        try
        {
            Environment.SetEnvironmentVariable(SerialKeyEnvVar, "ENV-KEY");

            var options = AppConfiguration.Build().GetEdiFabricOptions();

            Assert.Equal("ENV-KEY", options.SerialKey);
        }
        finally
        {
            Environment.SetEnvironmentVariable(SerialKeyEnvVar, original);
        }
    }
}
