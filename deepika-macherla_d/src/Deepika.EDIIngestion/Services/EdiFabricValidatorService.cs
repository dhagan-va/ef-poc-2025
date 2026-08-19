using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace Deepika.EDIIngestion.Services
{
    // Lightweight EdiFabric integration used for validation when a serial key is present.
    // Validates transactions and reports SNIP/parse errors.
    public class EdiFabricValidatorService : IEdiFabricValidatorService
    {
        public bool IsLicensed { get; private set; }
        private readonly HashSet<int> _requiredSnipLevels = new();

        public IEnumerable<int> RequiredSnipLevels => _requiredSnipLevels;

        public EdiFabricValidatorService()
        {
            // Assume license is set at startup (Program.Main) for demo simplicity.
            var license = Environment.GetEnvironmentVariable("TRIAL_EDIFABRIC_LICENSE")
                          ?? Environment.GetEnvironmentVariable("EDIFABRIC_LICENSE");

            // Try to validate the provided EdiFabric serial key to ensure it's not expired/invalid.
            if (!string.IsNullOrWhiteSpace(license))
            {
                try
                {
                    // Attempt to set the serial key; EdiFabric will throw if the token is invalid/expired.
                    EdiFabric.SerialKey.Set(license);
                    IsLicensed = true;
                }
                catch (Exception)
                {
                    // Invalid or expired token: mark as not licensed so we skip runtime validation.
                    IsLicensed = false;
                }
            }
            else
            {
                IsLicensed = false;
            }

            // Parse requested SNIP levels from environment variable EDIFABRIC_SNIP_LEVELS (e.g. "1,2")
            var snipEnv = Environment.GetEnvironmentVariable("EDIFABRIC_SNIP_LEVELS");
            if (!string.IsNullOrWhiteSpace(snipEnv))
            {
                var parts = snipEnv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var p in parts)
                {
                    if (int.TryParse(p, out var lvl))
                    {
                        _requiredSnipLevels.Add(lvl);
                    }
                }
            }
        }

        /// <summary>
        /// Validates an X12 EDI file using EdiFabric reader and returns whether parsing found no errors.
        /// When licensed, this will populate the error messages to the provided list.
        /// </summary>
        public bool ValidateFile(string path, out List<string> errors)
        {
            // License is assumed to be initialized at startup (Program.Main) for this demo.
            errors = new List<string>();

            if (!IsLicensed)
            {
                errors.Add("EdiFabric license not present or invalid/expired; skipping EdiFabric validation.");
                return false;
            }

            if (_requiredSnipLevels.Any())
            {
                errors.Add("Requested SNIP levels: " + string.Join(',', _requiredSnipLevels));
            }

            if (!File.Exists(path))
            {
                errors.Add("File not found: " + path);
                return false;
            }

            try
            {
                using var ediStream = File.OpenRead(path);
                using var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa");

                var ediItems = ediReader.ReadToEnd().ToList();
                var transactions = ediItems.OfType<TS837P>();

                var hasErrors = false;

                // Determine ValidationSettings based on requested SNIP levels (cumulative).
                ValidationSettings validationSettings = null;
                if (_requiredSnipLevels.Any())
                {
                    var maxLevel = _requiredSnipLevels.Max();
                    var lvl = maxLevel switch
                    {
                        1 => ValidationLevel.SyntaxOnly_SNIP1,
                        2 => ValidationLevel.LimitsAndCodes_SNIP2,
                        3 => ValidationLevel.Balancing_SNIP3,
                        _ => ValidationLevel.InterSegment_SNIP4,
                    };
                    validationSettings = new ValidationSettings { ValidationLevel = lvl };
                    errors.Add("Applying SNIP validation level: " + lvl);
                }

                foreach (var tx in transactions)
                {
                    if (validationSettings != null)
                    {
                        var isValid = tx.IsValid(out var errorContext, validationSettings);
                        if (!isValid)
                        {
                            hasErrors = true;
                            if (errorContext != null)
                            {
                                foreach (var e in errorContext.Flatten()) errors.Add(e);
                            }
                        }
                    }
                    else
                    {
                        // Fallback: use existing HasErrors behavior if no SNIP levels requested
                        if (tx.HasErrors)
                        {
                            hasErrors = true;
                            var errs = tx.ErrorContext.Flatten();
                            foreach (var e in errs)
                            {
                                errors.Add(e);
                            }
                        }
                    }
                }

                if (hasErrors && _requiredSnipLevels.Any())
                {
                    errors.Add($"Validation failed for requested SNIP levels: {string.Join(',', _requiredSnipLevels)}");
                }

                return !hasErrors;
            }
            catch (Exception ex)
            {
                errors.Add("EdiFabric validation exception: " + ex.Message);
                return false;
            }
        }

        // No lazy initialization required — license is set at startup in Program.Main for this demo.
    }
}
