using Edi837Ingester.Data.Entities;
using EdiFabric.Core.Model.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Data.Repositories;

public class EdiRepository(AppDbContext dbContext, ILogger<EdiRepository> logger) : IEdiRepository
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
        await SaveInBatchesAsync(items);
    }

    /// <summary>
    /// Saves the processed claims to the database.
    /// </summary>
    /// <param name="processedClaims">Processed claims to save.</param>
    /// <returns></returns>
    public async Task SaveProcessedClaims(IEnumerable<ProcessedClaim> processedClaims)
    {
        await SaveInBatchesAsync(processedClaims);

    }

    /// <summary>
    /// Saves items in batches to the database to optimize performance and reduce memory usage.
    /// Logs errors when chunk-level saves fail.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <param name="items">The collection of items to be saved. Cannot be null.</param>
    /// <param name="chunkSize">The size of each batch. Default is 1000.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    private async Task SaveInBatchesAsync<T>(IEnumerable<T> items, int chunkSize = 1000) where T : class
    {
        var list = items?.ToList() ?? new List<T>();
        if (list.Count == 0)
        {
            logger.LogDebug("No items to save for type {EntityType}", typeof(T).Name);
            return;
        }

        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            for (int i = 0; i < list.Count; i += chunkSize)
            {
                var chunk = list.Skip(i).Take(chunkSize).ToList();
                try
                {
                    await dbContext.AddRangeAsync(chunk);
                    await dbContext.SaveChangesAsync();
                    dbContext.ChangeTracker.Clear();
                    logger.LogDebug("Saved chunk {ChunkIndex} for {EntityType} with {Count} items", (i / chunkSize), typeof(T).Name, chunk.Count);
                }
                catch (Exception ex)
                {
                    // Log detailed context for the failing chunk and rethrow to allow upstream handling.
                    logger.LogError(ex, "Failed to save chunk {ChunkIndex} for {EntityType}. ChunkSize={ChunkSize}, ItemsInChunk={ItemsInChunk}", (i / chunkSize), typeof(T).Name, chunkSize, chunk.Count);
                    throw;
                }
            }
        }
        finally
        {
            dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }
}