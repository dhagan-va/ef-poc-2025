using Edi837Ingester.Data;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingester.Data.Repositories;

public class EdiRepository(AppDbContext dbContext) : IEdiRepository
{
    /// <summary>
    /// Saves a collection of items to the database asynchronously.
    /// </summary>
    /// <remarks>This method adds the specified items to the database context and commits the changes.  Ensure
    /// that the database context is properly configured before calling this method.</remarks>
    /// <typeparam name="T">The type of items to save. Must inherit from <see cref="EdiMessage"/>.</typeparam>
    /// <param name="items">The collection of items to be saved. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task Save<T>(List<T> items) where T : EdiMessage
    {
        await dbContext.AddRangeAsync(items);
        await dbContext.SaveChangesAsync();
    }
}