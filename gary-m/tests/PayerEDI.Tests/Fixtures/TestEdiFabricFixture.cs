using PayerEDI.Data;
using PayerEDI.Data.Helpers;

namespace PayerEDI.Tests.Fixtures;

public class TestEdiFabricFixture : IAsyncDisposable
{
    private const string EdiFabricKeyEnvironmentVariable = "EDI_PROCESSOR_KEY__EDIFABRIC";

    public TestEdiFabricFixture()
    {
        var ediFabricFreeDevKey =
            Environment.GetEnvironmentVariable(EdiFabricKeyEnvironmentVariable)
            ?? throw new InvalidOperationException(
                $"The {EdiFabricKeyEnvironmentVariable} environment variable is not set."
            );

        EdiFabricHelper.ConfigureEdiFabric(ediFabricFreeDevKey);
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
