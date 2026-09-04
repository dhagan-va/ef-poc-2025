namespace PayerEDI.Data.Models.Claims;

/// <summary>
/// Common domain representation for an X12 005010 837 health-care claim transaction.
/// </summary>
/// <remarks>
/// The transaction-specific records preserve the supported 837P and 837D claim
/// families while sharing the envelope and loop data represented by this record.
/// See the <see href="https://www.stedi.com/edi/x12-005010/837">X12 837
/// transaction reference</see>.
/// </remarks>
/// <param name="TransactedAt">
/// Functional-group date and time from GS04 and GS05. The value identifies when
/// the functional group was created; it is not necessarily the patient's date of
/// service or the time the claim was adjudicated. The claim factories combine
/// those elements into a <see cref="DateTime"/>.
/// </param>
/// <param name="Submitter">
/// Submitter from Loop 1000A, including the NM1 Submitter Name and PER Submitter
/// EDI Contact Information data.
/// </param>
/// <param name="Receiver">
/// Receiver from Loop 1000B's NM1 Receiver Name segment. In an 837 this is the
/// intended destination or receiver of the transaction, commonly a payer or
/// clearinghouse.
/// </param>
public abstract record HealthCareClaim(
    DateTime TransactedAt,
    ClaimSubmitter Submitter,
    IndividualOrOrganization Receiver
);

/// <summary>
/// Domain representation of an X12 005010 837P professional health-care claim.
/// </summary>
/// <remarks>
/// Professional claim content is parsed from the EdiFabric <c>TS837P</c>
/// transaction. This record aggregates the supported mapped loops; it does not
/// expose every segment or element in the 837P implementation guide.
/// </remarks>
/// <param name="TransactedAt">
/// Functional-group creation date and time from GS04 and GS05; see
/// <see cref="HealthCareClaim.TransactedAt"/>.
/// </param>
/// <param name="Submitter">Mapped submitter from Loop 1000A.</param>
/// <param name="Receiver">Mapped receiver from Loop 1000B NM1.</param>
/// <param name="Subscribers">
/// Subscribers mapped from the subscriber and dependent hierarchy beginning at
/// Loop 2000A, including the corresponding subscriber identity in Loop 2010BA
/// and dependents in Loop 2000C/2010CA when present.
/// </param>
/// <param name="HealthcareProviders">
/// Providers mapped from supported claim-level provider loops, such as the 2310
/// referring, rendering, or supervising provider NM1 loops. The concrete domain
/// type identifies the provider role.
/// </param>
/// <param name="Procedures">
/// Professional service lines mapped from the 2400 loop's SV1 Professional
/// Service segment and its related DTP and related-detail segments when exposed
/// by the EdiFabric template.
/// </param>
public sealed record ProfessionalCareClaim(
    DateTime TransactedAt,
    ClaimSubmitter Submitter,
    IndividualOrOrganization Receiver,
    IList<Subscriber> Subscribers,
    IList<HealthcareProvider> HealthcareProviders,
    IList<Procedure> Procedures
) : HealthCareClaim(TransactedAt, Submitter, Receiver);

/// <summary>
/// Domain representation of an X12 005010 837D dental health-care claim.
/// </summary>
/// <remarks>
/// Dental claim content is parsed from the EdiFabric <c>TS837D</c> transaction.
/// The shared property names do not imply identical source loops: dental
/// service-line data uses the 2400 loop's SV3 Dental Service segment rather than
/// the professional claim's SV1 segment.
/// </remarks>
/// <param name="TransactedAt">
/// Functional-group creation date and time from GS04 and GS05; see
/// <see cref="HealthCareClaim.TransactedAt"/>.
/// </param>
/// <param name="Submitter">Mapped submitter from Loop 1000A.</param>
/// <param name="Receiver">Mapped receiver from Loop 1000B NM1.</param>
/// <param name="Subscribers">
/// Subscribers mapped from the subscriber and dependent hierarchy beginning at
/// Loop 2000A, including the corresponding subscriber identity in Loop 2010BA
/// and dependents in Loop 2000C/2010CA when present.
/// </param>
/// <param name="HealthcareProviders">
/// Providers mapped from supported dental claim provider loops, including the
/// rendering provider NM1 loop where present in the EdiFabric template.
/// </param>
/// <param name="Procedures">
/// Dental service lines mapped from the 2400 loop's SV3 Dental Service segment
/// and its related DTP and related-detail segments when exposed by the EdiFabric
/// template.
/// </param>
public sealed record DentalCareClaim(
    DateTime TransactedAt,
    ClaimSubmitter Submitter,
    IndividualOrOrganization Receiver,
    IList<Subscriber> Subscribers,
    IList<HealthcareProvider> HealthcareProviders,
    IList<Procedure> Procedures
) : HealthCareClaim(TransactedAt, Submitter, Receiver);
