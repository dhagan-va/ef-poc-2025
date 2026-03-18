using Microsoft.Extensions.Options;
using X12EDI837Ingestion.Consumer.Configuration;
namespace X12EDI837Ingestion.Consumer.Validators;

public sealed class S3OptionsValidator : IValidateOptions<S3Information>
{
    public ValidateOptionsResult Validate(string? name, S3Information options)
    {


        var errors = new List<string>();

        if (options is null)
        {
            return ValidateOptionsResult.Fail("S3Options configuration is missing.");
        }

        if (string.IsNullOrWhiteSpace(options.Url))
            errors.Add("S3Information:Url is required.");

        if (string.IsNullOrWhiteSpace(options.Bucket))
            errors.Add("S3Information:Bucket is required.");

        if (string.IsNullOrWhiteSpace(options.AccessKey))
            errors.Add("S3Information:AccessKey is required.");

        if (string.IsNullOrWhiteSpace(options.SecretKey))
            errors.Add("S3Information:SecretKey is required.");

        if (string.IsNullOrWhiteSpace(options.Region))
            errors.Add("S3Information:Region is required.");

        if (!string.IsNullOrWhiteSpace(options.Url) &&
            !Uri.TryCreate(options.Url, UriKind.Absolute, out _))
        {
            errors.Add("S3Information:Url must be a valid absolute URI.");
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}