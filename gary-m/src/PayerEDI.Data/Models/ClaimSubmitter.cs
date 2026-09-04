namespace PayerEDI.Data.Models;

/// <summary>
/// Submitter information for an X12 837P professional or 837D dental claim.
/// </summary>
/// <remarks>
/// The submitter is mapped from Loop 1000A of the transaction. The identity is
/// read from the loop's NM1 Submitter Name segment, and the administrative
/// contact list is read from the loop's PER Submitter EDI Contact Information
/// segments. The <see cref="ExternalIdentifier"/> is derived from the NM1
/// identification qualifier and identification code rather than from a
/// separate segment.
/// </remarks>
/// <param name="Submitter">Submitter identity from Loop 1000A's NM1 Submitter Name segment.</param>
/// <param name="AdministrativeCommunicationsContact">Administrative contacts from the Loop 1000A PER Submitter EDI Contact Information segments.</param>
/// <param name="ExternalIdentifier">Submitter identifier composed of NM108 (identification code qualifier) and NM109 (identification code) in Loop 1000A.</param>
public record ClaimSubmitter(
    IndividualOrOrganization Submitter,
    IList<CommunicationsContact> AdministrativeCommunicationsContact,
    ExternalIdentifier ExternalIdentifier
)
{
    /// <summary>
    /// Internal identifier generated for this domain record; it is not sourced from the X12 transaction.
    /// </summary>
    public Guid Id { get; init; } = Guid.CreateVersion7();
}
