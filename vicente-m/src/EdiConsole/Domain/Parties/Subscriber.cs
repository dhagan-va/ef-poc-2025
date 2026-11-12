namespace EdiConsole.Domain.Parties;

/// <summary>
/// The person who holds the insurance policy: loop 2000B (SBR) + 2010BA.
/// </summary>
public sealed record Subscriber
{
    public required string MemberId { get; init; }             // 2010BA NM109 (NM108 = MI)
    public required PartyName Name { get; init; }              // 2010BA NM103–NM105
    public DateOnly? DateOfBirth { get; init; }                // DMG02
    public string? Gender { get; init; }                       // DMG03: F, M, U
    public required string PayerResponsibility { get; init; }  // SBR01: P primary, S secondary, T tertiary...
    public string? GroupNumber { get; init; }                  // SBR03
    public string? ClaimFilingIndicator { get; init; }         // SBR09: e.g. 12 PPO, CI commercial, MB Medicare B
    public required Address Address { get; init; }             // N3/N4 (fields null when not sent)
}
