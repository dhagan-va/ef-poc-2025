using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services;

    public class EdiParser(IEdiSaverService saverService, ILogger<EdiParser> logger) : IEdiParser
    {
        public async Task Parse(string filePath)
        {
            // Ensure extended encodings are supported (required for .NET Core/5+)
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found at {filePath}");
            }

            using var ediStream = File.OpenRead(filePath);

            var professionalClaims = new List<TS837P>();
            var institutionalClaims = new List<TS837I>();
            var dentalClaims = new List<TS837D>();

            // Point the reader to the assembly containing the 5010 templates (P, I, D)
            using var reader = new X12Reader(ediStream, _ => typeof(TS837P).Assembly);

            var ediItems = (await reader.ReadToEndAsync()).ToList();
            professionalClaims.AddRange(ediItems.OfType<TS837P>());
            institutionalClaims.AddRange(ediItems.OfType<TS837I>());
            dentalClaims.AddRange(ediItems.OfType<TS837D>());

            // Process the identified types
            await saverService.Save(professionalClaims);
            await saverService.Save(institutionalClaims);
            await saverService.Save(dentalClaims);
        }
    }