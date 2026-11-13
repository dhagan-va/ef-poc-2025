namespace EdiConsole.Domain.Parties;

/// <summary>
/// The person who was treated.
/// From loop 2000C/2010CA when the patient is a dependent; otherwise the mapper
/// copies the subscriber, with relationship "18" (self).
/// </summary>
public sealed record Patient
{
    /// <summary>The relationship code for "self" (SBR02 = 18; PAT01 never uses it): the patient is the subscriber.</summary>
    public const string SelfRelationshipCode = "18";

    public required string RelationshipToSubscriber { get; init; }  // "18" (self) or PAT01: 01 spouse, 19 child...
    public required PartyName Name { get; init; }                   // NM103–NM105
    public DateOnly? DateOfBirth { get; init; }                     // DMG02
    public string? Gender { get; init; }                            // DMG03: F, M, U

    /// <summary>
    /// True when the patient is the subscriber.
    /// Decided by the relationship code only, never by comparing the patient with the subscriber:
    /// a Jr. and a Sr. can share a name and an address and still be two different people.
    /// </summary>
    /// <remarks>
    /// Computed, so EF Core doesn't store it and can't translate it into SQL.
    /// In a database query, compare <see cref="RelationshipToSubscriber"/> with <see cref="SelfRelationshipCode"/>.
    /// </remarks>
    public bool IsSubscriber => RelationshipToSubscriber == SelfRelationshipCode;
}
