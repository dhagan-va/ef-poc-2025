using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Domain.Entities
{
    public class InterchangeHeader
    {
        public long Id { get; set; }

        public string? FileName { get; set; }
        public DateTime ReceivedAtUtc { get; set; } = DateTime.UtcNow;

        // ISA control info
        public string? AuthorizationInfoQualifier { get; set; }   // ISA01
        public string? SecurityInfoQualifier { get; set; }        // ISA03
        public string? SenderIdQualifier { get; set; }            // ISA05
        public string? SenderId { get; set; }                     // ISA06
        public string? ReceiverIdQualifier { get; set; }          // ISA07
        public string? ReceiverId { get; set; }                   // ISA08
        public DateTime? InterchangeDateTime { get; set; }        // ISA09+ISA10
        public string? ControlNumber { get; set; }                // ISA13

        public string? RawSegment { get; set; }                   // audit/debug

        public List<FunctionalGroupHeader> FunctionalGroups { get; set; } = new();
        public string? InterchangeControlNumber { get; set; }
        public string? Time { get; internal set; }
        public string? Date { get; internal set; }
        public string? SourceFile { get; internal set; }
    }
}
