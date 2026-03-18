using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Producer.Interfaces
{
    public interface IProducerService
    {
        Task UploadFolderAsync(CancellationToken cancellationToken = default);
        Task UploadUsingWorkerPoolAsync(CancellationToken cancellationToken = default);
        
    }
}
