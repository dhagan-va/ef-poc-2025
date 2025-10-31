using System.Text;

namespace EDI275AttachmentParser.Models
{
    /// <summary>
    /// Represents a parsed EDI 275 document with attachments
    /// </summary>
    public class EDI275Document
    {
        public string InterchangeControlNumber { get; set; } = string.Empty;
        public string GroupControlNumber { get; set; } = string.Empty;
        public string TransactionSetControlNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public List<PatientReport> PatientReports { get; set; } = new();
        public List<Attachment> Attachments { get; set; } = new();
    }

    /// <summary>
    /// Represents a patient report in the 275 transaction
    /// </summary>
    public class PatientReport
    {
        public string PatientId { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ReportTypeCode { get; set; } = string.Empty;
        public string ReportTransmissionCode { get; set; } = string.Empty;
        public List<string> ReferenceCodes { get; set; } = new();
    }

    /// <summary>
    /// Represents an attachment in the EDI 275
    /// </summary>
    public class Attachment
    {
        public string AttachmentControlNumber { get; set; } = string.Empty;
        public string AttachmentTypeCode { get; set; } = string.Empty;
        public string TransmissionCode { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public byte[]? BinaryData { get; set; }
        public string? Base64Data { get; set; }
        public string? TextData { get; set; }
        public DateTime AttachmentDate { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public string GetDecodedText()
        {
            if (!string.IsNullOrEmpty(TextData))
                return TextData;
                
            if (!string.IsNullOrEmpty(Base64Data))
            {
                var bytes = Convert.FromBase64String(Base64Data);
                return Encoding.UTF8.GetString(bytes);
            }
            
            if (BinaryData != null)
                return Encoding.UTF8.GetString(BinaryData);
                
            return string.Empty;
        }
        
        public void SaveToFile(string directory)
        {
            var filePath = Path.Combine(directory, FileName);
            
            if (BinaryData != null)
            {
                File.WriteAllBytes(filePath, BinaryData);
            }
            else if (!string.IsNullOrEmpty(Base64Data))
            {
                var bytes = Convert.FromBase64String(Base64Data);
                File.WriteAllBytes(filePath, bytes);
            }
            else if (!string.IsNullOrEmpty(TextData))
            {
                File.WriteAllText(filePath, TextData);
            }
        }
    }
}
