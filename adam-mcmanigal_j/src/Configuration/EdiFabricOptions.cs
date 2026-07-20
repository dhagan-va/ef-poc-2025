using EdiFabric.Core.Model.Edi;

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

    /// <summary>
    /// The WEDI SNIP validation level enforced on each parsed 837 transaction set at ingestion, bound
    /// by member name from <c>EdiFabric:ValidationLevel</c> (e.g. <c>LimitsAndCodes_SNIP2</c>). EdiFabric
    /// covers SNIP levels 1–4. <see langword="null"/> (the key absent, or explicitly null) disables
    /// validation. A file that fails validation at this level is dead-lettered rather than persisted.
    /// </summary>
    public ValidationLevel? ValidationLevel { get; set; }
}
