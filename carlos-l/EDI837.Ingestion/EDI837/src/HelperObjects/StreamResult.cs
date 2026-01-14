using System.ComponentModel.DataAnnotations;

namespace EDI837.src.HelperObjects
{
    public class StreamResult
    {
        public required string FileName { get; set; }
        public required Stream FileStream { get; set; }
    }
}
