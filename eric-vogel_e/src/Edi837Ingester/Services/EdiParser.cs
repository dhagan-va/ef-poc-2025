using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services;

public class EdiParser(IEdiSaverService saverService, ILogger<EdiParser> logger) : IEdiParser
{
    public async Task Parse(string filePath)
    {
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
        var professionalItems = ediItems.OfType<TS837P>();
        var institutionalItems = ediItems.OfType<TS837I>();
        var dentalItems = ediItems.OfType<TS837D>();

        if (professionalItems.Any())
        {
            logger.LogInformation("Found {Count} Professional claims", professionalItems.Count());
            professionalClaims.AddRange(professionalItems);
            await saverService.Save(professionalClaims);
        }

        if (institutionalItems.Any())
        {
            logger.LogInformation("Found {Count} Institutional claims", institutionalItems.Count());
            institutionalClaims.AddRange(institutionalItems);
            await saverService.Save(institutionalClaims);
        }

        if (dentalItems.Any())
        {
            logger.LogInformation("Found {Count} Dental claims", dentalItems.Count());
            dentalClaims.AddRange(dentalItems);
            await saverService.Save(dentalClaims);
        }
    }
}