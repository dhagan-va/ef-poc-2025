using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.CompilerServices;
using X12EDI.Abstractions.Services;

namespace X12EDI.Core.Services
{
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
            Func<ISA, GS, ST, TypeInfo> factory = (isa, gs, st) =>
            {
                if (st.TransactionSetIdentifierCode_01 == "837")
                {
                    // You can inspect GS.FunctionalIdentifierCode_01 or other values to choose between TS837P, TS837I, TS837D
                    switch (gs.CodeIdentifyingInformationType_1)
                    {
                        case "HC": return typeof(TS837P).GetTypeInfo(); // Professional
                        case "HI": return typeof(TS837I).GetTypeInfo(); // Institutional
                        case "HD": return typeof(TS837D).GetTypeInfo(); // Dental
                    }
                }

                return default!;
            };



            foreach (var (stream, identifier) in sources)
            {
                using var ediReader = new X12Reader(stream, factory, new X12ReaderSettings()
                {
                    ContinueOnError = true
                });

                while (await ediReader.ReadAsync(cancellationToken))
                {
                    if (ediReader.Item is IEdiItem transaction)
                    {
                        yield return new ParsedResult(identifier, transaction);
                    }
                    else
                    {

                    }
                }
            }
        }

        #endregion Public Methods
    }
}