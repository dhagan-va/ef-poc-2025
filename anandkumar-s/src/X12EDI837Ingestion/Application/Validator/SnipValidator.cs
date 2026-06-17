using X12EDI837Ingestion.Consumer.Application.Interfaces;
using X12EDI837Ingestion.Consumer.Application.Models;
using X12EDI837Ingestion.Consumer.Application.Services;
using X12EDI837Ingestion.Consumer.Application.Validator;

public sealed class SnipValidator : ISnipValidator
{
    private readonly IEdiX12DocumentReader _ediDocumentReader;
    private readonly IReadOnlyList<ISnipLevelValidator> _snipValidators;

    public SnipValidator(
        IEdiX12DocumentReader ediDocumentReader,
        IEnumerable<ISnipLevelValidator> snipValidators)
    {
        _ediDocumentReader = ediDocumentReader ?? throw new ArgumentNullException(nameof(ediDocumentReader));

        _snipValidators = (snipValidators ?? throw new ArgumentNullException(nameof(snipValidators)))
            .OrderBy(v => v.SnipLevel)
            .ToList();
    }

    public async Task<SnipValidationResult> ValidateAsync(
        Stream stream,
        string sourceName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceName);

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        var document = await _ediDocumentReader.ReadAsync(
            stream,
            sourceName,
            cancellationToken);

        var result = new SnipValidationResult
        {
            SourceName = sourceName,
            InterchangeControlNumber = document.Isa?.InterchangeControlNumber_13,
            GroupControlNumber = document.Gs?.GroupControlNumber_6,
            TransactionSetControlNumber = document.TransactionSets
                .FirstOrDefault()?.ST?.TransactionSetControlNumber_02
        };

        foreach (var validator in _snipValidators)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var errors = await validator.ValidateAsync(document, cancellationToken);

            if (errors is not null && errors.Any())
            {
                result.Errors.AddRange(errors);
            }
        }

        return result;
    }
}
