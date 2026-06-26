using Edi837Ingestion.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace test.Configuration;

/// <summary>
/// Covers the logic we own in <see cref="AppConfiguration.GetEdiFabricOptions"/>:
/// binding the "EdiFabric" section and failing fast when the serial key is missing.
/// </summary>
public class EdiFabricOptionsTests
{
    private const string SerialKeyPath =
        $"{EdiFabricOptions.SectionName}:{nameof(EdiFabricOptions.SerialKey)}"; // "EdiFabric:SerialKey"

    private static IConfiguration BuildConfig(Dictionary<string, string?> settings) =>
        new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

    [Fact]
    public void GetEdiFabricOptions_ReturnsSerialKey_WhenConfigured()
    {
        var options = BuildConfig(new() { [SerialKeyPath] = "TEST-KEY" }).GetEdiFabricOptions();

        Assert.Equal("TEST-KEY", options.SerialKey);
    }

    [Theory]
    [InlineData(null)]  // section/key entirely absent
    [InlineData("")]    // present but empty
    [InlineData("   ")] // whitespace only
    public void GetEdiFabricOptions_Throws_WhenSerialKeyMissingOrBlank(string? serialKey)
    {
        var settings = new Dictionary<string, string?>();
        if (serialKey is not null)
            settings[SerialKeyPath] = serialKey;

        Assert.Throws<InvalidOperationException>(() => BuildConfig(settings).GetEdiFabricOptions());
    }
}
