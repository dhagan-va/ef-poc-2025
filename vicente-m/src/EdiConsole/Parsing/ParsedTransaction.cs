using EdiFabric.Templates.Hipaa5010;

namespace EdiConsole.Parsing;

/// <summary>
/// One transaction set (ST…SE) read from a file, with the envelope values we track.
/// Either Transaction or Problem is set, never both.
/// </summary>
/// <param name="InterchangeControlNumber">ISA13.</param>
/// <param name="UsageIndicator">ISA15: T = test, P = production.</param>
/// <param name="TransactionSetControlNumber">ST02.</param>
/// <param name="Transaction">The 837P, or null when the transaction set couldn't be read as one.</param>
/// <param name="Problem">Why there's no transaction, e.g. "Unsupported transaction 837I".</param>
public sealed record ParsedTransaction(
    string? InterchangeControlNumber,
    string? UsageIndicator,
    string? TransactionSetControlNumber,
    TS837P? Transaction,
    string? Problem);
