# Gary's X12 Onboarding

## Progress

Developer status checklist

| Phase      | Task             | Sub-Task                                       | Status |
|------------|------------------|------------------------------------------------|:------:|
| 🟩 Phase 1 | Setup Project    | Create .NET 10 console/web app                 |   ✅   |
| 🟩 Phase 1 | Setup Project    | Add EdiFabric NuGet Packages                   |   ✅   |
| 🟩 Phase 1 | Setup Project    | Configure SQL Server Express connection        |   ✅   |
| 🟩 Phase 1 | Setup Project    | Use Entity Framework Core                      |   ✅   |
| 🟩 Phase 1 | Implementation   | Ingest sample 837 EDI file using EdiFabric     |   ✅   |
| 🟩 Phase 1 | Implementation   | Parse EDI segments (ISA, GS, ST, BHT, etc.)    |   ✅   |
| 🟩 Phase 1 | Implementation   | Map to database entities                       |   ✅   |
| 🟩 Phase 1 | Implementation   | Store parsed data in SQL Server Express        |   ✅   |
| 🟩 Phase 1 | Implementation   | Include 1 unit test (XUnit/NUnit)              |   ✅   |
| 🟩 Phase 1 | Deliverables     | Working C# application                         |   ✅   |
| 🟩 Phase 1 | Deliverables     | Database schema/migrations                     |   ✅   |
| 🟩 Phase 1 | Deliverables     | Sample 837 test file                           |   ✅   |
| 🟩 Phase 1 | Deliverables     | Unit test coverage                             |   ✅   |
| 🟩 Phase 1 | Deliverables     | README with setup instructions                 |   ✅   |
| 🟩 Phase 1 | Deliverables     | Phase 1 demo                                   |   ✅   |
| 🟦 Phase 2 | S3 Integration   | Use moto.py for S3                             |   ✅   |
| 🟦 Phase 2 | S3 Integration   | Read EDI file from S3 bucket                   |   ✅   |
| 🟦 Phase 2 | S3 Integration   | Process files asynchronously                   |   ✅   |
| 🟦 Phase 2 | Success Criteria | Application successfully parses 837 EDI file   |   ✅   |
| 🟦 Phase 2 | Success Criteria | Data is correctly stored in SQL Server Express |   ✅   |
| 🟦 Phase 2 | Success Criteria | At least one unit test passes                  |   ✅   |
| 🟦 Phase 2 | Success Criteria | Code is properly documented                    |   ⬛   |
| 🟦 Phase 2 | Success Criteria | Peer review completed                          |   ✅   |
| 🟦 Phase 2 | Success Criteria | Phase 2 demo                                   |   🟨   |
| 🟥 Phase 3 | SNIP Validation  | Add SNIP Validation Level                      |   ⬛   |

**Status Key**

- `⬛` Not started
- `🟨` In progress
- `✅` Complete
- `❌` Blocked

--- 

## Project Structure

* [src/PayerEDI.Data](./src/PayerEDI.Data/README.md)
    * Domain models and entities for processing X12 EDI data
* [src/PayerEDI.Processor.Console](./src/PayerEDI.Processor.Console/README.md)
    * Simple CLI tool for processing an EDI File
* [tests/PayerEDI.Tests]
    * Unit tests for small parts of the Data project
* [.agents/skills/map-837-to-domain](./.agents/skills/map-837-to-domain/SKILL.md)
    * Guidance for mapping EdiFabric transactions to the existing domain factories without assuming a fixed service-line
      segment

## Setup

1. Install .Net 10
    1. Using [dotnet-install](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script) and assuming
       its on your path run `dotnet-install.sh -Channel 10.0` or if 10 is still LTS run
       `dotnet-install.sh -Channel LTS`. This will put the installed tooling in _~/.dotnet_ so be sure you add that to
       your $PATH.
    2. Alternative method would be via package manager or download from Microsoft.
2. Clone the repo and change directory there. All additional steps will take place there.

## Usage

The database is hosted in Podman using the Compose specification. Podman requires a Compose provider such as
`podman-compose` for the `podman compose` command.

On Fedora, install the required tools with:

```shell
sudo dnf install podman podman-compose
```

Enable the rootless Podman API socket used by the Compose provider:

```shell
systemctl --user enable --now podman.socket
```

To keep the user service available after logging out, enable lingering for your user:

```shell
loginctl enable-linger "$USER"
```

