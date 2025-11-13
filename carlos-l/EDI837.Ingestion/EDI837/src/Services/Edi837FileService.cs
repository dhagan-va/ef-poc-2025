using EDI837.src.Models;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Templates.Hipaa5010;
using System.Text.Json;

namespace EDI837.src.Services
{
    public class Edi837FileService : IEdi837FileService
    {
        private readonly AppDataContext _context;
        private readonly ILogger _logger;

        public Edi837FileService(
            AppDataContext context, 
            ILogger<Edi837FileService> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<ProcessedClaim>> SaveOriginalClaim(IEnumerable<TS837P> transactions)
        {
            var jsonString = JsonSerializer.Serialize(transactions);
            List<ProcessedClaim> result = new List<ProcessedClaim>();

            foreach (var transaction in transactions)
            {
                ProcessedClaim processedClaim = new ProcessedClaim()
                {
                    Claim = jsonString,
                    ClaimControlNumber = transaction.ST?.TransactionSetControlNumber_02 ?? string.Empty,
                    ClaimConventionReference = transaction.ST?.ImplementationConventionPreference_03 ?? string.Empty,
                    ClaimIdentifier = transaction.ST?.TransactionSetIdentifierCode_01 ?? string.Empty,
                };

                try
                {
                    await this._context.ProcessedClaims.AddAsync(processedClaim);
                    await this._context.SaveChangesAsync();

                    result.Add(processedClaim);
                }
                catch (Exception)
                {
                    _logger.LogWarning("Unable to Save Transaction.");
                    throw new Exception ("Unable to process claim.");
                }
              
            }
           
            return result;
        }
    }
}
