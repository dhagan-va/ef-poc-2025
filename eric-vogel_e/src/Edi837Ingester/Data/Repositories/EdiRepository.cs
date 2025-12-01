using Edi837Ingester.Data;
using Edi837Ingester.Data.Entities;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingester.Data.Repositories;

public class EdiRepository(AppDbContext dbContext) : IEdiRepository
{
    /// <summary>
    /// Gets the processed claims of the specified claim type asynchronously.
    /// </summary>
    /// <param name="claimType">Claim type.</param>
    /// <returns>A list of processed claims.</returns>
    public async Task<List<ProcessedClaim>> GetProcessedClaims(ClaimTypeEnum claimType)
    {
        return await dbContext.ProcessedClaims
            .Where(pc => pc.ClaimTypeId == (int)claimType)
            .ToListAsync();
    }

    /// <summary>
    /// Saves a collection of items to the database asynchronously.
    /// </summary>
    /// <remarks>This method adds the specified items to the database context and commits the changes.  Ensure
    /// that the database context is properly configured before calling this method.</remarks>
    /// <typeparam name="T">The type of items to save. Must inherit from <see cref="EdiMessage"/>.</typeparam>
    /// <param name="items">The collection of items to be saved. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveClaims<T>(List<T> items) where T : EdiMessage
    {
        await dbContext.AddRangeAsync(items);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Saves the processed claims to the database.
    /// </summary>
    /// <param name="processedClaims">Processed claims to save.</param>
    /// <returns></returns>
    public async Task SaveProcessedClaims(IEnumerable<ProcessedClaim> processedClaims)
    {
        await dbContext.AddRangeAsync(processedClaims);
        await dbContext.SaveChangesAsync();
    }
}