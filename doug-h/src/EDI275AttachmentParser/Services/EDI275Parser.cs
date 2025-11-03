using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EDI275AttachmentParser.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EDI275AttachmentParser.Services
{
    /// <summary>
    /// Parser service for EDI 275 documents with attachment support
    /// </summary>
    public class EDI275Parser
    {
        private readonly ILogger<EDI275Parser> _logger;

        public EDI275Parser(ILogger<EDI275Parser> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parse an EDI 275 file and extract attachments
        /// </summary>
        public EDI275Document ParseFile(string filePath)
        {
            _logger.LogInformation("Parsing EDI 275 file: {FilePath}", filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"EDI file not found: {filePath}");
            }

            var document = new EDI275Document();

            try
            {
                using (var stream = File.OpenRead(filePath))
                using (var reader = new X12Reader(stream, "EdiFabric.Rules.X12008010.Rep"))
                {
                    while (reader.Read())
                    {
                        var item = reader.Item;

                        if (item is ISA isa)
                        {
                            ParseISA(isa, document);
                        }
                        else if (item is GS gs)
                        {
                            ParseGS(gs, document);
                        }
                        else if (item is ST st)
                        {
                            ParseST(st, document);
                        }
                        else
                        {
                            // Parse transaction-specific segments
                            ParseTransactionSegments(item, document);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex.Message.Contains("token was not set"))
            {
                _logger.LogError("EdiFabric license not set. Please provide a valid license key.");
                _logger.LogError("Get trial license at: https://www.edifabric.com/trial.html");
                throw new InvalidOperationException("EdiFabric license required. Use --license option or set EDIFABRIC_LICENSE environment variable.", ex);
            }

            _logger.LogInformation("Parsed {AttachmentCount} attachments from EDI 275", document.Attachments.Count);
            return document;
        }

        private void ParseISA(ISA isa, EDI275Document document)
        {
            // Use reflection to handle different EdiFabric versions
            var interchangeControl = isa.GetType().GetProperty("InterchangeControlNumber")?.GetValue(isa)?.ToString()
                ?? isa.GetType().GetProperty("ISA13")?.GetValue(isa)?.ToString()
                ?? string.Empty;
                
            var senderId = isa.GetType().GetProperty("InterchangeSenderID")?.GetValue(isa)?.ToString()
                ?? isa.GetType().GetProperty("ISA06")?.GetValue(isa)?.ToString()
                ?? string.Empty;
                
            var receiverId = isa.GetType().GetProperty("InterchangeReceiverID")?.GetValue(isa)?.ToString()
                ?? isa.GetType().GetProperty("ISA08")?.GetValue(isa)?.ToString()
                ?? string.Empty;

            document.InterchangeControlNumber = interchangeControl;
            document.SenderId = senderId;
            document.ReceiverId = receiverId;

            _logger.LogDebug("ISA - Control: {Control}, Sender: {Sender}, Receiver: {Receiver}",
                document.InterchangeControlNumber, document.SenderId, document.ReceiverId);
        }

        private void ParseGS(GS gs, EDI275Document document)
        {
            var groupControl = gs.GetType().GetProperty("GroupControlNumber")?.GetValue(gs)?.ToString()
                ?? gs.GetType().GetProperty("GS06")?.GetValue(gs)?.ToString()
                ?? string.Empty;
                
            var dateStr = gs.GetType().GetProperty("Date")?.GetValue(gs)?.ToString()
                ?? gs.GetType().GetProperty("GS04")?.GetValue(gs)?.ToString()
                ?? string.Empty;
                
            var timeStr = gs.GetType().GetProperty("Time")?.GetValue(gs)?.ToString()
                ?? gs.GetType().GetProperty("GS05")?.GetValue(gs)?.ToString()
                ?? string.Empty;
            
            document.GroupControlNumber = groupControl;
            
            if (DateTime.TryParseExact($"{dateStr}{timeStr}", 
                "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out var date))
            {
                document.TransactionDate = date;
            }

            _logger.LogDebug("GS - Control: {Control}, Date: {Date}", 
                document.GroupControlNumber, document.TransactionDate);
        }

        private void ParseST(ST st, EDI275Document document)
        {
            // EdiFabric ST segment properties
            var controlNumber = st.GetType().GetProperty("TransactionSetControlNumber")?.GetValue(st)?.ToString()
                ?? st.GetType().GetProperty("ST02")?.GetValue(st)?.ToString()
                ?? string.Empty;
            
            document.TransactionSetControlNumber = controlNumber;
            _logger.LogDebug("ST - Control: {Control}", document.TransactionSetControlNumber);
        }

        private void ParseTransactionSegments(object item, EDI275Document document)
        {
            var segmentType = item.GetType().Name;

            // Parse BIN segment (Binary Data Segment) for attachments
            if (segmentType == "BIN" || item.ToString()?.StartsWith("BIN") == true)
            {
                ParseBinarySegment(item, document);
            }
            // Parse PWK segment (Paperwork Segment) for attachment metadata
            else if (segmentType == "PWK" || item.ToString()?.StartsWith("PWK") == true)
            {
                ParsePaperworkSegment(item, document);
            }
            // Parse NM1 segment for patient information
            else if (segmentType == "NM1" || item.ToString()?.StartsWith("NM1") == true)
            {
                ParseNameSegment(item, document);
            }
        }

        private void ParseBinarySegment(object segment, EDI275Document document)
        {
            try
            {
                // The BIN segment contains binary data
                var segmentString = segment.ToString() ?? string.Empty;
                var elements = segmentString.Split('*');

                if (elements.Length > 1)
                {
                    var attachment = new Attachment
                    {
                        AttachmentDate = DateTime.UtcNow
                    };

                    // BIN01 - Length of Binary Data
                    if (elements.Length > 1 && long.TryParse(elements[1], out var length))
                    {
                        attachment.FileSize = length;
                    }

                    // BIN02 - Binary Data (Base64 encoded in EDI)
                    if (elements.Length > 2 && !string.IsNullOrEmpty(elements[2]))
                    {
                        try
                        {
                            attachment.Base64Data = elements[2];
                            attachment.BinaryData = Convert.FromBase64String(elements[2]);
                            attachment.FileName = $"attachment_{document.Attachments.Count + 1}.bin";
                        }
                        catch (FormatException)
                        {
                            // If not Base64, treat as text
                            attachment.TextData = elements[2];
                            attachment.FileName = $"attachment_{document.Attachments.Count + 1}.txt";
                        }
                    }

                    document.Attachments.Add(attachment);
                    _logger.LogInformation("Parsed BIN segment - Size: {Size} bytes", attachment.FileSize);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing BIN segment");
            }
        }

        private void ParsePaperworkSegment(object segment, EDI275Document document)
        {
            try
            {
                // PWK segment contains attachment metadata
                var segmentString = segment.ToString() ?? string.Empty;
                var elements = segmentString.Split('*');

                if (elements.Length > 1)
                {
                    // Get or create the last attachment
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

                    // PWK01 - Report Type Code
                    if (elements.Length > 1)
                    {
                        attachment.AttachmentTypeCode = elements[1];
                    }

                    // PWK02 - Report Transmission Code
                    if (elements.Length > 2)
                    {
                        attachment.TransmissionCode = elements[2];
                    }

                    // PWK05 - Identification Code (Control Number)
                    if (elements.Length > 5)
                    {
                        attachment.AttachmentControlNumber = elements[5];
                    }

                    // PWK06 - Description
                    if (elements.Length > 6)
                    {
                        attachment.Description = elements[6];
                    }

                    _logger.LogInformation("Parsed PWK segment - Type: {Type}, Description: {Desc}", 
                        attachment.AttachmentTypeCode, attachment.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing PWK segment");
            }
        }

        private void ParseNameSegment(object segment, EDI275Document document)
        {
            try
            {
                var segmentString = segment.ToString() ?? string.Empty;
                var elements = segmentString.Split('*');

                if (elements.Length > 1 && elements[1] == "IL") // Patient/Insured
                {
                    var patient = new PatientReport();

                    // NM103 - Last Name
                    if (elements.Length > 3)
                    {
                        patient.PatientName = elements[3];
                    }

                    // NM104 - First Name
                    if (elements.Length > 4 && !string.IsNullOrEmpty(elements[4]))
                    {
                        patient.PatientName = $"{elements[4]} {patient.PatientName}";
                    }

                    // NM109 - Patient ID
                    if (elements.Length > 9)
                    {
                        patient.PatientId = elements[9];
                    }

                    document.PatientReports.Add(patient);
                    _logger.LogInformation("Parsed patient: {Name} (ID: {Id})", 
                        patient.PatientName, patient.PatientId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing NM1 segment");
            }
        }

        /// <summary>
        /// Extract and save all attachments to a directory
        /// </summary>
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
