namespace EdiConsole.Domain.Parties;

/// <summary>
/// The person who was treated.
/// From loop 2000C/2010CA when the patient is a dependent; otherwise the mapper
/// copies the subscriber, with relationship "18" (self).
/// </summary>
public sealed record Patient
{
    public required string RelationshipToSubscriber { get; init; }  // "18" (self) or PAT01: 01 spouse, 19 child...
    public required PartyName Name { get; init; }                   // NM103–NM105
    public DateOnly? DateOfBirth { get; init; }                     // DMG02
    public string? Gender { get; init; }                            // DMG03: F, M, U
}
