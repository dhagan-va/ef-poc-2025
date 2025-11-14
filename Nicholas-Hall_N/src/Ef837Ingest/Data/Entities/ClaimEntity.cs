using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class ClaimEntity
    {
        public int Id { get; set; }

        public string? TransactionSetControlNumber { get; set; } 
        public string? PatientControlNumber { get; set; }  
        public string? PatientLastName { get; set; }
        public string? PatientFirstName { get; set; }

        public decimal? TotalClaimAmount { get; set; }       
        public DateTime? ServiceFromDate { get; set; }       

        public string? BillingProviderNpi { get; set; }      
    }
}
