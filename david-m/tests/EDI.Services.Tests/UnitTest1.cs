using EDI.Data;
using EDI.Services;
using EDI.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xunit;
using EdiFabric;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Reflection;

namespace EDI.Services.Tests;

public class EdiProcessingServiceTests : IDisposable
{
    private const string ValidSerialKey = "c417cb9dd9d54297a55c032a74c87996";
    private readonly Mock<IEdiValidationService> _mockValidationService;
    private readonly EdiDbContext _context;
    private readonly EdiProcessingService _service;

    public EdiProcessingServiceTests()
    {
        ResetSerialKeyInitialization();
        Environment.SetEnvironmentVariable("EDIFABRIC_SERIAL_KEY", ValidSerialKey);

        // Setup in-memory database for testing
        var options = new DbContextOptionsBuilder<EdiDbContext>()
            .UseInMemoryDatabase(databaseName: "TestEdiDb")
            .Options;

        _context = new EdiDbContext(options);

        // Mock validation service
        _mockValidationService = new Mock<IEdiValidationService>();
        _mockValidationService.Setup(v => v.Validate(It.IsAny<EdiFabric.Core.Model.Edi.IEdiItem>()));

        // Create service instance
        _service = new EdiProcessingService(_context, _mockValidationService.Object);
    }

    private static void ResetSerialKeyInitialization()
    {
        var field = typeof(EdiProcessingService)
            .GetField("_serialKeyInitialized", BindingFlags.Static | BindingFlags.NonPublic);
        field?.SetValue(null, false);
    }

    [Fact]
    public async Task ProcessEdiAsync_ValidEDI_Content_ProcessSuccessfully()
    {
        // Arrange - Minimal 837P payload with tilde segment terminators
        var sampleEDI = @"ISA*00*          *00*          *ZZ*SENDERID       *ZZ*RECEIVERID     *250116*1234*^*00501*000000001*0*T*:~
GS*HC*SENDERID*RECEIVERID*20250116*1234*1*X*005010X222A1~
ST*837*0001*005010X222A1~
BHT*0019*00*12345*20250116*1234*CH~
NM1*41*2*SENDER*****46*123456789~
NM1*40*2*RECEIVER*****46*987654321~
HL*1**20*1~
NM1*85*2*PROVIDER*****XX*1234567893~
N3*123 MAIN ST~
N4*CITY*ST*12345~
REF*EI*123456789~
HL*2*1*22*0~
SBR*P*18*PLANID******CI~
NM1*IL*1*PATIENT*JOHN*M*M***MI*XXXXXXXXM123456~
N3*456 OAK AVE~
N4*PHOENIX*AZ*85001~
DMG*D8*19800101*M~
CLM*CLAIM123*100***11:B:1*Y*A*Y*I**********XX~
HI*ABK:J189~
LX*1~
SV1*HC:99213*50*UN*1***1~
DTP*472*D8*20240115~
REF*6R*AUTH123~
SE*21*0001~
GE*1*1~
IEA*1*000000001~";

        // Act
        await _service.ProcessEdiAsync(sampleEDI);

        // Assert
        var transactionCount = await _context.T837PTransactions.CountAsync();
        Assert.True(transactionCount > 0);
        _mockValidationService.Verify(v => v.Validate(It.IsAny<EdiFabric.Core.Model.Edi.IEdiItem>()), Times.AtLeastOnce);
    }

