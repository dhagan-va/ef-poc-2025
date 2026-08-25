using EdiFabric.Templates.X12004010;
using EdiMettle.BLL;
using EdiMettle.Repositories;
using NLog;
using System.Text;

namespace AbsoluteUnitTests
{
    [TestClass]
    public sealed class EdiClaimTests
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Assert.IsTrue(true);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Assert.IsTrue(true);
        }

        [TestInitialize]
        public void TestInit()
        {
            Assert.IsTrue(true);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        [DataRow("\\Hipaa\\InstitutionalClaim.txt")]
        public void CanReadClaimsNoErrors(string ClaimFilePath)
        {
            int errorCount = 0;
            var transactions = ReadingRainbow.Read(ClaimFilePath);

            logger.Debug($"Found {transactions.Count()} in file \"{ClaimFilePath}\".");

            foreach (var trans in transactions)
            {
                logger.Debug($"{ClaimToString(trans)}");

                if (trans.HasErrors)
                {
                    errorCount++;
                    List<string> errors = [.. trans.ErrorContext.Flatten()];

                    foreach (var error in errors)
                    {
                        logger.Error(error);
                    }
                }
            }

            Assert.AreEqual(0, errorCount);
            Assert.IsNotEmpty(transactions);
        }

        [TestMethod]
        [DataRow("\\Hipaa\\InstitutionalClaim.txt")]
        public void CanReadClaimsToObject(string ClaimFilePath)
        {
            int errorCount = 0;
            var transactions = ReadingRainbow.Read(ClaimFilePath);

            logger.Debug($"Found {transactions.Count()} in file \"{ClaimFilePath}\".");

            foreach (var trans in transactions)
            {
                logger.Debug($"{ClaimToString(trans)}");

                if (trans.HasErrors)
                {
                    errorCount++;
                    List<string> errors = [.. trans.ErrorContext.Flatten()];

                    foreach (var error in errors)
                    {
                        logger.Error(error);
                    }
                }

                try
                {
                    var transEntity = ReadingRainbow.ClaimToEntity(trans);
                    Assert.IsNotNull(transEntity); // not a good test
                }
                catch (Exception exc)
                {
                    logger.Error(exc);
                    throw;
                }
            }

            Assert.AreEqual(0, errorCount);
            Assert.IsNotEmpty(transactions);
        }

        [TestMethod]
        [DataRow("\\Hipaa\\InstitutionalClaim.txt")]
        public void CanWriteToLocalDb(string ClaimFilePath)
        {
            int errorCount = 0;
            var transactions = ReadingRainbow.Read(ClaimFilePath);

            logger.Debug($"Found {transactions.Count()} in file \"{ClaimFilePath}\".");

            foreach (var trans in transactions)
            {
                logger.Debug($"{ClaimToString(trans)}");

                if (trans.HasErrors)
                {
                    errorCount++;
                    List<string> errors = [.. trans.ErrorContext.Flatten()];

                    foreach (var error in errors)
                    {
                        logger.Error(error);
                    }
                }

                try
                {
                    var transEntity = ReadingRainbow.ClaimToEntity(trans);

                    transEntity = RepositoryThing.AddClaim(transEntity);

                    // After adding to the database, the claim and navigation properties should have primary keys.
                    Assert.AreNotEqual(0, transEntity.Id);
                    Assert.IsNotNull(transEntity.GetTransactionSetHeader);
                    Assert.AreNotEqual(0, transEntity.GetTransactionSetHeader.Id);
                    Assert.IsNotNull(transEntity.GetBeginningHierarchicalTransaction);
                    Assert.AreNotEqual(0, transEntity.GetBeginningHierarchicalTransaction.Id);
                }
                catch (Exception exc)
                {
                    logger.Error(exc);
                    throw;
                }
            }

            Assert.AreEqual(0, errorCount);
            Assert.IsNotEmpty(transactions);
        }

        /// <summary>
        /// Quick and dirty string containing a few properties from the TS837 object.
        /// </summary>
        /// <param name="trans">Transaction object</param>
        /// <returns>Multi-line string for printing to console</returns>
        private static string ClaimToString(TS837 trans)
        {
            StringBuilder sb = new();
            var props = typeof(TS837).GetProperties();
            
            // Need recursion here to go deeper into the dream.
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(string))
                {
                    sb.Append($"{prop.Name} == \"{prop.GetValue(trans)}\" | ");
                }
                else
                {
                    var moreProps = prop.PropertyType.GetProperties();
                    foreach (var prop2 in moreProps)
                    {
                        sb.Append($"{prop2.Name} == \"{prop2.GetValue(trans)}\" | ");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
