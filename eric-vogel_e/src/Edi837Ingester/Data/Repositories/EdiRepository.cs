using Edi837Ingester.Data;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingester.Data.Repositories;

public class EdiRepository(AppDbContext dbContext) : IEdiRepository
{
    public async Task Save<T>(List<T> items) where T : EdiMessage
    {
        await dbContext.AddRangeAsync(items);
        await dbContext.SaveChangesAsync();
    }
}