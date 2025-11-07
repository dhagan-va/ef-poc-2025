# Started with previously installed dotnet-cli
```
brew install --cask dotnet-sdk
dotnet tool install --global dotnet-ef
```

# Installed .Net 8.0
```
https://dotnet.microsoft.com/en-us/download/dotnet/8.0
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.121-macos-arm64-installer
```

Verify it is there:
```
➜  ef-poc-2025 git:(clints-branch) dotnet --list-sdks   

8.0.121 [/usr/local/share/dotnet/sdk]
9.0.305 [/usr/local/share/dotnet/sdk]
```

# Create the Solution and Project Files
```
dotnet new sln -n EDI837.Ingestion
dotnet new console -n EDI837.Ingestion -o src/EDI837.Ingestion --framework net8.0
dotnet new xunit -n EDI837.Ingestion.Tests -o tests/EDI837.Ingestion.Tests --framework net8.0
dotnet sln add src/EDI837.Ingestion tests/EDI837.Ingestion.Tests
dotnet add tests/EDI837.Ingestion.Tests reference src/EDI837.Ingestion
```


# Add the packages
## Ingestion project
```
dotnet add package EdiFabric
dotnet add package EdiFabric.Templates.Hipaa
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Proxies
dotnet add package Microsoft.Extensions.Hosting
```


## Tests project
```
dotnet add package xunit
dotnet add package xunit.runner.console
dotnet add package coverlet.collector
dotnet tool install --global dotnet-reportgenerator-globaltool
```


# Previously had SQL Server running
```
# Pull the latest SQL Server 2022 image (Linux version)
docker pull mcr.microsoft.com/mssql/server:2022-latest

# Run it (replace YourStrongPassword! with your real password)
docker run -e "ACCEPT_EULA=Y" \
           -e "SA_PASSWORD=YourStrongPassword!" \
           -p 1433:1433 \
           --name sqlserver \
           -d mcr.microsoft.com/mssql/server:2022-latest

docker start sql2022
```

# Install Moto
```
pip install moto\[server\]
```

# Install AWS Packages
```
 dotnet add package AWSSDK.Core
 dotnet add package AWSSDK.S3
 ```

 # Linters/Formatters
 ```
dotnet add package Microsoft.CodeAnalysis.CSharp.CodeStyle
dotnet tool install -g csharpier
```