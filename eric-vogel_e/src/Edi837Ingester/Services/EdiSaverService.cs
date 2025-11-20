using Edi837Ingester.Data;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingester.Services;

public class EdiSaverService(AppDbContext dbContext) : IEdiSaverService
{
    public async Task Save(List<TS837P> ts837Ps)
    {
        await dbContext.AddRangeAsync(ts837Ps);
        await dbContext.SaveChangesAsync();
    }

    public async Task Save(List<TS837I> institutionalClaims)
    {
        await dbContext.AddRangeAsync(institutionalClaims);
        await dbContext.SaveChangesAsync();
    }

    public async Task Save(List<TS837D> institutionalClaims)
    {
        await dbContext.AddRangeAsync(institutionalClaims);
        await dbContext.SaveChangesAsync();
    }
}