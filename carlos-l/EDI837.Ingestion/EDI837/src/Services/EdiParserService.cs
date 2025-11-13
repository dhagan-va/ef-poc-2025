using EDI837.src.Models;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.FileProviders;
using EdiFabric.Templates.Hipaa5010;

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

        public IEnumerable<TS837P> ExtractValid837PTransactions(string fileName)
        {
            
            Stream stream = this.GetStreamByFileName(fileName);
            
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
                    if (transaction.HasErrors)
                    {
                        //  partially parsed
                        var errors = transaction.ErrorContext.Flatten();
                        this._logger.LogWarning(transaction.ST?.TransactionSetControlNumber_02 != null ? $"Transaction: {transaction.ST.TransactionSetControlNumber_02} has errors." : "");

                        foreach (var err in errors)
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
                _logger.LogWarning("No valid Stream was provided.");
            }

            _logger.LogInformation($"{validTransactions.Count} were parsed successfully.");
            return validTransactions;
        }

        private Stream GetStreamByFileName(string fileName)
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
