using Amazon.S3;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using EdiParser.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EdiParser.Services;

public interface IEdiValidatorService
{
    Task<List<T>> ValidateItems<T>(List<T> items,  ClaimTypeEnum claimType,
        ValidationLevel validationLevel = ValidationLevel.SyntaxOnly_SNIP1) where T : EdiMessage;
}


public class EdiValidatorService : IEdiValidatorService
{
    private readonly ILogger<Application> _logger;
    private readonly IConfiguration _configuration;
    

    public EdiValidatorService(ILogger<Application> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<List<T>> ValidateItems<T>(List<T> items, ClaimTypeEnum claimType,
        ValidationLevel validationLevel = ValidationLevel.SyntaxOnly_SNIP1) where T : EdiMessage
    {
            
        _logger.LogInformation($"Validating with {validationLevel}");
        var invalidItems = new List<T>();

        // SNIP Level 1 validation (pre-parsing errors surfaced by the reader)
        var level1Errors = items.Where(x => x.ErrorContext != null && x.ErrorContext.HasErrors).ToList();
        foreach (var error in level1Errors)
        {
            invalidItems.Add(error);
            _logger.LogError("Error parsing {ClaimType} claim: {Errors}", claimType, string.Join(", ",
                error.ErrorContext.Errors.Select(e => e.Message)));
        }
        _logger.LogInformation($"Passed SNIP1 validation...");

        // Perform higher-level validation 
        var toValidate = items.Except(invalidItems).ToList();
        // Continue only if validation level higher than SNIP1 is requested
        if (validationLevel != ValidationLevel.SyntaxOnly_SNIP1)
        {
            foreach (var item in toValidate)
            {
                try
                {
                    // Call IsValid() on each transaction for higher SNIP validation level
                    var (valid, msgError) = await item.IsValidAsync(new ValidationSettings()
                        { ValidationLevel = validationLevel });

                    if (!valid)
                    {
                        invalidItems.Add(item);
                        var controlNumber = msgError?.ControlNumber ?? "<unknown>";
                        var messages =
                            msgError?.Errors?.Select(e =>
                                e.Name + " " + string.Join(", ", e.Errors.Select(err => err.Message))) ??
                            Enumerable.Empty<string>();
                        _logger.LogError(
                            $"Error parsing transaction with control #: {controlNumber}: {string.Join(", ", messages)}");
                    }
                    else
                    {
                        {
                            _logger.LogInformation($"Passed all validation...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    invalidItems.Add(item);
                    _logger.LogError(
                        $"Validation caused exception for {claimType} transaction. Item will be considered invalid. {ex.Message}");
                }
            }
        }

        // Return only ones that passed validation
        var claims = new List<T>();
        claims.AddRange(items.Except(invalidItems));
        if (invalidItems.Any())
        {
            _logger.LogInformation($"Excluding {invalidItems.Count()} invalid {claimType} claims from save operation");
        }

        if (claims.Any())
        {
            return claims;
        }
        else
        {
            _logger.LogWarning($"No valid {claimType} claims to save after validation");
            throw new Exception("No claim passed validation");
        }
    }
}