Rootless Podman can use the published port `1433` because it is above the privileged-port threshold. The Compose file
adds an SELinux relabel option to the read-only bootstrap-script mount for Fedora hosts.

`podman compose` is a thin wrapper around an external Compose provider. If Docker Compose is installed, Podman may use
it as the provider while still running containers through Podman's API. The provider warning is informational. Set
`PODMAN_COMPOSE_WARNING_LOGS=false` to suppress it.

If a Compose command reports that `/run/user/<uid>/podman/podman.sock` is missing, check the socket with:

```shell
systemctl --user is-active podman.socket
ls -l "$XDG_RUNTIME_DIR/podman/podman.sock"
podman compose ps
```

Start the socket with `systemctl --user start podman.socket` if it is inactive.

### The Direct Way

#### Step 1 Start the database in the foreground

`podman compose up vadb`

Wait for the db to become healthy, watch the output from the terminal

Compose gives you the option of detaching by adding the `-d` switch if you prefer not to watch the terminal once the
database starts up.

#### Step 2 Initialize the database

```shell
podman compose run --rm --no-deps db-init
```

```shell 
export EDI_PROCESSOR_CONNECTIONSTRINGS__MIGRATION="Server=localhost,1433;Database=PayerEdi;User Id=sa;Password=password_123;TrustServerCertificate=True"
dotnet tool restore
dotnet ef database update \
    --project src/PayerEDI.Data \
    --startup-project src/PayerEDI.Processor.Console
```

#### Step 3. Run Some Software

A simple console app has been built in src/PayerEDI.Processor.Console that has 3 predefined launch profiles. The app
itself isn't very smart, it just parses and EDI file and somes some data to the database that should be now running and
prepared. An environment variable called **EDI_PROCESSOR_KEY__EDIFABRIC** will need to be exported before running
anything through this console app. Note that the **EDI_PROCESSOR_CONNECTIONSTRINGS__DEFAULT** is stored in the
src/PayerEDI.Processor.Console/Properties/launchSettings.json for each profile configured there. If you run anything
other than the provided launch profiles you'll need to export that as well (See above for that).

Processing the 837D - Dental File

```shell
dotnet run --project src/PayerEDI.Processor.Console --launch-profile dental
```

Processing the simple fields 837P - Professional File

```shell
dotnet run --project src/PayerEDI.Processor.Console --launch-profile professional
```

Processing the all fields 837P - Professional File

```shell
dotnet run --project src/PayerEDI.Processor.Console --launch-profile professionalall
```

#### Step 4 Tear It All Down

```shell
podman compose down --remove-orphans --volumes
```

### The Helper Scripts Way

I had AI generate a bunch of shell scripts to make some this easier

#### With Helper Scripts

```shell
./.helpers/dev-start
./.helpers/dev-migrate
./.helpers/dental
./.helpers/professional
./.helpers/professionalall
./.helpers/dev-down
```

Bellow is a breakdown of all the available scripts.

| Script                | Purpose                                                                                                         |
|-----------------------|-----------------------------------------------------------------------------------------------------------------|
| `dev-start`           | Starts SQL Server and MotoServer, waits for SQL Server health, and reruns the idempotent database bootstrap.   |
| `dev-migrate`         | Applies pending EF Core migrations with the administrative connection.                                          |
| `dev-truncate`        | Truncates / empties all user tables in the `PayerEdi` database.                                                 |
| `dev-down`            | Stops the development stack; prompts before removing the `vadb` volume, or use `--reset` to skip the prompt.    |
| `run-dental`          | Runs the console with the `dental` launch profile.                                                              |
| `run-professional`    | Runs the console with the `professional` launch profile.                                                        |
| `run-professionalall` | Runs the console with the `professionalall` launch profile.                                                     |
| `local-down`          | Legacy alias for stopping the local Compose stack while preserving named volumes.                               |
| `pretty-code`         | Runs csharpier against _src/_ and _tests/_                                                                      |
| `style-check`         | Runs the Roslyn code-style checker without modifying files.                                                     |

Use `dev-truncate` to quickly clear all data from user tables while preserving the database schema and migration history.
Use `dev-down` for a normal shutdown. Answer `y` or `yes` when prompted, or pass `--reset`, when the local database
should be recreated from scratch; this deletes all data in the named volume.

## Formatting

To format the code consistently using CSharpier, run

### The Easy Way

```shell
./.helpers/pretty-code
```

### The Hard Way

```shell
dotnet csharpier format src/
dotnet csharpier format tests/
```
