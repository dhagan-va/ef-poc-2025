using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using X12EDI837Ingestion.Consumer.Application.Models;

namespace X12EDI837Ingestion.Consumer.Application.Services;

public sealed class EdiDocumentReader : IEdiX12DocumentReader
{
    public Task<ParsedEdiDocument> ReadAsync(
        Stream stream,
        string sourceName,
        CancellationToken cancellationToken = default)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        if (string.IsNullOrWhiteSpace(sourceName))
            throw new ArgumentException("Source name is required.", nameof(sourceName));

        if (stream.CanSeek)
            stream.Position = 0;

        var document = new ParsedEdiDocument
        {
            SourceName = sourceName
        };

        using var reader = new X12Reader(stream, "EdiFabric.Templates.X12");

        while (reader.Read())
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (reader.Item)
            {
                case ISA isa:
                    document.Isa = isa;
                    break;

                case GS gs:
                    document.Gs = gs;
                    break;

                case TS837P ts:
                    document.TransactionSets.Add(ts);
                    break;

                case ReaderErrorContext readerError:
                    document.ReaderErrors.Add(readerError);
                    break;
               
            }
        }

        return Task.FromResult(document);
    }
}