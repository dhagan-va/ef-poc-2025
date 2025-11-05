using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class ValidationIssue
    {
        public long Id { get; set; }
        public string Transaction { get; set; } = "837P";
        public string? ControlNumber { get; set; }
        public string Level { get; set; } = "Snip4";
        public string? SegmentId { get; set; }
        public int? Position { get; set; }
        public string? Code { get; set; }
        public string Message { get; set; } = "";
        public string Severity { get; set; } = "Error";
        public string? SourceUri { get; set; }     
        public DateTime OccurredUtc { get; set; } = DateTime.UtcNow;

        public string? RawContextJson { get; set; }
    }

}
