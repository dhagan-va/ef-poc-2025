using EDI837.src.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;

namespace EDI837.Tests;

[TestFixture]
public class Edi837FileServiceTests
{
    [SetUp]
    public void Setup()
    {
        //_configurationMock = new Mock<IConfiguration>();
        //_configurationMock.Setup(c => c["LocalFileFolder"]).Returns("samples\\");
        //_configurationMock.Setup(c => c["EdiFabricSerialKey"]).Returns("c417cb9dd9d54297a55c032a74c87996");

        //_loggerMock = new Mock<ILogger<EdiParserService>>();
        //_parserService = new EdiParserService(_configurationMock.Object, new PhysicalFileProvider("C:\\Users\\CarlosLadino\\source\\repos\\ef-poc-2025\\carlos-l\\EDI837.Ingestion\\EDI837\\"), _loggerMock.Object);

        ////Set EDI Key
        //EdiFabric.SerialKey.Set(_configurationMock.Object["EdiFabricSerialKey"]);
    }
}
