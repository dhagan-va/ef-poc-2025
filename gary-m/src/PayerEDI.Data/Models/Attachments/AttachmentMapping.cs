namespace PayerEDI.Data.Models.Attachments;

/// <summary>A 275 domain mapping and any non-parser extraction errors.</summary>
/// <param name="Transaction">Mapped transaction data.</param>
/// <param name="Errors">Errors that must be persisted with the original document.</param>
public sealed record AttachmentMapping(
    AttachmentTransaction Transaction,
    IReadOnlyCollection<AttachmentMappingError> Errors
);
