
# Installed .Net 8.0
```
https://dotnet.microsoft.com/en-us/download/dotnet/8.0
```

Verify it is there:
```
dotnet --version
```

# Create the Solution and Project Files
```
dotnet new sln -n EDI837IngestionTask
dotnet new console -n EDI837IngestionTask -o src/EDI837IngestionTask --framework net8.0
dotnet new xunit -n EDI837IngestionTaskTests -o tests/EDI837IngestionTaskTests --framework net8.0
dotnet sln add src/EDI837IngestionTask tests/EDI837IngestionTaskTests
dotnet add tests/EDI837IngestionTaskTests reference src/EDI837IngestionTask
```


# Add the packages
## Ingestion project
```
cd src/EDI837IngestionTask

dotnet add package EdiFabric --version 10.7.5
dotnet add package EdiFabric.Templates.Hipaa --version 2.7.6
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.10
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.10
dotnet add package Microsoft.EntityFrameworkCore.Proxies --version 9.0.10
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.10
dotnet add package Microsoft.Extensions.Configuration --version 9.0.10
dotnet add package Microsoft.Extensions.Configuration.Binder --version 9.0.10
dotnet add package Microsoft.Extensions.FileExtensions --version 9.0.10
dotnet add package Microsoft.Extensions.Json --version 9.0.10
dotnet add package AWSSDK.S3 --version 4.0.9.1
dotnet tool install --global dotnet-ef
```


## Tests project
```
cd tests/EDI837IngestionTaskTests

dotnet add package xunit --version 2.9.3
dotnet add package xunit.runner.console --version 2.9.3
dotnet add package coverlet.collector --version 6.0.4
dotnet add package Moq --version 4.20.72
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool
```


# Previously had SQL Server running
```
# Pull the latest SQL Server 2022 image (Linux version)
docker pull mcr.microsoft.com/mssql/server:2022-latest

# Run it (replace Your_Strong_Password! with your real password or can use it as pwd)
docker run -e "ACCEPT_EULA=Y" \
           -e "SA_PASSWORD=Your_Strong_Password!" \
           -p 1433:1433 \
           --name sqlserver \
           -d mcr.microsoft.com/mssql/server:2022-latest

docker start sqlserver

modify src/{your project}/appsettings.json db Password part
```

# S3 Integration
```
python -m pip install moto[all] flask flask_cors

python start_moto.py
python uploadFile.py

```

# Running Instruction
```
First running, please follow migrations.md to conduct the misrations first so that database will be created.
/src/EDI837IngestionTask provide two run mode, local and s3
Running in local mode, use this command:
dotnet run -- --mode local

or
dotnet run -- local

or

dotnet run

Running in S3 mode, please run above python script 
which under /src/S3Integration to start the moto_server and upload sample file into S3 mock.
Then, use this command to run:
dotnet run -- --mode s3

or 

dotnet run -- s3

```