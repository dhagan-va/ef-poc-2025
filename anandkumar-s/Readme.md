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

## Application Flow
EDI File (837)
      ↓
Program.cs (startup + DI)
      ↓
ProcessIngestionAsync
      ↓
EDIFabric parsing
      ↓
Map segments → Domain Entities
      ↓
Repository (InsertInterchangeAsync)
      ↓
EF Core
      ↓
SQL Server tables populated

Improvements:
The following improvements can be made to enhance the functionality and robustness of the application (production grade):

Observability: Implement logging to track processing steps, errors, and performance metrics. Use AWS tools such as AWS CloudWatch logs, CloudWatch Metrics for centralized logging and monitoring.
Idempotency: Ensure that the ingestion process can handle duplicate files gracefully without creating duplicate records in the database. This can be achieved by implementing a unique constraint on the Interchange Control Number (ISA segment) and possibly other fields and handling exceptions accordingly.
Tracing: Implement distributed tracing to track the flow of data through the application, especially if it interacts with other services or components. This can help identify bottlenecks and troubleshoot issues effectively.
Error Handling: Implement robust error handling to catch and log exceptions that may occur during file parsing, database operations, or other processing steps. Consider retry mechanisms for transient errors.
Circuit Breaker: Implement a circuit breaker pattern to prevent cascading failures in case of downstream service issues (e.g.,third party APIs, remote services,possibly SFTP etc.) or calling third party APIs.

References:
- X12 837 Implementation Guide: https://x12.org/examples/005010x222
- Stedi EDI Reference: https://www.stedi.com/edi/x12/transaction-set/837
- EDI Academy: https://ediacademy.com/blog/x12-edi-segments/

