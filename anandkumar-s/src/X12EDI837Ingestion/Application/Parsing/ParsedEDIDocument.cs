using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;

namespace X12EDI837Ingestion.Consumer.Application.Models;

public sealed class ParsedEdiDocument
{
    public string SourceName { get; set; } = string.Empty;

    public ISA? Isa { get; set; }
    public GS? Gs { get; set; }
    public GE? Ge { get; set; }
    public IEA? Iea { get; set; }

    public ST? St { get; set; }
    public SE? Se { get; set; }

    public List<TS837P> TransactionSets { get; } = new();
    public List<ReaderErrorContext> ReaderErrors { get; } = new();

    public int ActualSegmentCount { get; set; }

    public int ActualTransactionSetCount => TransactionSets.Count;
}