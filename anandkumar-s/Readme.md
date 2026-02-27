# X12 EDI 837 Ingestion – SQL Express (Console Application)

## Overview

This project demonstrates ingestion of an X12 837 healthcare claim file, parsing key segments, and persisting the hierarchical structure into SQL Server Express using Entity Framework Core.

The solution follows clean architecture principles with separation between:

- Domain (Entities + Interfaces)
- Infrastructure (DbContext + Repository)
- Service Layer (Orchestration)
- Parser Layer (Pluggable: Fake parser / EdiFabric parser)

---

## Architecture

X12 Envelope Hierarchy:

ISA (Interchange)
 └── GS (Functional Group)
      └── ST (Transaction Set)
           ├── NM1 (Parties)
           ├── CLM (Claims)
           │    └── SV1 (Service Lines)

Database Tables:

1. InterchangeHeaders (ISA)
2. FunctionalGroupHeaders (GS)
3. TransactionSetHeaders (ST + BHT)
4. Parties (NM1)
5. Claims (CLM)
6. ServiceLines (SV1)

Each table uses:

- BIGINT IDENTITY(1,1) as primary key
- Foreign key relationships to preserve hierarchy
- Explicit configuration via EF Core Fluent API

---

## Technologies Used

- .NET 8.0
- EF Core 8.x
- SQL Server Express
- Microsoft.Extensions.Hosting
- Dependency Injection
- Repository Pattern
- Transaction Handling
- (Optional) EdiFabric Parser

---

## Project Structure

