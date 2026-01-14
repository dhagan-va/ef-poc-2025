using EDI837.src.Models;
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

        
        /// <summary>
        /// Processes the specified file by name if it exists in the configured local file folder.
        /// </summary>
        /// <remarks>The method attempts to locate and process a file with the given name from the folder
        /// specified in the application's configuration. If the file is found, it is read and processed; otherwise, an
        /// exception is thrown.</remarks>
        /// <param name="fileName">The name of the file to process. This value must not be null or empty.</param>
        /// <exception cref="Exception">Thrown if the specified file does not exist in the configured local file folder.</exception>
        public void GetFileByName(string fileName)
        {
            var fileInfo = this._fileProvider.GetFileInfo($"{_configuration["LocalFileFolder"]}\\{fileName}");
            if (fileInfo.Exists)
            {

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

        /// <summary>
        /// Method saves the transaction to the TS837 defined structure in the database.
        /// </summary>
        /// <param name="transactions">Parsed collection of transactions.</param>
        /// <returns>The JSON Object saved in the database.</returns>
        /// <exception cref="Exception">Get logged to the selected media.</exception>
        public async Task<IEnumerable<TS837P>> Save837PClaims(IEnumerable<TS837P> transactions)
        {
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

        /// <summary>
        /// Method ensures the claim is unique and a copy of it has not been stored in the database.
        /// </summary>
        /// <param name="claimControlNumber">Claim control number from the claim claim's header.</param>
        /// <param name="claimConventionReference">Claim conversion reference from the claim's header.</param>
        /// <param name="claimIdentifier">Claim identifier from the claim's header.</param>
        /// <returns>The JSON Object saved in the database.</returns>
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
