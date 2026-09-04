# Phase 1 Demo

Run these commands from the repository root, one at a time.

## 1. Start the development stack

Start the local SQL Server and MotoServer containers, wait for SQL Server to become healthy, and initialize the database.

```shell
./.helpers/dev-start
```

## 2. Apply database migrations

Apply all pending Entity Framework Core migrations to the database.

```shell
dotnet ef database update \
  --project src/PayerEDI.Data \
  --startup-project src/PayerEDI.Processor.Console
```

## 3. Configure the console application

Set the EdiFabric license key before processing an EDI file; replace the placeholder with the key supplied for the demo.

```shell
export EDI_PROCESSOR_KEY__EDIFABRIC="<your-edifabric-key>"
```

When using `--save`, set the application connection string; replace the placeholder with the local PayerEdi application connection string.

```shell
export EDI_PROCESSOR_CONNECTIONSTRINGS__DEFAULT="Server=localhost,1433;Database=PayerEdi;User Id=payeredi_app;Password=payeredi_app_password;TrustServerCertificate=True"
export AWS_ENDPOINT_URL="http://localhost:3000"
```

## 4. Process a professional claim without saving

Parse the sample 837P file and log the mapped transaction without writing claim data to the database.

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \
  samples/EDI/837p-sample.edi
```

## 5. Process a professional claim and save it

Parse the sample 837P file and persist the original document and mapped data by adding `--save`.

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \
  samples/EDI/837p-sample.edi \
  --save
```

## 6. Process a dental claim and save it

Parse the sample 837D dental claim and persist the result.

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \
  samples/EDI/837d-sample-3.edi \
  --save
```

## 7. Process the full-field professional claim and save it

Use the broader 837P sample to show additional mapped service-line and claim fields.

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \ 
  samples/EDI/837P-all-fields.edi \
  --save
```

## 8. Process an unexpected-segment sample

Parse the sample containing an unexpected segment to demonstrate parser/error handling without saving it.

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \
  samples/EDI/837p-unexpected-segment.edi
```

## 9. Process a 275 attachment transaction

Parse the sample X12 275 transaction and log its extracted attachment metadata; add `--save` if the document and attachment metadata should be persisted.

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \
  samples/EDI/sample_275_pdf_attachment.edi
```

```shell
dotnet run \
  --project src/PayerEDI.Processor.Console -- \
  samples/EDI/sample_275_pdf_attachment.edi \
  --save
```

The positional argument is the EDI file path, and `--save` is the only application option; without `--save`, the application parses and logs transactions but does not configure or use the EF Core persistence services.

## Domain models

- `HealthCareClaim` is the shared base for 837 claims and contains the transaction time, submitter, and receiver.
- `ProfessionalCareClaim` represents an 837P professional claim with subscribers, providers, and professional procedures.
- `DentalCareClaim` represents an 837D dental claim with subscribers, providers, and dental procedures.
- `AttachmentTransaction` contains the transaction time, control number, subjects, and extracted 275 attachment metadata.
- `AttachmentSubject` identifies a 275 patient or member.
- `Attachment` describes one extracted attachment without retaining its binary content.
- `AttachmentReference` stores a qualifier and reference value associated with an attachment.
- `AttachmentStatus` identifies whether attachment extraction succeeded or failed.
- `AttachmentMappingError` records a safe application-level error found while extracting attachment metadata.
- `AttachmentMapping` combines a mapped attachment transaction with its extraction errors.
- `ProcessedEdiTransaction` is the base result containing the parsed EdiFabric message.
- `ProcessedProfessionalClaim` pairs a parsed TS837P message with its mapped professional claim.
- `ProcessedDentalClaim` pairs a parsed TS837D message with its mapped dental claim.
- `ProcessedAttachmentTransaction` pairs a parsed TS275 message with its mapped attachment data.
- `Procedure` represents an 837 professional or dental service line and its parsed service details.
- `ClaimSubmitter` contains the submitter identity, administrative contacts, and external identifier.
- `HealthcareProvider` is the base for role-specific provider identities.
- `ReferringProvider` represents a provider who referred the patient or claim.
- `RenderingProvider` represents the provider who rendered the billed service.
- `SupervisingProvider` represents the provider who supervised the reported care.
- `CommunicationNumber` stores a contact number and its X12 qualifier.
- `CommunicationsContact` stores a submitter contact and up to three communication numbers.
- `CommunicationNumberQualifier` enumerates X12 communication-number qualifier codes.
- `EntityRelationshipCode` enumerates X12 NM1 relationship codes.
- `ExternalIdentifier` stores an externally assigned identifier and its qualifier.
- `IndividualOrOrganization` is the shared base for NM1 person and organization identities.
- `Person` represents an NM1 person identity and name fields.
- `NonPerson` represents an NM1 organization or other non-person identity.
- `Subscriber` contains the primary insured identity and dependent identities.

The model factory classes under `Models/Factory` and `Models/Claims/Factory` map EdiFabric transaction templates and segments into these domain records.

## Database mapping and migrations

The domain models are converted into persistence records rather than being persisted directly: `DocumentTable` stores the original EDI XML and transaction metadata, `PatientTable` stores NM1 identities, `EdiErrorTable` and `EdiSegmentErrorTable` store parser errors, and `DocumentAttachmentTable` stores 275 attachment metadata.

`PayerEdiDbContext` maps those table records to SQL Server tables in `OnModelCreating`, including keys, column types, required fields, lengths, indexes, relationships, cascade deletes, and JSON conversion for error-code arrays.

Each migration under `src/PayerEDI.Data/Database/Migrations` is a versioned schema change with `Up` and `Down` operations; `./.helpers/dev-migrate` runs `dotnet ef database update` using the migration connection string and applies any migrations not yet recorded in the database.

## Notes
I tried my hand at having AI generate teh 275 Attachment mining.  What it came up with I do not like it doesn't match
the spirit of the Claims section, so I need to figure out a better prompt. It did mine the data and save it in the DB 
though so I guess mission accomplished.
