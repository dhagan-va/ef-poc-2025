using Edi837Ingestion.Domain.Entities;

namespace Ef837Ingest.Data
{
    public class Claim837
    {
        public int Id { get; set; }
        public string? StControlNumber { get; set; }
        public string? Bht03ReferenceId { get; set; }
        public string? PatientLastName { get; set; }
        public string? PatientFirstName { get; set; }
        public decimal? TotalClaimAmount { get; set; }
        public int FunctionalGroupId { get; set; }
        public FunctionalGroup FunctionalGroup { get; set; } = null!;
    }
}