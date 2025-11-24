# EDI 837 Ingestetion

A command-line tool built with .NET 8 and EdiFabric to parse EDI 837 files and save them to SQL Server.

## Features

- ✅ Parse EDI 837 files with EdiFabric
- ✅ Save 837 P/D/I files to SQL Server
- ✅ Unit tests for all features
- ✅ Loads EDI Fabric trial key from environment variable
- ✅ Command-line interface with options

## Prerequisites

- .NET 8 SDK
- EdiFabric license (trial or commercial)

## EdiFabric License Setup

EdiFabric requires a valid license key to function. 

### Get a License Key

1. **Trial License** (Free): Visit https://www.edifabric.com/trial.html
2. **Commercial License**: Visit https://www.edifabric.com/ for production use

### Set the License Key

**Option 1: Using .env file** (Recommended):

1. Copy `.env.example` to `.env`:
   ```bash
   cp .env.example .env
   ```

2. Edit `.env` and add your license key:
   ```
   TRIAL_EDIFABRIC_LICENSE=your-license-key-here
   ```

The application automatically loads the `.env` file at startup.

**Option 2: Environment variable**:

```powershell
# Windows PowerShell
$env:TRIAL_EDIFABRIC_LICENSE = "your-license-key-here"

# Windows CMD
set TRIAL_EDIFABRIC_LICENSE=your-license-key-here

# Linux/Mac
export TRIAL_EDIFABRIC_LICENSE="your-license-key-here"
```

## Installation

```bash
cd eric-vogel_e/src/Edi837Ingester
dotnet restore
dotnet build
```

## Usage

### Basic Usage

Parse an EDI 837 file:

```bash
dotnet run
Enter EDI file path when prompted.
```

### Command line arguments

Parse an EDI 837 file:

```bash
dotnet run --file "C:\path\to\your\file.edi"
```

Parse an EDI 837 file with license key:

```bash
dotnet run --file "C:\path\to\your\file.edi" --license "YOUR_LICENSE_KEY"
```

### Using Environment Variable

```bash
# Set license once
$env:EDIFABRIC_LICENSE="YOUR_LICENSE_KEY"
```

## Output

The tool produces:

1. **Console Summary** - Overview of parsed data


### Example Output

```
Enter the path to the EDI 837 file:
C:\path\to\your\file.edi
Parsing EDI 837 file...
Done parsing the EDI 837 file.
============================================================
```

## Sample EDI 837 Files

Sample files are included at `samples/'
ClaimPayment.edi - 837P
DentalClaim.edi - 837D
InstitutionalClaim.edi - 837I
## Project Structure

```
eric-vogel_e/
├── src/
│   └── Edi837Ingester/
│       ├── Data/
│       │   └── AppDbContext.cs          # EF Core database context
│       │   ├── Repositories/
│       │   │   ├── IEdiRepository.cs    # EDI repository interface
│       │   │   └── EdiRepository.cs     # EDI repository implementation
│       ├── Migrations/                  # EF Core migrations
│       ├── Services/
│       │   ├── IEdiParserService.cs     # EdiParser interface
│       │   └── EdiParserService.cs      # EdiParser implementation
│       │   └── IS3EdiParserService.cs   # S3EdiParser interface
│       │   └── S3EdiParserService.cs    # S3EdiParser implementation
│       │   └── IS3Service.cs            # S3Service interface
│       │   └── S3Service.cs             # S3Service implementation
│       ├── Program.cs                   # CLI entry point
│       └── Edi837Ingester.csproj
│   └── S3Integration/
│       └── start_moto.py 			  # Script to start local S3 server with Moto
│       └── uploadFile.py 			  # Script to upload files to local S3 server with Moto
├── samples/
│   └── ClaimPayment.edi         # Sample 837P EDI file
│   └── DentalClaim.edi          # Sample 837D EDI file
│   └── InstitutionalClaim.edi   # Sample 837I EDI file
├── tests/
│   └── Edi837Ingestor.Tests/
│       ├── EdiParserTest.cs     # EdiParserService unit tests
└── README.md
```

## How It Works

1. **Read EDI File** - Uses EdiFabric's X12Reader to parse the file
2. **Save Data to Database** - Saves parsed EDI data to SQL Server using Entity Framework Core
3. **Outputs processed transactions count** - Displays number of transactions processed to console


## Troubleshooting

### "The token was not set!" Error

This means EdiFabric license is not configured. Solutions:

1. Get a free trial license from https://www.edifabric.com/trial.html
2. Set the license using `EDIFABRIC_LICENSE` environment variable

### File Not Found

Check the file path is correct and use absolute or relative paths:
```bash
dotnet run
```
## License

MIT License

## Credits

Built with:
- [EdiFabric](https://www.edifabric.com/) - EDI parsing library
- [System.CommandLine](https://github.com/dotnet/command-line-api) - Command-line interface
- .NET 8
