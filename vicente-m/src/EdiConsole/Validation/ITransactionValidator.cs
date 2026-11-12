using EdiConsole.Domain.Tracking;
using EdiFabric.Templates.Hipaa5010;

namespace EdiConsole.Validation;

/// <summary>
/// Checks one 837P transaction at a given SNIP level.
/// </summary>
public interface ITransactionValidator
{
    /// <summary>
    /// Validates the whole transaction at <paramref name="snipLevel"/> (1–4, EdiFabric's ValidationLevel).
    /// An empty list means the transaction is valid.
    /// </summary>
    IReadOnlyList<ValidationIssue> Validate(TS837P transaction, byte snipLevel);
}
