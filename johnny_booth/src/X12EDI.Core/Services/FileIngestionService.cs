using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Repositories;
using X12EDI.Abstractions.Services;
using X12EDI.Data.Repositories;

namespace X12EDI.Core.Services
{
    public class FileIngestionService : IFileIngestionService
    {
        #region Private Fields

        private readonly IEdiRepository _ediRepository;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<FileIngestionService> _logger;
        private readonly IX12ParserService _parser;

        #endregion Private Fields

        #region Public Constructors

        public FileIngestionService(
            IFileProvider fileProvider,
            IX12ParserService parser,
            ILogger<FileIngestionService> logger, IEdiRepository ediRepository)
        {
            _fileProvider = fileProvider;
            _parser = parser;
            _logger = logger;
            _ediRepository = ediRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>Ingests all .edi files provided by the file provider asynchronously.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task IngestAllAsync(CancellationToken cancellationToken = default)
        {
            var contents = _fileProvider.GetDirectoryContents(string.Empty);

            if (!contents.Exists)
            {
                _logger.LogWarning("Directory not found or inaccessible.");
                return;
            }

            foreach (var file in contents.Where(f => f.Exists && !f.IsDirectory))
            {
                await IngestAsync(file.Name, cancellationToken);
            }
        }

        /// <summary>Ingests a sinle file asynchronously.</summary>
        /// <param name="subpath">The subpath.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task IngestAsync(string subpath, CancellationToken cancellationToken = default)
        {
            var fileInfo = _fileProvider.GetFileInfo(subpath);

            if (!fileInfo.Exists)
            {
                _logger.LogWarning("File {File} not found.", subpath);
                return;
            }

            var identifier = fileInfo.PhysicalPath ?? fileInfo.Name;
            _logger.LogInformation("Ingesting file {File}", identifier);

            using var stream = fileInfo.CreateReadStream();

            var items = new List<object>();

            await foreach (var result in _parser.ParseEdiTransactionsAsync(
                               new[] { (stream, identifier) }, cancellationToken))
            {
                _logger.LogInformation("Parsed {Type} from {File}",
                    result.Item.GetType().Name,
                    result.FilePath);

                var item = result.Item;
                if (item is ReaderErrorContext errorContext)
                {
                    _logger.LogError("Parsing error in file {File}", result.FilePath);
                    _logger.LogError("Error Message: {Message}", errorContext.Message);
                }
                else if (item is EdiMessage message && message.HasErrors)
                {
                    _logger.LogError("EdiMessage has errors: ");
                    foreach (var error in message.ErrorContext.Errors)
                    {
                        _logger.LogError(" - {Error}", error.Message);
                    }
                }
                else
                {
                    _logger.LogInformation("Successfully processed item of type {Type} from file {File}",
                        item.GetType().Name,
                        result.FilePath);
                }

                if (item != null)
                {
                    items.Add(item); // collect all items regardless of error state
                }
            }

            if (await _ediRepository.SaveFileAsync(identifier, items, cancellationToken))
            {
                _logger.LogInformation("Persisted file {File} with {ItemCount} items", identifier, items.Count);
            }
            else
            {
                _logger.LogWarning("File {File} was not persisted (possible duplicate)", identifier);
            }
        }

        #endregion Public Methods
    }
}