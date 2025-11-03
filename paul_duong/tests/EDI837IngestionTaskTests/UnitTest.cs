using EDI837IngestionTask.Services;

namespace EDI837IngestionTaskTests;

public class UnitTest
{
    public UnitTest()
    {
        EnvSetup.SetEdiSerialKey();
    }
    [Fact]
    public void Test1()
    {
        var claimFilePath = EnvSetup.GetSampleFile();
        var tran = EdiReaderParser.ReadAndParse(claimFilePath);
        Assert.NotEmpty(tran);
        Assert.NotNull(tran);

    }

    [Fact]
    public void Test2()
    {
        var claimFilePath = EnvSetup.GetSampleEmptyFile();
        var tran = EdiReaderParser.ReadAndParse(claimFilePath);
        Assert.Empty(tran);
    }
}