using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837Ingestion.EF.Entities
{
    public class InterchangeControl
    {
        public int Id { get; set; }
        public string SenderId { get; set; }      // SUBMITTER99
        public string ReceiverId { get; set; }    // RECEIVER88
        public string ControlNumber { get; set; } // 000000001
        public DateTime TransmissionDate { get; set; }
        public List<FunctionalGroup> FunctionalGroups { get; set; }
    }

    public class FunctionalGroup
    {
        public int Id { get; set; }
        public int InterchangeControlId { get; set; }
        public string GroupControlNumber { get; set; } // 1
        public string VersionCode { get; set; }        // 005010X222A1
        public List<ClaimBatch> ClaimBatches { get; set; }
    }

    public class ClaimBatch
    {
        public int Id { get; set; }
        public int FunctionalGroupId { get; set; }
        public string TransactionId { get; set; }        // 0001
        public string ReferenceNumber { get; set; }      // 12345 (BHT03)
        public string SubmitterName { get; set; }        // TEST SUBMITTER CLEARINGHOUSE
        public string SubmitterContactPhone { get; set; } // 8005550100
        public string PayerName { get; set; }            // BLUE SHIELD TEST PAYER
        public List<MedicalClaim> MedicalClaims { get; set; }
    }

    public class BillingProvider
    {
        public int Id { get; set; }
        public string LastName { get; set; }   // SMITH
        public string? FirstName { get; set; }  // JOHN
        public string Npi { get; set; }        // 1234567890
        public string Address { get; set; }    // 123 HEALTHCARE WAY
        public string City { get; set; }       // HARRISBURG
        public string State { get; set; }      // PA
        public string ZipCode { get; set; }    // 17101
        public string TaxId { get; set; }      // 987654321
    }

    public class SubscriberPatient
    {
        public int Id { get; set; }
        public string MemberId { get; set; }   // 123456789A
        public string LastName { get; set; }   // DOE
        public string? FirstName { get; set; }  // JANE
        public string? Address { get; set; }    // 456 PATIENT ST
        public string? City { get; set; }       // HERSHEY
        public string? State { get; set; }      // PA
        public string? ZipCode { get; set; }    // 17033
        public DateTime BirthDate { get; set; } // 1980-01-01
        public string Gender { get; set; }     // F
    }

    public class MedicalClaim
    {
        public int Id { get; set; }
        public int ClaimBatchId { get; set; }
        public string ClaimSubmitterIdentifier { get; set; } // 1000123 (CLM01)
        public decimal TotalClaimChargeAmount { get; set; }    // 150.50 (CLM02)
        public string FacilityCode { get; set; }              // 11 (Office - CLM05-1)
        public DateTime StatementDate { get; set; }           // 2026-07-07 (DTP)

        // Foreign keys to separate master directories (or nested copies)
        public int BillingProviderId { get; set; }
        public int SubscriberPatientId { get; set; }

        public List<ClaimServiceLine> ServiceLines { get; set; }
        public List<ClaimDiagnosis> Diagnoses { get; set; }
    }

    public class ClaimServiceLine
    {
        public int Id { get; set; }
        public int MedicalClaimId { get; set; }
        public int LineNumber { get; set; }            // 1 (LX01)
        public string ProcedureCode { get; set; }      // 99213 (SV101-2)
        public decimal LineChargeAmount { get; set; }  // 150.50 (SV102)
        public decimal UnitCount { get; set; }         // 1 (SV104)
        public DateTime ServiceDate { get; set; }      // 2026-07-07 (DTP)
    }

    public class ClaimDiagnosis
    {
        public int Id { get; set; }
        public int MedicalClaimId { get; set; }
        public string DiagnosisCode { get; set; }      // J019, A09 (HI)
        public string DiagnosisType { get; set; }      // BK (Principal), BF (Secondary)
    }
}
