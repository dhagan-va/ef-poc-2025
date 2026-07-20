using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837Ingestion.BusinessLayer.FilesProviders
{
    public interface IEdiSourceProvider
    {
        /// <summary>
        /// Retrieves the raw EDI text payloads from the underlying storage infrastructure.
        /// </summary>
        Task<IEnumerable<string>> GetEdiPayloadsAsync();
    }
}
