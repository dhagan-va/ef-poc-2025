using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

/// <summary>
/// Developer task runner for the EDI 837 ingestion worker.
///
/// Run with the bootstrap wrappers from the project root:
///   ./build.sh &lt;Target&gt; [--options]      (macOS / Linux)
///   .\build.cmd &lt;Target&gt; [--options]      (Windows)
///
/// List every target and its help text with:  ./build.sh --help
///
/// The three primary workflows:
///   InfraUp   - infra + migrations in Docker; run the app on the host with `Run`.
///   AppUp     - the whole stack (app profile) in Docker.
///   Migrate   - re-apply migrations via the init-container after adding one.
/// </summary>
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Build configuration - 'Debug' locally, 'Release' on CI.")]
    readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    [Parameter("Name for the new EF migration (required by AddMigration).")]
    readonly string MigrationName;

    [Parameter("837 file to upload with Seed - relative to your working directory or absolute. Defaults to samples/837-sample-file.edi.")]
    readonly AbsolutePath SampleFile;

    AbsolutePath SolutionFile => RootDirectory / "EDI837Ingestion.sln";
    AbsolutePath SrcProject => RootDirectory / "src" / "src.csproj";
    AbsolutePath TestsProject => RootDirectory / "tests" / "tests.csproj";
    AbsolutePath CoverageDirectory => RootDirectory / "coverage";
    AbsolutePath CoverageSettings => RootDirectory / ".runsettings";
    AbsolutePath IntegrationTestsProject => RootDirectory / "integration-tests" / "integration-tests.csproj";

    // --------------------------------------------------------------------- //
    //  Build / quality
    // --------------------------------------------------------------------- //

    Target Clean => _ => _
        .Description("Delete bin/obj output across the solution (leaves the build project's own output).")
        .Executes(() =>
        {
            foreach (var dir in RootDirectory.GlobDirectories("**/bin", "**/obj"))
            {
                // Skip the build project's own output - it's the assembly this target is running from.
                if (BuildProjectDirectory != null && dir.ToString().StartsWith(BuildProjectDirectory))
                    continue;
                dir.DeleteDirectory();
            }
        });

    Target Restore => _ => _
        .Description("Restore NuGet packages for the solution.")
        .Executes(() => DotNetRestore(s => s.SetProjectFile(SolutionFile)));

    Target Compile => _ => _
        .Description("Build the whole solution.")
        .DependsOn(Restore)
        .Executes(() => DotNetBuild(s => s
            .SetProjectFile(SolutionFile)
            .SetConfiguration(Configuration)
            .EnableNoRestore()));

    Target Test => _ => _
        .Description("Run the unit tests. The EdiFabric-licensed tests self-warm the license token cache (see the README's 'EdiFabric license under a test host'), so they pass on a cold first run.")
        .DependsOn(Compile)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(TestsProject)
            .SetConfiguration(Configuration)
            .EnableNoBuild()));

    Target IntegrationTest => _ => _
        .Description("Run the Testcontainers integration tests. Requires a running Docker engine.")
        .DependsOn(Compile)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(IntegrationTestsProject)
            .SetConfiguration(Configuration)
            .EnableNoBuild()));

    Target Coverage => _ => _
        .Description("Run the UNIT tests with code coverage and generate an HTML + console report under coverage/ (fast; no Docker).")
        .DependsOn(Compile)
        .Executes(() => CollectCoverage(TestsProject));

    Target CoverageAll => _ => _
        .Description("Run the unit AND integration tests with coverage and merge them into one report under coverage/. Requires a running Docker engine.")
        .DependsOn(Compile)
        .Executes(() => CollectCoverage(TestsProject, IntegrationTestsProject));

    Target ServeCoverage => _ => _
        .Description("Serve the generated coverage report over HTTP and open it in the browser (run Coverage/CoverageAll first). Blocks until Ctrl-C.")
        .Executes(() =>
        {
            var report = CoverageDirectory / "report" / "index.html";
            Assert.FileExists(report, "No coverage report found - run `Coverage` or `CoverageAll` first.");

            // A dedicated static server rooted at the report directory, so index.html resolves at `/`
            // (avoids the 404 you get when an IDE serves it from the repo root instead).
            DotNetToolRestore();
            DotNet($"serve --directory {CoverageDirectory}/report --port 5050 --open-browser", RootDirectory);
        });

    Target Format => _ => _
        .Description("Apply 'dotnet format' to the solution.")
        .Executes(() => DotNet($"format {SolutionFile}", RootDirectory));

    // --------------------------------------------------------------------- //
    //  Docker environment
    // --------------------------------------------------------------------- //

    Target InfraUp => _ => _
        .Description("Start infra and apply migrations, blocking until ready, then leave it running in the background. Run the app on the host with `Run`.")
        .Executes(() =>
        {
            // `--wait` only works for services that stay running/healthy; the one-shot moto-init and
            // migrate containers exit, which `--wait` reports as a failure. So wait on the long-running
            // infra here, then run the init jobs to completion as blocking `run` calls (which surface
            // their real exit codes).
            Compose("up -d --wait db moto");    // start + wait for db and moto to be healthy
            Compose("run --rm moto-init");      // create the S3 bucket and SQS queues
            Compose("run --rm --build migrate"); // apply EF migrations (--build picks up new ones)
        });

    Target AppUp => _ => _
        .Description("Run the FULL stack in containers (infra + app profile) in the background. View output with `Logs`, stop with `AppDown`.")
        .Executes(() => Compose("--profile app up -d --build"));

    Target Migrate => _ => _
        .Description("Re-run the migrations init-container (after adding a migration). --build picks up new migrations; idempotent.")
        .Executes(() => Compose("run --rm --build migrate"));

    Target InfraDown => _ => _
        .Description("Stop and remove the containers, keeping the db volume (data survives).")
        .Executes(() => Compose("down"));

    Target InfraReset => _ => _
        .Description("Stop and remove containers AND volumes - wipes the database and moto (S3/SQS) state.")
        .Executes(() => Compose("down -v"));

    Target AppDown => _ => _
        .Description("Stop the full stack, including the app-profile service.")
        .Executes(() => Compose("--profile app down"));

    Target Logs => _ => _
        .Description("Tail logs from the running compose services (Ctrl-C to stop).")
        .Executes(() => Compose("logs -f --tail=100"));

    // --------------------------------------------------------------------- //
    //  Local inner loop (app on host, infra in Docker)
    // --------------------------------------------------------------------- //

    Target Run => _ => _
        .Description("Run the ingestion worker on the host against the compose infra. Start it first with `InfraUp`.")
        .DependsOn(Compile)
        .Executes(() => DotNetRun(s => s
            .SetProjectFile(SrcProject)
            .SetConfiguration(Configuration)
            .EnableNoBuild()));

    Target Seed => _ => _
        .Description("Upload a sample 837 file to the local S3 bucket (moto), triggering ingestion. Needs the AWS CLI and a running InfraUp. Override the file with --sample-file <path>.")
        .Executes(() =>
        {
            var file = SampleFile ?? RootDirectory / "samples" / "837-sample-file.edi";
            Assert.FileExists(file, $"Sample file not found: {file}");
            // Unique key each run so moto fires a fresh S3->SQS ObjectCreated event (the ledger still
            // dedupes by content hash, so re-uploading the same payload is a no-op past the first).
            var key = $"nuke-seed-{DateTime.UtcNow:yyyyMMdd-HHmmss}.edi";

            // moto ignores the credentials but the AWS CLI still requires them to be present.
            var env = EnvironmentInfo.Variables.ToDictionary(x => x.Key, x => x.Value);
            env["AWS_ACCESS_KEY_ID"] = "test";
            env["AWS_SECRET_ACCESS_KEY"] = "test";
            env["AWS_DEFAULT_REGION"] = "us-east-1";
            env["AWS_PAGER"] = "";

            var aws = ToolResolver.GetPathTool("aws");
            aws($"--endpoint-url http://localhost:5001 s3 cp {file} s3://edi-bucket/{key}", RootDirectory, env);
            Log.Information("Uploaded {File} to s3://edi-bucket/{Key}", file, key);
        });

    // --------------------------------------------------------------------- //
    //  EF migrations (host-side, via the dotnet-ef local tool)
    // --------------------------------------------------------------------- //

    Target AddMigration => _ => _
        .Description("Add an EF migration. Pass --migration-name <Name>.")
        .Requires(() => MigrationName)
        .Executes(() =>
        {
            DotNetToolRestore();
            DotNet($"ef migrations add {MigrationName} --project {SrcProject}", RootDirectory);
        });

    Target RemoveMigration => _ => _
        .Description("Remove the last (unapplied) EF migration.")
        .Executes(() =>
        {
            DotNetToolRestore();
            DotNet($"ef migrations remove --project {SrcProject}", RootDirectory);
        });

    Target UpdateDatabase => _ => _
        .Description("Apply pending migrations to the db from the host (host-side alternative to the `Migrate` container).")
        .Executes(() =>
        {
            DotNetToolRestore();
            DotNet($"ef database update --project {SrcProject}", RootDirectory);
        });

    Target DropDatabase => _ => _
        .Description("Drop the database from the host.")
        .Executes(() =>
        {
            DotNetToolRestore();
            DotNet($"ef database drop --force --project {SrcProject}", RootDirectory);
        });

    // --------------------------------------------------------------------- //

    /// <summary>
    /// Invoke `docker compose &lt;arguments&gt;` from the project root. The command is built as a plain
    /// literal string (not an interpolation hole) so Nuke's ArgumentStringHandler passes the flags
    /// through verbatim instead of quoting them all as a single argument.
    /// </summary>
    void Compose(string arguments)
    {
        var docker = ToolResolver.GetPathTool("docker");
        // Compose streams container logs and progress on stderr; the default logger would paint all of
        // it red as [ERR]. Log every line at Information instead - a real failure still exits non-zero
        // and fails the target regardless of log level, so no error signal is lost.
        docker("compose " + arguments, RootDirectory, logger: (_, text) => Log.Information(text));
    }

    /// <summary>
    /// Run each test project with the XPlat coverage collector into coverage/, then merge every
    /// emitted cobertura file into one HTML + console report. coverlet writes each run to its own
    /// GUID subdirectory, and ReportGenerator merges them all via the `**` glob - so passing more
    /// than one project yields a single combined report.
    /// </summary>
    void CollectCoverage(params AbsolutePath[] testProjects)
    {
        CoverageDirectory.CreateOrCleanDirectory();

        foreach (var project in testProjects)
            DotNetTest(s => s
                .SetProjectFile(project)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .SetDataCollector("XPlat Code Coverage")
                .SetSettingsFile(CoverageSettings)
                .SetResultsDirectory(CoverageDirectory));

        DotNetToolRestore();
        DotNet(
            $"reportgenerator -reports:{CoverageDirectory}/**/coverage.cobertura.xml " +
            $"-targetdir:{CoverageDirectory}/report -reporttypes:Html;TextSummary",
            RootDirectory);

        var summary = CoverageDirectory / "report" / "Summary.txt";
        if (summary.FileExists())
            Log.Information("Coverage summary:\n{Summary}", summary.ReadAllText());
        Log.Information("HTML report: {Report}", CoverageDirectory / "report" / "index.html");
    }
}
