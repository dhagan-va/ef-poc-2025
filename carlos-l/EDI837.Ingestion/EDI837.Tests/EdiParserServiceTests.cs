using Castle.Core.Logging;
using EDI837.src.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;

namespace EDI837.Tests
{

    [TestFixture]
    public class EdiParserServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogger<EdiParserService>> _loggerMock;
        private EdiParserService _parserService;
        
        [SetUp]
        public void Setup()
        {
            string currentDirectory = "C:\\Users\\CarlosLadino\\source\\repos\\ef-poc-2025\\carlos-l\\EDI837.Ingestion\\EDI837.Tests\\";
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["LocalFileFolder"]).Returns("samples\\");
            _configurationMock.Setup(c => c["EdiFabricSerialKey"]).Returns("c417cb9dd9d54297a55c032a74c87996");

            _loggerMock = new Mock<ILogger<EdiParserService>>();
            _parserService = new EdiParserService(_configurationMock.Object, new PhysicalFileProvider(currentDirectory), _loggerMock.Object);

            //Set EDI Key
            EdiFabric.SerialKey.Set(_configurationMock.Object["EdiFabricSerialKey"]);
        }

        [Test]
        public void ExtractValid837PTransactions_NonExistentFie()
        {
            //Arrange
            var fileName = "837FileNonExistent.edi";
            List<string> errors = new List<string>();

            //Act 
            var transactions = this._parserService.ExtractValid837PTransactions(fileName, errors);
            Assert.IsEmpty(transactions);
        }

        [Test]
        public void ExtractValid837PTransactions_ExistentFie()
        {
            //Arrange
            var fileName = "837File.edi";
            List<string> errors = new List<string>();

            //Act 
            var transactions = this._parserService.ExtractValid837PTransactions(fileName, errors);
            Assert.IsTrue(transactions.Count() >= 1);
        }
    }
}