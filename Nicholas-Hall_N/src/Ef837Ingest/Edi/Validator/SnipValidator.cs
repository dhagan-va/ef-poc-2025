using System;
using System.Threading;
using System.Threading.Tasks;
using EdiFabric.Core.Model.Edi;                  // EdiMessage, ValidationSettings, ValidationLevel
using EdiFabric.Core.Model.Edi.ErrorContexts;   // MessageErrorContext
using EdiFabric.Templates.Hipaa5010;            // TS837P

namespace Ef837Ingest.Edi.Validator
{
    public enum SnipLevel
    {
        Snip1 = 1,   // Syntax only
        Snip2 = 2,   // Limits & codes
        Snip3 = 3,   // Balancing
        Snip4 = 4    // Inter-segment
    }

    public sealed class SnipOptions
    {
        public SnipLevel Level { get; init; } = SnipLevel.Snip4;
        /// <summary>If true, ValidateAndThrow will throw on any SNIP error.</summary>
        public bool FailOnError { get; init; } = true;
        /// <summary>If true, throws ArgumentNullException when message is null.</summary>
        public bool ThrowOnNullMessage { get; init; } = true;
    }

    public sealed class SnipResult
    {
        public bool IsValid { get; init; }
        public MessageErrorContext? ErrorContext { get; init; }
        public SnipLevel Level { get; init; }
    }

    public static class SnipValidator
    {
        // -------- Public API (TS837P) --------

        public static SnipResult Validate(TS837P message, SnipOptions? options = null)
        {
            options ??= new SnipOptions();

            if (message is null)
            {
                if (options.ThrowOnNullMessage) throw new ArgumentNullException(nameof(message));
                return new SnipResult { IsValid = false, ErrorContext = null, Level = options.Level };
            }

            var settings = new ValidationSettings
            {
                ValidationLevel = MapLevel(options.Level)
            };

            var ok = message.IsValid(out MessageErrorContext ctx, settings);

            return new SnipResult
            {
                IsValid = ok,
                ErrorContext = ctx,
                Level = options.Level
            };
        }

        public static async Task<SnipResult> ValidateAsync(
            TS837P message,
            SnipOptions? options = null,
            CancellationToken ct = default)
        {
            options ??= new SnipOptions();

            if (message is null)
            {
                if (options.ThrowOnNullMessage) throw new ArgumentNullException(nameof(message));
                return new SnipResult { IsValid = false, ErrorContext = null, Level = options.Level };
            }

            var settings = new ValidationSettings
            {
                ValidationLevel = MapLevel(options.Level)
            };

            var (ok, ctx) = await message.IsValidAsync(ct, settings);

            return new SnipResult
            {
                IsValid = ok,
                ErrorContext = ctx,
                Level = options.Level
            };
        }

        /// <summary>
        /// Validates and throws InvalidOperationException on failure (when FailOnError = true).
        /// Returns the SnipResult regardless, so caller can still access ErrorContext.
        /// </summary>
        public static SnipResult ValidateAndThrow(TS837P message, SnipOptions? options = null)
        {
            var result = Validate(message, options);
            if (!result.IsValid && (options?.FailOnError ?? true))
            {
                throw new InvalidOperationException($"SNIP validation failed (Level={result.Level}).");
            }
            return result;
        }

        public static async Task<SnipResult> ValidateAndThrowAsync(
            TS837P message,
            SnipOptions? options = null,
            CancellationToken ct = default)
        {
            var result = await ValidateAsync(message, options, ct);
            if (!result.IsValid && (options?.FailOnError ?? true))
            {
                throw new InvalidOperationException($"SNIP validation failed (Level={result.Level}).");
            }
            return result;
        }

        // -------- Internals --------

        private static ValidationLevel MapLevel(SnipLevel level) => level switch
        {
            SnipLevel.Snip1 => ValidationLevel.SyntaxOnly_SNIP1,
            SnipLevel.Snip2 => ValidationLevel.LimitsAndCodes_SNIP2,
            SnipLevel.Snip3 => ValidationLevel.Balancing_SNIP3,
            _ => ValidationLevel.InterSegment_SNIP4
        };
    }
}
