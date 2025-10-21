# Load variables from .env
ifneq (,$(wildcard .env))
	include .env
	export $(shell sed 's/=.*//' .env)
endif

.PHONY: run-sql start-sql stop-sql rm-sql shell-sql logs-sql status help

run-sql:
	@echo "🚀 Starting SQL Server container ($(SQL_CONTAINER))..."
	docker run --name $(SQL_CONTAINER) \
		-e ACCEPT_EULA=Y \
		-e SA_PASSWORD=$(SQL_PASSWORD) \
		-p $(SQL_PORT):1433 \
		-d --platform=linux/amd64 \
		$(SQL_IMAGE)

start-sql:
	@echo "▶Starting existing SQL Server container..."
	docker start $(SQL_CONTAINER)

stop-sql:
	@echo "Stopping SQL Server container..."
	docker stop $(SQL_CONTAINER)

rm-sql:
	@echo "Removing SQL Server container..."
	docker rm -f $(SQL_CONTAINER)

shell-sql:
	@echo "Opening sqlcmd shell inside container..."
	docker exec -it $(SQL_CONTAINER) /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '$(SQL_PASSWORD)'

logs-sql:
	docker logs $(SQL_CONTAINER)

status:
	@echo "Docker containers:"
	docker ps -a | grep $(SQL_CONTAINER) || echo "No container named $(SQL_CONTAINER) found."

test:
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./TestResults/

help:
	@echo ""
	@echo "Available commands:"
	@echo "  make run-sql     -> Create & start new SQL Server container"
	@echo "  make start-sql   -> Start existing container"
	@echo "  make stop-sql    -> Stop container"
	@echo "  make rm-sql      -> Remove container completely"
	@echo "  make shell-sql   -> Open SQL shell inside container"
	@echo "  make logs-sql    -> View logs"
	@echo "  make status      -> Show container status"
	@echo "  make test        -> Run tests"
