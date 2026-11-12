namespace EdiConsole.Domain.Parties;

/// <summary>
/// The provider who is owed the money: loop 2010AA.
/// A snapshot of what the claim said, not a master-data record.
/// </summary>
public sealed record BillingProvider
{
    public required string Npi { get; init; }       // NM109 (NM108 = XX)
    public required PartyName Name { get; init; }   // NM103–NM105
    public required string TaxId { get; init; }     // REF02 (REF01 = EI or SY)
    public required Address Address { get; init; }  // N3/N4
}
