using System;

namespace Deepika.EDIIngestion.Models
{
    public class EdiInterchange
    {
        public int Id { get; set; }
        public string? IsaControlNumber { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? GsControlNumber { get; set; }
        public string? TransactionSetId { get; set; }
        public string? TransactionSetControlNumber { get; set; }
        public string? BhtReference { get; set; }
        public string? RawIsa { get; set; }
        public string? RawGs { get; set; }
        public string? RawSt { get; set; }
        public string? RawBht { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Relationships
        public int? ProviderId { get; set; }
        public Provider? Provider { get; set; }
        public ICollection<Claim>? Claims { get; set; }
    }
}
