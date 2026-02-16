##EDI 837 Ingestetion

A command-line tool built with .NET 8 and EdiFabric to parse EDI 837 files and save them to SQL Server.

Features

- ✅ Parse EDI 837 files with EdiFabric
- ✅ Save 837 files to SQLite
- ✅ Unit tests

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

1. Copy `env.example` to `.env`:
   ```bash
   cp .env.example .env
   ```

2. Edit `.env` and add your license key and S3 credentials:
   ```
   TRIAL_EDIFABRIC_LICENSE=your-license-key-here
   S3_ACCESS_KEY_ID=your-access-key-id-here
   S3_SECRET_ACCESS_KEY=your-secret-access-key-here
   ```

The application automatically loads the `.env` file at startup.

**Option 2: Environment variable**:

```powershell
# Windows PowerShell
$env:TRIAL_EDIFABRIC_LICENSE = "your-license-key-here"
$env:S3_ACCESS_KEY_ID = "your-access-key-id-here"
$env:S3_SECRET_ACCESS_KEY = "your-secret-access-key-here"

# Windows CMD
set TRIAL_EDIFABRIC_LICENSE=your-license-key-here
set S3_ACCESS_KEY_ID=your-access-key-id-here
set S3_SECRET_ACCESS_KEY=your-secret-access-key-here

# Linux/Mac
export TRIAL_EDIFABRIC_LICENSE="your-license-key-here"
export S3_ACCESS_KEY_ID="your-access-key-id-here"
export S3_SECRET_ACCESS_KEY="your-secret-access-key-here"
```

## Installation

```bash
cd andrey-s-837-ingestion/EDIParser/src/EDIParser
dotnet restore
dotnet build
```

## Usage

### Basic Usage

Parse an EDI 837 file:

```bash
dotnet run
```

### Command line arguments

Parse an EDI 837 file:

```bash
dotnet run
```

Parse an EDI 837 file with setting SNIP level:

```bash
dotnet run --file "C:\path\to\your\file.edi" --validation 1
```

Parse an EDI 837 file from S3:

```bash
dotnet run --s3 "file.edi"
```

Parse an EDI 837 file from S3 and setting S3 bucket:

```bash
dotnet run --s3 "file.edi" --s3-bucket "your-bucket-name"
```

### Moto.py S3

Run S3 Server with Moto.py:

```bash
python start_moto.py
```

Upload sample files to Moto.py S3 server:

```bash
python uploadFile.py
```

### Using Environment Variable

```bash
# Set license once
$env:EDIFABRIC_LICENSE="YOUR_LICENSE_KEY"
```

## Output

The tool produces:

1. **Console Summary** - Overview of parsed data
2. **Log files** - Details of the executed operation in /Logs 


### Example Output
TBD
```

============================================================
```

## Sample EDI 837 Files

Sample files are included at `samples/'
ClaimPayment.edi - 837P
DentalClaim.edi - 837D
InstitutionalClaim.edi - 837I

## How It Works

1. **Read EDI File** - Uses EdiFabric's X12Reader to parse the file
2. **Save Data to Database** - Saves parsed EDI data to SQLite using Entity Framework Core
3. **Outputs processed transactions count** - 


## Troubleshooting

## License

MIT License

## Credits

Built with:
- [EdiFabric](https://www.edifabric.com/) - EDI parsing library
- [System.CommandLine](https://github.com/dotnet/command-line-api) - Command-line interface
- .NET 8
