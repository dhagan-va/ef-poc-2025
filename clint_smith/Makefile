# Load variables from .env
ifneq (,$(wildcard .env))
	include .env
	export $(shell sed 's/=.*//' .env)
endif

.PHONY: run-sql start-sql stop-sql rm-sql shell-sql logs-sql status help test format coverage \
 moto populate-s3 reset-db run-ingest-local run-ingest-s3 build drop-db create-db

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
	@echo "Starting moto_server..."
	@moto_server -p 5000 & \
	MOTO_PID=$$!; \
	sleep 2; \
	dotnet test tests/EDI837.Ingestion.Tests; \
	kill $$MOTO_PID

coverage:
	@echo "Starting moto_server..."
	@moto_server -p 5000 & \
	MOTO_PID=$$!; \
	sleep 2; \
	dotnet test tests/EDI837.Ingestion.Tests --collect:"XPlat Code Coverage" -s .runsettings --verbosity minimal; \
	kill $$MOTO_PID

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
	dotnet format \
	--include ./src/EDI837.Ingestion \
	--include ./tests/EDI837.Ingestion.Tests \
	--fix-whitespace \
	--fix-style \
	--fix-analyzers \
	--verbosity detailed
	csharpier format .


moto:
	@echo "Starting Moto mock server (S3 included) on port 5000..."
	@moto_server -p 5000

# Upload sample EDI file to Moto S3 mock
SAMPLE_PATH ?= samples/837-sample-file.edi
populate-s3:
	@echo "Creating bucket (if needed) and uploading sample EDI file..."
	@aws --endpoint-url=http://localhost:5000 s3 mb s3://edi-bucket 2>/dev/null || true
	@aws --endpoint-url=http://localhost:5000 s3 cp $(SAMPLE_PATH) s3://edi-bucket/incoming/$(notdir $(SAMPLE_PATH))
	@echo "Uploaded $(SAMPLE_PATH) to edi-bucket/incoming/"

drop-db:
	@echo "Dropping both EDI and Claim Staging databases..."
	dotnet ef database drop -p src/EDI837.Ingestion/EDI837.Ingestion.csproj -c HIPAA_5010_837P_Context --force --no-build
	dotnet ef database drop -p src/EDI837.Ingestion/EDI837.Ingestion.csproj -c ClaimStagingContext --force --no-build
	@echo "Databases dropped successfully."

create-db:
	@echo "Creating both EDI and Claim Staging databases..."
	dotnet ef database update -p src/EDI837.Ingestion/EDI837.Ingestion.csproj -c HIPAA_5010_837P_Context --no-build
	dotnet ef database update -p src/EDI837.Ingestion/EDI837.Ingestion.csproj -c ClaimStagingContext --no-build
	@echo "Databases created successfully."

reset-db: drop-db create-db
	@echo "Dropping and recreating both EDI and Claim Staging databases..."

run-ingest-local:
	@echo "Running Ingestion project synchronoushly with local sample file."
	cd src/EDI837.Ingestion && dotnet run -- local

run-ingest-s3:
	@echo "Running Ingestion project synchronoushly with local sample file."
	@echo "Starting moto_server..."
	@moto_server -p 5000 & \
	MOTO_PID=$$!; \
	sleep 2; \
	cd src/EDI837.Ingestion && dotnet run -- s3
	kill $$MOTO_PID

build:
	cd src/EDI837.Ingestion && dotnet build

help:
	@echo ""
	@echo "Available commands:"
	@echo "  make run-sql     			-> Create & start new SQL Server container"
	@echo "  make start-sql   			-> Start existing container"
	@echo "  make stop-sql    			-> Stop container"
	@echo "  make rm-sql      			-> Remove container completely"
	@echo "  make shell-sql   			-> Open SQL shell inside container"
	@echo "  make logs-sql    			-> View logs"
	@echo "  make status      			-> Show container status"
	@echo "  make test        			-> Run tests"
	@echo "  make format	  			-> Format the code"
	@echo "  make coverage    			-> Run tests with coverage"
	@echo "  make moto        			-> Run moto server"
	@echo "  make populate-s3 			-> Add files in sample directory to s3"
	@echo "  make drop-db    			-> Drop local DBs"
	@echo "  make create-db    			-> Drop local DBs"
	@echo "  make reset-db    			-> Drop local DBs and recreate"
	@echo "  make run-ingest-local    	-> Run ingestion synchronously with a local sample file"
	@echo "  make run-ingest-s3    		-> Run ingestion asynchronously; polling s3 bucket for incoming edi files"
	@echo "  make build    				-> Build the main project"


