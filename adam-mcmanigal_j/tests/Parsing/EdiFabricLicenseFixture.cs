using Edi837Ingestion.LicenseWarmer;

namespace test.Parsing;

/// <summary>
/// xUnit class fixture that applies the EdiFabric license once before parser tests run.
/// The key is read from the tests' user-secrets (dev) or the EdiFabric__SerialKey
/// environment variable (CI) and fails fast if absent — parsing cannot run unlicensed.
/// </summary>
/// <remarks>
/// Applying the license reads EdiFabric's isolated-storage token cache, which a test host (Rider's
/// runner or VSTest) can read but cannot reliably write the first time. <see cref="LicenseWarmer"/>
/// handles that: it applies in-process when the cache is warm and otherwise warms it in a short-lived
/// child console process first, so a cold cache no longer fails the first run. It resolves the key from
/// the shared user-secrets (dev) or the <c>EdiFabric__SerialKey</c> environment variable (CI).
/// </remarks>
public sealed class EdiFabricLicenseFixture
{
    public EdiFabricLicenseFixture() => LicenseWarmer.EnsureLicensed();
}
