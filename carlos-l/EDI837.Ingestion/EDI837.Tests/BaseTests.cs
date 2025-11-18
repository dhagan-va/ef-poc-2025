using EDI837.src.Models;
using EDI837.src.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;

namespace EDI837.Tests;

public class BaseTests
{
    protected Mock<IConfiguration> _configurationMock;
    protected Mock<ILogger<EdiParserService>> _parserLoggerMock;
    protected Mock<ILogger<Edi837FileService>> _fileLoggerMock;
    protected EdiParserService _parserService;
    protected Edi837FileService _edi837Fileservice;
    private AppDataContext _appDataContext; 

    [SetUp]
    public void Setup()
    {
        // Change the AppDataContext to save in-memory.
        var options = new DbContextOptionsBuilder<AppDataContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;
        _appDataContext = new AppDataContext(options);

        // Set the current directory to the folder where the files reside.
        string currentDirectory = "C:\\Users\\CarlosLadino\\source\\repos\\ef-poc-2025\\carlos-l\\EDI837.Ingestion\\EDI837.Tests\\";
        
        //Mock the needed parts of the appsettings.json
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LocalFileFolder"]).Returns("samples\\");
        _configurationMock.Setup(c => c["EdiFabricSerialKey"]).Returns("c417cb9dd9d54297a55c032a74c87996");

        //Mock the loggers.
        _parserLoggerMock = new Mock<ILogger<EdiParserService>>();
        _fileLoggerMock = new Mock<ILogger<Edi837FileService>>();

        //Instantiate the services to be tested.
        _parserService = new EdiParserService(_configurationMock.Object, new PhysicalFileProvider(currentDirectory), _parserLoggerMock.Object);
        _edi837Fileservice = new Edi837FileService(_appDataContext, _fileLoggerMock.Object);

        //Set FabricEDI Key
        EdiFabric.SerialKey.Set(_configurationMock.Object["EdiFabricSerialKey"]);
    }

    [TearDown]
    public void Cleanup()
    {
        this._appDataContext.Dispose();
    }
}

