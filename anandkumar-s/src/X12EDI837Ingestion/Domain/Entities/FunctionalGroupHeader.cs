using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Domain.Entities
{
   
    public class FunctionalGroupHeader
    {
        public long Id { get; set; }

        public long InterchangeHeaderId { get; set; }
        public InterchangeHeader InterchangeHeader { get; set; } = null!;

        public string? FunctionalIdCode { get; set; }     // GS01 (e.g., HC)
        public string? SenderCode { get; set; }           // GS02
        public string? ReceiverCode { get; set; }         // GS03
        public DateTime? GroupDateTime { get; set; }      // GS04+GS05
        public string? GroupControlNumber { get; set; }   // GS06
        public string? Version { get; set; }              // GS08 (e.g., 005010X222A1)

        public string? RawSegment { get; set; }

        public List<TransactionSetHeader> TransactionSets { get; set; } = new();
        public object FunctionalIdentifierCode { get; internal set; }
        public string Date { get; internal set; }
        public string Time { get; internal set; }
    }

}
