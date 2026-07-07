# EDI 837 Ingestion

A proof-of-concept service that ingests X12 EDI **837** health-care claim files, parses them with
[EdiFabric](https://www.edifabric.com/), and persists them with Entity Framework Core into SQL Server.
It handles all three 837 variants — **837P** (professional), **837I** (institutional), and **837D**
(dental).

## What it does

The app is a long-running worker that drains a queue of newly-arrived claim files:

```
S3 (raw file lands)  ──►  S3 event  ──►  SQS  ──►  IngestionWorker
                                                        │
                                                        ├─ read raw bytes from S3
                                                        ├─ parse with EdiFabric   (Edi837Parser)
                                                        ├─ dedup by SHA-256 content hash
                                                        └─ persist  (SQL Server, via EF Core)
```

- **`IngestionWorker`** (`src/Ingestion/`) is a `BackgroundService` that owns the process lifetime and
  poll loop: it long-polls SQS, hands each batch to `IngestionService`, backs off when the queue is
  empty, and recovers from transient queue failures without tearing down the host.
- **`IngestionService`** is the passive unit of work — one SQS batch per call, directly testable. For
  each message it fetches the S3 object, parses it, and writes the result in one transaction.
- **`Edi837Parser`** (`src/Parsing/`) turns a raw stream into a single `ParsedInterchange`: the ISA
  envelope plus three typed lists of transaction sets (P/I/D). Non-837 messages are ignored.

The unit of work is **one interchange** (one ISA…IEA file), matching the X12 999 acknowledgment boundary.

## Schema design and trade-offs

The persistence model is deliberately **not** a full relational shred of the EDI. There are two kinds of
tables — an idempotency ledger and per-variant transaction-set tables — and the key decisions behind them
are worth understanding before extending the schema.

### 1. Idempotency ledger — `Interchanges` (`IngestedInterchange`)

One row per ingested file. The dedup guard is **`ContentHash`**: a SHA-256 over the raw interchange
bytes, with a **unique index**. A re-sent file is byte-identical → same hash → the database rejects the
insert, so re-delivery (SQS at-least-once, payer resends) is a no-op rather than a duplicate.

> **Why a content hash and not the control numbers?** The obvious key — ISA13/GS06/ST02 — doesn't work:
> the ISA13 interchange control number recycles (it wraps at 9 digits) and is only unique per sender, so
> it can't guarantee resend detection across senders or over time. The content hash can.

The ledger also keeps envelope identity (`SenderId`/ISA06, `ReceiverId`/ISA08,
`InterchangeControlNumber`/ISA13) for observability and conflict detection, `ReceivedAt`, and
**`PayloadS3Key`** — a pointer to the raw file archived in S3. The S3 archive plus the content hash are
what enable **reflow**: on a downstream error you can re-fetch the exact original bytes and re-parse,
which is the driving requirement behind archiving the raw file rather than only the parsed form.

### 2. Per-variant transaction-set tables — one JSON column, no shred

The parsed claims live in three tables — `ProfessionalTransactionSets`, `InstitutionalTransactionSets`,
`DentalTransactionSets` — each row carrying a required, cascading FK back to its `Interchanges` row, the
GS06/ST02 control numbers, and **the entire EdiFabric message stored as a single `nvarchar(max)` JSON
column** (System.Text.Json via an EF `ValueConverter`).

> **Why JSON and not relational columns?** An 837 claim graph is enormous. The
> `EdiFabric.Templates.Hipaa` (2.7.7) `TS837P/I/D` types expand to **~336 generated classes** (loops +
> segments). Shredding that into relational tables would mean 300+ tables of join-hell to write, migrate,
> and query, for a PoC whose job is to *store and reflow* claims, not run analytics over individual
> segments. Keeping the POCO intact in one JSON column preserves the full fidelity of the parse at a
> fraction of the schema cost. The table is a structured, queryable *complement* to the S3 archive, not
> a replacement for it.
>
> **Trade-off:** you can't (yet) index or query into individual EDI segments in SQL — deep filtering
> means JSON path queries or going back to the parsed object. That's an acceptable cost for a
> store-and-reflow pipeline; it would not be if the primary use case were segment-level reporting.

> **Why three tables via generics, not EF inheritance?** The three entities share a shape, so it's
> declared once on an abstract generic base `Edi837TransactionSet<TMessage>`; each variant closes the
> generic (e.g. `Edi837ProfessionalTransactionSet : Edi837TransactionSet<TS837P>`). This is **plain C#
> reuse, not an EF mapping hierarchy** — the concrete types have distinct closed base types and the open
> generic is never mapped, so EF emits three independent tables with **no inheritance discriminator**.
> Variant is carried by *type*, so consumers never downcast.

### What's deferred

This is a PoC; some pieces are intentionally not built yet:

- Persisting the **fuller ISA envelope** (qualifiers, dates, usage indicator) — the parser captures them;
  the ledger entity stores only a subset.
- **SNIP validation** (WEDI validation levels) and anywhere to store validation results — see the design
  notes; it's additive and would also require relaxing the "≥1 transaction set" invariant on
  `IngestedInterchange.From` so that rejected/parse-failed files can be recorded.

---

## Getting started

### Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Docker](https://docs.docker.com/get-docker/) + Docker Compose (runs SQL Server and a mock AWS
  endpoint — [moto](https://github.com/getmoto/moto) — for S3 and SQS locally)
- An **EdiFabric serial key** (required — the parser validates it against EdiFabric's licensing service
  over the network at startup, so the machine also needs outbound network access)

### 1. Configure secrets

Copy the sample env file and fill in your serial key. `.env` is gitignored and is read by the app, the
integration tests, and Docker Compose alike (keys use the `Section__Key` double-underscore convention):

```bash
cp .env.sample .env
# then edit .env and set EdiFabric__SerialKey=<your-key>
```

The SA password defaults to the dev value in `.env.sample`; change it if you like — it is single-sourced
from there into the db, migrate, and app containers.

> For host-only development you can instead keep the key in
> [.NET user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):
> `dotnet user-secrets set "EdiFabric:SerialKey" "<your-key>" --project src/src.csproj`. Environment
> variables / `.env` win over user-secrets, which win over `appsettings.json`.

### 2. Run it

There are two modes, selected by Docker Compose profiles.

**Mode A — infra in Docker, app on the host** (fastest inner loop for development):

```bash
docker compose up -d          # starts db, moto, moto-init, and runs migrate (applies the schema)
dotnet run --project src/src.csproj
```

`docker compose up` (no profile) brings up the backing services **and** runs the `migrate`
init-container, so the database is migrated and ready. The host app defaults to the `Development`
environment and talks to `localhost:1433` (db) and `localhost:5001` (moto).

**Mode B — everything in containers** (full-fidelity system test):

```bash
docker compose --profile app up -d --build
```

This builds the app image and starts the whole stack in dependency order: db (healthy) → migrate
(completed) → moto (healthy) → moto-init (completed) → app. The app container runs with
`DOTNET_ENVIRONMENT=Docker` and talks to the `db` and `moto:5000` service endpoints.

Tear down with `docker compose --profile app down` (add `-v` to drop volumes).

### 3. Try it end to end

With either mode running, drop a sample claim into the bucket and watch it get ingested. The bucket is
wired to notify SQS on `ObjectCreated`, which is what the worker polls:

```bash
# requires the AWS CLI; creds are the moto placeholders
AWS_ACCESS_KEY_ID=test AWS_SECRET_ACCESS_KEY=test AWS_DEFAULT_REGION=us-east-1 \
  aws --endpoint-url http://localhost:5001 s3 cp samples/837-sample-file.edi s3://edi-bucket/
```

The worker logs the ingest, e.g. `Ingested interchange <hash> from s3://edi-bucket/... : 1P/0I/0D
transaction sets.` You should then see a row in `Interchanges` and one in `ProfessionalTransactionSets`.
Uploading the same file again is deduped (no new rows) by the content-hash unique index.

## Developer tasks (Nuke)

The raw `docker compose` and `dotnet ef` commands above are also wrapped as [Nuke](https://nuke.build)
targets, so you don't have to remember the flags. The bootstrap needs only the .NET SDK — no global
tool install. Use `./build.sh` on macOS/Linux and `.\build.cmd` on Windows — they take the same
targets and arguments (the table below uses `./build.sh`):

```bash
./build.sh --help          # macOS/Linux: list every target
.\build.cmd --help         # Windows: same
```

Common workflows:

| Task | Target | Wraps |
| --- | --- | --- |
| Infra in Docker, app on host (Mode A) | `./build.sh InfraUp` then `./build.sh Run` | `docker compose up -d --wait db moto` + `run` moto-init/migrate, then `dotnet run` |
| Whole stack in containers (Mode B) | `./build.sh AppUp` | `docker compose --profile app up -d --build` (detached — returns once up) |
| Watch logs / stop the app stack | `./build.sh Logs` / `./build.sh AppDown` | `docker compose logs -f` / `--profile app down` |
| Drop a test 837 into the bucket | `./build.sh Seed` (or `--sample-file <path>`, relative or absolute) | `aws s3 cp … s3://edi-bucket/` |
| Re-apply migrations after adding one | `./build.sh Migrate` | `docker compose run --rm --build migrate` |
| Stop / stop + wipe volumes | `./build.sh InfraDown` / `InfraReset` | `docker compose down` / `down -v` |
| Add a migration | `./build.sh AddMigration --migration-name AddFoo` | `dotnet ef migrations add` |
| Build / unit tests / integration tests | `./build.sh Compile` / `Test` / `IntegrationTest` | `dotnet build` / `dotnet test` |
| Unit tests with coverage report | `./build.sh Coverage` | `dotnet test --collect` + ReportGenerator → `coverage/report/index.html` |
| Combined unit + integration coverage | `./build.sh CoverageAll` (needs Docker) | both suites collected + merged into one `coverage/report` |
| View the coverage report in a browser | `./build.sh ServeCoverage` | serves `coverage/report` at http://localhost:5050 (rooted so index.html resolves) |

The build definition is `nuke/Build.cs`; `Test` and `IntegrationTest` are split because the former
hits the EdiFabric license quirk and the latter needs a running Docker engine for Testcontainers.

**Prerequisites per target.** The build project itself needs only the .NET SDK. Beyond that: the
Docker targets need a running Docker engine; `Seed` needs the AWS CLI on your PATH; and the coverage
targets use the `dotnet-ef`, ReportGenerator, and `dotnet-serve` local tools, which are restored
automatically from `.config/dotnet-tools.json` on first use (no manual install).

**One run at a time.** Nuke holds a lock on `.nuke/temp/build.log` for the life of a run, so two
`./build.sh` commands can't run concurrently. `AppUp` is detached so it returns immediately, but
`Logs` and `ServeCoverage` block the terminal on purpose — start further commands in another terminal
(or after Ctrl-C).

## Database migrations

Migrations live in `src/Migrations/` (currently just `InitialCreate`). The `migrate` service applies
them via `dotnet ef database update` once the db is healthy, so you normally never run EF by hand.

After **adding** a migration, rebuild and re-run the init-container to apply it (EF migrations are
idempotent, so this is always safe):

```bash
docker compose run --rm --build migrate
```

> The `migrate` container logs a benign `"EdiFabric serial key is not configured"` line. That's EF's
> tooling probing the app host first (which validates the key and throws) before falling back to the
> design-time `Edi837DbContextFactory` — which is what actually applies the migration. It's expected, not
> an error.

## Configuration reference

Configuration binds to strongly-typed options (`EdiFabricOptions`, `SqlServerOptions`, `AwsOptions`) from
layered sources, later winning over earlier:

1. `appsettings.json` (committed, secret-free) — base settings; `Server: localhost`, moto at
   `localhost:5001`, bucket `edi-bucket`, queues `edi-queue` / `edi-queue-dlq`.
2. `appsettings.{Environment}.json` — e.g. `appsettings.Docker.json` overrides the endpoints to the
   in-container `db` / `moto:5000`. Environment comes from `DOTNET_ENVIRONMENT` (default `Development`).
3. `.env` / environment variables — the secrets (`EdiFabric__SerialKey`, `SqlServer__Password`).

The app **fails fast** with a clear `InvalidOperationException` if a required setting (serial key, SQL
password, AWS region/resource names) is missing.

### CI/CD and deployment

user-secrets are a developer-machine-only mechanism. In CI and deployed environments the key is supplied
as an **environment variable** (`EdiFabric__SerialKey`), typically sourced from the platform's secret
store (e.g. GitHub Actions secrets, AWS Secrets Manager).

> **Note:** because `SerialKey.Set(...)` validates the key over the network, the CI runner needs
> **outbound network access** to EdiFabric's licensing service in addition to the secret — otherwise the
> parser tests fail with "The serial key is invalid!" even when the key is correct.

## Testing

```bash
dotnet test
```

Integration tests under `integration-tests/` use Testcontainers and require a running Docker engine.
