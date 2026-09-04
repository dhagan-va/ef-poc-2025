# PayerEDI Processor Console REPL: EF Core Issues

This document records the EF Core considerations for converting
`PayerEDI.Processor.Console` into a long-running REPL that loads EDI files,
saves claims, and queries the database.

## Summary

The current dependency-injection design is suitable for a REPL as long as
each command uses a short-lived dependency-injection scope. The REPL should
not keep one `PayerEdiDbContext` alive for the entire process lifetime.

The primary risks are:

- EF Core change tracking retaining entities in a long-lived context.
- Unbounded query results being materialized into memory.
- Entire EDI files and all parsed transactions being loaded at once.
- Parsed claims, XML strings, or query results being retained by REPL history.
- Document and patient writes being separate operations and therefore not
  atomic as a unit.

## Current Lifetime Design

`PayerEdiDbContext` is registered with `AddDbContext`, which gives it a scoped
lifetime. `DocumentTableRepository`, `PatientRepository`, and
`PersistenceService` are also scoped. This is the correct lifetime model for
short-lived units of work.

The current console processing path creates a scope for a loaded file. A REPL
should apply the same pattern to every command, or to a deliberately bounded
batch of commands:

```csharp
await using var scope = serviceProvider.CreateAsyncScope();

var persistenceService = scope.ServiceProvider
    .GetRequiredService<PersistenceService>();

var result = await persistenceService.Save(claim);
```

The scope should be disposed before the next independent command begins.
Disposing the scope disposes the associated `DbContext` and releases its
change tracker and other scoped resources.

## Change-Tracking Risks

An EF Core context tracks entities that are added, queried, or attached. A
context reused for the entire REPL session can therefore grow continuously as
more documents and patients are loaded or saved.

Recommended practices:

- Create one context scope per REPL command.
- Use `AsNoTracking()` for read-only queries.
- Project query results into purpose-specific DTOs when full table entities
  are not needed.
- Avoid retaining entity instances in command history or global collections.
- Use `ChangeTracker.Clear()` only as a bounded-batch safeguard; it should not
  replace normal scope disposal.
- Do not use a `DbContext` concurrently from multiple REPL commands or
  background operations. A context is not thread-safe.

For higher-throughput workloads, consider registering and using
`AddPooledDbContextFactory<PayerEdiDbContext>` or
`AddPooledDbContextFactory` through an application-specific factory service.
Pooling can reduce context allocation overhead, but it does not remove the
need to dispose each context promptly or to avoid retaining tracked entities.

## Query Risks

Future repository query methods should follow these rules:

- Add `AsNoTracking()` to read-only queries.
- Apply `Where`, ordering, and projection before materialization.
- Prefer `FirstOrDefaultAsync` or `SingleOrDefaultAsync` when one result is
  expected.
- Use pagination for result sets that may grow without a known upper bound.
- Avoid unbounded `ToListAsync()` calls in REPL commands.
- Pass a `CancellationToken` from the REPL command to database operations.

For example:

```csharp
var patients = await context.Patients
    .AsNoTracking()
    .Where(patient => patient.LastName == lastName)
    .OrderBy(patient => patient.FirstName)
    .Select(patient => new PatientSummary(patient.Id, patient.FirstName, patient.LastName))
    .Take(100)
    .ToListAsync(cancellationToken);
```

## EDI Import Memory Use

`EdiProcessor.ProcessEdi` currently reads all transactions into a list before
processing them. This means a large EDI file can consume substantial memory
even if EF Core is configured correctly.

The current behavior is effectively:

```csharp
var transactions = edi.ReadToEnd().ToList();
```

For large files or frequent REPL imports, consider a streaming or bounded
batch design:

1. Read one transaction or bounded batch.
2. Map the transaction to the domain claim.
3. Persist the document and patients.
4. Dispose the command scope or clear the bounded batch state.
5. Continue with the next transaction or batch.

The REPL should also avoid keeping the complete parsed claim list after an
import command finishes. Any displayed history should be bounded and should
store summaries rather than full EDI objects or XML payloads.

## Save Atomicity

The current persistence API separates document and claim saves:

```csharp
await persistenceService.Save(ts837P);
await persistenceService.Save(professionalCareClaim);
```

This separation is useful for API clarity, but it means a document can be
saved successfully while patient persistence fails. If the document and its
patients must always be committed together, introduce a higher-level import
operation that uses one `DbContext` transaction for both writes. This is an
atomicity concern, not a memory leak, but it matters for REPL error recovery.

If separate writes remain intentional, the REPL should report partial failure
clearly and include the persisted document identifier in the error output.

## Recommended REPL Structure

- Keep `EdiProcessor` singleton only while it remains stateless.
- Keep persistence and query services scoped.
- Create and dispose an async scope for each command.
- Make the REPL loop fully asynchronous; avoid `.GetAwaiter().GetResult()`.
- Pass cancellation tokens to parsing, persistence, and query operations where
  supported.
- Bound imported-file size, batch size, query result size, and command history.
- Log command-level summaries rather than retaining full object graphs.
- Add monitoring for process working-set size, command duration, tracked entity
  counts during development, and database connection failures.

## Practical Acceptance Criteria

A REPL implementation should satisfy the following checks:

- Repeated import/query commands do not cause `DbContext` instances to remain
  rooted after command completion.
- Read-only queries do not add returned entities to the change tracker.
- Large query results are paginated or explicitly bounded.
- Large EDI inputs are processed in bounded memory or are rejected with a
  clear size limit.
- Cancellation exits an active command without leaving its scope undisposed.
- Document-only and patient-only failures are distinguishable, or both writes
  are wrapped in an explicitly atomic import operation.
