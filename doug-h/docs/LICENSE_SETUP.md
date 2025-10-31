# EdiFabric License Setup

## Free Trial Serial Key

EdiFabric provides a **free serial key** that you can use for development and testing purposes:

```
1BB5-6C32-3E81-4380-F8DE-A92C-0123-4567
```

**Reference**: [Free code to master your EDI files](https://support.edifabric.com/hc/en-us/articles/360000280532-Free-code-to-master-your-EDI-files)

## How to Set the Serial Key

### Option 1: In Code (Recommended for Trial)

Uncomment the serial key line in `Program.cs`:

```csharp
// Set EdiFabric free trial serial key
// NOTE: The exact namespace may vary depending on your EdiFabric version
EdiFabric.Core.Model.Edi.SerialKey.Set("1BB5-6C32-3E81-4380-F8DE-A92C-0123-4567");
```

**Important**: The exact namespace for `SerialKey` may vary depending on your EdiFabric version. Common namespaces include:
- `EdiFabric.Core.Model.Edi.SerialKey`
- `EdiFabric.Framework.SerialKey`

Check the EdiFabric documentation or use IntelliSense to find the correct namespace for your version.

### Option 2: Environment Variable

Set the `EDIFABRIC_LICENSE` environment variable:

**Windows (PowerShell)**:
```powershell
$env:EDIFABRIC_LICENSE = "1BB5-6C32-3E81-4380-F8DE-A92C-0123-4567"
```

**Windows (Command Prompt)**:
```cmd
set EDIFABRIC_LICENSE=1BB5-6C32-3E81-4380-F8DE-A92C-0123-4567
```

**Linux/Mac**:
```bash
export EDIFABRIC_LICENSE="1BB5-6C32-3E81-4380-F8DE-A92C-0123-4567"
```

## Commercial License

For production use, you need a commercial license from EdiFabric:

1. Visit [EdiFabric](https://www.edifabric.com/)
2. Choose the appropriate license for your needs
3. Purchase and obtain your license key
4. Replace the trial serial key with your commercial key

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
