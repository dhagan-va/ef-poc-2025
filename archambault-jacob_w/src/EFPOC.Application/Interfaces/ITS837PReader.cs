
using EdiFabric.Templates.Hipaa5010;
namespace EFPOC.Application.Interfaces;

public interface ITS837PReader
{
        public IEnumerable<TS837P> Read();
    
}