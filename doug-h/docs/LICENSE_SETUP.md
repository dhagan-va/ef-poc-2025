# EdiFabric License Setup

EdiFabric requires a license to run. You have two options:

## Option 1: Trial License (Recommended for POC)

1. Visit https://www.edifabric.com/trial.html
2. Sign up for a free trial
3. You'll receive a license key via email
4. Set the license in your code before creating the X12Reader:

```csharp
using EdiFabric.Core.Model.Edi;

// At the start of your application
SerialKey.Set("YOUR_LICENSE_KEY_HERE");
```

## Option 2: Use Alternative Approach (No EdiFabric)

For a simple POC without EdiFabric licensing, you can parse EDI 275 manually by:

1. Reading the file as text
2. Splitting by segment terminators (~)
3. Parsing each segment by splitting on delimiters (*)
4. Extracting the BIN and PWK segments manually

## Quick Start with Trial

1. Get trial license from EdiFabric
2. Create a file `appsettings.json`:

```json
{
  "EdiFabric": {
    "LicenseKey": "YOUR_LICENSE_KEY"
  }
}
```

3. Run the application

## Alternative: Manual Parser

See the `ManualEDI275Parser.cs` file for a simple implementation that doesn't require EdiFabric.
