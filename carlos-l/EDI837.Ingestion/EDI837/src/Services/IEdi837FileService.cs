using EDI837.src.Models;
using EdiFabric.Templates.Hipaa5010;

namespace EDI837.src.Services
{
    public interface IEdi837FileService
    {
        public Task<IEnumerable<ProcessedClaim>> SaveOriginalClaim(IEnumerable<TS837P> transactions);
    }
}
