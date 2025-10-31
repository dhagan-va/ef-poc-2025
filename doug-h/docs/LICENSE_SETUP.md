# EdiFabric License Setup

## Getting a License Key

EdiFabric requires a valid license key to function.

### Option 1: Free Trial License

1. Visit https://www.edifabric.com/trial.html
2. Sign up with your email address
3. Receive your trial license key via email
4. Trial licenses are valid for a limited time (typically 30 days)

### Option 2: Commercial License

For production use:
1. Visit https://www.edifabric.com/
2. Choose the appropriate license tier
3. Purchase and obtain your commercial license key

## Setting the License Key

### Environment Variable (Recommended)

Set the `EDIFABRIC_LICENSE` environment variable with your license key:

**Windows (PowerShell)**:
```powershell
$env:EDIFABRIC_LICENSE = "your-license-key-here"
```

**Windows (Command Prompt)**:
```cmd
set EDIFABRIC_LICENSE=your-license-key-here
```

**Linux/Mac**:
```bash
export EDIFABRIC_LICENSE="your-license-key-here"
```

For permanent setup, add the environment variable to your system settings.

The application automatically reads this environment variable at startup and configures EdiFabric.

## Alternative: Manual Parser

If you don't want to use EdiFabric, the project includes a `ManualEDI275Parser` class that doesn't require any license. It parses EDI files using string manipulation instead of the EdiFabric library.

To use the manual parser, modify the dependency injection in `Program.cs`:

```csharp
// Replace:
services.AddTransient<EDI275Parser>();

// With:
services.AddTransient<ManualEDI275Parser>();
```

## License Limitations

The free trial serial key has the following limitations:

- **For development and testing only**
- Not for production use
- May have message size or volume restrictions
- Check EdiFabric's terms of service for specific limitations

For production deployments, purchase a commercial license.

## Troubleshooting

### SerialKey Class Not Found

If you get a compile error that `SerialKey` doesn't exist:

1. Check your EdiFabric version
2. The namespace may have changed between versions
3. Try searching the EdiFabric assemblies for the correct namespace
4. Alternatively, use the `ManualEDI275Parser` which doesn't require a license

### Runtime License Errors

If you encounter license errors at runtime:

1. Make sure the serial key is set before any EdiFabric calls
2. Verify the serial key is correct (no extra spaces or characters)
3. Check that the EdiFabric package version is compatible with the serial key

## Support

For license-related issues:
- **EdiFabric Support**: [https://support.edifabric.com](https://support.edifabric.com)
- **EdiFabric Documentation**: [https://docs.edifabric.com](https://docs.edifabric.com)
- **Free Trial Info**: [https://support.edifabric.com/hc/en-us/articles/360000280532](https://support.edifabric.com/hc/en-us/articles/360000280532)
