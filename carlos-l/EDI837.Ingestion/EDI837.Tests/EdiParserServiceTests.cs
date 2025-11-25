namespace EDI837.Tests
{

    [TestFixture]
    public class EdiParserServiceTests : BaseTests
    {
        [Test]
        public void ExtractValid837PTransactions_NonExistentFie()
        {
            //Arrange
            var fileName = "837FileNonExistent.edi";
            List<string> errors = new List<string>();

            //Act 
            Stream stream = this._parserService.GetStreamByFileName(fileName);
            var transactions = this._parserService.ExtractValid837PTransactions(stream, errors);

            //Assert
            Assert.IsEmpty(transactions);
        }

        [Test]
        public void ExtractValid837PTransactions_ExistentFie()
        {
            //Arrange
            var fileName = "837File.edi";
            List<string> errors = new List<string>();

            //Act 
            Stream stream = this._parserService.GetStreamByFileName(fileName);
            var transactions = this._parserService.ExtractValid837PTransactions(stream, errors);

            //Assert
            Assert.IsTrue(transactions.Count() >= 1);
        }
    }
}