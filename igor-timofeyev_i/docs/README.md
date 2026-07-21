# EDI837 Ingestion (PayerEDI)

Simple README for the EDI837 ingestion solution — contains EF Core models, ingestion service using Edifabric, and tests.

## Summary
This project ingests X12 837P files (via Edifabric), maps them to EF Core entities and persists them into the PayerEDI database. It includes:
- AppDbContext and EF models (identity int PKs)
- Edi837IngestionService (file path from IConfiguration)
- Mapping routine MapEdiToEntities for TS837P -> domain entities
- Unit tests using EF Core InMemory for DbContext

## Prerequisites
- .NET SDK (6/7/8 as used by the project)
- SQL Server (local or remote) for runtime DB
- Optional: Visual Studio or VS Code
- NuGet packages:
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Design
  - Microsoft.EntityFrameworkCore.Tools
  - Microsoft.EntityFrameworkCore.InMemory (for tests)
  - Edifabric (edifabric nuget package) for parsing 837 files

## Configuration
Provide connection string and EDI file path in appsettings.json or environment variables.

Example appsettings.json:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=Igor-Surface\\SQLEXPRESS;Database=PayerEDI;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "FilePaths": {
    "Edi837_Path": "C:\\Projects\\VA\\EDI 837\\igor-timofeyev_i\\samples"
  }
```

Environment variable names (if used): `"Default_Connection,
        "Edi837_Path",
        "AWS__UseLocalMoto",
        "AWS__MotoServiceUrl",
        "UseLocalFileDirectory",
        "FileSearchPattern"`.


## Build & Run (local)
- Restore / build:
  dotnet restore
  dotnet build

- Run:
  cd src/EDI837Ingestion
  dotnet run

## Migrations
# apply all pending to get your db schema built out:
Update-Database -Context AppDbContext

## Tests
Unit tests use xUnit and the EF Core InMemory provider.

Install InMemory package into test project:
  dotnet add <test-project> package Microsoft.EntityFrameworkCore.InMemory

Run tests:
  dotnet test

Notes:
- Use an in-memory AppDbContext instance in tests, not a Mock<AppDbContext>.
- Mock Edifabric parsing and other external dependencies.

## DB Cleanup (dev)
DELETE FROM dbo.ClaimServiceLines;
DELETE FROM dbo.ClaimDiagnoses;
DELETE FROM dbo.MedicalClaims;
DELETE FROM dbo.ClaimBatches;
DELETE FROM dbo.SubscriberPatients;
DELETE FROM dbo.BillingProviders;
DELETE FROM dbo.FunctionalGroups;
DELETE FROM dbo.InterchangeControls;

DBCC CHECKIDENT('dbo.ClaimServiceLines', RESEED, 0);
DBCC CHECKIDENT('dbo.ClaimDiagnoses', RESEED, 0);
DBCC CHECKIDENT('dbo.MedicalClaims', RESEED, 0);
DBCC CHECKIDENT('dbo.ClaimBatches', RESEED, 0);
DBCC CHECKIDENT('dbo.SubscriberPatients', RESEED, 0);
DBCC CHECKIDENT('dbo.BillingProviders', RESEED, 0);
DBCC CHECKIDENT('dbo.FunctionalGroups', RESEED, 0);
DBCC CHECKIDENT('dbo.InterchangeControls', RESEED, 0);

## Verify records after ingestion (sample):
SELECT * FROM dbo.ClaimServiceLines ORDER BY 1 DESC;
SELECT * FROM dbo.ClaimDiagnoses ORDER BY 1 DESC;
SELECT * FROM dbo.MedicalClaims ORDER BY 1 DESC;
SELECT * FROM dbo.ClaimBatches ORDER BY 1 DESC;
SELECT * FROM dbo.SubscriberPatients ORDER BY 1 DESC;
SELECT * FROM dbo.BillingProviders ORDER BY 1 DESC;
SELECT * FROM dbo.FunctionalGroups ORDER BY 1 DESC;
SELECT * FROM dbo.InterchangeControls ORDER BY 1 DESC;

## Development notes / best practices
- Register services in DI: `services.AddDbContext<AppDbContext>(...)` and `services.AddScoped<IEdi837IngestionService, Edi837IngestionService>()`.
- Inject `IConfiguration` or use Options pattern for file paths.
- Persist raw EDI bytes and parsed JSON for replay.
- Implement idempotency using interchange/group/transaction control numbers.
- Keep database changes via migrations; test migrations in a dev DB first.

# AWS S3 & Local Mocking Architecture
The system supports dual-mode ingestion strategies via Amazon S3 file streams. When running locally, it leverages Moto to emulate a production AWS infrastructure completely in-memory without connecting to live cloud datacenters.

## Local Mocking Setup (Zero-Python Moto Server)
Spin up the detached local mock engine container matching the application ports via PowerShell:
docker run --name moto-local -d -p 5000:5000 motoserver/moto

Useful Container Cleanup Commands:
# Stop and delete the container (completely wipes mock data caches)
docker stop moto-local; docker rm moto-local

## Ingestion Flow & Cloud Logic
- Automated Seeding: On startup, if useLocalMoto is evaluated, Program.cs intercepts the connection, creates the target edi-claims-storage bucket, and pushes mock EDI transaction files into Moto.
- Paginated Retrieval Loop: The service makes an initial metadata list call using ListObjectsV2Async. It utilizes a standardized do-while loop monitoring the IsTruncated token flag to gracefully handle pagination thresholds above 1,000 files.
- Sequential Processing: Files matching the prefix pattern are downloaded using isolated stream buffers, passed into EdiFabric loops one by one, and tracking pointers are stored.


## Contributing
- Create a branch for your feature/fix.
- Add tests for behavior changes.
- Run `dotnet ef migrations add <Name>` only when model changes require schema updates.
- Open PR for review.

## Contact / Issues
- Open an issue in the repo with reproducible steps and any error messages.

End.