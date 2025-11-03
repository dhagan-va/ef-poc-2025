using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EDI275AttachmentParser.Models;
using EDI275AttachmentParser.Templates;
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
                using (var reader = new X12Reader(stream, "EDI275AttachmentParser.Templates"))
                {
                    while (reader.Read())
                    {
                        var item = reader.Item;
                        
                        if (item is ISA isa)
                        {
                            ParseISASegment(isa, document);
                        }
                        else if (item is GS gs)
                        {
                            ParseGSSegment(gs, document);
                        }
                        else if (item is TS275 ts275)
                        {
                            Parse275Transaction(ts275, document);
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
        
        private void Parse275Transaction(TS275 transaction, EDI275Document document)
        {
            // Parse ST segment
            if (transaction.ST != null)
            {
                document.TransactionSetControlNumber = transaction.ST.TransactionSetControlNumber_02 ?? string.Empty;
                _logger.LogDebug("ST - Control: {Control}", document.TransactionSetControlNumber);
            }
            
            // Parse each information source loop (2000)
            if (transaction.Loop2000 != null)
            {
                foreach (var loop2000 in transaction.Loop2000)
                {
                    // Parse patient loops (2100)
                    if (loop2000.Loop2100 != null)
                    {
                        foreach (var loop2100 in loop2000.Loop2100)
                        {
                            ParsePatientLoop(loop2100, document);
                        }
                    }
                }
            }
        }
        
        private void ParsePatientLoop(Loop_2100_275 patientLoop, EDI275Document document)
        {
            // Parse patient information
            var patient = new PatientReport();
            
            if (patientLoop.NM1 != null)
            {
                patient.PatientName = $"{patientLoop.NM1.NameFirst_04} {patientLoop.NM1.NameLast_03}".Trim();
                patient.PatientId = patientLoop.NM1.IdentificationCode_09 ?? string.Empty;
                
                _logger.LogDebug("Parsed patient: {Name} (ID: {Id})", patient.PatientName, patient.PatientId);
            }
            
            if (patientLoop.DMG != null)
            {
                patient.Gender = patientLoop.DMG.GenderCode_03 ?? string.Empty;
                
                if (DateTime.TryParseExact(patientLoop.DMG.DateTimePeriod_02, 
                    "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var dob))
                {
                    patient.DateOfBirth = dob;
                }
            }
            
            document.PatientReports.Add(patient);
            
            // Parse attachment loops (2110)
            if (patientLoop.Loop2110 != null)
            {
                foreach (var loop2110 in patientLoop.Loop2110)
                {
                    ParseAttachmentLoop(loop2110, document);
                }
            }
        }
        
        private void ParseAttachmentLoop(Loop_2110_275 attachmentLoop, EDI275Document document)
        {
            var attachment = new Attachment
            {
                AttachmentDate = DateTime.UtcNow
            };
            
            // Parse PWK segment
            if (attachmentLoop.PWK != null)
            {
                attachment.AttachmentTypeCode = attachmentLoop.PWK.ReportTypeCode_01 ?? string.Empty;
                attachment.TransmissionCode = attachmentLoop.PWK.ReportTransmissionCode_02 ?? string.Empty;
                attachment.Description = attachmentLoop.PWK.Description_06 ?? string.Empty;
                
                // If description looks like a filename, use it with timestamp
                if (!string.IsNullOrEmpty(attachment.Description) && attachment.Description.Contains('.'))
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var ext = Path.GetExtension(attachment.Description);
                    var nameWithoutExt = Path.GetFileNameWithoutExtension(attachment.Description);
                    attachment.FileName = $"{nameWithoutExt}_{timestamp}{ext}";
                }
                
                _logger.LogInformation("Parsed PWK segment - Type: {Type}, Description: {Desc}", 
                    attachment.AttachmentTypeCode, attachment.Description);
            }
            
            // Parse BIN segments
            if (attachmentLoop.BIN != null && attachmentLoop.BIN.Count > 0)
            {
                foreach (var bin in attachmentLoop.BIN)
                {
                    ParseBINSegment(bin, attachment, document);
                }
            }
            
            // Only add if we have actual data
            if (!string.IsNullOrEmpty(attachment.Base64Data) || attachment.BinaryData != null)
            {
                document.Attachments.Add(attachment);
            }
        }
        
        private void ParseBINSegment(BIN_BinaryData bin, Attachment attachment, EDI275Document document)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                
                // BIN01 - Length of Binary Data
                if (long.TryParse(bin.LengthofBinaryData_01, out var length))
                {
                    attachment.FileSize = length;
                }

                // BIN02 - Binary Data (Base64 encoded)
                if (!string.IsNullOrEmpty(bin.BinaryData_02))
                {
                    var base64Data = bin.BinaryData_02.TrimEnd('~');
                    attachment.Base64Data = base64Data;

                    // Try to decode
                    try
                    {
                        var bytes = Convert.FromBase64String(base64Data);
                        attachment.BinaryData = bytes;
                        
                        // Check if it looks like text
                        if (IsLikelyText(bytes))
                        {
                            attachment.TextData = Encoding.UTF8.GetString(bytes);
                            if (string.IsNullOrEmpty(attachment.FileName))
                            {
                                attachment.FileName = $"attachment_{timestamp}_{document.Attachments.Count + 1}.txt";
                            }
                        }
                        else if (string.IsNullOrEmpty(attachment.FileName))
                        {
                            attachment.FileName = $"attachment_{timestamp}_{document.Attachments.Count + 1}.bin";
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not decode Base64 data");
                        if (string.IsNullOrEmpty(attachment.FileName))
                        {
                            attachment.FileName = $"attachment_{timestamp}_{document.Attachments.Count + 1}.bin";
                        }
                    }
                }

                _logger.LogInformation("Parsed BIN segment - Size: {Size} bytes, File: {FileName}", 
                    attachment.FileSize, attachment.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing BIN segment");
            }
        }
        
        private bool IsLikelyText(byte[] data)
        {
            if (data.Length == 0) return false;
            
            // Check if most bytes are printable ASCII
            int printableCount = 0;
            foreach (var b in data)
            {
                if ((b >= 32 && b <= 126) || b == 9 || b == 10 || b == 13) // printable + tab/newline/CR
                {
                    printableCount++;
                }
            }
            
            return (double)printableCount / data.Length > 0.8; // 80% printable = likely text
        }
        
        private void ParseISASegment(ISA isa, EDI275Document document)
        {
            document.InterchangeControlNumber = isa.InterchangeControlNumber_13 ?? string.Empty;
            document.SenderId = isa.InterchangeSenderID_06 ?? string.Empty;
            document.ReceiverId = isa.InterchangeReceiverID_08 ?? string.Empty;

            _logger.LogDebug("ISA - Control: {Control}, Sender: {Sender}, Receiver: {Receiver}",
                document.InterchangeControlNumber, document.SenderId, document.ReceiverId);
        }

        private void ParseGSSegment(GS gs, EDI275Document document)
        {
            document.GroupControlNumber = gs.GroupControlNumber_06 ?? string.Empty;
            
            var dateStr = gs.Date_04 ?? string.Empty;
            var timeStr = gs.Time_05 ?? string.Empty;
            
            if (DateTime.TryParseExact($"{dateStr}{timeStr}", 
                "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out var date))
            {
                document.TransactionDate = date;
            }

            _logger.LogDebug("GS - Control: {Control}, Date: {Date}", 
                document.GroupControlNumber, document.TransactionDate);
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
                    var savedPath = attachment.SaveToFile(outputDirectory);
                    _logger.LogInformation("Saved attachment to: {FilePath}", savedPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving attachment: {FileName}", attachment.FileName);
                }
            }
        }
        
        /// <summary>
        /// Parse an EDI 275 file and extract attachments to a directory, returning the list of saved file paths
        /// </summary>
        public List<string> ExtractAttachmentsFromFile(string filePath, string outputDirectory)
        {
            var document = ParseFile(filePath);
            var savedPaths = new List<string>();
            
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
                    var savedPath = attachment.SaveToFile(outputDirectory);
                    savedPaths.Add(savedPath);
                    _logger.LogInformation("Saved attachment to: {FilePath}", savedPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving attachment: {FileName}", attachment.FileName);
                }
            }
            
            return savedPaths;
        }
    }
}
