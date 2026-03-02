using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Domain.Entities
{
 
    public class ServiceLine
    {
        public long Id { get; set; }

        public long ClaimId { get; set; }
        public Claim Claim { get; set; } = null!;

        public string? ProcedureCode { get; set; }            // from SV101 (HC:xxxxx)
        public decimal? LineItemChargeAmount { get; set; }    // SV102
        public decimal? UnitCount { get; set; }               // SV104
        public string? PlaceOfService { get; set; }           // SV105 (if present)

        public string? RawSegment { get; set; }
    
        public string? UnitOrBasis { get; set; }
    }

}
