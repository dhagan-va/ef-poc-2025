# EDI 837 Ingestion

A proof-of-concept for parsing X12 EDI 837 (professional claim) files with
[EdiFabric](https://www.edifabric.com/) and persisting them with Entity Framework Core.

## Initial Setup

### Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Docker](https://docs.docker.com/get-docker/) and Docker Compose (used to run
  SQL Server and a mock AWS endpoint for S3 and SQS locally)
- An EdiFabric serial key (required to run the parser)

### 1. Restore and build

```bash
dotnet build
```

### 2. Start the backing services

The local SQL Server database and a mock S3 (moto) endpoint run in Docker:

```bash
docker compose up -d
```

This starts:

- **db** — SQL Server 2022 on `localhost:1433`
- **moto** — mock S3 on `localhost:5001`

Stop them later with `docker compose down`.

### 3. Add your EdiFabric serial key

The serial key is read from configuration and is **required** — the app fails
fast with an `InvalidOperationException` if it is missing.

It is never committed to source control. In development it is supplied through
[.NET user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets),
which are stored outside the repository. Both projects share the same
`UserSecretsId`, so a single command sets the key for both:

```bash
dotnet user-secrets set "EdiFabric:SerialKey" "<your-key>" --project src/src.csproj
```

Replace `<your-key>` with your actual EdiFabric serial key.

### 4. Verify the key is loaded

```bash
dotnet run --project src/src.csproj
```

You should see `EdiFabric serial key loaded.` If the key is not set, you'll get
a clear error telling you which command to run.

## Configuration

Configuration is bound to strongly-typed options (`EdiFabricOptions`) from the
`EdiFabric` section of `appsettings.json`. A single `appsettings.json` lives at
the repository root and is **linked** into both `src` and `tests`, so it is
edited in one place:

```json
{
  "EdiFabric": {
    "SerialKey": ""
  }
}
```

Sources are layered in increasing order of precedence (later wins):

1. `appsettings.json` (committed, key left empty)
2. user-secrets — development only
3. environment variables — CI/CD and deployment

Leave `SerialKey` empty in `appsettings.json`; the real value comes from
user-secrets (dev) or environment variables (CI/deploy), so it stays out of
source control.

### CI/CD and deployment

user-secrets are a **developer-machine-only** mechanism — they are never built
or deployed. In CI/CD and deployed environments the key is supplied as an
**environment variable** instead. .NET maps the double-underscore form onto the
config key, so `EdiFabric:SerialKey` is set via:

```
EdiFabric__SerialKey=<your-key>
```

- **CI (GitHub Actions):** store the key as a repository/environment secret and
  expose it to the build/test step (illustrative — no workflow file is included):

  ```yaml
  steps:
    - run: dotnet test
      env:
        EdiFabric__SerialKey: ${{ secrets.EDIFABRIC_SERIAL_KEY }}
  ```

  > **Note:** the parser tests apply the EdiFabric license via `SerialKey.Set(...)`,
  > which validates the key against EdiFabric's licensing service over the network.
  > The CI runner therefore needs **outbound network access** to that service in
  > addition to the `EdiFabric__SerialKey` secret — without it, license validation
  > fails and the parser tests error with "The serial key is invalid!" even when the
  > key is correct.

- **Containers / deployment:** set `EdiFabric__SerialKey` as a secret environment
  variable, e.g.:

  ```bash
  docker run -e EdiFabric__SerialKey=<your-key> <image>
  ```

  In production this is typically sourced from your orchestrator's secret store
  or a cloud secret manager (e.g. AWS Secrets Manager) and surfaced to the
  process as that environment variable.
