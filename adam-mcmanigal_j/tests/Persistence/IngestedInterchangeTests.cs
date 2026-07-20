using Edi837Ingestion.Parsing;
using Edi837Ingestion.Persistence;
using Xunit;

namespace test.Persistence;

/// <summary>
/// Verifies <see cref="IngestedInterchange.From"/>: a parsed interchange projects to a ledger
/// record carrying its content hash (the dedup key), envelope identity, and a null payload
/// pointer (the S3 archive step is deferred). Requires the EdiFabric license (parsing the sample).
/// </summary>
[Collection("EdiFabric")]
public sealed class IngestedInterchangeTests
{
    private static string SamplePath =>
        Path.Combine(AppContext.BaseDirectory, "samples", "837-sample-file.edi");

    private static IngestedInterchange ParseAndProject(out string contentHash)
    {
        using var stream = File.OpenRead(SamplePath);
        var interchange = new Edi837Parser().Parse(stream);
        contentHash = interchange.ContentHash;

        return IngestedInterchange.From(interchange, receivedAtUtc: DateTime.UnixEpoch);
    }

    [Fact]
    public void From_PromotesEnvelopeIdentity_AndContentHash()
    {
        var interchange = ParseAndProject(out var expectedHash);

        Assert.Equal(expectedHash, interchange.ContentHash);
        Assert.Equal("1234567", interchange.SenderId);
        Assert.Equal("11111", interchange.ReceiverId);
        Assert.Equal("000000101", interchange.InterchangeControlNumber);
        Assert.Equal(DateTime.UnixEpoch, interchange.ReceivedAt);
    }

    [Fact]
    public void From_LeavesPayloadPointerNull_UntilArchived()
    {
        var interchange = ParseAndProject(out _);

        Assert.Null(interchange.PayloadS3Key);
    }

    [Fact]
    public void From_Throws_WhenNoTransactions()
    {
        var empty = new ParsedInterchange
        {
            ContentHash = "abc",
            SenderIdQualifier = "",
            SenderId = "",
            ReceiverIdQualifier = "",
            ReceiverId = "",
            InterchangeDate = "",
            InterchangeTime = "",
            InterchangeControlVersionNumber = "",
            InterchangeControlNumber = "",
            UsageIndicator = "",
            ProfessionalTransactionSets = [],
            InstitutionalTransactionSets = [],
            DentalTransactionSets = [],
        };

        Assert.Throws<ArgumentException>(() =>
            IngestedInterchange.From(empty, DateTime.UnixEpoch));
    }
}
