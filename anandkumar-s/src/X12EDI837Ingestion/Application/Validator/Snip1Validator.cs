using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using X12EDI837Ingestion.Consumer.Application.Models;

namespace X12EDI837Ingestion.Consumer.Application.Validator;

public sealed class Snip1Validator : ISnipLevelValidator
{
    private readonly ILogger<Snip1Validator> _logger;

    public Snip1Validator(ILogger<Snip1Validator> logger)
    {
        _logger = logger;
    }

    public int SnipLevel => 1;

    

    private IEnumerable<SnipValidationError> MapErrors(List<SegmentErrorContext> isaErrors, string v)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<SnipValidationError>> ValidateAsync(
    ParsedEdiDocument document,
    CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);

        var errors = new List<SnipValidationError>();

        // ISA validation
        if (document.Isa is null)
        {
            errors.Add(CreateError(
                "MISSING_ISA",
                "ISA segment is missing.",
                "ISA"));
        }
        else
        {
            var isaErrors = document.Isa.Validate();
            errors.AddRange(MapErrors(isaErrors, "ISA"));
        }

        // GS validation
        if (document.Gs is null)
        {
            errors.Add(CreateError(
                "MISSING_GS",
                "GS segment is missing.",
                "GS"));
        }
        else
        {
            var gsErrors = document.Gs.Validate();
            errors.AddRange(MapErrors(gsErrors, "GS"));

            if (string.IsNullOrWhiteSpace(document.Gs.VersionAndRelease_8))
            {
                errors.Add(CreateError(
                    "MISSING_GS08",
                    "GS08 Version/Release/Industry Identifier Code is missing.",
                    "GS08"));
            }
            else if (!string.Equals(
                document.Gs.VersionAndRelease_8,
                "005010X222A1",
                StringComparison.Ordinal))
            {
                errors.Add(CreateError(
                    "INVALID_GS08",
                    "GS08 version is invalid for this X12 release.",
                    "GS08"));
            }
        }

        // ST validation
        if (document.St is null)
        {
            errors.Add(CreateError(
                "MISSING_ST",
                "ST segment is missing.",
                "ST"));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(document.St.TransactionSetIdentifierCode_01))
            {
                errors.Add(CreateError(
                    "MISSING_ST01",
                    "ST01 Transaction Set Identifier Code is missing.",
                    "ST01"));
            }

            if (string.IsNullOrWhiteSpace(document.St.TransactionSetControlNumber_02))
            {
                errors.Add(CreateError(
                    "MISSING_ST02",
                    "ST02 Transaction Set Control Number is missing.",
                    "ST02"));
            }
        }

        // SE validation
        int? seSegmentCount = null;

        if (document.Se is null)
        {
            errors.Add(CreateError(
                "MISSING_SE",
                "SE segment is missing.",
                "SE"));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(document.Se.NumberofIncludedSegments_01))
            {
                errors.Add(CreateError(
                    "MISSING_SE01",
                    "SE01 Number of Included Segments is missing.",
                    "SE01"));
            }
            else if (!int.TryParse(document.Se.NumberofIncludedSegments_01, out var parsedCount) || parsedCount <= 0)
            {
                errors.Add(CreateError(
                    "INVALID_SE01",
                    "SE01 Number of Included Segments is invalid.",
                    "SE01"));
            }
            else
            {
                seSegmentCount = parsedCount;
            }

            if (string.IsNullOrWhiteSpace(document.Se.TransactionSetControlNumber_02))
            {
                errors.Add(CreateError(
                    "MISSING_SE02",
                    "SE02 Transaction Set Control Number is missing.",
                    "SE02"));
            }
        }

        // ST/SE cross-checks
        if (document.St is not null && document.Se is not null)
        {
            if (!string.IsNullOrWhiteSpace(document.St.TransactionSetControlNumber_02) &&
                !string.IsNullOrWhiteSpace(document.Se.TransactionSetControlNumber_02) &&
                !string.Equals(
                    document.St.TransactionSetControlNumber_02,
                    document.Se.TransactionSetControlNumber_02,
                    StringComparison.Ordinal))
            {
                errors.Add(CreateError(
                    "ST_SE_MISMATCH",
                    "ST02 and SE02 control numbers do not match.",
                    "SE"));
            }

            
            #region "Commenting/Parking the segments count logic for now as it requires additional analysis"
            //if (seSegmentCount.HasValue &&
            //    !string.IsNullOrWhiteSpace(document.ActualSegmentCount) &&
            //    int.TryParse(document.ActualSegmentCount, out var actualSegmentCount) &&
            //    seSegmentCount.Value != actualSegmentCount)
            //{
            //    errors.Add(CreateError(
            //        "SE01_COUNT_MISMATCH",
            //        "SE01 does not match the actual number of segments in the transaction set.",
            //        "SE01"));
            //}
            #endregion
        }

        // Reader errors
        foreach (var readerError in document.ReaderErrors)
        {
            cancellationToken.ThrowIfCancellationRequested();

            errors.Add(CreateError(
                "READER_ERROR",
                readerError.Message ?? "Unknown reader error."));
        }

        return Task.FromResult<IReadOnlyList<SnipValidationError>>(errors);
    }

    private static SnipValidationError CreateError(
        string errorCode,
        string message,
        string? segmentId = null)
    {
        return new SnipValidationError
        {
            SnipLevel = 1,
            ErrorCode = errorCode,
            Message = message,
            SegmentId = segmentId
        };
    }
}