using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.CompilerServices;
using X12EDI.Abstractions.Services;
using X12EDI.Core.Config;

namespace X12EDI.Core.Services
{
    /// <summary>
    /// A service for parsing X12 EDI files and streams into structured objects.
    /// </summary>
    public class X12ParserService : IX12ParserService
    {
        #region Private Fields

        private readonly EdiOptions _ediOptions;
        private readonly ILogger<X12ParserService> _logger;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="X12ParserService"/> class.
        /// </summary>
        /// <param name="logger">The logger for recording information and errors.</param>
        /// <param name="ediOptions">The configuration options for EDI processing.</param>
        public X12ParserService(ILogger<X12ParserService> logger, EdiOptions ediOptions)
        {
            _logger = logger;
            _ediOptions = ediOptions;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Asynchronously parses EDI transactions from one or more stream sources.
        /// </summary>
        /// <param name="sources">An enumerable of tuples, each containing a stream and a unique identifier for the source.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// An asynchronous stream of <see cref="ParsedResult"/> objects, each containing a parsed EDI item and its source identifier.
        /// </returns>
        public async IAsyncEnumerable<ParsedResult> ParseEdiTransactionsAsync(
           IEnumerable<(Stream stream, string Identifier)> sources,
           [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var (stream, identifier) in sources)
            {
                using var ediReader = new X12Reader(stream, MessageFactory, new X12ReaderSettings()
                {
                    ContinueOnError = _ediOptions.ContinueOnError
                });

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

        #region Private Methods

        private static TypeInfo? MessageFactory(ISA isa, GS gs, ST st)
        {
            if (st.TransactionSetIdentifierCode_01 == "837")
            {
                // You can inspect GS.FunctionalIdentifierCode_01 or other values to choose between TS837P, TS837I, TS837D
                return gs.CodeIdentifyingInformationType_1 switch
                {
                    "HC" => typeof(TS837P).GetTypeInfo(), // Professional
                    "HI" => typeof(TS837I).GetTypeInfo(), // Institutional
                    "HD" => typeof(TS837D).GetTypeInfo(), // Dental
                    _ => default,
                };
            }

            return default;
        }

        #endregion Private Methods
    }
}