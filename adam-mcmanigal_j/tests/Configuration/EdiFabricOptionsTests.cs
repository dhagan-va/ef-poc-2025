using Edi837Ingestion.Configuration;
using EdiFabric.Core.Model.Edi;
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

    private const string ValidationLevelPath =
        $"{EdiFabricOptions.SectionName}:{nameof(EdiFabricOptions.ValidationLevel)}"; // "EdiFabric:ValidationLevel"

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

    [Theory]
    [InlineData("SyntaxOnly_SNIP1", ValidationLevel.SyntaxOnly_SNIP1)]
    [InlineData("LimitsAndCodes_SNIP2", ValidationLevel.LimitsAndCodes_SNIP2)]
    [InlineData("Balancing_SNIP3", ValidationLevel.Balancing_SNIP3)]
    [InlineData("InterSegment_SNIP4", ValidationLevel.InterSegment_SNIP4)]
    public void GetEdiFabricOptions_BindsValidationLevel_ByMemberName(
        string configured, ValidationLevel expected)
    {
        var options = BuildConfig(new()
        {
            [SerialKeyPath] = "TEST-KEY",
            [ValidationLevelPath] = configured,
        }).GetEdiFabricOptions();

        Assert.Equal(expected, options.ValidationLevel);
    }

    [Fact]
    public void GetEdiFabricOptions_DefaultsValidationLevelToNull_WhenAbsent()
    {
        var options = BuildConfig(new() { [SerialKeyPath] = "TEST-KEY" }).GetEdiFabricOptions();

        Assert.Null(options.ValidationLevel);
    }

    [Fact]
    public void GetEdiFabricOptions_Throws_WhenValidationLevelIsNotAKnownMember()
    {
        var settings = new Dictionary<string, string?>
        {
            [SerialKeyPath] = "TEST-KEY",
            [ValidationLevelPath] = "NotALevel",
        };

        Assert.Throws<InvalidOperationException>(() => BuildConfig(settings).GetEdiFabricOptions());
    }
}
