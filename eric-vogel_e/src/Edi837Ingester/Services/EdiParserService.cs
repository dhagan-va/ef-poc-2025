using Edi837Ingester.Data;
using Edi837Ingester.Data.Repositories;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services;

public class EdiParserService(IEdiRepository ediRepository,
    ILogger<EdiParserService> logger) : IEdiParserService
{
    public async Task Parse(Stream stream)
    {
        // Point the reader to the assembly containing the 5010 templates (P, I, D)
        using var reader = new X12Reader(stream, _ => typeof(TS837P).Assembly);

        var ediItems = (await reader.ReadToEndAsync()).ToList();

        if (!ediItems.Any())
        {
            logger.LogWarning("No EDI transactions found in file");
            return;
        }

        var professionalItems = ediItems.OfType<TS837P>();
        var institutionalItems = ediItems.OfType<TS837I>();
        var dentalItems = ediItems.OfType<TS837D>();

        var claimType = professionalItems.Any() ? ClaimTypeEnum.Professional :
            institutionalItems.Any() ? ClaimTypeEnum.Institutional :
            dentalItems.Any() ? ClaimTypeEnum.Dental : ClaimTypeEnum.Unknown;

        switch(claimType)
        {
            case ClaimTypeEnum.Professional:
                await ParseItems(professionalItems, claimType);
                break;
            case ClaimTypeEnum.Institutional:
                await ParseItems(institutionalItems, claimType);
                break;
            case ClaimTypeEnum.Dental:
                await ParseItems(dentalItems, claimType);
                break;
            default:
                logger.LogWarning("No recognized 837 claim transactions found in file");
                return;
        }
    }

    private async Task ParseItems<T>(IEnumerable<T> items, ClaimTypeEnum claimType)
        where T : EdiMessage
    {
        var invalidItems = await ValidateItems(items, claimType);
        LogCount(items, claimType);
        var claims = new List<T>();
        claims.AddRange(items.Except(invalidItems));
        await ediRepository.Save(claims);
    }

    public async Task Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found at {filePath}");
        }

        using var stream = File.OpenRead(filePath);
        await Parse(stream);
    }

    private void LogCount(IEnumerable<EdiMessage> items, ClaimTypeEnum claimType)
    {
        logger.LogInformation("Found {Count} {ClaimType} claims", items.Count(), claimType);
    }

    // ValidateItems now returns the subset of items that have validation errors.
    // Returns distinct items that either have ErrorContext errors or fail IsValidAsync().
    private async Task<IEnumerable<T>> ValidateItems<T>(IEnumerable<T> items, ClaimTypeEnum claimType) where T : EdiMessage
    {
        var erroredItems = new List<T>();

        // SNIP Level 1 validation (pre-parsing errors surfaced by the reader)
        var level1Errors = items.Where(x => x.ErrorContext != null && x.ErrorContext.HasErrors).ToList();
        foreach (var error in level1Errors)
        {
            erroredItems.Add(error);
            logger.LogError("Error parsing {ClaimType} claim: {Errors}", claimType, string.Join(", ",
                error.ErrorContext.Errors.Select(e => e.Message)));
        }

        // Perform higher-level validation per item (IsValidAsync). Avoid re-checking items already captured.
        var toValidate = items.Except(erroredItems).ToList();
        foreach (var item in toValidate)
        {
            try
            {
                var (valid, errorContext) = await item.IsValidAsync();
                if (!valid)
                {
                    erroredItems.Add(item);
                    var controlNumber = errorContext?.ControlNumber ?? "<unknown>";
                    var messages = errorContext?.Errors?.Select(e => e.Name) ?? Enumerable.Empty<string>();
                    logger.LogError("Error parsing transaction with control #: {ControlNumber}: {Errors}", controlNumber, string.Join(", ", messages));
                }
            }
            catch (Exception ex)
            {
                // If IsValidAsync throws, treat as validation error for this item and log exception details.
                erroredItems.Add(item);
                logger.LogError(ex, "Validation routine threw for {ClaimType} transaction. Item will be considered invalid.", claimType);
            }
        }

        // Return distinct errored items (reference equality is fine for EdiMessage instances)
        return [.. erroredItems.Distinct()];
    }
}