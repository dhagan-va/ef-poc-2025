using Edi837Ingestion.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace test.Configuration;

/// <summary>
/// Covers the logic we own in <see cref="AppConfiguration.GetSqlServerOptions"/>:
/// binding the "SqlServer" section and failing fast when the topology or the
/// secret password is missing. The composition of the connection string itself is
/// <see cref="Microsoft.Data.SqlClient.SqlConnectionStringBuilder"/>'s job and is
/// not asserted here.
/// </summary>
public class SqlServerOptionsTests
{
    private static Dictionary<string, string?> FullSettings() => new()
    {
        ["SqlServer:Server"] = "localhost",
        ["SqlServer:Port"] = "1433",
        ["SqlServer:Database"] = "Edi837Ingestion",
        ["SqlServer:User"] = "sa",
        ["SqlServer:Password"] = "p@ssw0rd",
        ["SqlServer:TrustServerCertificate"] = "true",
    };

    private static IConfiguration BuildConfig(Dictionary<string, string?> settings) =>
        new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

    [Fact]
    public void GetSqlServerOptions_BindsSection_WhenConfigured()
    {
        var options = BuildConfig(FullSettings()).GetSqlServerOptions();

        Assert.Equal("localhost", options.Server);
        Assert.Equal(1433, options.Port);
        Assert.Equal("Edi837Ingestion", options.Database);
        Assert.Equal("sa", options.User);
        Assert.False(string.IsNullOrWhiteSpace(options.ConnectionString)); // smoke check only
    }

    [Theory]
    [InlineData(null)]  // password key absent
    [InlineData("")]    // present but empty
    [InlineData("   ")] // whitespace only
    public void GetSqlServerOptions_Throws_WhenPasswordMissingOrBlank(string? password)
    {
        var settings = FullSettings();
        if (password is null) settings.Remove("SqlServer:Password");
        else settings["SqlServer:Password"] = password;

        Assert.Throws<InvalidOperationException>(() => BuildConfig(settings).GetSqlServerOptions());
    }

    // Server is omitted here: SqlServerOptions.Server defaults to "localhost", so its
    // absence in config is not "missing". Database and User default to empty, so they are.
    [Theory]
    [InlineData("SqlServer:Database")]
    [InlineData("SqlServer:User")]
    public void GetSqlServerOptions_Throws_WhenTopologyFieldMissing(string keyToRemove)
    {
        var settings = FullSettings();
        settings.Remove(keyToRemove);

        Assert.Throws<InvalidOperationException>(() => BuildConfig(settings).GetSqlServerOptions());
    }
}
