using EDI837IngestionTask.Services;

namespace EDI837IngestionTaskTests;

public class EnvSetupTest
{

    [Fact]
    public void EnvSetup_ShouldLoadSettings()
    {
        EnvSetup.GeneralInitalize();
        Assert.Equal(30, EnvSetup.PollingSeconds);
    }
}