using System;

namespace Deepika.EDIIngestion.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public string? ErrorMessage { get; set; }
        public string? ErrorType { get; set; }
        public string? StackTrace { get; set; }
        public string? FileSnippet { get; set; }
    }
}
