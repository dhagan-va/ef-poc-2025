using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class StEntity
    {
        public long Id { get; set; }
        public string? TransactionSetId { get; set; }  
        public string? ControlNumber { get; set; }
        public string RawJson { get; set; } = "{}";
    }
}
