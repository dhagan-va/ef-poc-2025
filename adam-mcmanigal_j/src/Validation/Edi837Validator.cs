using Edi837Ingestion.Parsing;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;

namespace Edi837Ingestion.Validation;

/// <summary>
/// Enforces a configured WEDI SNIP <see cref="ValidationLevel"/> on a parsed interchange by running
/// EdiFabric's template validation (<see cref="EdiMessage.IsValid"/>) over every 837 transaction set —
/// professional, institutional, and dental. Returns an aggregate <see cref="Edi837ValidationResult"/>
/// rather than throwing, so the caller decides what a failure means; in the ingestion path a failure is
/// treated as a poison (dead-letter) outcome.
/// </summary>
/// <remarks>
/// The EdiFabric license must already be applied at startup (as for parsing) — validation reads the
/// same ambient license state. A <see langword="null"/> level means validation is disabled, so
/// <see cref="Validate"/> is a no-op that always passes. A fresh <see cref="ValidationSettings"/> is
/// built per call rather than shared, so the singleton validator is safe under the concurrent message
/// handling the ingestion service allows.
/// </remarks>
public sealed class Edi837Validator(ValidationLevel? level)
{
    /// <summary>The configured level (<see langword="null"/> = disabled), for logging / the DLQ reason.</summary>
    public ValidationLevel? Level { get; } = level;

    /// <summary>
    /// Validates every transaction set in <paramref name="interchange"/> at the configured level. Short
    /// -circuits to <see cref="Edi837ValidationResult.Valid"/> when validation is disabled; otherwise
    /// collects one description per failing set and returns a failing result if any set failed.
    /// </summary>
    public Edi837ValidationResult Validate(ParsedInterchange interchange)
    {
        ArgumentNullException.ThrowIfNull(interchange);

        if (Level is not { } validationLevel)
            return Edi837ValidationResult.Valid;

        // A per-call settings object: IsValid stamps each message's ErrorContext, and messages are not
        // shared between calls, so nothing mutable is shared across concurrent validations.
        var settings = new ValidationSettings { ValidationLevel = validationLevel };

        var errors = new List<string>();
        Collect(interchange.ProfessionalTransactionSets, settings, errors);
        Collect(interchange.InstitutionalTransactionSets, settings, errors);
        Collect(interchange.DentalTransactionSets, settings, errors);

        return errors.Count == 0 ? Edi837ValidationResult.Valid : new Edi837ValidationResult(false, errors);
    }

    private static void Collect<TMessage>(
        IReadOnlyList<ParsedTransactionSet<TMessage>> transactionSets,
        ValidationSettings settings,
        List<string> errors)
        where TMessage : EdiMessage
    {
        foreach (var transactionSet in transactionSets)
        {
            if (!transactionSet.Message.IsValid(out var context, settings))
                errors.Add(Describe(transactionSet, context));
        }
    }

    // A concise, single-line summary of one failing transaction set: its variant and ST02 control
    // number, then the message-level error (if any) followed by each segment/element error detail
    // EdiFabric collected. Blank messages are dropped so the line stays readable.
    private static string Describe<TMessage>(
        ParsedTransactionSet<TMessage> transactionSet, MessageErrorContext context)
        where TMessage : EdiMessage
    {
        var messages = context.Errors
            .SelectMany(segment => segment.Errors
                .Select(element => element.Message)
                .Prepend(segment.Message))
            .Prepend(context.Message)
            .Where(message => !string.IsNullOrWhiteSpace(message));

        return $"{typeof(TMessage).Name} (ST02 {transactionSet.TransactionSetControlNumber}): " +
               string.Join(", ", messages);
    }
}
