namespace Edi837Ingestion.Configuration;

/// <summary>
/// Strongly-typed binding for the EdiFabric license configuration.
/// Bound from the "EdiFabric" section of appsettings.json. The actual
/// <see cref="SerialKey"/> value is supplied via user-secrets in development
/// (dotnet user-secrets set "EdiFabric:SerialKey" "&lt;key&gt;").
/// </summary>
public sealed class EdiFabricOptions
{
    /// <summary>Configuration section name this type binds to.</summary>
    public const string SectionName = "EdiFabric";

    /// <summary>The EdiFabric serial / license key.</summary>
    public string SerialKey { get; set; } = string.Empty;
}
