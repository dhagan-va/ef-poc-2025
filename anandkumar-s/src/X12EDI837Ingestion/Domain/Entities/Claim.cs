using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Domain.Entities
{
  
    public class Claim
    {
        public long Id { get; set; }

        public long TransactionSetHeaderId { get; set; }
        public TransactionSetHeader TransactionSetHeader { get; set; } = null!;

        // Optional link if we want to associate claims to subscriber later:
        public long? SubscriberPartyId { get; set; }
        public Party? SubscriberParty { get; set; }

        public string? PatientControlNumber { get; set; }      // CLM01
        public decimal? TotalClaimChargeAmount { get; set; }   // CLM02
        public string? ClaimFilingIndicator { get; set; }      // derived from context/other segments
        public string? FacilityTypeCode { get; set; }          // from CLM05-1 etc (simplified)
        public string? ClaimFrequencyCode { get; set; }        // simplified

        public string? RawSegment { get; set; }

        public List<ServiceLine> ServiceLines { get; set; } = new();
        public string? ClaimSubmitterId { get; set; }
        public string? FacilityCode { get;  set; }
       
    }

}
