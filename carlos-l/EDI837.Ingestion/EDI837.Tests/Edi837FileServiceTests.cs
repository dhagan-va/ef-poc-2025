namespace EDI837.Tests;

[TestFixture]
public class Edi837FileServiceTests : BaseTests
{
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task SaveOriginalClaim_ValidClaim_ShouldReturnTrue(int snipLevel)
    {
        //Arrange
        var fileName = "837File.edi";
        List<string> errors = new List<string>();

        //Act 
        Stream stream = this._parserService.GetStreamByFileName(fileName);
        var transactions = this._parserService.ExtractValid837PTransactions(stream, errors, snipLevel);
        var savedClaims = await this._edi837Fileservice.SaveOriginalClaim(transactions);

        //Assert
        Assert.IsTrue(savedClaims.Count() >= 1);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task Save837PClaims_ValidClaim_ShouldReturnTrue(int snipLevel)
    {
        //Arrange
        var fileName = "837File.edi";
        List<string> errors = new List<string>();

        //Act 
        Stream stream = this._parserService.GetStreamByFileName(fileName);
        var transactions = this._parserService.ExtractValid837PTransactions(stream, errors, snipLevel);
        var savedTransactions = await this._edi837Fileservice.Save837PClaims(transactions);

        //Assert
        Assert.IsTrue(savedTransactions.Count() >= 1);
    }
}
