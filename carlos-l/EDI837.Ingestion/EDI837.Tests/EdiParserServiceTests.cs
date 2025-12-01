namespace EDI837.Tests
{

    [TestFixture]
    public class EdiParserServiceTests : BaseTests
    {
        [Test]
        public void ExtractValid837PTransactions_NonExistentFie_ShouldReturnEmpty()
        {
            //Arrange
            var fileName = "837FileNonExistent.edi";
            List<string> errors = new List<string>();
            var snipLevel = 3; // The enum os Zero based. 3 == SNIP validation level 4.   

            //Act 
            Stream stream = this._parserService.GetStreamByFileName(fileName);
            var transactions = this._parserService.ExtractValid837PTransactions(stream, errors, snipLevel);

            //Assert
            Assert.IsEmpty(transactions);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]

        public void ExtractValid837PTransactions_ExistentFie_ShouldReturnTrue(int snipLevel)
        {
            //Arrange
            var fileName = "837File.edi";
            List<string> errors = new List<string>();

            //Act 
            Stream stream = this._parserService.GetStreamByFileName(fileName);
            var transactions = this._parserService.ExtractValid837PTransactions(stream, errors, snipLevel);

            //Assert
            Assert.IsTrue(transactions.Count() >= 1);
        }

        [TestCase(2)]      
        public void ExtractValid837PTransactions_ExistentFie_ShouldTrueForFailingSnipLevel2(int snipLevel)
        {
            //Arrange
            var fileName = "837FileFailsSnipLevel2.edi";
            List<string> errors = new List<string>();

            //Act 
            Stream stream = this._parserService.GetStreamByFileName(fileName);
            var transactions = this._parserService.ExtractValid837PTransactions(stream, errors, snipLevel);

            //Assert
            Assert.IsTrue(transactions.Count() == 0);
        }
    }
}