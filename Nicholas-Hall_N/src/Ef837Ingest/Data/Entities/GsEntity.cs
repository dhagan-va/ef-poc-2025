using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class GsEntity
    {
        public long Id { get; set; }
        public string? FunctionalIdCode { get; set; }
        public string? AppSenderCode { get; set; }
        public string? AppReceiverCode { get; set; }
        public string? GroupControlNumber { get; set; }
        public string RawJson { get; set; } = "{}";
    }
}
