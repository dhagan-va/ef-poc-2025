using EdiFabric.Core.Model.Edi;
using System.Runtime.CompilerServices;

namespace X12EDI.Abstractions.Services
{

    public record ParsedResult(string FilePath, IEdiItem Item);

    public interface IX12ParserService
    {
        IAsyncEnumerable<ParsedResult> ParseEdiTransactionsAsync(IEnumerable<(Stream stream, string Identifier)> sources, CancellationToken cancellationToken);
    }
}
