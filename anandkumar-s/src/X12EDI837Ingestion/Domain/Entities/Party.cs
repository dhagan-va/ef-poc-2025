using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Domain.Entities
{
 
    public class Party
    {
        public long Id { get; set; }

        public long TransactionSetHeaderId { get; set; }
        public TransactionSetHeader TransactionSetHeader { get; set; } = null!;

        // What role is this NM1 representing?
        // Examples: Submitter, Receiver, BillingProvider, Subscriber, Patient, Payer
        public string Role { get; set; } = "";                 // derived from context/loop

        public string PartyType { get; set; } = "";                 // derived from context/loop (e.g. "BillingProvider", "Subscriber", "Payer")
        public string? EntityIdentifierCode { get; set; }      // NM101
        public string? EntityTypeQualifier { get; set; }       // NM102 (1=person, 2=non-person)
        public string? LastNameOrOrgName { get; set; }         // NM103
        public string? FirstName { get; set; }                 // NM104
        public string? IdCodeQualifier { get; set; }           // NM108
        public string? IdCode { get; set; }                    // NM109

        public string? RawSegment { get; set; }
    }

}
