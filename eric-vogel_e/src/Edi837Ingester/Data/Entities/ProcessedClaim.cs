using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edi837Ingester.Data.Entities
{
    public class ProcessedClaim
    {
        public  int Id { get; set; }
        [Required]
        public required string ClaimControlNumber { get; set; }
        [Required]
        public required string ClaimXml { get; set; }
        public required int ClaimTypeId { get; set; }
        public virtual ClaimType ClaimType { get; set; } = null!;
        public DateTime ProcessedOn { get; set; } = DateTime.UtcNow;
    }
}