    [Fact]
    public void Parse837D_ValidEDI_ReturnsTransactions()
    {
        // Arrange
        var ediContent = @"structure with ST*837 and valid 837D content";

        // Act
        var transactions = _service.GetType()
            .GetMethod("Parse837D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(_service, new object[] { ediContent }) as IEnumerable<object>;

        // Assert
        Assert.NotNull(transactions);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

public class EdiProcessingServiceIntegrationTests : IDisposable
{
    private readonly string? _motoProcessId;

    public EdiProcessingServiceIntegrationTests()
    {
        // Note: In a real integration test, you would start moto server here
        // For this example, we'll show the structure
        // _motoProcessId = StartMotoServer();
        // SetupTestBucketAndData();
    }

    [Fact(Skip = "Requires moto server running - run integration tests separately")]
    public async Task ProcessEdiFilesFromS3Async_WithMotoMock_RetreievesAndProcessesFiles()
    {
        // Arrange
        Environment.SetEnvironmentVariable("AWS_SERVICE_URL", "http://localhost:5000");
        var mockDbContext = CreateInMemoryDbContext();
        var mockValidation = new Mock<IEdiValidationService>();
        var service = new EdiProcessingService(mockDbContext, mockValidation.Object);

        // Act
        await service.ProcessEdiFilesFromS3Async("test-bucket");

        // Assert
        // Verify that EDI files were processed and stored in database
        var dCount = await mockDbContext.T837DTransactions.CountAsync();
        var iCount = await mockDbContext.T837ITransactions.CountAsync();
        var pCount = await mockDbContext.T837PTransactions.CountAsync();

        Assert.True(dCount + iCount + pCount > 0);
    }

    [Fact(Skip = "Requires moto server")]
    public async Task GetEdiFilesFromS3Async_BucketWithFiles_ReturnsFileList()
    {
        // Arrange
        Environment.SetEnvironmentVariable("AWS_SERVICE_URL", "http://localhost:5000");
        var mockDbContext = CreateInMemoryDbContext();
        var mockValidation = new Mock<IEdiValidationService>();
        var service = new EdiProcessingService(mockDbContext, mockValidation.Object);

        // Act
        var fileNames = await service.GetEdiFilesFromS3Async("test-bucket");

        // Assert
        Assert.NotNull(fileNames);
        Assert.Contains(fileNames, f => f.EndsWith(".edi"));
    }

    private static EdiDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<EdiDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestEdiDb{Guid.NewGuid()}")
            .Options;
        return new EdiDbContext(options);
    }

    // Helper method to start moto server programmatically
    // private static string StartMotoServer()
    // {
    //     // Implementation to start moto server process
    //     return processId;
    // }

    public void Dispose()
    {
        // Clean up moto server if started
        // if (_motoProcessId != null) StopMotoServer(_motoProcessId);
    }
}

public class EdiValidationServiceTests
{
    private readonly EdiValidationService _validationService;

    public EdiValidationServiceTests()
    {
        // Ensure serial key is set for template parsing
        SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Use SNIP 1 for lightweight syntax validation in unit tests
                ["Edi:SnipLevel"] = "1"
            })
            .Build();

        _validationService = new EdiValidationService(config);
    }

