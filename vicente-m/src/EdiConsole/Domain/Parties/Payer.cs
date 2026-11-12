namespace EdiConsole.Domain.Parties;

/// <summary>
/// The health plan that should pay this claim: loop 2010BB.
/// </summary>
public sealed record Payer
{
    public required string PayerId { get; init; }  // NM109 (NM108 = PI)
    public required string Name { get; init; }     // NM103
}
