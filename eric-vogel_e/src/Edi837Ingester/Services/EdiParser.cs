using Edi837Ingester.Data.Entities;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using Microsoft.Extensions.Logging;

namespace Edi837Ingester.Services;

public class EdiParser(ILogger<EdiParser> logger)
{
    public Edi837Document Parse(string filePath)
    {
        logger.LogInformation("Parsing EDI 837 file: {FilePath}", filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"EDI file not found: {filePath}");
        }
        
        return new Edi837Document();
    }
}