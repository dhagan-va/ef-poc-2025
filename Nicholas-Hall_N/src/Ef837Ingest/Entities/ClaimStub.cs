namespace Edi837Ingestion.Domain.Entities;

public class ClaimStub
{
    public int Id { get; set; }

    // FK to TransactionSet
    public int TransactionSetId { get; set; }
    public TransactionSet TransactionSet { get; set; } = null!;

    public string PatientControlNumber { get; set; } = string.Empty;
    public decimal? TotalClaimCharge { get; set; }                   
}
