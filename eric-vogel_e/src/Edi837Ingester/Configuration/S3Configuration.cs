using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edi837Ingester.Configuration
{
    public class S3Configuration
    {
        public string? ServiceUrl { get; set; }
        public string? Bucket { get; set; }
    }
}
