using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI837Ingestion.Consumer.Application.Models
{
    public sealed class SnipValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public string SourceName { get; init; } = string.Empty;
        public string? InterchangeControlNumber { get; init; }
        public string? GroupControlNumber { get; init; }
        public string? TransactionSetControlNumber { get; init; }
        public List<SnipValidationError> Errors { get; } = new();
    }
}
