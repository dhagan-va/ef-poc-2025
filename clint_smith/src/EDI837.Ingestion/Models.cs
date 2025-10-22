namespace EDI837.Ingestion.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string ClaimNumber { get; set; }
        public string PayerId { get; set; }
        public string ProviderId { get; set; }
        public string SubscriberId { get; set; }
        public string PatientId { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public string ClaimType { get; set; }  // P, I, D
        public string Status { get; set; }

        public Provider Provider { get; set; }
        public Subscriber Subscriber { get; set; }
        public Patient Patient { get; set; }

        public ICollection<ServiceLine> ServiceLines { get; set; } = new List<ServiceLine>();
    }

    public class Provider
    {
        public int Id { get; set; }
        public string Npi { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
    }

    public class Subscriber
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
    }

    public class ServiceLine
    {
        public int Id { get; set; }
        public string ProcedureCode { get; set; }
        public decimal ChargeAmount { get; set; }
        public int Units { get; set; }
        public DateTime ServiceDate { get; set; }

        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
