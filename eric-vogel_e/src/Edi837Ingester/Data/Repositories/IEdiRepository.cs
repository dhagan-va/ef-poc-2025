using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingester.Data.Repositories;

public interface IEdiRepository
{
    Task Save<T>(List<T> items) where T : EdiMessage;
}