using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X12EDI.Data.DBContext;
using X12EDI.Data.Repositories;

namespace X12EDI.Test
{
    [TestClass]
    public class EdiRepositoryTests
    {
        #region Private Fields

        private EdiDbContext? _dbContext;
        private EdiRepository? _repository;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext?.Dispose();
        }

        [TestMethod]
        public async Task SaveFileAsync_NewFileWithValidTransaction_MissingIdentifier()
        {
            // Arrange
            // An empty identifier to simulate missing identifier
            var identifier = string.Empty;
            var cancellationToken = CancellationToken.None;

            var message = new TS837P();
            var items = new List<object> { message };

            // Sanity check the _repository and _dbcontext
            Assert.IsNotNull(_repository);
            Assert.IsNotNull(_dbContext);

            bool bExceptionThrown = false;

            // Act
            try
            {
                var result = await _repository.SaveFileAsync(identifier, items, cancellationToken);
            }
            catch
            {
                bExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(bExceptionThrown);

            var savedFile = _dbContext.EdiFiles
                .Include(f => f.Transactions)
                .Include(f => f.Errors)
                .FirstOrDefault(f => f.Identifier == identifier);

            Assert.IsNull(savedFile);
        }

        [TestMethod]
        public async Task SaveFileAsync_NewFileWithValidTransaction_PersistsSuccessfully()
        {
            // Arrange
            var identifier = "FILE123";
            var cancellationToken = CancellationToken.None;

            var message = new TS837P();
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

        #endregion Public Methods
    }
}