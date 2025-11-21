using System.Linq.Expressions;
using DotNetEnv;
using Edi837Ingester.Data.Repositories;
using Edi837Ingester.Services;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;
using Moq;

namespace Edi837Ingestor.Tests;

public class EdiParserTest
{
    private EdiParserService _ediParser;
    private Mock<ILogger<EdiParserService>> _logger;
    private Mock<IEdiRepository> _ediSaverService;

    [SetUp]
    public void Setup()
    {
        LoadEnvironment();
        _logger = new Mock<ILogger<EdiParserService>>();
        _ediSaverService = new Mock<IEdiRepository>();
        _ediParser = new EdiParserService(_ediSaverService.Object, _logger.Object);
    }

    [Test]
    public async Task Parse_ValidProfessionalFile_SavesClaims()
    {
        // get file in root solution samples folder
        var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
            "../../../../../", "samples", "ClaimPayment.edi"));
        await _ediParser.Parse(path);
        _ediSaverService.Verify(x => x.Save(It.IsAny<List<TS837P>>()), Times.Once);
    }
    
    [Test]
    public async Task Parse_ValidDentalFile_SavesClaims()
    {
        // get file in root solution samples folder
        var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
            "../../../../../", "samples", "DentalClaim.edi"));
        await _ediParser.Parse(path);
        _ediSaverService.Verify(x => x.Save(It.IsAny<List<TS837D>>()), Times.Once);
    }
    
    [Test]
    public async Task Parse_ValidInstituionFile_SavesClaims()
    {
        // get file in root solution samples folder
        var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
            "../../../../../", "samples", "InstitutionalClaim.edi"));
        await _ediParser.Parse(path);
        _ediSaverService.Verify(x => x.Save(It.IsAny<List<TS837I>>()), Times.Once);
    }
    
    private void LoadEnvironment()
    {
        // Load environment variables from .env file
        // Load .env file if it exists
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
            Console.WriteLine("✓ Loaded configuration from .env file");
        }

        var editSerialKey = Env.GetString("TRIAL_EDIFABRIC_LICENSE");

        if(string.IsNullOrWhiteSpace(editSerialKey))
        {
            editSerialKey = "c417cb9dd9d54297a55c032a74c87996";
        }

        EdiFabric.SerialKey.Set(editSerialKey);
    }
}