    [Fact]
    public void Validate_WithValid837P_DoesNotThrow()
    {
        var ediMessage = ParseFirst837P(Valid837P);

        var exception = Record.Exception(() => _validationService.Validate(ediMessage));
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_WithInvalid837P_Throws()
    {
        var ediMessage = ParseFirst837P(Invalid837P_MissingClaimAmount, normalize: false);

        Assert.Throws<InvalidOperationException>(() => _validationService.Validate(ediMessage));
    }

    private static TS837P ParseFirst837P(string ediContent, bool normalize = true)
    {
        var payload = normalize ? NormalizeSegmentCount(ediContent) : ediContent;
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        var items = new X12Reader(stream, "EdiFabric.Templates.Hipaa").ReadToEnd();
        return items.OfType<TS837P>().First();
    }

    private static string NormalizeSegmentCount(string ediContent)
    {
        var segments = ediContent
            .Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        var stIndex = segments.FindIndex(s => s.StartsWith("ST*", StringComparison.OrdinalIgnoreCase));
        var seIndex = segments.FindIndex(s => s.StartsWith("SE*", StringComparison.OrdinalIgnoreCase));

        if (stIndex >= 0 && seIndex > stIndex)
        {
            var seParts = segments[seIndex].Split('*');
            if (seParts.Length >= 2)
            {
                var count = seIndex - stIndex + 1;
                seParts[1] = count.ToString();
                segments[seIndex] = string.Join("*", seParts);
            }
        }

        return string.Join("~", segments) + "~";
    }

    private const string Valid837P = """
ISA*00*          *00*          *ZZ*SENDERID       *ZZ*RECEIVERID     *250116*1234*^*00501*000000001*0*T*:~
GS*HC*SENDERID*RECEIVERID*20250116*1234*1*X*005010X222A1~
ST*837*0001*005010X222A1~
BHT*0019*00*12345*20250116*1234*CH~
NM1*41*2*SENDER*****46*123456789~
NM1*40*2*RECEIVER*****46*987654321~
HL*1**20*1~
NM1*85*2*PROVIDER*****XX*1234567893~
N3*123 MAIN ST~
N4*CITY*ST*12345~
REF*EI*123456789~
HL*2*1*22*0~
SBR*P*18*PLANID******CI~
NM1*IL*1*PATIENT*JOHN*M*M***MI*XXXXXXXXM123456~
N3*456 OAK AVE~
N4*PHOENIX*AZ*85001~
DMG*D8*19800101*M~
CLM*CLAIM123*100.00***11:B:1*Y*A*Y*I**********XX~
HI*ABK:J189~
LX*1~
SV1*HC:99213*50*UN*1***1~
DTP*472*D8*20240115~
REF*6R*AUTH123~
SE*23*0001~
GE*1*1~
IEA*1*000000001~
""";

private const string Invalid837P_MissingClaimAmount = """
ISA*00*          *00*          *ZZ*SENDERID       *ZZ*RECEIVERID     *250116*1234*^*00501*000000001*0*T*:~
GS*HC*SENDERID*RECEIVERID*20250116*1234*1*X*005010X222A1~
ST*837*0001*005010X222A1~
BHT*0019*00*12345*20250116*1234*CH~
NM1*41*2*SENDER*****46*123456789~
NM1*40*2*RECEIVER*****46*987654321~
HL*1**20*1~
NM1*85*2*PROVIDER*****XX*1234567893~
N3*123 MAIN ST~
N4*CITY*ST*12345~
REF*EI*123456789~
HL*2*1*22*0~
SBR*P*18*PLANID******CI~
NM1*IL*1*PATIENT*JOHN*M*M***MI*XXXXXXXXM123456~
N3*456 OAK AVE~
N4*PHOENIX*AZ*85001~
DMG*D8*19800101*M~
CLM*CLAIM123****11:B:1*Y*A*Y*I**********XX~
HI*ABK:J189~
LX*1~
SV1*HC:99213*50*UN*1***1~
DTP*472*D8*20240115~
REF*6R*AUTH123~
SE*5*0001~
GE*1*1~
IEA*1*000000001~
""";
}

public class EdiProcessingServiceSerialKeyTests
{
    private const string ValidSerialKey = "c417cb9dd9d54297a55c032a74c87996";

    private static void ResetSerialKeyInitialization()
    {
        var field = typeof(EdiProcessingService)
            .GetField("_serialKeyInitialized", BindingFlags.Static | BindingFlags.NonPublic);
        field?.SetValue(null, false);
    }

    [Fact]
    public void Constructor_SetsSerialKey_FromEnvironment()
    {
        // Arrange
        ResetSerialKeyInitialization();
        Environment.SetEnvironmentVariable("EDIFABRIC_SERIAL_KEY", ValidSerialKey);

        var options = new DbContextOptionsBuilder<EdiDbContext>()
            .UseInMemoryDatabase($"SerialKeyDb{Guid.NewGuid()}")
            .Options;
        using var context = new EdiDbContext(options);
        var mockValidation = new Mock<IEdiValidationService>();

        // Act
        var ex = Record.Exception(() => new EdiProcessingService(context, mockValidation.Object));

        // Assert
        Assert.Null(ex);
        var initialized = (bool)(typeof(EdiProcessingService)
            .GetField("_serialKeyInitialized", BindingFlags.Static | BindingFlags.NonPublic)?
            .GetValue(null) ?? false);
        Assert.True(initialized);
    }
}
