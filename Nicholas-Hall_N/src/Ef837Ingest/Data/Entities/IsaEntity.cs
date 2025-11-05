using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Data.Entities
{
    public class IsaEntity
    {
        public long Id { get; set; }
        public string? InterchangeControlNumber { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string RawJson { get; set; } = "{}";
    }
}
