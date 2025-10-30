using EdiFabric.Core.Model.Edi;

namespace X12EDI.Abstractions.Services
{
    public interface IX12ParserService
    {
        Task<IEnumerable<T>> ParseEdiTransactionsAsync<T>(string ediPath, CancellationToken cancellationToken) where T : IEdiItem;
        Task RunAsync(string[] args, CancellationToken none);
    }
}
