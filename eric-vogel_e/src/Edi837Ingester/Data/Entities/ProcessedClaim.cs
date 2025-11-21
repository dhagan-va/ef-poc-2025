using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edi837Ingester.Data.Entities
{
    public class ProcessedClaim
    {
        public required int Id { get; set; }
        public int ClaimTypeId { get; set; }
        [ForeignKey(nameof(ClaimTypeId))]
        public virtual ClaimType ClaimType { get; set; } = null!;
        public string? ProviderNPI { get; set; }
        public string? TransactionControlNumber { get; set; }
        public DateTime Received { get; set; }
    }
}
