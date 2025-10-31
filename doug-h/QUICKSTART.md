# EDI 275 Attachment Parser - Quick Start Guide

## Summary

A .NET 8 command-line tool that parses EDI 275 files (Additional Information to Support Health Care Claims) and extracts attachments using EdiFabric.

## What You Need

1. **.NET 8 SDK** - https://dotnet.microsoft.com/download/dotnet/8.0
2. **EdiFabric License** - Free trial at https://www.edifabric.com/trial.html

## 5-Minute Setup

### 1. Get EdiFabric License (One Time)

```bash
# Visit https://www.edifabric.com/trial.html
# Sign up with your email
# Copy the license key from your email
```

### 2. Set License

**Option A: Using .env file (Recommended)**

```bash
# Copy the example file
cp .env.example .env

# Edit .env and add your license key
# TRIAL_EDIFABRIC_LICENSE=your-license-key-here
```

**Option B: Environment Variable**

**Windows PowerShell:**
```powershell
$env:TRIAL_EDIFABRIC_LICENSE="YOUR_LICENSE_KEY_HERE"
```

**Windows CMD:**
```cmd
set TRIAL_EDIFABRIC_LICENSE=YOUR_LICENSE_KEY_HERE
```

**Linux/Mac:**
```bash
export EDIFABRIC_LICENSE="YOUR_LICENSE_KEY_HERE"
```

### 3. Build and Run

```bash
cd doug-h/src/EDI275AttachmentParser
dotnet build
dotnet run -- --file ../../samples/sample_275_with_attachment.edi --json
```

## Usage Examples

### Basic Parse
```bash
dotnet run -- --file myfile.edi
```

### With Custom Output Directory
```bash
dotnet run -- --file myfile.edi --output ./my-attachments
```

### Export as JSON
```bash
dotnet run -- --file myfile.edi --json
```

### Pass License on Command Line (Alternative to Environment Variable)
```bash
dotnet run -- --file myfile.edi --license "YOUR_LICENSE_KEY"
```

## What It Does

✅ Parses EDI 275 interchange structure (ISA/GS/ST/SE/GE/IEA)  
✅ Extracts patient information (NM1 segments)  
✅ Reads attachment metadata (PWK segments)  
✅ Decodes binary attachments (BIN segments with Base64)  
✅ Saves attachments to files  
✅ Generates summary report  
✅ Exports parsed data as JSON (optional)

## Sample Output

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

## Troubleshooting

**"The token was not set!" Error**
- Get license from https://www.edifabric.com/trial.html
- Set `EDIFABRIC_LICENSE` environment variable or use `--license` option

**File Not Found**
- Use absolute path: `dotnet run -- --file "C:\path\to\file.edi"`
- Or relative: `dotnet run -- --file ../../samples/sample_275_with_attachment.edi`

**No Attachments Found**
- Ensure EDI file contains PWK (attachment metadata) and BIN (binary data) segments

## Files Included

- `doug-h/src/EDI275AttachmentParser/` - Source code
- `doug-h/samples/sample_275_with_attachment.edi` - Sample EDI file
- `doug-h/docs/LICENSE_SETUP.md` - Detailed license setup
- `doug-h/README.md` - Complete documentation

## Command-Line Options

| Option | Description |
|--------|-------------|
| `--file` or `-f` | EDI 275 file path (required) |
| `--output` or `-o` | Output directory (default: `./attachments`) |
| `--license` or `-l` | EdiFabric license key |
| `--json` or `-j` | Export as JSON |
| `--verbose` or `-v` | Verbose logging |

## Next Steps

1. Get your EdiFabric license key
2. Try with the included sample file
3. Parse your own EDI 275 files
4. Review extracted attachments
5. Use JSON output for further processing

---

**Need Help?** See the full README.md for detailed documentation.
