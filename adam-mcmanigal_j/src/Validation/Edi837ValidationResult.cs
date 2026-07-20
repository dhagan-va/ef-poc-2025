namespace Edi837Ingestion.Validation;

/// <summary>
/// The outcome of SNIP-validating one parsed interchange's transaction sets: whether every set passed
/// the configured level and, if not, a per-failure description (variant + ST02 control number + the
/// EdiFabric error detail) for logging and the dead-letter reason.
/// </summary>
public sealed record Edi837ValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    /// <summary>A passing result — either validation was disabled or every transaction set was valid.</summary>
    public static readonly Edi837ValidationResult Valid = new(true, []);

    /// <summary>The failures joined into a single line for a log message or dead-letter reason.</summary>
    public string Summary => string.Join("; ", Errors);
}
