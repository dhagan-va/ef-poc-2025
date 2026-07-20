using Edi837Ingestion.Parsing;
using EdiFabric.Templates.Hipaa5010;
using Xunit;

namespace test.Parsing;

/// <summary>
/// Drives <see cref="Edi837Parser"/> with every bundled 837 sample and asserts each parses
/// into a <see cref="ParsedInterchange"/> with the right envelope and transaction set across all
/// three 837 variants (professional/institutional/dental). The samples are used here only
/// (the src project is source-agnostic and receives streams from S3 in prod).
/// </summary>
[Collection("EdiFabric")]
public class Edi837ParserTests
{
    private static ParsedInterchange ParseSample(string fileName)
    {
        using var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "samples", fileName));
        return new Edi837Parser().Parse(stream);
    }

    // The professional samples each land a single 837P transaction set in the professional list (and
    // nothing in the other variants' lists), carrying the expected GS06/ST02 control numbers.
    [Theory]
    [InlineData("837-sample-file.edi", "1239")]
    [InlineData("ClaimPayment.txt", "0021")]
    [InlineData("ClaimPaymentEVV.txt", "0021")]
    public void Parse_ReadsSingleProfessionalTransactionSet_WithControlNumbers(
        string fileName, string expectedSt02)
    {
        var interchange = ParseSample(fileName);

        var transactionSet = Assert.Single(interchange.ProfessionalTransactionSets);
        Assert.Equal("101", transactionSet.GroupControlNumber);
        Assert.Equal(expectedSt02, transactionSet.TransactionSetControlNumber);
        Assert.Empty(interchange.InstitutionalTransactionSets);
        Assert.Empty(interchange.DentalTransactionSets);
    }

    [Fact]
    public void Parse_ReadsSingleInstitutionalTransactionSet_WithControlNumbers()
    {
        var interchange = ParseSample("InstitutionalClaim.txt");

        var transactionSet = Assert.Single(interchange.InstitutionalTransactionSets);
        Assert.Equal("101", transactionSet.GroupControlNumber);
        Assert.Equal("987654", transactionSet.TransactionSetControlNumber);
        Assert.Empty(interchange.ProfessionalTransactionSets);
        Assert.Empty(interchange.DentalTransactionSets);
    }

    [Fact]
    public void Parse_ReadsSingleDentalTransactionSet_WithControlNumbers()
    {
        var interchange = ParseSample("DentalClaim.txt");

        var transactionSet = Assert.Single(interchange.DentalTransactionSets);
        Assert.Equal("101", transactionSet.GroupControlNumber);
        Assert.Equal("3456", transactionSet.TransactionSetControlNumber);
        Assert.Empty(interchange.ProfessionalTransactionSets);
        Assert.Empty(interchange.InstitutionalTransactionSets);
    }

    // All bundled samples share one ISA envelope, so envelope parsing is asserted uniformly
    // across every variant — the result carries the same identity regardless of claim type.
    [Theory]
    [InlineData("837-sample-file.edi")]
    [InlineData("ClaimPayment.txt")]
    [InlineData("ClaimPaymentEVV.txt")]
    [InlineData("InstitutionalClaim.txt")]
    [InlineData("DentalClaim.txt")]
    public void Parse_PopulatesInterchangeEnvelope(string fileName)
    {
        var interchange = ParseSample(fileName);

        Assert.Equal("ZZ", interchange.SenderIdQualifier);
        Assert.Equal("1234567", interchange.SenderId);
        Assert.Equal("ZZ", interchange.ReceiverIdQualifier);
        Assert.Equal("11111", interchange.ReceiverId);
        Assert.Equal("170508", interchange.InterchangeDate);
        Assert.Equal("1141", interchange.InterchangeTime);
        Assert.Equal("00501", interchange.InterchangeControlVersionNumber);
        Assert.Equal("000000101", interchange.InterchangeControlNumber);
        Assert.Equal("P", interchange.UsageIndicator);
    }

    // Claim-tree navigation is 837P-specific; the professional sample carries two claims.
    [Fact]
    public void Parse_FindsAllClaims_FromProfessionalSample()
    {
        var interchange = ParseSample("837-sample-file.edi");

        var claimIds = interchange.ProfessionalTransactionSets
            .Select(t => t.Message)
            .SelectMany(t => t.Loop2000A ?? [])
            .SelectMany(a => a.Loop2000B ?? [])
            .SelectMany(b => b.Loop2300 ?? [])
            .Select(c => c.CLM_ClaimInformation?.PatientControlNumber_01)
            .ToList();

        Assert.Equal(["1000A", "1001A"], claimIds);
    }
}
