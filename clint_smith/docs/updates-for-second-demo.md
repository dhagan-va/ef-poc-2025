# Updates

## Summary
1. Updated project to use a DI in a more formalized manner (using a Generic Host)
2. Moved the S3 Polling to IngestionWorker and made IW DI compatible
3. Moved from Console.WriteLine to use formal Logger
4. Stricter SNIP Validation Settings and added a couple of tests to prove it is working.
5. S3Gateway refactor to accept S3 client via DI. Unit tests for Gateway


## Details

## Dependency Injection & Hosting (commit `f5b9154`)
- Replaced the ad-hoc `Program.Main` bootstrap with the .NET Generic Host so configuration, logging, and DI are centralized.
- Promoted `TransactionSaver`, `EdiParser`, and the EF Core DbContexts to DI-managed services; removed manual `new` calls.
- Added `IngestionWorker` as a `BackgroundService` to own the S3 polling loop, and introduced `S3GatewayFactory` so gateways can be created via DI.
- Tightened logging/validation: everything now uses `ILogger<T>` plus stricter `ValidationSettings`, and EF connection strings come from `appsettings`.

## Test Infrastructure (commit `791cf9c`)
- Updated the test project to reference `Microsoft.Extensions.Logging.Abstractions` so unit tests can build injectable services (e.g., parser with `NullLogger`).
- Refactored `EdiParserTests` to instantiate the parser via DI-friendly constructors, keeping unit coverage after the host refactor.

## Parser API Cleanup (commit `9758020`)
- Removed the now-redundant `EdiParser.RunAsync` helper; all orchestration lives in the worker/Program classes and tests call the instance APIs directly.

## S3 Gateway Enhancements (commit `352dc8e`)
- Refactored `S3Gateway` to accept an `IAmazonS3` client + logger via DI, enabling mocking and cleaner ownership of AWS resources.
- Added comprehensive unit tests for every gateway method (list, get content/stream, upload, delete) plus mocked sample files for error scenarios.
