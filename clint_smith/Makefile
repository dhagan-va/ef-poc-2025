# Load variables from .env
ifneq (,$(wildcard .env))
	include .env
	export $(shell sed 's/=.*//' .env)
endif

.PHONY: run-sql start-sql stop-sql rm-sql shell-sql logs-sql status help test format coverage moto populate-s3

run-sql:
	@echo "Starting SQL Server container ($(SQL_CONTAINER))..."
	docker run --name $(SQL_CONTAINER) \
		-e ACCEPT_EULA=Y \
		-e SA_PASSWORD=$(SQL_PASSWORD) \
		-p $(SQL_PORT):1433 \
		-d --platform=linux/amd64 \
		$(SQL_IMAGE)

start-sql:
	@echo "Starting existing SQL Server container..."
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
	dotnet test tests/EDI837.Ingestion.Tests

coverage:
	dotnet test tests/EDI837.Ingestion.Tests --collect:"XPlat Code Coverage" -s .runsettings --verbosity minimal

	# Find the most recent coverage.cobertura.xml file (sorted by modification time)
	report_file=$$(find ./tests/EDI837.Ingestion.Tests/TestResults -name "coverage.cobertura.xml" -type f -print0 | xargs -0 ls -t | head -n 1); \
	if [ -z "$$report_file" ]; then \
		echo "No coverage.cobertura.xml file found."; \
		exit 1; \
	fi; \
	echo "Using coverage report: $$report_file"; \
	reportgenerator \
		-reports:"$$report_file" \
		-targetdir:"./tests/EDI837.Ingestion.Tests/TestResults/CoverageReport" \
		-reporttypes:Html; \
	open ./tests/EDI837.Ingestion.Tests/TestResults/CoverageReport/index.html; \
	echo "Coverage report generated: ./tests/EDI837.Ingestion.Tests/TestResults/CoverageReport/index.html"

format:
	dotnet format --include ./src/EDI837.Ingestion --include ./tests/EDI837.Ingestion.Tests

moto:
	@echo "Starting Moto mock server (S3 included) on port 5000..."
	@moto_server -p 5000

# Upload sample EDI file to Moto S3 mock
populate-s3:
	@echo "Creating bucket (if needed) and uploading sample EDI file..."
	@echo "Run from EDI837.Ingestion folder due to relative path."
	@aws --endpoint-url=http://localhost:5000 s3 mb s3://edi-bucket 2>/dev/null || true
	@aws --endpoint-url=http://localhost:5000 s3 cp ../../samples/837-sample-file.edi s3://edi-bucket/incoming/837-sample-file.edi
	@echo "Uploaded clint_smith/samples/837-sample-file.edi to edi-bucket/incoming/"


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
	@echo "  make format	  -> Format the code"
	@echo "  make coverage    -> Run tests with coverage"
	@echo "  make moto        -> Run moto server"
	@echo "  make populate-s3 -> Run moto server"

