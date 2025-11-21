using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingester.Data.Repositories;

public interface IEdiRepository
{
    Task Save(List<TS837P> ts837Ps);
    Task Save(List<TS837I> institutionalClaims);
    Task Save(List<TS837D> institutionalClaims);
}