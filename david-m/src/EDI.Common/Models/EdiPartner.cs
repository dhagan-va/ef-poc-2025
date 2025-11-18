namespace EDI.Common.Models
{
    public class EdiPartner
    {
        public int Id { get; set; }
        public string PartnerId { get; set; } = string.Empty;
        public string PartnerName { get; set; } = string.Empty;
        public string PartnerType { get; set; } = string.Empty; // Sender, Receiver, Provider, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
