using System.Reflection;
using System.Text.RegularExpressions;
using DotNetEnv;
using EDI275AttachmentParser.Services;
using Microsoft.Extensions.Logging;

namespace EDI275AttachmentParser.Tests;

public class EDI275ParserTests
{
    static EDI275ParserTests()
    {
        // Load .env file from the project root (5 levels up from test binary)
        var testDir = Directory.GetCurrentDirectory();
        var projectRoot = Path.Combine(testDir, "..", "..", "..", "..", "..");
        var envPath = Path.Combine(projectRoot, ".env");
        
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
            Console.WriteLine($"✓ Loaded .env file from: {envPath}");
        }
        else
        {
            Console.WriteLine($"⚠ No .env file found at: {envPath}");
        }

        // Set EdiFabric trial key once for all tests
        SetEdiFabricTrialKey();
    }

    private static void SetEdiFabricTrialKey()
    {
        // Get license key from environment variable
        var licenseKey = Environment.GetEnvironmentVariable("TRIAL_EDIFABRIC_LICENSE");
        
        if (string.IsNullOrEmpty(licenseKey))
        {
            Console.WriteLine("⚠ Warning: TRIAL_EDIFABRIC_LICENSE environment variable not set");
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

    private ILogger<EDI275Parser> CreateLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        return loggerFactory.CreateLogger<EDI275Parser>();
    }

    [Fact]
    public void ParseHelloWorldSample_ShouldExtractAttachmentWithTimestamp()
    {
        // Arrange
        var logger = CreateLogger();
        var parser = new EDI275Parser(logger);
        var samplePath = Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_hello_world.edi");
        var fullPath = Path.GetFullPath(samplePath);

        Console.WriteLine($"Testing with file: {fullPath}");
        Assert.True(File.Exists(fullPath), $"Sample file not found: {fullPath}");

        // Act
        var result = parser.ParseFile(fullPath);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Attachments);
        
        var attachment = result.Attachments[0];
        
        Console.WriteLine($"Attachment filename: {attachment.FileName}");
        Console.WriteLine($"Base64 data: {attachment.Base64Data}");
        Console.WriteLine($"Decoded text: {attachment.GetDecodedText()}");
        
        // Verify attachment has content
        Assert.NotNull(attachment.Base64Data);
        Assert.Equal("SGVsbG8gV29ybGQ=", attachment.Base64Data);
        
        // Verify decoded text
        var decodedText = attachment.GetDecodedText();
        Assert.Equal("Hello World", decodedText);
        
        // Verify filename has timestamp pattern
        Assert.NotNull(attachment.FileName);
        Assert.Matches(@"HelloWorld_\d{8}_\d{6}\.txt", attachment.FileName);
        
        // Verify PWK metadata
        Assert.Equal("OZ", attachment.AttachmentTypeCode);
        Assert.Equal("HelloWorld.txt", attachment.Description);
        
        Console.WriteLine("✓ Test passed: Hello World sample parsed correctly with timestamp");
    }

    [Fact]
    public void ParseHelloWorldSample_SaveToFile_ShouldCreateFileWithCorrectContent()
    {
        // Arrange
        var logger = CreateLogger();
        var parser = new EDI275Parser(logger);
        var samplePath = Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_hello_world.edi");
        var fullPath = Path.GetFullPath(samplePath);
        var outputDir = Path.Combine(Path.GetTempPath(), "edi275_tests", Guid.NewGuid().ToString());

        Console.WriteLine($"Output directory: {outputDir}");

        try
        {
            // Act
            var result = parser.ParseFile(fullPath);
            var attachment = result.Attachments[0];
            var savedFilePath = attachment.SaveToFile(outputDir);

            Console.WriteLine($"Saved file: {savedFilePath}");

            // Assert
            Assert.True(File.Exists(savedFilePath), $"Output file not created: {savedFilePath}");
            
            var fileContent = File.ReadAllText(savedFilePath);
            Assert.Equal("Hello World", fileContent);
            
            // Verify filename pattern
            var fileName = Path.GetFileName(savedFilePath);
            Assert.Matches(@"HelloWorld_\d{8}_\d{6}\.txt", fileName);
            
            Console.WriteLine("✓ Test passed: File saved with correct content and timestamp");
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
        }
    }

    [Fact]
    public void ParseMultipleTimes_ShouldCreateUniqueTimestamps()
    {
        // Arrange
        var logger = CreateLogger();
        var parser = new EDI275Parser(logger);
        var samplePath = Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_hello_world.edi");
        var fullPath = Path.GetFullPath(samplePath);

        // Act - Parse the same file twice with a small delay
        var result1 = parser.ParseFile(fullPath);
        Console.WriteLine($"First parse - filename: {result1.Attachments[0].FileName}");
        
        Thread.Sleep(1100); // Sleep slightly over 1 second to ensure different timestamp
        
        var result2 = parser.ParseFile(fullPath);
        Console.WriteLine($"Second parse - filename: {result2.Attachments[0].FileName}");

        // Assert
        var fileName1 = result1.Attachments[0].FileName;
        var fileName2 = result2.Attachments[0].FileName;
        
        Assert.NotNull(fileName1);
        Assert.NotNull(fileName2);
        Assert.NotEqual(fileName1, fileName2); // Timestamps should be different
        
        Console.WriteLine("✓ Test passed: Multiple parses create unique timestamps");
    }

    [Fact]
    public void ParseOriginalSample_ShouldExtractAttachmentWithTimestamp()
    {
        // Arrange
        var logger = CreateLogger();
        var parser = new EDI275Parser(logger);
        var samplePath = Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_with_attachment.edi");
        var fullPath = Path.GetFullPath(samplePath);

        if (!File.Exists(fullPath))
        {
            Console.WriteLine($"⚠ Skipping test: Original sample not found at {fullPath}");
            return;
        }

        // Act
        var result = parser.ParseFile(fullPath);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Attachments);
        
        var attachment = result.Attachments[0];
        
        Console.WriteLine($"Attachment filename: {attachment.FileName}");
        
        // Verify attachment has content
        Assert.NotNull(attachment.Base64Data);
        Assert.NotEmpty(attachment.Base64Data);
        
        // Verify decoded text contains expected content
        var decodedText = attachment.GetDecodedText();
        Assert.Contains("This is a sample attachment", decodedText);
        
        // Verify filename has timestamp pattern
        Assert.NotNull(attachment.FileName);
        Assert.Matches(@"attachment_\d{8}_\d{6}_\d+\.(txt|bin)", attachment.FileName);
        
        Console.WriteLine("✓ Test passed: Original sample parsed correctly with timestamp");
    }

    [Fact]
    public void ParseBothSamples_ShouldCreateSeparateAttachments()
    {
        // Arrange
        var logger = CreateLogger();
        var parser = new EDI275Parser(logger);
        var helloWorldPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_hello_world.edi"));
        var originalPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_with_attachment.edi"));
        var outputDir = Path.Combine(Path.GetTempPath(), "edi275_tests", Guid.NewGuid().ToString());

        try
        {
            // Act
            var result1 = parser.ParseFile(helloWorldPath);
            var result2 = File.Exists(originalPath) ? parser.ParseFile(originalPath) : null;

            // Save both attachments
            var savedPath1 = result1.Attachments[0].SaveToFile(outputDir);
            Console.WriteLine($"Saved Hello World to: {savedPath1}");
            
            string? savedPath2 = null;
            if (result2 != null)
            {
                savedPath2 = result2.Attachments[0].SaveToFile(outputDir);
                Console.WriteLine($"Saved original sample to: {savedPath2}");
            }

            // Assert
            Assert.True(File.Exists(savedPath1));
            Assert.Equal("Hello World", File.ReadAllText(savedPath1));
            
            if (savedPath2 != null)
            {
                Assert.True(File.Exists(savedPath2));
                Assert.NotEqual(savedPath1, savedPath2); // Different files
                Assert.Contains("This is a sample attachment", File.ReadAllText(savedPath2));
                Console.WriteLine("✓ Test passed: Both samples created separate attachments");
            }
            else
            {
                Console.WriteLine("✓ Test passed: Hello World sample created attachment (original sample not available)");
            }
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
        }
    }

    [Fact]
    public void ExtractAttachments_ShouldSaveAllAttachmentsToDirectory()
    {
        // Arrange
        var logger = CreateLogger();
        var parser = new EDI275Parser(logger);
        var samplePath = Path.Combine("..", "..", "..", "..", "..", "samples", "sample_275_hello_world.edi");
        var fullPath = Path.GetFullPath(samplePath);
        var outputDir = Path.Combine(Path.GetTempPath(), "edi275_tests", Guid.NewGuid().ToString());

        try
        {
            // Act
            var savedPaths = parser.ExtractAttachmentsFromFile(fullPath, outputDir);

            Console.WriteLine($"Extracted {savedPaths.Count} attachment(s):");
            foreach (var path in savedPaths)
            {
                Console.WriteLine($"  - {path}");
            }

            // Assert
            Assert.Single(savedPaths);
            Assert.All(savedPaths, path => Assert.True(File.Exists(path)));
            
            var content = File.ReadAllText(savedPaths[0]);
            Assert.Equal("Hello World", content);
            
            Console.WriteLine("✓ Test passed: ExtractAttachments saved all files correctly");
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
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
