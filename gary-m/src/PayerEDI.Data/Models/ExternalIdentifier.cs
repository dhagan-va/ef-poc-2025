namespace PayerEDI.Data.Models;

/// <summary>
/// An externally assigned identifier together with the qualifier that defines it.
/// </summary>
public sealed record ExternalIdentifier
{
    public ExternalIdentifier(string? qualifier, string? value)
    {
        Qualifier = qualifier?.Trim();
        Value = value?.Trim();
    }

    public string? Qualifier { get; }

    public string? Value { get; }
}
