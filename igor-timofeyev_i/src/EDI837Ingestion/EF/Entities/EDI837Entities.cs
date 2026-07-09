using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI837Ingestion.EF.Entities
{
    public class RawEdiFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }           // raw bytes
        public long Size { get; set; }
        public DateTimeOffset ReceivedAt { get; set; }
    }

    public class Interchange
    {
        public int Id { get; set; }
        public string IsaControlNumber { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ICollection<FunctionalGroup> FunctionalGroups { get; set; }
    }

    public class FunctionalGroup
    {
        public int Id { get; set; }
        public string GroupControlNumber { get; set; }
        public string FunctionalId { get; set; }

        public int InterchangeId { get; set; }
        public Interchange Interchange { get; set; }

        public ICollection<TransactionSet> TransactionSets { get; set; }
    }

    public class TransactionSet
    {
        public int Id { get; set; }
        public string TransactionControlNumber { get; set; }
        public string TransactionType { get; set; } // "837"
        public DateTimeOffset TransactionDate { get; set; }
        public string Status { get; set; }
        public string RawJson { get; set; }

        public int? InterchangeId { get; set; }
        public Interchange Interchange { get; set; }

        public int? FunctionalGroupId { get; set; }
        public FunctionalGroup FunctionalGroup { get; set; }

        public int? RawEdiFileId { get; set; }
        public RawEdiFile RawEdiFile { get; set; }

        public ICollection<Claim> Claims { get; set; }
        public ICollection<ValidationIssue> ValidationIssues { get; set; }
    }

    public class Payer
    {
        public int Id { get; set; }
        public string PayerIdentifier { get; set; } // e.g., NM109
        public string Name { get; set; }

        public ICollection<Claim> Claims { get; set; }
    }

    public class Patient
    {
        public int Id { get; set; }
        public string MemberId { get; set; } // Subscriber/Member ID
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
    }

    public class Provider
    {
        public int Id { get; set; }
        public string Npi { get; set; }
        public string TaxId { get; set; }
        public string Name { get; set; }
        public string ProviderType { get; set; } // Billing, Rendering, etc.
    }

    public class Claim
    {
        public int Id { get; set; }
        public int TransactionSetId { get; set; }
        public TransactionSet TransactionSet { get; set; }

        public string ClaimNumber { get; set; } // CLM01 or generated
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }
        public int? PayerId { get; set; }
        public Payer Payer { get; set; }

        public int? BillingProviderId { get; set; }
        public Provider BillingProvider { get; set; }

        public int? RenderingProviderId { get; set; }
        public Provider RenderingProvider { get; set; }

        public decimal TotalChargeAmount { get; set; }
        public DateTimeOffset? ClaimDate { get; set; }

        public ICollection<ClaimLineItem> LineItems { get; set; }
        public ICollection<Diagnosis> Diagnoses { get; set; }
    }

    public class ClaimLineItem
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        public int LineNumber { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public string ProcedureCode { get; set; }
        public string Modifiers { get; set; } // optional JSON or delimited
        public decimal Units { get; set; }
        public decimal UnitCharge { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class Diagnosis
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        public int Position { get; set; } // sequence in HI segment
        public string Code { get; set; }
        public string Version { get; set; } // ICD10/ICD9
    }

    public class ValidationIssue
    {
        public int Id { get; set; }
        public int? TransactionSetId { get; set; }
        public TransactionSet TransactionSet { get; set; }
        public int? ClaimId { get; set; }
        public Claim Claim { get; set; }

        public string Segment { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
