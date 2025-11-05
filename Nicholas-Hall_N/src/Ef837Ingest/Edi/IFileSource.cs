using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Edi
{
    public interface IFileSource
    {
        Task<Stream> OpenAsync(string locator, CancellationToken ct = default);
    }

}
