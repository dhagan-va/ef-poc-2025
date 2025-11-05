using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Edi
{
    public interface IIngestionQueue
    {
        ValueTask EnqueueAsync(string s3Uri, CancellationToken ct = default);
    }
}
