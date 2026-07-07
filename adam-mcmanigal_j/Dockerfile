# Multi-stage build for the EDI 837 ingestion worker.
# Build context must be this project's root (adam-mcmanigal_j) because src/src.csproj links
# ../appsettings.json and ../appsettings.Docker.json into its output.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

# Restore first, against just the project file, so the restore layer caches independently of source.
COPY src/src.csproj src/
RUN dotnet restore src/src.csproj

# Then bring in the rest of the context (source + the linked appsettings files at the root) and publish.
COPY . .
RUN dotnet publish src/src.csproj -c Release -o /app --no-restore

# Migration runner: applies pending EF migrations to the db, then exits (a compose init-container).
# Reuses the build stage (SDK + full source + restored packages) and adds the EF CLI, which drives
# the project's IDesignTimeDbContextFactory to build the connection from config + env. `database update`
# is idempotent, so re-running it is safe; rebuild this stage (`--build`) to pick up new migrations.
FROM build AS migrate
RUN dotnet tool install --global dotnet-ef --version 8.0.11
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "src/src.csproj"]

# Runtime image: the worker is a console/BackgroundService host, so the plain runtime (not aspnet).
# Debian-based, which ships ca-certificates — required because EdiFabric's SerialKey.Set validates
# the license over HTTPS at startup. Do not swap to an Alpine/chiseled base without adding certs.
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "src.dll"]
