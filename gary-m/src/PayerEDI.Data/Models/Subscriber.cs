namespace PayerEDI.Data.Models;

/// <summary>
/// Primary person on the insurance policy and their dependents
/// </summary>
/// <param name="Primary">2000A Loop: 2000B Loop: 2010BA Loop -> NM1 - Primary Insured.</param>
/// <param name="Dependents">2000A Loop: 2000B Loop: 2000C Loop -> PAT - Dependents of Primary Insured</param>
public record Subscriber(
    IndividualOrOrganization Primary,
    IList<IndividualOrOrganization> Dependents
)
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
}
