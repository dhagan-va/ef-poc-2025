namespace PayerEDI.Data.Models;

/// <summary>
/// A communication number reported for an X12 contact.
/// </summary>
/// <remarks>
/// In an 837P or 837D transaction, the number and its qualifier are one of the
/// communication pairs in the Loop 1000A PER Submitter EDI Contact Information
/// segment. The qualifier is X12 data element 365 and identifies how the value
/// in the corresponding communication-number element is to be used, for
/// example, <c>TE</c> for telephone or <c>EX</c> for telephone extension.
/// The number is retained as sent; no telephone formatting or normalization is
/// performed by this model.
/// </remarks>
/// <param name="Number">
/// The communication value from PER04, PER06, or PER08, depending on which
/// communication pair produced this record.
/// </param>
/// <param name="Qualifier">
/// The parsed X12 element 365 value from the corresponding PER03, PER05, or
/// PER07 qualifier element.
/// </param>
public record CommunicationNumber(string Number, CommunicationNumberQualifier Qualifier);

/// <summary>
/// An administrative contact for the submitter of an X12 837P professional or
/// 837D dental claim.
/// </summary>
/// <remarks>
/// This record represents a Loop 1000A PER Submitter EDI Contact Information
/// segment. The same mapping applies to both supported transaction types:
/// <list type="bullet">
/// <item><description><c>PER01</c> is the contact function code.</description></item>
/// <item><description><c>PER02</c> is the contact name.</description></item>
/// <item><description><c>PER03</c>/<c>PER04</c> is the primary qualifier and number pair.</description></item>
/// <item><description><c>PER05</c>/<c>PER06</c> is the secondary qualifier and number pair.</description></item>
/// <item><description><c>PER07</c>/<c>PER08</c> is the tertiary qualifier and number pair.</description></item>
/// </list>
/// A transaction may contain more than one PER segment in Loop 1000A; each
/// segment becomes one <see cref="CommunicationsContact"/> in the submitter's
/// administrative contact list. The PER segment's contact function code is
/// required by the 837 implementation, while the name and communication pairs
/// may be absent. A communication pair is represented only when both its
/// number and qualifier are present and the qualifier is recognized by this
/// domain's X12 element 365 code set.
/// </remarks>
/// <param name="ContactFunctionCode">
/// The X12 contact function code from PER01 (data element 366). It describes
/// the purpose of the contact, such as <c>IC</c> for an information contact;
/// the code is retained rather than translated into a domain-specific label.
/// </param>
/// <param name="Name">
/// The contact name from PER02. This is null when PER02 is empty.
/// </param>
/// <param name="PrimaryNumber">
/// The first communication pair in the PER segment: PER04 is the number and
/// PER03 is its qualifier. This is null when either element is absent or the
/// qualifier cannot be parsed.
/// </param>
/// <param name="SecondaryNumber">
/// The second communication pair in the PER segment: PER06 is the number and
/// PER05 is its qualifier. This is null when either element is absent or the
/// qualifier cannot be parsed.
/// </param>
/// <param name="TertiaryNumber">
/// The third communication pair in the PER segment: PER08 is the number and
/// PER07 is its qualifier. This is null when either element is absent or the
/// qualifier cannot be parsed.
/// </param>
public record CommunicationsContact(
    string ContactFunctionCode,
    string? Name,
    CommunicationNumber? PrimaryNumber,
    CommunicationNumber? SecondaryNumber,
    CommunicationNumber? TertiaryNumber
);
