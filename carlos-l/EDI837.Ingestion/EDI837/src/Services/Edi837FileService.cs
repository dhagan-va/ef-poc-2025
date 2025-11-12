using EDI837.src.Models;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EDI837.src.Services
{
    public class Edi837FileService : IEdi837FileService
    {
        private readonly AppDataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger _logger;

        public Edi837FileService(AppDataContext context, IConfiguration configuration, IFileProvider fileProvider, ILogger<Edi837FileService> logger)
        {
            this._context = context;
            this._configuration = configuration;
            this._fileProvider = fileProvider;
            this._logger = logger;
        }

        
        public IEnumerable<TS837P> ExtractValid837PTransactions(Stream stream)
        {
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
    }
}
