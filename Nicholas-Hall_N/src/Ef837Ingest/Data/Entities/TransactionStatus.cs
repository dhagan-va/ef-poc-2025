using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class TransactionStatus
    {
        public int Id { get; set; }
        public string? InterchangeControlNumber { get; set; }
        public string? GroupControlNumber { get; set; }
        public string? TransactionSetControlNumber { get; set; }
        public string TransactionType { get; set; } = "837P"; 
        public bool IsValid { get; set; }
        public int ErrorCount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime IngestedAt { get; set; }
        public string? SourceObjectKey { get; set; } 
    }

}
