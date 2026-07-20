using EdiFabric;

namespace Edi837Ingestion.Configuration;

/// <summary>
/// Applies the EdiFabric license to the current process. EdiFabric holds the license in
/// global static state, so this must be called once at startup before any parsing — the
/// parser itself takes no key and reads that ambient state. Shared by the application
/// entrypoint and the parser tests' license fixture so the license is applied in one place.
/// </summary>
public static class EdiFabricLicense
{
    /// <summary>
    /// Validates and registers the serial key with EdiFabric. The key is validated against
    /// EdiFabric's licensing service, so this requires outbound network access.
    /// </summary>
    public static void Apply(EdiFabricOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        SerialKey.Set(options.SerialKey);
    }
}
