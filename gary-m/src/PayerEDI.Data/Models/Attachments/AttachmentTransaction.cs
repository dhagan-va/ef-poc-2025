using System.Text.Json.Serialization;

namespace PayerEDI.Data.Models.Attachments;

/// <summary>
/// A parsed X12 275 attachment transaction.
/// </summary>
/// <remarks>
/// The transaction is mapped from the installed EdiFabric <c>TS275</c> template
/// for implementation convention <c>005010X215</c>. Subject identity and
/// references are taken from the template's NM1 and REF loops. Attachment
/// metadata and content are taken from the LX/DTP/EFI/BIN hierarchy exposed by
/// that template. Binary content is intentionally not retained by this domain
/// model; <see cref="Attachment.StorageLocation"/> is reserved for the future
/// S3 object location.
/// </remarks>
/// <param name="TransactedAt">Functional-group creation date/time from GS04 and GS05.</param>
/// <param name="TransactionControlNumber">ST02 transaction control number.</param>
/// <param name="Subjects">Subject/member identities mapped from 275 NM1 loops.</param>
/// <param name="Attachments">Attachment metadata mapped from 275 LX/EFI/BIN structures.</param>
public sealed record AttachmentTransaction(
    DateTime TransactedAt,
    string? TransactionControlNumber,
    IList<AttachmentSubject> Subjects,
    IList<Attachment> Attachments
);

/// <summary>
/// A patient or member identified in an X12 275 subject NM1 loop.
/// </summary>
/// <param name="EntityIdentifierCode">NM101 entity identifier code.</param>
/// <param name="PatientMemberIdQualifier">NM108 identification-code qualifier.</param>
/// <param name="PatientMemberId">NM109 identification code used as the patient/member identifier.</param>
/// <param name="LastName">NM103 primary last or organization name.</param>
/// <param name="FirstName">NM104 first name.</param>
public sealed record AttachmentSubject(
    string EntityIdentifierCode,
    string? PatientMemberIdQualifier,
    string PatientMemberId,
    string? LastName,
    string? FirstName
);

/// <summary>
/// Extracted metadata for one X12 275 attachment.
/// </summary>
/// <remarks>
/// The claim reference is retained as searchable metadata and is not the
/// attachment identifier. The storage location is a placeholder for a future
/// S3 object key. No decoded binary content is stored.
/// </remarks>
/// <param name="SequenceNumber">LX01 attachment sequence number.</param>
/// <param name="PatientMemberId">Patient/member identifier associated with this attachment.</param>
/// <param name="PatientMemberIdQualifier">Qualifier associated with the patient/member identifier.</param>
/// <param name="ClaimReferences">REF qualifier/value pairs associated with the attachment subject.</param>
/// <param name="FileName">EFI11 file name when exposed by the template.</param>
/// <param name="ContentType">EFI07 interchange format when exposed by the template.</param>
/// <param name="DeclaredLength">BIN01 declared decoded binary length.</param>
/// <param name="StorageLocation">Future S3 location placeholder; no attachment bytes are stored.</param>
/// <param name="Status">Extraction status for this attachment.</param>
public sealed record Attachment(
    string? SequenceNumber,
    string? PatientMemberId,
    string? PatientMemberIdQualifier,
    IList<AttachmentReference> ClaimReferences,
    string? FileName,
    string? ContentType,
    string? DeclaredLength,
    string? StorageLocation,
    AttachmentStatus Status
);

/// <summary>A reference associated with a 275 subject or attachment.</summary>
/// <param name="Qualifier">REF01 reference identification qualifier.</param>
/// <param name="Value">REF02 reference identification value.</param>
public sealed record AttachmentReference(string Qualifier, string Value);

/// <summary>The result of extracting an attachment from a 275 transaction.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<AttachmentStatus>))]
public enum AttachmentStatus
{
    Extracted,
    Failed,
}

/// <summary>An application-level error found while extracting a 275 attachment.</summary>
/// <param name="SegmentName">Source segment name, such as BIN or EFI.</param>
/// <param name="LoopId">Source loop identifier when available.</param>
/// <param name="Message">Safe validation message that does not contain attachment content.</param>
/// <param name="SpecReference">Source element reference, such as BIN01 or BIN02.</param>
/// <param name="Code">Stable application error code.</param>
public sealed record AttachmentMappingError(
    string SegmentName,
    string? LoopId,
    string Message,
    string? SpecReference,
    string Code
);
