using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using X12EDI.Abstractions.Services;

namespace X12EDI.Core.Services
{
    public class FileIngestionService : IFileIngestionService
    {
        #region Private Fields

        private readonly IFileProvider _fileProvider;
        private readonly ILogger<FileIngestionService> _logger;
        private readonly IX12ParserService _parser;

        #endregion Private Fields

        #region Public Constructors

        public FileIngestionService(
            IFileProvider fileProvider,
            IX12ParserService parser,
            ILogger<FileIngestionService> logger)
        {
            _fileProvider = fileProvider;
            _parser = parser;
            _logger = logger;
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
                    // Process the successfully parsed item (e.g., save to database, further processing, etc.)
                    _logger.LogInformation("Successfully processed item of type {Type} from file {File}",
                        item.GetType().Name,
                        result.FilePath);
                }
            }
        }

        #endregion Public Methods
    }
}