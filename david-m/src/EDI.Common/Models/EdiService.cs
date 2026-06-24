namespace EDI.Common.Models
{
    public class EdiService
    {
        public int Id { get; set; }
        public string ClaimId { get; set; } = string.Empty;
        public EdiClaim? Claim { get; set; }
        public string ProcedureCode { get; set; } = string.Empty; // HC:99213
        public string Modifier1 { get; set; } = string.Empty;
        public string Modifier2 { get; set; } = string.Empty;
        public string Modifier3 { get; set; } = string.Empty;
        public string Modifier4 { get; set; } = string.Empty;
        public decimal LineItemChargeAmount { get; set; }
        public string UnitOfMeasurement { get; set; } = string.Empty; // UN = Units
        public decimal Quantity { get; set; }
        public string DiagnosisCodePointer { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
