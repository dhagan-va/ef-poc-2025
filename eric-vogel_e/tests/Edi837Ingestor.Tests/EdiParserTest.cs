using System.Linq.Expressions;
using Edi837Ingester.Services;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;
using Moq;

namespace Edi837Ingestor.Tests;

public class EdiParserTest
{
    private readonly EdiParser _ediParser;
    private readonly Mock<ILogger<EdiParser>> _logger;
    private readonly Mock<IEdiSaverService> _ediSaverService;

    public EdiParserTest()
    {
        EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");
        _logger = new Mock<ILogger<EdiParser>>();
        _ediSaverService = new Mock<IEdiSaverService>();
        _ediParser = new EdiParser(_ediSaverService.Object, _logger.Object);
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
}