#!/usr/bin/env bash
# Bootstrap for the Nuke build. Runs the build project with the installed .NET SDK.
#   ./build.sh <Target> [--options]
#   ./build.sh --help          # list all targets
set -eo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
dotnet run --project "$SCRIPT_DIR/nuke/_build.csproj" -- "$@"
