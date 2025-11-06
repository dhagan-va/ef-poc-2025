using EDI837IngestionTask.Services;

namespace EDI837IngestionTaskTests;

public class EdiReaderParserTest
{
    public EdiReaderParserTest()
    {
        EnvSetup.SetEdiSerialKey();
    }
    [Fact]
    public void EdiReaderParser_ShouldReadFile()
    {
        var claimFilePath = EnvSetup.GetSampleFile();
        var tran = EdiReaderParser.ReadAndParse(claimFilePath);
        Assert.NotEmpty(tran);
        Assert.NotNull(tran);

    }

    [Fact]
    public void EdiReaderParser_ShouldReadFileAndReturnEmpty()
    {
        var claimFilePath = EnvSetup.GetSampleFile1();
        var tran = EdiReaderParser.ReadAndParse(claimFilePath);
        Assert.Empty(tran);
    }

    [Fact]
    public void EdiReaderParser_ShouldReturnEmpty()
    {
        var claimFilePath = EnvSetup.GetSampleEmptyFile();
        var tran = EdiReaderParser.ReadAndParse(claimFilePath);
        Assert.Empty(tran);
    }
}