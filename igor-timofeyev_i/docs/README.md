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
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=<your SQLEXPRESS instance>;Database=PayerEDI;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "FilePaths": {
	"Edi837PathWithFilename": "<your edi 837 filepath>"
  }
}
```

Environment variable names (if used): `ConnectionStrings__DefaultConnection` and `FilePaths__Edi837PathWithFilename`.


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

## Contributing
- Create a branch for your feature/fix.
- Add tests for behavior changes.
- Run `dotnet ef migrations add <Name>` only when model changes require schema updates.
- Open PR for review.

## Contact / Issues
- Open an issue in the repo with reproducible steps and any error messages.

End.