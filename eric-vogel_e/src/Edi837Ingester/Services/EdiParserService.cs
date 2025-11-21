using Edi837Ingester.Data.Repositories;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services;

public class EdiParserService(IEdiRepository ediRepository, ILogger<EdiParserService> logger) : IEdiParserService
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

        // Separate by type
        if (!ediItems.Any())
        {
            logger.LogWarning("No EDI transactions found in file {FilePath}", filePath);
            return;
        }

        var professionalItems = ediItems.OfType<TS837P>();
        var institutionalItems = ediItems.OfType<TS837I>();
        var dentalItems = ediItems.OfType<TS837D>();

        var claimType = professionalItems.Any() ? "Professional" :
            institutionalItems.Any() ? "Institutional" :
            dentalItems.Any() ? "Dental" : "Unknown";

        switch(claimType)
        {
            case "Professional":
                ReportErrors(professionalItems, claimType);
                LogCount(professionalItems, claimType);
                professionalClaims.AddRange(professionalItems);
                await ediRepository.Save(professionalClaims);
                break;
            case "Institutional":
                ReportErrors(institutionalItems, claimType);
                LogCount(institutionalItems, claimType);
                institutionalClaims.AddRange(institutionalItems);
                await ediRepository.Save(institutionalClaims);
                break;
            case "Dental":
                ReportErrors(dentalItems, claimType);
                LogCount(dentalItems, claimType);
                dentalClaims.AddRange(dentalItems);
                await ediRepository.Save(dentalClaims);
                break;
            case "Unknown":
                logger.LogWarning("No recognized 837 claim transactions found in file {FilePath}", filePath);
                return;
        }
    }

    private void LogCount(IEnumerable<EdiMessage> items, string claimType)
    {
        logger.LogInformation("Found {Count} {ClaimType} claims", items.Count(), claimType);
    }

    private void ReportErrors(IEnumerable<EdiMessage> items, string claimType)
    {
        var errors = items.Where(x => x.ErrorContext != null && x.ErrorContext.HasErrors);
        foreach (var error in errors)
        {
            logger.LogError("Error parsing {ClaimType} claim: {Errors}", claimType, string.Join(", ",
                error.ErrorContext.Errors.Select(e => e.Message)));
        }
    }
}