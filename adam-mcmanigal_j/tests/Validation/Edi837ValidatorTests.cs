using Edi837Ingestion.Parsing;
using Edi837Ingestion.Validation;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using Xunit;

namespace test.Validation;

/// <summary>
/// Covers the <see cref="Edi837Validator"/> short-circuit that needs no EdiFabric license: a
/// <see langword="null"/> level disables validation and <see cref="Edi837Validator.Validate"/> passes
/// without inspecting any message. Built from a hand-made <see cref="ParsedInterchange"/> so the test
/// does not parse (and therefore needs no license).
/// </summary>
public class Edi837ValidatorDisabledTests
{
    [Fact]
    public void Validate_ReturnsValid_WhenLevelIsNull()
    {
        var result = new Edi837Validator(level: null).Validate(EmptyInterchange());

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_Throws_WhenInterchangeIsNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Edi837Validator(level: null).Validate(null!));
    }

    private static ParsedInterchange EmptyInterchange() => new()
    {
        ContentHash = "hash",
        SenderIdQualifier = "ZZ",
        SenderId = "SENDER",
        ReceiverIdQualifier = "ZZ",
        ReceiverId = "RECEIVER",
        InterchangeDate = "170508",
        InterchangeTime = "1141",
        InterchangeControlVersionNumber = "00501",
        InterchangeControlNumber = "000000101",
        UsageIndicator = "P",
        ProfessionalTransactionSets = [],
        InstitutionalTransactionSets = [],
        DentalTransactionSets = [],
    };
}

/// <summary>
/// Drives <see cref="Edi837Validator"/> against real 837 samples at each SNIP level, asserting the
/// level boundary at which each sample starts failing. Needs the EdiFabric license (to parse and to
/// validate), so it opts into the shared <c>EdiFabric</c> collection like the parser tests.
/// </summary>
[Collection("EdiFabric")]
public class Edi837ValidatorSnipTests
{
    private static ParsedInterchange ParseSample(string fileName)
    {
        using var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "samples", fileName));
        return new Edi837Parser().Parse(stream);
    }

    // A fully conformant professional sample passes even the strictest level we enforce (SNIP 4).
    [Fact]
    public void Validate_ReturnsValid_ForConformantProfessionalSample_AtStrictestLevel()
    {
        var result = new Edi837Validator(ValidationLevel.InterSegment_SNIP4)
            .Validate(ParseSample("837-sample-file.edi"));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    // The institutional sample is syntactically well-formed (passes SNIP 1) but is missing mandatory
    // elements and carries invalid codes, so it fails at SNIP 2 (limits and codes).
    [Fact]
    public void Validate_ReturnsValid_ForInstitutionalSample_AtSnip1()
    {
        var result = new Edi837Validator(ValidationLevel.SyntaxOnly_SNIP1)
            .Validate(ParseSample("InstitutionalClaim.txt"));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ReturnsInvalid_ForInstitutionalSample_AtSnip2()
    {
        var result = new Edi837Validator(ValidationLevel.LimitsAndCodes_SNIP2)
            .Validate(ParseSample("InstitutionalClaim.txt"));

        Assert.False(result.IsValid);
        // The failure is described with the variant and its ST02 control number for the DLQ reason.
        var error = Assert.Single(result.Errors);
        Assert.Contains(nameof(TS837I), error);
        Assert.Contains("987654", error);
    }

    // The EVV sample balances neither claim total, so it passes SNIP 2 but fails at SNIP 3 (balancing) —
    // proving the configured level is honoured, not just "valid or not".
    [Fact]
    public void Validate_ReturnsValid_ForUnbalancedSample_AtSnip2()
    {
        var result = new Edi837Validator(ValidationLevel.LimitsAndCodes_SNIP2)
            .Validate(ParseSample("ClaimPaymentEVV.txt"));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ReturnsInvalid_ForUnbalancedSample_AtSnip3()
    {
        var result = new Edi837Validator(ValidationLevel.Balancing_SNIP3)
            .Validate(ParseSample("ClaimPaymentEVV.txt"));

        Assert.False(result.IsValid);
        Assert.Contains("0021", Assert.Single(result.Errors));
    }
}
