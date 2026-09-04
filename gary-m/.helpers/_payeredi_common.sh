#!/usr/bin/env bash
set -Eeuo pipefail

cd "${0%/*}"
cd ..

repo_root="$PWD"
local_bin_dir="$repo_root/.helpers"

container_runtime=podman

if [[ -f "$repo_root/.env" ]]; then
    set -a
    source "$repo_root/.env"
    set +a
fi

cd "$repo_root"

require_command() {
    command -v "$1" >/dev/null 2>&1 || {
        printf 'Required command not found: %s\n' "$1" >&2
        exit 1
    }
}

require_container_runtime() {
    [[ -n "$container_runtime" ]] || {
        printf 'Required container runtime not found: podman\n' >&2
        exit 1
    }

    require_command "$container_runtime"
}

compose() {
    "$container_runtime" compose -f "$repo_root/docker-compose.yaml" "$@"
}

wait_for_database() {
    local container_id health_status attempt

    container_id="$("$container_runtime" inspect --format '{{.Id}}' vadb 2>/dev/null || true)"
    if [[ -z "$container_id" ]]; then
        printf 'SQL Server container was not created.\n' >&2
        exit 1
    fi

    for attempt in {1..60}; do
        health_status="$("$container_runtime" inspect --format '{{.State.Health.Status}}' "$container_id" 2>/dev/null || true)"
        case "$health_status" in
            healthy)
                return 0
                ;;
            unhealthy)
                compose logs vadb >&2 || true
                printf 'SQL Server reported an unhealthy status.\n' >&2
                exit 1
                ;;
        esac
        sleep 2
    done

    compose logs vadb >&2 || true
    printf 'Timed out waiting for SQL Server to become healthy.\n' >&2
    exit 1
}

migration_connection_string="${EDI_PROCESSOR_CONNECTIONSTRINGS__MIGRATION:-Server=localhost,1433;Database=PayerEdi;User Id=sa;Password=${MSSQL_SA_PASSWORD:-password_123};TrustServerCertificate=True}"
