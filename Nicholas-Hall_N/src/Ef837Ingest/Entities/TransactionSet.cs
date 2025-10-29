namespace Edi837Ingestion.Domain.Entities;

public class TransactionSet
{
    public int Id { get; set; }

    public int FunctionalGroupId { get; set; }
    public FunctionalGroup FunctionalGroup { get; set; } = null!;

    // ST/BHT highlights
    public string? St01 { get; set; }           
    public string? ControlNumber { get; set; }  
    public string? Bht02 { get; set; }          
    public string? Bht06 { get; set; }      

    // Raw serialized EDI model for diagnostics/audit
    public string? RawJson { get; set; }

    public ICollection<ClaimStub> ClaimStubs { get; set; } = new List<ClaimStub>();
}
