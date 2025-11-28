using EDI837.src.Models;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.FileProviders;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Core.Model.Edi.ErrorContexts;

namespace EDI837.src.Services
{
    public class EdiParserService : IEdiParserService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger _logger;

        public EdiParserService(
            IConfiguration configuration, 
            IFileProvider fileProvider, 
            ILogger<EdiParserService> logger)
        {
            this._configuration = configuration;
            this._fileProvider = fileProvider;
            _logger = logger;
        }

        /// <summary>
        /// Method extracts the claim's transactions by using the file name as identifier. 
        /// </summary>
        /// <param name="fileName">File name of the claim to process.</param>
        /// <param name="parsingErrors">Collection of parsing errors passed by reference.</param>
        /// <returns>The collection of transactions in the claim.</returns>
        public IEnumerable<TS837P> ExtractValid837PTransactions(Stream stream, IEnumerable<string> parsingErrors, int validationLevel)
        {

            ValidationSettings validationSettings = new ValidationSettings()
            {
                ValidationLevel = (ValidationLevel)validationLevel
            }; 

            var validTransactions = new List<TS837P>();

            if (stream != null)
            {
                List<IEdiItem> ediItems;
                using (var ediReader = new X12Reader(stream, "EdiFabric.Templates.Hipaa"))
                {
                    ediItems = ediReader.ReadToEnd().ToList();
                }


                var transactions = ediItems.OfType<TS837P>();

                foreach (var transaction in transactions)
                {
                    MessageErrorContext validationErrors;
                    var isTransactionValid = transaction.IsValid(out validationErrors , validationSettings);
                    
                    if (!isTransactionValid)
                    {
                        //  partially parsed
                        parsingErrors = validationErrors.Flatten();
                        this._logger.LogWarning(transaction.ST?.TransactionSetControlNumber_02 != null ? 
                            $"{Enum.GetName(typeof(ValidationLevel),validationLevel)} - Transaction: {transaction.ST.TransactionSetControlNumber_02} has errors." 
                            : "The Transaction has Errors");

                        foreach (var err in parsingErrors)
                        {
                            _logger.LogWarning($" {err}");
                        }
                    }
                    else
                    {
                        validTransactions.Add(transaction);
                    }
                }
            }
            else
            {
                _logger.LogWarning("The File did not generate a valid Stream.");
            }

            _logger.LogInformation($"{validTransactions.Count} were parsed successfully.");
            return validTransactions;
        }

        /// <summary>
        /// Method creates a Stream based on the file name and the location of the file.
        /// </summary>
        /// <param name="fileName">Name of the file to be processed.</param>
        /// <returns>Readable Stream or a Null stream if the file does not exists.</returns>
        public Stream GetStreamByFileName(string fileName)
        {
            var fileInfo = this._fileProvider.GetFileInfo($"{_configuration["LocalFileFolder"]}\\{fileName}");
            this._logger.LogInformation(fileInfo.Exists ? $"{fileInfo.Name} does exist." : $"{fileInfo.Name} does not exist.");

            if (fileInfo.Exists)
            {
                return fileInfo.CreateReadStream();
            }

            return Stream.Null;
        }
    }
}
