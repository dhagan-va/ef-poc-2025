using H2.EdiIngestor.Data;

namespace H2.EdiIngestor;

/// <summary>
/// Parses X12N 837 claim files into <see cref="Claim"/> domain objects.
/// </summary>
public interface IX12N837Parser
{
    /// <summary>
    /// Reads claims from the specified EDI 837 file.
    /// </summary>
    /// <param name="fileName">Path to the EDI file to read.</param>
    /// <param name="cancellationToken">Token to observe while reading.</param>
    /// <returns>An async sequence of <see cref="Claim"/> instances.</returns>
    IAsyncEnumerable<Claim> ReadFile(string fileName, CancellationToken cancellationToken);
}
