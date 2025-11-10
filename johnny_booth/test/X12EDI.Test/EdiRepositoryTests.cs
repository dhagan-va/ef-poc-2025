using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X12EDI.Test.Templates;
using X12EDI.Data.DBContext;
using X12EDI.Data.Repositories;

namespace X12EDI.Test
{
    [TestClass]
    public class EdiRepositoryTests
    {
        private EdiDbContext? _dbContext;
        private EdiRepository? _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EdiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new EdiDbContext(options);

            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
            var logger = loggerFactory.CreateLogger<EdiRepository>();

            _repository = new EdiRepository(_dbContext, logger);
        }

        [TestMethod]
        public async Task SaveFileAsync_NewFileWithValidTransaction_PersistsSuccessfully()
        {
            // Arrange
            var identifier = "FILE123";
            var cancellationToken = CancellationToken.None;

            var message = new TS837Stub(); 
            var items = new List<object> { message };

            // Sanity check the _repository and _dbcontext
            Assert.IsNotNull(_repository);
            Assert.IsNotNull(_dbContext);

            // Act
            var result = await _repository.SaveFileAsync(identifier, items, cancellationToken);

            // Assert
            Assert.IsTrue(result);


            var savedFile = _dbContext.EdiFiles
                .Include(f => f.Transactions)
                .Include(f => f.Errors)
                .FirstOrDefault(f => f.Identifier == identifier);

            Assert.IsNotNull(savedFile);
            Assert.AreEqual(1, savedFile.Transactions.Count);
            Assert.AreEqual(0, savedFile.Errors.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext?.Dispose();
        }
    }
}
