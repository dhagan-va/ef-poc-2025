# ef-poc-2025
EdiFabric familiarity task


EDI 837 Ingestion Task

Objective

Create .NET C# application using EdiFabric to ingest sample 837 EDI file, store to SQL Server Express, with unit tests.

Requirements

Phase 1: Core Implementation (2 weeks)

1. Setup Project

Create .NET 8 console/web application
Add EdiFabric NuGet packages
Configure SQL Server Express connection
Use Entity Framework Core
2. Implementation

Ingest sample 837 EDI file using EdiFabric
Parse EDI segments (ISA, GS, ST, BHT, etc.)
Map to database entities
Store parsed data in SQL Server Express
Include 1 unit test (XUnit/NUnit)
3. Folder Structure

 

4. Deliverables

Working C# application
Database schema/migrations
Sample 837 test file
Unit test coverage
README with setup instructions
Phase 2: S3 Integration

Use moto.py for S3 mock testing
Read EDI files from S3 bucket
Process files asynchronously
Submission

Repository: github.com/dhagan-va/ef-2025-poc
Branch: feature/firstname-lastinitial-837-ingestion
Deadline: 2 weeks from start
Review: Peer review required before merge
Development Options

Local: Visual Studio 2022 + SQL Server Express
Online: GitHub Codespaces, Repl.it, or CodeSandbox
Alternative: Docker containers if hardware limited
Getting Started

Create your feature branch
Start with EdiFabric trial license
Use basic 837 sample file for testing
Implement incrementally with commits
Submit for peer review when complete
Success Criteria

 Application successfully parses 837 EDI file
 Data is correctly stored in SQL Server Express
 At least one unit test passes
 Code is properly documented
 Peer review completed and approved
