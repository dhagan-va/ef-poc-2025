using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
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


    public async IAsyncEnumerable<ParsedResult> ParseEdiTransactionsAsync(
       IEnumerable<(Stream stream, string Identifier)> sources,
       [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var (stream, identifier) in sources)
        {
            using var ediReader = new X12Reader(stream);

            while (await ediReader.ReadAsync(cancellationToken))
            {
                if (ediReader.Item is IEdiItem transaction)
                {
                    yield return new ParsedResult(identifier, transaction);
                }
            }
        }
    }





    #endregion Public Methods
}