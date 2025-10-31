# EDI 275 Attachment Parser

A command-line tool built with .NET 8 and EdiFabric to parse EDI 275 (Additional Information to Support a Health Care Claim or Encounter) files and extract attachments.

## Features

- ✅ Parse EDI 275 files with EdiFabric
- ✅ Extract binary attachments (BIN segments)
- ✅ Parse attachment metadata (PWK segments)
- ✅ Extract patient information
- ✅ Save attachments to disk
- ✅ Export parsed data as JSON
- ✅ Command-line interface with options

## Prerequisites

- .NET 8 SDK
- EdiFabric license (trial or commercial)

## EdiFabric License Setup

### Option 1: Get Free Trial License (Recommended)

1. Visit **https://www.edifabric.com/trial.html**
2. Sign up for a free trial account
3. Check your email for the license key
4. Use the license key with one of these methods:

**Method A: Command-line option**
```bash
dotnet run -- --file myfile.edi --license "YOUR_LICENSE_KEY"
```

**Method B: Environment variable**
```bash
# Windows PowerShell
$env:EDIFABRIC_LICENSE="YOUR_LICENSE_KEY"
dotnet run -- --file myfile.edi

# Windows CMD
set EDIFABRIC_LICENSE=YOUR_LICENSE_KEY
dotnet run -- --file myfile.edi

# Linux/Mac
export EDIFABRIC_LICENSE="YOUR_LICENSE_KEY"
dotnet run -- --file myfile.edi
```

### Option 2: Commercial License

If you have a commercial EdiFabric license, use it the same way as the trial license above.

## Installation

```bash
cd doug-h/src/EDI275AttachmentParser
dotnet restore
dotnet build
```

## Usage

### Basic Usage

Parse an EDI 275 file and extract attachments:

```bash
dotnet run -- --file ../../samples/sample_275_with_attachment.edi --license "YOUR_LICENSE_KEY"
```

### Using Environment Variable

```bash
# Set license once
$env:EDIFABRIC_LICENSE="YOUR_LICENSE_KEY"

# Then run without --license option
dotnet run -- --file ../../samples/sample_275_with_attachment.edi
```

### Specify Output Directory

```bash
dotnet run -- --file myfile.edi --output ./extracted --license "YOUR_LICENSE_KEY"
```

### Export as JSON

```bash
dotnet run -- --file myfile.edi --json --license "YOUR_LICENSE_KEY"
```

### All Options Together

```bash
dotnet run -- --file myfile.edi --output ./attachments --json --verbose --license "YOUR_LICENSE_KEY"
```

## Command-Line Options

| Option | Alias | Description | Default |
|--------|-------|-------------|---------|
| `--file` | `-f` | Path to EDI 275 file (required) | - |
| `--output` | `-o` | Output directory for attachments | `./attachments` |
| `--license` | `-l` | EdiFabric license key | (reads from env var) |
| `--json` | `-j` | Export parsed data as JSON | `false` |
| `--verbose` | `-v` | Enable verbose logging | `false` |

## EDI 275 Structure

The parser handles the following EDI segments:

- **ISA** - Interchange Control Header
- **GS** - Functional Group Header
- **ST** - Transaction Set Header (275)
- **BHT** - Beginning of Hierarchical Transaction
- **NM1** - Individual or Organizational Name (Patient info)
- **PWK** - Paperwork (Attachment metadata)
- **BIN** - Binary Data Segment (Attachment content)
- **SE** - Transaction Set Trailer
- **GE** - Functional Group Trailer
- **IEA** - Interchange Control Trailer

## Output

The tool produces:

1. **Console Summary** - Overview of parsed data
2. **Extracted Attachments** - Binary files saved to output directory
3. **JSON Export** (optional) - Complete parsed data in JSON format

### Example Output

```
============================================================
EDI 275 PARSING SUMMARY
============================================================
Interchange Control Number: 000000001
Sender ID: SENDERID
Receiver ID: ReceiverId
Transaction Date: 10/31/2023 2:30:00 PM
Patient Reports: 1
Attachments Found: 1
============================================================

PATIENT INFORMATION:
  - JOHN DOE (ID: 123456789)

ATTACHMENTS:
  [1] Control#: 
      Type: OZ
      Description: Medical Records Attachment
      Size: 1024 bytes
      File: attachment_1.bin

Extracting attachments to: ./attachments
✓ Attachments extracted successfully!
✓ JSON export saved to: ./attachments/parsed_data.json
============================================================
```

## Sample EDI 275 File

A sample file is included at `samples/sample_275_with_attachment.edi` demonstrating:
- Patient information (NM1 segment)
- Attachment metadata (PWK segment)
- Binary attachment data (BIN segment with Base64 encoding)

## Project Structure

```
doug-h/
├── src/
│   └── EDI275AttachmentParser/
│       ├── Models/
│       │   └── EDI275Models.cs          # Data models
│       ├── Services/
│       │   ├── EDI275Parser.cs          # EdiFabric parser
│       │   └── ManualEDI275Parser.cs    # Manual parser (no license needed)
│       ├── Program.cs                    # CLI entry point
│       └── EDI275AttachmentParser.csproj
├── samples/
│   └── sample_275_with_attachment.edi   # Sample EDI file
├── docs/
│   └── LICENSE_SETUP.md                 # License setup guide
└── README.md
```

## How It Works

1. **Read EDI File** - Uses EdiFabric's X12Reader to parse the file
2. **Extract Segments** - Identifies ISA, GS, ST, NM1, PWK, and BIN segments
3. **Parse Metadata** - Extracts patient info and attachment metadata
4. **Decode Attachments** - Decodes Base64 binary data from BIN segments
5. **Save Files** - Writes attachments to the output directory
6. **Generate Report** - Displays summary and optionally exports JSON

## Attachment Handling

The parser supports multiple attachment formats:

- **Binary Data** - Direct binary content
- **Base64 Encoded** - Standard EDI approach for binary data
- **Text Data** - Plain text attachments

## Troubleshooting

### "The token was not set!" Error

This means EdiFabric license is not configured. Solutions:

1. Get a free trial license from https://www.edifabric.com/trial.html
2. Set the license using `--license` option or `EDIFABRIC_LICENSE` environment variable

### No Attachments Found

Ensure your EDI 275 file contains:
- PWK segment (attachment metadata)
- BIN segment (attachment data)

### File Not Found

Check the file path is correct and use absolute or relative paths:
```bash
dotnet run -- --file "C:\path\to\file.edi" --license "YOUR_KEY"
```

## Alternative: Manual Parser (No License Required)

If you don't want to use EdiFabric, a manual parser is included in `Services/ManualEDI275Parser.cs` that parses EDI files without requiring a license. This is suitable for simple parsing needs.

## License

MIT License

## Credits

Built with:
- [EdiFabric](https://www.edifabric.com/) - EDI parsing library
- [System.CommandLine](https://github.com/dotnet/command-line-api) - Command-line interface
- .NET 8
