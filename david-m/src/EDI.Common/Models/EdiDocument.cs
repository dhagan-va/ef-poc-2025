using EDI.Common.Constants;

namespace EDI.Common.Models
{
    public class EdiDocument
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; } = string.Empty;
        public EdiDocumentType DocumentType { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public EdiProcessingStatus Status { get; set; } = EdiProcessingStatus.Pending;
    }
}
