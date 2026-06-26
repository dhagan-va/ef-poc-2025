using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace Edi837Ingestion.Parsing;

/// <summary>
/// Parses raw X12 EDI 837 (professional claim) streams into EdiFabric's strongly-typed
/// <see cref="TS837P"/> transaction objects, using the HIPAA 5010 templates.
/// </summary>
/// <remarks>
/// The EdiFabric license must already be applied (via <c>EdiFabric.SerialKey.Set(...)</c>)
/// before calling <see cref="Parse"/>; that is an application-startup concern, kept out of
/// this class so it stays a pure parser.
/// </remarks>
public sealed class Edi837Parser
{
    // Keep reading after a recoverable error so we collect as many transactions as possible.
    private static readonly X12ReaderSettings ReaderSettings = new()
    {
        ContinueOnError = true,
    };

    /// <summary>
    /// Reads every 837P transaction from the given EDI stream. The stream is read to the
    /// end in a single pass; the caller owns the stream's lifetime.
    /// </summary>
    public IReadOnlyList<TS837P> Parse(Stream ediStream)
    {
        ArgumentNullException.ThrowIfNull(ediStream);

        using var reader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa", ReaderSettings);

        return reader.ReadToEnd().OfType<TS837P>().ToList();
    }
}
