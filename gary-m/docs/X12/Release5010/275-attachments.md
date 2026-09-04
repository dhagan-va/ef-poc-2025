# X12 275 Attachments

The processor recognizes the `005010X215` 275 implementation convention as
`EdiFabric.Templates.X12004010.TS275`.

## Persistence

The original parsed 275 XML is stored in `documents`. Extracted attachment
metadata is stored in `document_attachment`, linked to the document. Binary
content is decoded only long enough to validate Base64 and `BIN01`; it is not
stored. `StorageLocation` is reserved for the future S3 object location and is
currently a placeholder for successfully validated attachments.

Attachments are associated with the patient/member identifier from the subject
NM1 (`NM108` qualifier and `NM109` value). Claim references from the subject
REF loops (`REF01` and `REF02`) are retained as metadata and are not attachment
identifiers.

## Failures

Malformed or incomplete attachment content produces a failed attachment row.
The original document is still persisted, and the extraction error is stored in
`edi_error` with child details in `edi_segment_error`. Attachment content is
never included in error messages or structured logs.

The installed EdiFabric 275 type currently exposes attachment data through its
`LX`/`DTP`/`EFI`/`BIN` hierarchy. It does not expose the `BHT` or `PWK` segments
present in some 275 payloads; those structures must be explicitly handled when
the selected EdiFabric template supports them.
