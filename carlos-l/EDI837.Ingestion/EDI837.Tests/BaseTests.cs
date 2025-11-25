using Amazon.Runtime;
using Amazon.S3;
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
    protected Mock<ILogger<S3FileService>> _s3LoggerMock;
    protected EdiParserService _parserService;
    protected Edi837FileService _edi837Fileservice;
    protected S3FileService _s3FileService;
    private AppDataContext _appDataContext; 


    [SetUp]
    public void Setup()
    {
        // Change the AppDataContext to save in-memory.
        var options = new DbContextOptionsBuilder<AppDataContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;
        _appDataContext = new AppDataContext(options);

        // Set the current directory to the folder where the files reside. BaseDirectory points to where the exe. To get the 
        // current directory adjust to three levels back.        
        string currentDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
        
        //Mock the needed parts of the appsettings.json
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LocalFileFolder"]).Returns("samples\\");
        _configurationMock.Setup(c => c["EdiFabricSerialKey"]).Returns("c417cb9dd9d54297a55c032a74c87996");

        //Mock AWS Settings
        _configurationMock.Setup(c => c["ServiceURL"]).Returns("http://localhost:5001");
        _configurationMock.Setup(c => c["Region"]).Returns("us-east-1");
        _configurationMock.Setup(c => c["AccessKey"]).Returns("testing");
        _configurationMock.Setup(c => c["SecretKey"]).Returns("testing");

        //Mock the loggers.
        _parserLoggerMock = new Mock<ILogger<EdiParserService>>();
        _fileLoggerMock = new Mock<ILogger<Edi837FileService>>();
        _s3LoggerMock = new Mock<ILogger<S3FileService>>();

        //Instantiate the services to be tested.
        _parserService = new EdiParserService(_configurationMock.Object, new PhysicalFileProvider(currentDirectory), _parserLoggerMock.Object);
        _edi837Fileservice = new Edi837FileService(_appDataContext, _fileLoggerMock.Object);

        // S3FileService
        var credentials = new BasicAWSCredentials(
            _configurationMock.Object["AccessKey"],
            _configurationMock.Object["SecretKey"]
        );

        var config = new AmazonS3Config
        {
            ServiceURL = _configurationMock.Object["ServiceURL"], // Points to Moto
            ForcePathStyle = true,                // REQUIRED for Moto
            AuthenticationRegion = _configurationMock.Object["Region"]
        };

        _s3FileService = new S3FileService(new AmazonS3Client(credentials, config), _s3LoggerMock.Object);

        //Set FabricEDI Key
        EdiFabric.SerialKey.Set(_configurationMock.Object["EdiFabricSerialKey"]);
    }

    [TearDown]
    public void Cleanup()
    {
        this._appDataContext.Dispose();
    }
}

