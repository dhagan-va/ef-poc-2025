using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Application.Interfaces
{
    public interface IX12EDI837IngestionService
    {
        Task ProcessIngestionAsync(
            string filePath,
            CancellationToken cancellationToken = default);
    }
}
