using System.Collections.Generic;

namespace Edi837Ingestion.Domain.Entities;

public class FunctionalGroup
{
    public int Id { get; set; }

    // FK to Interchange
    public int InterchangeId { get; set; }
    public Interchange Interchange { get; set; } = null!;

    // GS fields
    public string? Gs01 { get; set; }               
    public string? Gs02 { get; set; }              
    public string? Gs03 { get; set; }             
    public string? Gs04Date { get; set; }        
    public string? Gs05Time { get; set; }          
    public string? ControlNumber { get; set; }     
    public string? TransactionTypeCode { get; set; }
    public string? VersionAndRelease { get; set; }  

    public ICollection<TransactionSet> TransactionSets { get; set; } = new List<TransactionSet>();
}
