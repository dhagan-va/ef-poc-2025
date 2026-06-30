using Edi837Ingestion.Parsing;
using EdiFabric.Templates.Hipaa5010;
using Xunit;

namespace test.Parsing;

/// <summary>
/// Drives <see cref="Edi837Parser"/> with every bundled 837 sample and asserts each parses
/// into a <see cref="ParsedInterchange"/> with the right envelope and transaction across all
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

    // Every bundled sample carries a single transaction; this pins each file to the 837
    // variant, EdiFabric message type, and ST02 control number the parser must produce.
    [Theory]
    [InlineData("837-sample-file.edi", Edi837Variant.Professional, typeof(TS837P), "1239")]
    [InlineData("ClaimPayment.txt", Edi837Variant.Professional, typeof(TS837P), "0021")]
    [InlineData("ClaimPaymentEVV.txt", Edi837Variant.Professional, typeof(TS837P), "0021")]
    [InlineData("InstitutionalClaim.txt", Edi837Variant.Institutional, typeof(TS837I), "987654")]
    [InlineData("DentalClaim.txt", Edi837Variant.Dental, typeof(TS837D), "3456")]
    public void Parse_ReadsSingleTransaction_WithVariantAndControlNumbers(
        string fileName, Edi837Variant expectedVariant, Type expectedMessageType, string expectedSt02)
    {
        var interchange = ParseSample(fileName);

        var transaction = Assert.Single(interchange.Transactions);
        Assert.Equal(expectedVariant, transaction.Variant);
        Assert.IsType(expectedMessageType, transaction.Transaction);
        Assert.Equal("101", transaction.GroupControlNumber);
        Assert.Equal(expectedSt02, transaction.TransactionSetControlNumber);
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

        var claimIds = interchange.Transactions
            .Select(t => (TS837P)t.Transaction)
            .SelectMany(t => t.Loop2000A ?? [])
            .SelectMany(a => a.Loop2000B ?? [])
            .SelectMany(b => b.Loop2300 ?? [])
            .Select(c => c.CLM_ClaimInformation?.PatientControlNumber_01)
            .ToList();

        Assert.Equal(["1000A", "1001A"], claimIds);
    }
}
