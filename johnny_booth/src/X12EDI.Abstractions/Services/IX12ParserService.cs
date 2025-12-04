using EdiFabric.Core.Model.Edi;

namespace X12EDI.Abstractions.Services
{
    /// <summary>
    /// Represents the result of a parsing operation, linking a parsed EDI item to its source file.
    /// </summary>
    /// <param name="FilePath">The path of the file from which the item was parsed.</param>
    /// <param name="Item">The parsed EDI item.</param>
    public record ParsedResult(string FilePath, IEdiItem Item);

    /// <summary>
    /// Defines the contract for a service that parses X12 EDI transactions.
    /// </summary>
    public interface IX12ParserService
    {
        #region Public Methods

        /// <summary>
        /// Asynchronously parses EDI transactions from a collection of stream sources.
        /// </summary>
        /// <param name="sources">An enumerable of tuples, each containing a stream and its identifier.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An asynchronous stream of <see cref="ParsedResult"/> objects.</returns>
        IAsyncEnumerable<ParsedResult> ParseEdiTransactionsAsync(IEnumerable<(Stream stream, string Identifier)> sources, CancellationToken cancellationToken);

        #endregion Public Methods
    }
}