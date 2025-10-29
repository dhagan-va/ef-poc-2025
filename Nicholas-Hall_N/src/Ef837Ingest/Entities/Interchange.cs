using System.Collections.Generic;

namespace Edi837Ingestion.Domain.Entities;

public class Interchange
{
    public int Id { get; set; }

    public string? Isa01 { get; set; }
    public string? Isa02 { get; set; }
    public string? Isa03 { get; set; }
    public string? Isa04 { get; set; }
    public string? Isa05 { get; set; }
    public string? Isa06 { get; set; }
    public string? Isa07 { get; set; }
    public string? Isa08 { get; set; }
    public string? Isa09Date { get; set; }
    public string? Isa10Time { get; set; }
    public string? ControlNumber { get; set; }

    public ICollection<FunctionalGroup> FunctionalGroups { get; set; } = new List<FunctionalGroup>();
}
