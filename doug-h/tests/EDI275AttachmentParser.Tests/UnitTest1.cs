using System.Reflection;

namespace EDI275AttachmentParser.Tests;

public class EDI275ParserIntegrationTests
{
    static EDI275ParserIntegrationTests()
    {
        // Set EdiFabric trial key once for all tests
        SetEdiFabricTrialKey();
    }

    private static void SetEdiFabricTrialKey()
    {
        // Get license key from environment variable
        var licenseKey = Environment.GetEnvironmentVariable("EDIFABRIC_LICENSE");
        
        if (string.IsNullOrEmpty(licenseKey))
        {
            Console.WriteLine("⚠ Warning: EDIFABRIC_LICENSE environment variable not set");
            Console.WriteLine("Tests using EdiFabric parser will fail without a valid license");
            return;
        }

        try
        {
            var ediFabricAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "EdiFabric");

            if (ediFabricAssembly == null)
            {
                // Force load EdiFabric by referencing it
                var dummyType = typeof(EdiFabric.Framework.Readers.X12Reader);
                ediFabricAssembly = dummyType.Assembly;
            }

            var serialKeyType = ediFabricAssembly.GetType("EdiFabric.SerialKey");
            if (serialKeyType != null)
            {
                var setMethod = serialKeyType.GetMethod("Set", BindingFlags.Public | BindingFlags.Static);
                if (setMethod != null)
                {
                    var parameters = setMethod.GetParameters();
                    if (parameters.Length == 2 && parameters[1].ParameterType == typeof(bool))
                    {
                        // Set(String serialKey, Boolean enforce)
                        setMethod.Invoke(null, new object[] { licenseKey, true });
                        Console.WriteLine("✓ EdiFabric trial key set successfully");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠ Warning: Could not set EdiFabric trial key: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }

    [Fact]
    public async Task Main_WithSampleFile_ShouldParseSuccessfully()
    {
        // Arrange
        var sampleFilePath = Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_with_attachment.edi");
        var absolutePath = Path.GetFullPath(sampleFilePath);
        var outputDir = Path.Combine(Path.GetTempPath(), $"edi275_test_{Guid.NewGuid()}");
        
        Console.WriteLine($"Sample file: {absolutePath}");
        Console.WriteLine($"Output directory: {outputDir}");
        
        Assert.True(File.Exists(absolutePath), $"Sample file not found: {absolutePath}");

        // Act
        var args = new[]
        {
            "--file", absolutePath,
            "--output", outputDir,
            "--json"
        };

        var exitCode = await Program.Main(args);

        // Assert
        Assert.Equal(0, exitCode);
        Assert.True(Directory.Exists(outputDir), "Output directory should be created");
        
        var jsonFile = Path.Combine(outputDir, "parsed_data.json");
        Assert.True(File.Exists(jsonFile), "JSON file should be created");
        
        var jsonContent = await File.ReadAllTextAsync(jsonFile);
        Assert.NotEmpty(jsonContent);
        
        Console.WriteLine($"\n✓ Test completed successfully");
        Console.WriteLine($"JSON output:\n{jsonContent}");
        
        // Cleanup
        if (Directory.Exists(outputDir))
        {
            Directory.Delete(outputDir, true);
        }
    }
}
