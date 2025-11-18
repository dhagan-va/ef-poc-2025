namespace EDI.Common.Models
{
    public class EdiClaim
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Use string since CLM ids are alphanumeric
        public string EdiDocumentId { get; set; } = string.Empty;
        public EdiDocument? EdiDocument { get; set; }
        public string PatientControlNumber { get; set; } = string.Empty;
        public decimal TotalClaimChargeAmount { get; set; }
        public string ClaimStatusCode { get; set; } = string.Empty;
        public string ProviderAcceptAssignmentCode { get; set; } = string.Empty;
        public bool PatientSignatureSourceCode { get; set; }
        public string RelatedCausesCode { get; set; } = string.Empty;
        public string SpecialProgramCode { get; set; } = string.Empty;
        public bool PatientPaidAmount { get; set; }
        public string FacilityTypeCode { get; set; } = string.Empty;
        public string BenefitAssignmentCertificationIndicator { get; set; } = string.Empty;
        public string ReleaseInformationCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
