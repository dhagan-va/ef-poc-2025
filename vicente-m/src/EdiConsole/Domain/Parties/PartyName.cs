namespace EdiConsole.Domain.Parties;

/// <summary>
/// A person's or organization's name, as sent in an NM1 segment.
/// Organizations (NM102 = 2) use only <see cref="LastOrOrganizationName"/>.
/// </summary>
public sealed record PartyName
{
    public required string LastOrOrganizationName { get; init; }  // NM103
    public string? FirstName { get; init; }                       // NM104
    public string? MiddleName { get; init; }                      // NM105
}
