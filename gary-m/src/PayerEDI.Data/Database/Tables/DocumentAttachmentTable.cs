namespace PayerEDI.Data.Database.Tables;

/// <summary>Persisted metadata for an attachment extracted from an X12 275 document.</summary>
/// <param name="DocumentId">Parent documents row containing the original 275 XML.</param>
/// <param name="PatientMemberId">Patient/member identifier from the subject NM109 when available.</param>
/// <param name="PatientMemberIdQualifier">Qualifier from the subject NM108.</param>
/// <param name="ClaimReference">Claim/reference value from REF02.</param>
/// <param name="ClaimReferenceQualifier">Claim/reference qualifier from REF01.</param>
/// <param name="SequenceNumber">Attachment sequence from LX01.</param>
/// <param name="FileName">Attachment file name from EFI11 when available.</param>
/// <param name="ContentType">Attachment content/interchange format from EFI07 when available.</param>
/// <param name="DeclaredLength">Decoded binary length declared in BIN01.</param>
/// <param name="StorageLocation">Future S3 location placeholder; binary content is not persisted.</param>
/// <param name="Status">Extraction status.</param>
public sealed record DocumentAttachmentTable(
    Guid DocumentId,
    string? PatientMemberId,
    string? PatientMemberIdQualifier,
    string? ClaimReference,
    string? ClaimReferenceQualifier,
    string? SequenceNumber,
    string? FileName,
    string? ContentType,
    string? DeclaredLength,
    string? StorageLocation,
    string Status
)
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
}
