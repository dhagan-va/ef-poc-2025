## Solution Structure
```
- EDI_NCPDP_Ingestion
  - scr/
    - EDI_NCPDP_Ingestion/    <- Main Project
      - App.config            <- Connection values for the project
      - Contextdb.cs          <- Database Model
      - MotoInitializer.cs    <- Launches Moto and uploads a sample file
      - Program.cs            <- Main application
      - ReadNCPDP.cs          <- Reads and Parses the file
      - S3FileLoad.cs         <- Parses the S3 file
      - SaveNCPDP.cs          <- Saves the parsed file to the database
      - run-moto.cs           <- Starts the local moto server
  - tests/
    - NCPDP_Test/             <- Test Project
      - NCPDPTest.cs          <- Main Test application
      - MSTestSettings.cs     
  - samples/                  <- Testing Files
  - docs/
```

## NuGet Packages and Dependencies

- EdiFabric
- EdiFabric.Templates.NCPDP
- EduFabric.Framework.Readers
- EntityFramework
- Microsoft.EntityFrameworkCore
- Microsoft.Extentions.Configuration
- Amazon.S3
- Amazon.S3.Model
- System.Diagnostics
- System.Net.Sockets
- System.Configuration
- Moq
- MSTest
- localdb
- .NET8 SDK

## Set Up
Copy the contents of the samples folder to a location of your choice and update App.Config and NCPDPTest.cs with the folder location before executing the main application. (Currently set to C:\Files.)
To test with Moto.py, run ```run-moto.ps1``` using Windows PowerShell. This will launch a local server to mimic AWS.

### Database Migration Creation
Run the following in PowerShell or the Package Manager Console in Visual Studio
1. Add-Migration InitialCreate
2. Update-Database
  
