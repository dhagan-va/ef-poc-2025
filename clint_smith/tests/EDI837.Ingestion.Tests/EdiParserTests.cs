using EDI837.Ingestion.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace EDI837.Ingestion.Tests;

public class EdiParserTests : TestBase
{
    private readonly EdiParser _parser;

    public EdiParserTests()
    {
        _parser = new EdiParser(NullLogger<EdiParser>.Instance);
    }

    [Fact]
    public void TestParseEdiFileSuccess()
    {
        var file_path = "../../../samples/837-sample-file-success.edi";
        var transactions = _parser.ParseEdiFileFromPath(file_path);

        Assert.NotNull(transactions);
        Assert.NotEmpty(transactions);
        Assert.Single(transactions);
    }

    [Fact]
    public void TestParseEdiFileFailNoIea_ShouldReturnValidTransaction()
    {
        var file_path = "../../../samples/837-sample-file-fail-no-iea.edi";

        var transactions = _parser.ParseEdiFileFromPath(file_path);

        Assert.NotNull(transactions);
        Assert.NotEmpty(transactions);
        Assert.Single(transactions);
    }

    [Fact]
    public void TestParseEdiFileFailNoClaim_Fails()
    {
        var file_path = "../../../samples/837-sample-file-empty.edi";

        var transactions = _parser.ParseEdiFileFromPath(file_path);

        Assert.Empty(transactions);
    }
}
