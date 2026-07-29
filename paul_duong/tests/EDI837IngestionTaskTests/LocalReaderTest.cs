using EDI837IngestionTask.Services;

namespace EDI837IngestionTaskTests;

public class LocalReaderTest
{

    [Fact]
    public void LocalReader_ShouldReturnSamplesFileList()
    {
        var fileList = LocalReader.ListFiles();
        Assert.NotEmpty(fileList);
    }
}