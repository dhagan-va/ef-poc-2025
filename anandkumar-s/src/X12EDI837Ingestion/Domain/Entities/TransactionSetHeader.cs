using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Domain.Entities
{
  
    public class TransactionSetHeader
    {
        public long Id { get; set; }

        public long FunctionalGroupHeaderId { get; set; }
        public FunctionalGroupHeader FunctionalGroupHeader { get; set; } = null!;

        // ST
        public string? TransactionSetIdCode { get; set; }     // ST01 (837)
        public string? TransactionSetControlNumber { get; set; } // ST02

        // BHT
        public string? HierarchicalStructureCode { get; set; }   // BHT01
        public string? TransactionSetPurposeCode { get; set; }   // BHT02
        public string? ReferenceId { get; set; }                 // BHT03
        public DateTime? TransactionDateTime { get; set; }       // BHT04+BHT05

        public string? RawStSegment { get; set; }
        public string? RawBhtSegment { get; set; }

        public List<Party> Parties { get; set; } = new();
        public List<Claim> Claims { get; set; } = new();
        public dynamic? TransactionSetId { get; internal set; }
        public dynamic? ImplementationConvention { get; internal set; }
        public dynamic? BhtReferenceId { get; internal set; }
    }

}
