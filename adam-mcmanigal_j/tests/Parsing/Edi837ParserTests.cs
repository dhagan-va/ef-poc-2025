using Edi837Ingestion.Parsing;
using Xunit;

namespace test.Parsing;

/// <summary>
/// Drives <see cref="Edi837Parser"/> with the bundled sample 837 file and asserts it
/// parses into the expected EdiFabric transaction/claim structure. The sample is used
/// here only (the src project is source-agnostic and receives streams from S3 in prod).
/// </summary>
public class Edi837ParserTests : IClassFixture<EdiFabricLicenseFixture>
{
    private static FileStream OpenSample() =>
        File.OpenRead(Path.Combine(AppContext.BaseDirectory, "samples", "837-sample-file.edi"));

    [Fact]
    public void Parse_ReadsSingleTransaction_FromSample()
    {
        using var stream = OpenSample();

        var transactions = new Edi837Parser().Parse(stream);

        var transaction = Assert.Single(transactions);
        Assert.Equal("1239", transaction.ST?.TransactionSetControlNumber_02);
    }

    [Fact]
    public void Parse_FindsAllClaims_FromSample()
    {
        using var stream = OpenSample();

        var transactions = new Edi837Parser().Parse(stream);

        var claimIds = transactions
            .SelectMany(t => t.Loop2000A ?? [])
            .SelectMany(a => a.Loop2000B ?? [])
            .SelectMany(b => b.Loop2300 ?? [])
            .Select(c => c.CLM_ClaimInformation?.PatientControlNumber_01)
            .ToList();

        Assert.Equal(["1000A", "1001A"], claimIds);
    }
}
