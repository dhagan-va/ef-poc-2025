using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class Ts850Entity
    {
        public long Id { get; set; }
        public string? PurchaseOrderNumber { get; set; } 
        public string RawJson { get; set; } = "{}";
    }
}
