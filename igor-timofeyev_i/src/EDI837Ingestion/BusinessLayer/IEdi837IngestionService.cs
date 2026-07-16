using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837Ingestion.BusinessLayer
{
    public interface IEdi837IngestionService
    {
        Task IngestEdi837(bool useLocalMoto);
    }
}
