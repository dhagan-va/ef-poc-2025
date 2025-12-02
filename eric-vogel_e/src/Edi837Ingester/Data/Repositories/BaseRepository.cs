using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Data.Repositories
{
    public abstract class BaseRepository(AppDbContext dbContext,
                              ILogger<BaseRepository> logger)
    {
        /// <summary>
        /// Saves items in batches to the database to optimize performance and reduce memory usage.
        /// Logs errors when chunk-level saves fail.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="items">The collection of items to be saved. Cannot be null.</param>
        /// <param name="chunkSize">The size of each batch. Default is 1000.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        protected async Task SaveInBatchesAsync<T>(IEnumerable<T> items, int chunkSize = 1000) where T : class
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
}