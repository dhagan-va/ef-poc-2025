# X12EDI.Data

Entity Framework Core data layer for EDI ingestion, envelope capture, and transaction deduplication.

## 📦 Projects

- `X12EDI.Data`: EF Core implementation and migrations
- `X12EDI.Abstractions`: Domain models and ingestion contracts

## 🧩 Entities

- `EdiFile`: Represents an ingested EDI file
- `EdiEnvelope`: Captures ISA/GS metadata
- `EdiTransaction`: Stores raw XML and SHA256 checksum
- `EdiError`: Logs parsing or ingestion errors

## 🧠 Features

- EF Core-backed persistence with async-native ingestion
- Unique constraint on `Checksum` for transaction deduplication
- Envelope merge logic from ISA and GS segments
- File-level idempotency via `Identifier`
- Error logging for malformed segments

## 🚀 Usage

```csharp
await ingestionService.SaveFileAsync(identifier, items, cancellationToken);
