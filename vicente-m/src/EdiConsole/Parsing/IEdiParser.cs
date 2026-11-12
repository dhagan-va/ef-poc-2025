namespace EdiConsole.Parsing;

/// <summary>
/// Turns a stream of EDI into transaction sets. Reading only: validation is a separate step.
/// </summary>
public interface IEdiParser
{
    /// <summary>
    /// Yields one item per transaction set (ST…SE), in file order. Yields nothing for an empty file.
    /// </summary>
    IAsyncEnumerable<ParsedTransaction> ParseAsync(Stream stream, CancellationToken cancellationToken = default);
}
