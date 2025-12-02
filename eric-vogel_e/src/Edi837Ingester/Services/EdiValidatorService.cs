using Edi837Ingester.Data;
using Edi837Ingester.Data.Entities;
using Edi837Ingester.Data.Repositories;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services
{
    public class EdiValidatorService(IEdiRepository ediRepository, ILogger<EdiValidatorService> logger) : IEdiValidatorService
    {
        // ValidateItems now returns the subset of items that have validation errors.
        // Returns distinct items that either have ErrorContext errors or fail IsValidAsync().
        public async Task<IEnumerable<T>> ValidateItems<T>(IEnumerable<T> items, ClaimTypeEnum claimType,
            ValidationLevel validationLevel = ValidationLevel.SyntaxOnly_SNIP1) where T : EdiMessage
        {
            var erroredItems = new List<T>();

            // check if claims have already been processed
            var processedItems = await ediRepository.GetProcessedClaims(claimType) ?? Enumerable.Empty<ProcessedClaim>();

            var duplicateItems = new List<T>();

            switch (claimType)
            {
                case ClaimTypeEnum.Professional:
                    duplicateItems.AddRange(items.Cast<TS837P>()
                        .Where(i => processedItems.Any(p => p.ClaimControlNumber == i.ST.TransactionSetControlNumber_02))
                        .Cast<T>());
                    break;
                case ClaimTypeEnum.Institutional:
                    duplicateItems.AddRange(items.Cast<TS837I>()
                        .Where(i => processedItems.Any(p => p.ClaimControlNumber == i.ST.TransactionSetControlNumber_02))
                        .Cast<T>());
                    break;
                case ClaimTypeEnum.Dental:
                    duplicateItems.AddRange(items.Cast<TS837D>()
                        .Where(i => processedItems.Any(p => p.ClaimControlNumber == i.ST.TransactionSetControlNumber_02))
                        .Cast<T>());
                    break;
            }

            if (duplicateItems.Any())
            {
                logger.LogWarning("Excluding {Count} duplicate {ClaimType} claims that have already been processed",
                                    duplicateItems.Count, claimType);

                erroredItems.AddRange(duplicateItems);
            }

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
                    var (valid, errorContext) = await item.IsValidAsync(new ValidationSettings() { ValidationLevel = validationLevel });
                    if (!valid)
                    {
                        erroredItems.Add(item);
                        var controlNumber = errorContext?.ControlNumber ?? "<unknown>";
                        var messages = errorContext?.Errors?.Select(e => e.Name + " " + string.Join(", ", e.Errors.Select(err => err.Message))) ?? Enumerable.Empty<string>();
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
}
