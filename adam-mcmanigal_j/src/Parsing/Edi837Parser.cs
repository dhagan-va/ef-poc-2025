using System.Security.Cryptography;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingestion.Parsing;

/// <summary>
/// Parses raw X12 EDI 837 (health care claim) streams into a single <see cref="ParsedInterchange"/>:
/// the interchange envelope (sender/receiver, control number, usage indicator, …) plus every 837
/// transaction it contains — professional (837P), institutional (837I), and dental (837D) — as
/// EdiFabric's strongly-typed messages, using the HIPAA 5010 templates.
/// </summary>
/// <remarks>
/// The EdiFabric license must already be applied (via <c>EdiFabric.SerialKey.Set(...)</c>)
/// before calling <see cref="Parse"/>; that is an application-startup concern, kept out of
/// this class so it stays a pure parser. One interchange per stream is assumed (one file =
/// one ledger row).
/// </remarks>
public sealed class Edi837Parser
{
    // Keep reading after a recoverable error so we collect as many transactions as possible.
    private static readonly X12ReaderSettings ReaderSettings = new()
    {
        ContinueOnError = true,
    };

    /// <summary>
    /// Reads the interchange from the given EDI stream into a <see cref="ParsedInterchange"/>,
    /// pairing each 837 transaction with its group (GS06) and transaction-set (ST02) control
    /// numbers. The stream is read to the end in a single pass; the caller owns its lifetime.
    /// </summary>
    public ParsedInterchange Parse(Stream ediStream)
    {
        ArgumentNullException.ThrowIfNull(ediStream);

        var (buffer, contentHash) = CreateContentHash(ediStream);

        using var reader = new X12Reader(buffer, "EdiFabric.Templates.Hipaa", ReaderSettings);

        ISA? envelope = null;
        var groupControlNumber = string.Empty;
        var transactions = new List<Edi837Transaction>();

        // ReadToEnd yields the envelope items (ISA, GS, ...) interleaved with the transactions
        // in document order, so we keep the most recent ISA envelope and group control number
        // and attach them to each 837 transaction as we encounter it.
        foreach (var item in reader.ReadToEnd())
        {
            switch (item)
            {
                case ISA isa:
                    envelope = isa;
                    break;
                case GS gs:
                    groupControlNumber = gs.GroupControlNumber_6?.Trim() ?? string.Empty;
                    break;
                case TS837P p:
                    transactions.Add(ToTransaction(Edi837Variant.Professional, p.ST, p, groupControlNumber));
                    break;
                case TS837I i:
                    transactions.Add(ToTransaction(Edi837Variant.Institutional, i.ST, i, groupControlNumber));
                    break;
                case TS837D d:
                    transactions.Add(ToTransaction(Edi837Variant.Dental, d.ST, d, groupControlNumber));
                    break;
            }
        }

        return ToInterchange(contentHash, envelope, transactions);
    }

    // All three 837 variants expose the same X12 ST segment, so reading ST02 is shared here.
    private static Edi837Transaction ToTransaction(
        Edi837Variant variant, ST? st, EdiMessage message, string groupControlNumber) =>
        new(variant,
            groupControlNumber,
            st?.TransactionSetControlNumber_02?.Trim() ?? string.Empty,
            message);

    private static ParsedInterchange ToInterchange(
        string contentHash, ISA? isa, IReadOnlyList<Edi837Transaction> transactions) =>
        new()
        {
            ContentHash = contentHash,
            SenderIdQualifier = isa?.SenderIDQualifier_5?.Trim() ?? string.Empty,
            SenderId = isa?.InterchangeSenderID_6?.Trim() ?? string.Empty,
            ReceiverIdQualifier = isa?.ReceiverIDQualifier_7?.Trim() ?? string.Empty,
            ReceiverId = isa?.InterchangeReceiverID_8?.Trim() ?? string.Empty,
            InterchangeDate = isa?.InterchangeDate_9?.Trim() ?? string.Empty,
            InterchangeTime = isa?.InterchangeTime_10?.Trim() ?? string.Empty,
            InterchangeControlVersionNumber = isa?.InterchangeControlVersionNumber_12?.Trim() ?? string.Empty,
            InterchangeControlNumber = isa?.InterchangeControlNumber_13?.Trim() ?? string.Empty,
            UsageIndicator = isa?.UsageIndicator_15?.Trim() ?? string.Empty,
            Transactions = transactions,
        };

    private static (MemoryStream Buffer, string ContentHash) CreateContentHash(Stream ediStream)
    {
        var buffer = new MemoryStream();
        ediStream.CopyTo(buffer);
        var contentHash = Convert.ToHexString(SHA256.HashData(buffer.ToArray())).ToLowerInvariant();
        buffer.Position = 0;

        return (buffer, contentHash);
    }
}
