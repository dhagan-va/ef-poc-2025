using EDI275AttachmentParser.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EDI275AttachmentParser.Services
{
    /// <summary>
    /// Manual EDI 275 parser that doesn't require EdiFabric license
    /// Simple implementation for parsing basic EDI 275 structure
    /// </summary>
    public class ManualEDI275Parser
    {
        private readonly ILogger<ManualEDI275Parser> _logger;
        private const char SEGMENT_TERMINATOR = '~';
        private const char ELEMENT_SEPARATOR = '*';

        public ManualEDI275Parser(ILogger<ManualEDI275Parser> logger)
        {
            _logger = logger;
        }

        public EDI275Document ParseFile(string filePath)
        {
            _logger.LogInformation("Parsing EDI 275 file (manual parser): {FilePath}", filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"EDI file not found: {filePath}");
            }

            var content = File.ReadAllText(filePath);
            var document = new EDI275Document();
            
            // Split into segments
            var segments = content.Split(SEGMENT_TERMINATOR, StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {
                var trimmed = segment.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                var elements = trimmed.Split(ELEMENT_SEPARATOR);
                var segmentId = elements[0];

                switch (segmentId)
                {
                    case "ISA":
                        ParseISA(elements, document);
                        break;
                    case "GS":
                        ParseGS(elements, document);
                        break;
                    case "ST":
                        ParseST(elements, document);
                        break;
                    case "BHT":
                        ParseBHT(elements, document);
                        break;
                    case "NM1":
                        ParseNM1(elements, document);
                        break;
                    case "PWK":
                        ParsePWK(elements, document);
                        break;
                    case "BIN":
                        ParseBIN(elements, document);
                        break;
                }
            }

            _logger.LogInformation("Parsed {AttachmentCount} attachments", document.Attachments.Count);
            return document;
        }

        private void ParseISA(string[] elements, EDI275Document document)
        {
            // ISA*00*          *00*          *ZZ*SENDERID       *ZZ*ReceiverId     *231031*1430*^*00801*000000001*0*P*:
            if (elements.Length >= 14)
            {
                document.SenderId = elements[6].Trim();
                document.ReceiverId = elements[8].Trim();
                document.InterchangeControlNumber = elements[13].Trim();

                _logger.LogDebug("ISA - Sender: {Sender}, Receiver: {Receiver}, Control: {Control}",
                    document.SenderId, document.ReceiverId, document.InterchangeControlNumber);
            }
        }

        private void ParseGS(string[] elements, EDI275Document document)
        {
            // GS*HS*SENDERAPP*RECEIVERAPP*20231031*1430*1*X*008010
            if (elements.Length >= 7)
            {
                document.GroupControlNumber = elements[6].Trim();

                if (elements.Length >= 5)
                {
                    var dateStr = elements[4].Trim();
                    var timeStr = elements.Length > 5 ? elements[5].Trim() : "0000";
                    
                    if (DateTime.TryParseExact($"{dateStr}{timeStr.PadLeft(4, '0')}", 
                        "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out var date))
                    {
                        document.TransactionDate = date;
                    }
                }

                _logger.LogDebug("GS - Control: {Control}, Date: {Date}", 
                    document.GroupControlNumber, document.TransactionDate);
            }
        }

        private void ParseST(string[] elements, EDI275Document document)
        {
            // ST*275*0001*005010X215
            if (elements.Length >= 3)
            {
                document.TransactionSetControlNumber = elements[2].Trim();
                _logger.LogDebug("ST - Control: {Control}", document.TransactionSetControlNumber);
            }
        }

        private void ParseBHT(string[] elements, EDI275Document document)
        {
            // BHT*0085*01*275REFERENCE*20231031*1430
            _logger.LogDebug("BHT segment parsed");
        }

        private void ParseNM1(string[] elements, EDI275Document document)
        {
            // NM1*IL*1*DOE*JOHN****MI*123456789
            if (elements.Length >= 2 && elements[1] == "IL") // Patient/Insured
            {
                var patient = new PatientReport();

                if (elements.Length >= 4)
                {
                    patient.PatientName = elements[3].Trim(); // Last name
                }

                if (elements.Length >= 5 && !string.IsNullOrEmpty(elements[4]))
                {
                    patient.PatientName = $"{elements[4].Trim()} {patient.PatientName}"; // First name + Last name
                }

                if (elements.Length >= 10)
                {
                    patient.PatientId = elements[9].Trim();
                }

                document.PatientReports.Add(patient);
                _logger.LogInformation("Parsed patient: {Name} (ID: {Id})", 
                    patient.PatientName, patient.PatientId);
            }
        }

        private void ParsePWK(string[] elements, EDI275Document document)
        {
            // PWK*OZ*EL*****Medical Records Attachment
            var attachment = new Attachment
            {
                AttachmentDate = DateTime.UtcNow
            };

            if (elements.Length >= 2)
            {
                attachment.AttachmentTypeCode = elements[1].Trim();
            }

            if (elements.Length >= 3)
            {
                attachment.TransmissionCode = elements[2].Trim();
            }

            if (elements.Length >= 7 && !string.IsNullOrEmpty(elements[6]))
            {
                attachment.Description = elements[6].Trim();
            }

            // Add to document or update last one if BIN segment follows
            document.Attachments.Add(attachment);
            
            _logger.LogInformation("Parsed PWK - Type: {Type}, Description: {Desc}",
                attachment.AttachmentTypeCode, attachment.Description);
        }

        private void ParseBIN(string[] elements, EDI275Document document)
        {
            // BIN*1024*VGhpcyBpcyBhIHNhbXBsZSBhdHRhY2htZW50...
            if (elements.Length >= 3)
            {
                // Get the last attachment (should have been created by PWK)
                Attachment attachment;
                if (document.Attachments.Count > 0)
                {
                    attachment = document.Attachments[^1];
                }
                else
                {
                    attachment = new Attachment { AttachmentDate = DateTime.UtcNow };
                    document.Attachments.Add(attachment);
                }

                // BIN01 - Length
                if (long.TryParse(elements[1].Trim(), out var length))
                {
                    attachment.FileSize = length;
                }

                // BIN02 - Binary Data (usually Base64)
                var binaryData = elements[2].Trim();
                
                try
                {
                    // Try to decode as Base64
                    attachment.Base64Data = binaryData;
                    attachment.BinaryData = Convert.FromBase64String(binaryData);
                    attachment.FileName = $"attachment_{document.Attachments.Count}.bin";
                    
                    // Try to detect if it's text
                    try
                    {
                        var text = Encoding.UTF8.GetString(attachment.BinaryData);
                        if (text.All(c => c < 128 || char.IsWhiteSpace(c))) // ASCII check
                        {
                            attachment.TextData = text;
                            attachment.FileName = $"attachment_{document.Attachments.Count}.txt";
                            attachment.MimeType = "text/plain";
                        }
                    }
                    catch
                    {
                        attachment.MimeType = "application/octet-stream";
                    }
                }
                catch (FormatException)
                {
                    // Not Base64, treat as plain text
                    attachment.TextData = binaryData;
                    attachment.FileName = $"attachment_{document.Attachments.Count}.txt";
                    attachment.MimeType = "text/plain";
                }

                _logger.LogInformation("Parsed BIN - Size: {Size} bytes, File: {File}",
                    attachment.FileSize, attachment.FileName);
            }
        }

        public void ExtractAttachments(EDI275Document document, string outputDirectory)
        {
            _logger.LogInformation("Extracting {Count} attachments to {Dir}",
                document.Attachments.Count, outputDirectory);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            foreach (var attachment in document.Attachments)
            {
                try
                {
                    attachment.SaveToFile(outputDirectory);
                    _logger.LogInformation("Saved attachment: {FileName}", attachment.FileName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving attachment: {FileName}", attachment.FileName);
                }
            }
        }
    }
}
