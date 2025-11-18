using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services;

public class EdiParser(ILogger<EdiParser> logger)
{
    public List<TS837P> Parse(string filePath)
    {
        logger.LogInformation("Parsing EDI 837 file: {FilePath}", filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"EDI file not found: {filePath}");
        }
        
        using var ediStream = File.OpenRead(filePath);
        using var ediReader = new X12Reader(ediStream, "EdiFabric.Templates.Hipaa");

        var ediItems = ediReader.ReadToEnd().ToList();

        var transactions = ediItems.OfType<TS837P>();
        var transactionsList = new List<TS837P>();

        foreach (var transaction in transactions)
        {
            if (transaction.HasErrors)
            {
                var errors = transaction.ErrorContext.Flatten();
                foreach (var err in errors)
                {
                    logger.LogError($"ERROR: {err}");
                }
            }
            else
            {
                transactionsList.Add(transaction);
            }
        }

        return transactionsList;
    }
}