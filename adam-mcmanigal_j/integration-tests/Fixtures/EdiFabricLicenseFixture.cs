using Edi837Ingestion.LicenseWarmer;

namespace integration.Fixtures;

/// <summary>
/// Applies the EdiFabric license once before any ingestion test parses an 837. The key is read from
/// the shared user-secrets (dev) or the <c>EdiFabric__SerialKey</c> environment variable (CI) and
/// fails fast if absent — parsing cannot run unlicensed.
/// </summary>
/// <remarks>
/// Delegates to <see cref="LicenseWarmer"/>, which applies the license in-process when EdiFabric's
/// isolated-storage token cache is warm and otherwise warms it in a short-lived child console process
/// first — a test host can read that cache but cannot reliably write it the first time. Applied exactly
/// once through this single collection fixture rather than per test class.
/// </remarks>
public sealed class EdiFabricLicenseFixture
{
    public EdiFabricLicenseFixture() => LicenseWarmer.EnsureLicensed();
}
