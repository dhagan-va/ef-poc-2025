using Edi837Ingester.Data;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingester.Services;

public class EdiSaverService(AppDbContext dbContext)
{
    public void Save(List<TS837P> ts837Ps)
    {
        
    }
}