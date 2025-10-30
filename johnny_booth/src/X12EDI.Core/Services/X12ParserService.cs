using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Services;

public class X12ParserService : IX12ParserService
{
    #region Private Fields

    private ILogger<X12ParserService> _logger;

    #endregion Private Fields

    #region Public Constructors

    public X12ParserService(ILogger<X12ParserService> logger)
    {
        _logger = logger;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<IEnumerable<T>> ParseEdiTransactionsAsync<T>(string ediPath, CancellationToken cancellationToken) where T : IEdiItem
    {
        var results = new List<T>();
        using var stream = File.OpenRead(ediPath);
        using var ediReader = new X12Reader(stream);

        while (await ediReader.ReadAsync(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogWarning("EDI parsing cancelled for type {Type}", typeof(T).Name);
                return results;
            }

            if (ediReader.Item is T transaction)
            {
                results.Add(transaction);
            }
        }
        return results;
    }

    public Task RunAsync(string[] args, CancellationToken none)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}