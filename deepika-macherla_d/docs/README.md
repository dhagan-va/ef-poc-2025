# EDI 837 Ingestion — Implementation Summary

This document summarizes the work completed so far (project skeleton, EF Core schema, sample file and commands). EdiFabric integration is planned once a trial license is provided.

Project layout (relevant paths)
- src/Deepika.EDIIngestion/ — main console app project
- src/Deepika.EDIIngestion/Models/ — entity classes (EdiInterchange, Provider, Claim)
- src/Deepika.EDIIngestion/Data/ — AppDbContext, DesignTimeDbContextFactory
- src/Deepika.EDIIngestion/Migrations/ — InitialCreate migration and model snapshot
- src/Deepika.EDIIngestion/Properties/launchSettings.json — local debug env placeholders
- samples/sample_837.edi — sample EDI file (contains multiple CLM segments)
- tests/Deepika.EDIIngestion.Tests/ — xUnit tests (parsing unit test)

What was added
- EF Core packages and Microsoft.EntityFrameworkCore.Design reference (for migrations)
- Entity models: EdiInterchange, Provider, Claim
- AppDbContext with DbSet for Interchanges, Providers, Claims and basic relationships
- DesignTimeDbContextFactory to support `dotnet ef` tooling
- Initial EF migration files (InitialCreate) and snapshot under Migrations/
- launchSettings.json (local environment variable placeholders)
- sample EDI file with multiple CLM segments at samples/sample_837.edi

Database schema (high level)
- Providers: Id, ProviderType, Name, Identifier
- Interchanges: Id, IsaControlNumber, SenderId, ReceiverId, GsControlNumber, TransactionSetId, TransactionSetControlNumber, BhtReference, RawIsa/RawGs/RawSt/RawBht, CreatedAt, ProviderId (FK)
- Claims: Id, ClaimNumber, TotalChargeAmount (decimal), ServiceDate, InterchangeId (FK), ProviderId (FK)

Table descriptions (one-line)
- Providers — stores provider identification (type, name, identifier such as NPI) used by claims and interchanges.
- Interchanges — stores envelope/transaction metadata (ISA/GS/ST/BHT fields plus raw segments) for each processed EDI interchange.
- Claims — stores claim-level records (claim number, total charge, service date) with foreign keys to the Interchange and Provider.

Key files to review
- src/Deepika.EDIIngestion/Program.cs — DI setup, DbContext registration, app entry
- src/Deepika.EDIIngestion/Data/AppDbContext.cs — DbContext and OnModelCreating
- src/Deepika.EDIIngestion/Migrations/InitialCreate.cs — migration creating the three tables

Commands
- Build solution:
  dotnet build
- Apply existing migration to local DB (creates schema):
  cd deepika-macherla_d
  dotnet ef database update --project src/Deepika.EDIIngestion --startup-project src/Deepika.EDIIngestion
- Run the console app (parses sample and persists rows):
  dotnet run --project src/Deepika.EDIIngestion

Command explanations
- cd deepika-macherla_d
  This changes your shell’s current directory to the project solution folder (deepika-macherla_d) so subsequent dotnet commands run with the correct relative paths and the EF tools / design-time factory can find the project files, appsettings.json and the Migrations folder.

- dotnet ef database update --project src/Deepika.EDIIngestion --startup-project src/Deepika.EDIIngestion
  This invokes the Entity Framework Core command-line tool to apply any pending migrations to the database: the --project option points to the assembly that contains the Migrations C# files (the schema changes) and the --startup-project option tells EF which project to start in so it can read configuration and create the DbContext; the command runs each migration’s Up() code against the database using the connection string from appsettings or environment variables and creates/updates the tables accordingly.

- dotnet run --project src/Deepika.EDIIngestion
  This builds and runs the console application in the specified project: Program.Main executes, the app configures DI and the AppDbContext, reads the connection string and environment settings, parses the sample EDI file (using the current parser), and saves the parsed entities to the database; run the database update first so the schema from migrations exists before the app inserts rows.
- Run tests:
  dotnet test tests/Deepika.EDIIngestion.Tests

Simple code example (saving an interchange)

using (var scope = serviceProvider.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	var interchange = new EdiInterchange { IsaControlNumber = "0001", SenderId = "SENDER" };
	db.Interchanges.Add(interchange);
	db.SaveChanges();
}

EdiFabric license
- To provide the EdiFabric trial license to the app, set the environment variable the code reads (TRIAL_EDIFABRIC_LICENSE or EDIFABRIC_LICENSE).
  - PowerShell (temporary for current session):
	$env:TRIAL_EDIFABRIC_LICENSE = "<your-license-key>"
  - PowerShell (persist for current user):
	setx TRIAL_EDIFABRIC_LICENSE "<your-license-key>"

