using EDI837.Ingestion.Services;

namespace EDI837.Ingestion.Tests;

public class EdiParserTests
{
    public EdiParserTests()
    {
        EnvSetup.SetEdiTokenKey();
    }

    [Fact]
    public void TestParseEdiFileSuccess()
    {
        var file_path = "../../../samples/837-sample-file-success.edi";

        var transactions = EdiParser.ParseEdiFile(file_path);

        Assert.NotNull(transactions);
        Assert.NotEmpty(transactions);
        Assert.Single(transactions);
    }

    [Fact]
    public void TestParseEdiFileFailNoIea_ShouldReturnValidTransaction()
    {
        var file_path = "../../../samples/837-sample-file-fail-no-iea.edi";

        var transactions = EdiParser.ParseEdiFile(file_path);

        Assert.NotNull(transactions);
        Assert.NotEmpty(transactions);
        Assert.Single(transactions);
    }
    
    [Fact]
    public void TestParseEdiFileFailNoClaim_Fails()
    {
        var file_path = "../../../samples/837-sample-file-empty.edi";

        var transactions = EdiParser.ParseEdiFile(file_path);

        Assert.Empty(transactions);
    }
}
