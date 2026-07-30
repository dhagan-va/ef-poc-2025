using System;

namespace Deepika.EDIIngestion.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string? ClaimNumber { get; set; }
        public decimal? TotalChargeAmount { get; set; }
        public DateTime? ServiceDate { get; set; }

        // Foreign keys
        public int? InterchangeId { get; set; }
        public EdiInterchange? Interchange { get; set; }

        public int? ProviderId { get; set; }
        public Provider? Provider { get; set; }
    }
}
