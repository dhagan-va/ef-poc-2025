using System;
using System.Collections.Generic;
using System.Linq;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using Microsoft.Extensions.Configuration;

namespace EDI.Services
{
    public class EdiValidationService : IEdiValidationService
    {
        private readonly ValidationSettings _validationSettings;

        public EdiValidationService(IConfiguration configuration)
        {
            _validationSettings = BuildValidationSettings(configuration);
        }

        public void Validate(IEdiItem transaction)
        {
            if (transaction is not EdiMessage message)
            {
                return;
            }

            MessageErrorContext errorContext;
            if (!message.IsValid(out errorContext, _validationSettings))
            {
                var details = errorContext?
                    .Flatten()?
                    .Where(message => !string.IsNullOrWhiteSpace(message))
                    .ToList() ?? new List<string>();

                var errorMessage = details.Count > 0
                    ? string.Join("; ", details)
                    : "Unknown validation error.";

                throw new InvalidOperationException($"SNIP validation (level {_validationSettings.ValidationLevel}) failed: {errorMessage}");
            }
        }

        private static ValidationSettings BuildValidationSettings(IConfiguration configuration)
        {
            var configuredLevel =
                configuration?["Edi:SnipLevel"] ??
                Environment.GetEnvironmentVariable("EDI_SNIP_LEVEL");

            var validationLevel = ParseValidationLevel(configuredLevel);

            return new ValidationSettings
            {
                ValidationLevel = validationLevel
            };
        }

        private static ValidationLevel ParseValidationLevel(string? configuredLevel)
        {
            if (string.IsNullOrWhiteSpace(configuredLevel))
            {
                return ValidationLevel.InterSegment_SNIP4;
            }

            if (int.TryParse(configuredLevel, out var numericLevel))
            {
                return numericLevel switch
                {
                    <= 1 => ValidationLevel.SyntaxOnly_SNIP1,
                    2 => ValidationLevel.LimitsAndCodes_SNIP2,
                    3 => ValidationLevel.InterSegment_SNIP4,
                    4 => ValidationLevel.InterSegment_SNIP4,
                    _ => ValidationLevel.InterSegment_SNIP4
                };
            }

            if (Enum.TryParse<ValidationLevel>(configuredLevel, true, out var parsedLevel))
            {
                return parsedLevel;
            }

            return ValidationLevel.InterSegment_SNIP4;
        }
    }
}
