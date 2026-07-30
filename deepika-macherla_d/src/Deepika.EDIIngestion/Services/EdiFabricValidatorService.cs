using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace Deepika.EDIIngestion.Services
{
    // Lightweight EdiFabric integration used for validation when a serial key is present.
    // Validates transactions and reports SNIP/parse errors.
    public class EdiFabricValidatorService : IEdiFabricValidatorService
    {
        public bool IsLicensed { get; private set; }

        public EdiFabricValidatorService()
        {
            // Assume license is set at startup (Program.Main) for demo simplicity.
            var license = Environment.GetEnvironmentVariable("TRIAL_EDIFABRIC_LICENSE")
                          ?? Environment.GetEnvironmentVariable("EDIFABRIC_LICENSE");
            IsLicensed = !string.IsNullOrWhiteSpace(license);
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
                errors.Add("EdiFabric license not present; skipping EdiFabric validation.");
                return false;
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
                foreach (var tx in transactions)
                {
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
