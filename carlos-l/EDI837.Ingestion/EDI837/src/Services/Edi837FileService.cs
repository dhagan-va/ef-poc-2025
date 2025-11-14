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
            _logger.LogInformation("The transactions collection was null");
            ArgumentNullException.ThrowIfNull(nameof(transactions));

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

                if (!this.IsClaimDuplicate(processedClaim.ClaimControlNumber, processedClaim.ClaimConventionReference, processedClaim.ClaimIdentifier))
                {
                    try
                    {
                        await this._context.ProcessedClaims.AddAsync(processedClaim);
                        await this._context.SaveChangesAsync();

                        result.Add(processedClaim);
                    }
                    catch (Exception)
                    {
                        _logger.LogWarning("Unable to Save Transaction.");
                        throw new Exception("Unable to process claim.");
                    }
                }
                else
                {
                    _logger.LogInformation("Duplicate Claim");
                }
            }
           
            return result;
        }

        public async Task<IEnumerable<TS837P>> Save837PClaims(IEnumerable<TS837P> transactions)
        {
            _logger.LogInformation("The transactions collection was null");
            ArgumentNullException.ThrowIfNull(nameof(transactions));

            List<TS837P> result = new List<TS837P>();

            foreach (var transaction in transactions)
            {
                try
                {
                    await this._context.TS837Ps.AddAsync(transaction);
                    await this._context.SaveChangesAsync();

                    result.Add(transaction);
                }
                catch (Exception)
                {
                    _logger.LogWarning("Unable to Save Transaction.");
                    throw new Exception("Unable to save the trasaction.");
                }
            }
            
            return result;
        }
        private bool IsClaimDuplicate(string claimControlNumber, string claimConventionReference, string claimIdentifier)
        {
            var record = this._context.ProcessedClaims.FirstOrDefault(
                c => c.ClaimControlNumber.ToUpper() == claimControlNumber.ToUpper() && 
                c.ClaimConventionReference.ToUpper() == claimConventionReference.ToUpper() && 
                c.ClaimIdentifier.ToUpper() == claimIdentifier.ToUpper());
            
            return record != null;
        }
    }
}
