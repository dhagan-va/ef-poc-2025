using Edi837Ingester.Data;
using Edi837Ingester.Data.Entities;
using Edi837Ingester.Data.Repositories;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.Logging;
using System.Xml.Serialization;

namespace Edi837Ingester.Services;

public class EdiParserService(IEdiRepository ediRepository, 
    ILogger<EdiParserService> logger, IEdiValidatorService ediValidatorService) : IEdiParserService
{
    /// <summary>
    /// Read and parse the EDI 837 file from the provided stream.
    /// </summary>
    /// <param name="stream">Stream containing the EDI 837 file data.</param>
    /// <param name="validationLevel">The SNIP level of validation to apply during parsing.</param>
    /// <returns></returns>
    public async Task Parse(Stream stream, ValidationLevel validationLevel)
    {
        // Point the reader to the assembly containing the 5010 templates (P, I, D)
        using var reader = new X12Reader(stream, _ => typeof(TS837P).Assembly);

        var ediItems = (await reader.ReadToEndAsync()).ToList();

        if (!ediItems.Any())
        {
            logger.LogWarning("No EDI transactions found in file");
            return;
        }

        var professionalItems = ediItems.OfType<TS837P>();
        var institutionalItems = ediItems.OfType<TS837I>();
        var dentalItems = ediItems.OfType<TS837D>();

        var claimType = professionalItems.Any() ? ClaimTypeEnum.Professional :
            institutionalItems.Any() ? ClaimTypeEnum.Institutional :
            dentalItems.Any() ? ClaimTypeEnum.Dental : ClaimTypeEnum.Unknown;

        switch(claimType)
        {
            case ClaimTypeEnum.Professional:
                await ParseItems(professionalItems, claimType, validationLevel);
                break;
            case ClaimTypeEnum.Institutional:
                await ParseItems(institutionalItems, claimType, validationLevel);
                break;
            case ClaimTypeEnum.Dental:
                await ParseItems(dentalItems, claimType, validationLevel);
                break;
            default:
                logger.LogWarning("No recognized 837 claim transactions found in file");
                return;
        }
    }

    /// <summary>
    /// Parse and save the provided EDI items after validation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">Transactions</param>
    /// <param name="claimType">Claim type - Professional, Institutional, or Dental</param>
    /// <param name="validationLevel"></param>
    /// <returns></returns>
    private async Task ParseItems<T>(IEnumerable<T> items, ClaimTypeEnum claimType,
        ValidationLevel validationLevel = ValidationLevel.SyntaxOnly_SNIP1)
        where T : EdiMessage
    {
        var invalidItems = await ediValidatorService.ValidateItems(items, claimType, validationLevel);
        LogCount(items, claimType);
        var claims = new List<T>();
        claims.AddRange(items.Except(invalidItems));
        if (invalidItems.Any())
        {
            logger.LogInformation("Excluding {Count} invalid {ClaimType} claims from save operation",
                invalidItems.Count(), claimType);
        }

        if (claims.Any())
        {
            logger.LogInformation("Saving {Count} valid {ClaimType} claims", claims.Count, claimType);
            await ediRepository.SaveClaims(claims);
            var processedClaims = new List<ProcessedClaim>();
            switch(claimType)
            {
                case ClaimTypeEnum.Professional:
                    processedClaims.AddRange(claims.Cast<TS837P>().Select(c => new ProcessedClaim
                    {
                        ClaimControlNumber = c.ST.TransactionSetControlNumber_02,
                        ClaimXml = SerializeEdiMessageToXml(c),
                        ClaimTypeId = (int)ClaimTypeEnum.Professional,
                        ProcessedOn = DateTime.UtcNow
                    }));
                    break;
                case ClaimTypeEnum.Institutional:
                    processedClaims.AddRange(claims.Cast<TS837I>().Select(c => new ProcessedClaim
                    {
                        ClaimControlNumber = c.ST.TransactionSetControlNumber_02,
                        ClaimXml = SerializeEdiMessageToXml(c),
                        ClaimTypeId = (int)ClaimTypeEnum.Institutional,
                        ProcessedOn = DateTime.UtcNow
                    }));
                    break;
                case ClaimTypeEnum.Dental:
                    processedClaims.AddRange(claims.Cast<TS837D>().Select(c => new ProcessedClaim
                    {
                        ClaimControlNumber = c.ST.TransactionSetControlNumber_02,
                        ClaimXml = SerializeEdiMessageToXml(c),
                        ClaimTypeId = (int)ClaimTypeEnum.Dental,
                        ProcessedOn = DateTime.UtcNow
                    }));
                    break;
            }
            await ediRepository.SaveProcessedClaims(processedClaims);
        }
        else
        {
            logger.LogWarning("No valid {ClaimType} claims to save after validation", claimType);
        }
    }

    /// <summary>
    /// Read and parse the EDI 837 file from the provided file path.
    /// </summary>
    /// <param name="filePath">Path to the EDI file to parse.</param>
    /// <param name="validationLevel">The SNIP level of validation to apply during parsing.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task Parse(string filePath, ValidationLevel validationLevel)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found at {filePath}");
        }

        using var stream = File.OpenRead(filePath);
        await Parse(stream, validationLevel);
    }

    private void LogCount(IEnumerable<EdiMessage> items, ClaimTypeEnum claimType)
    {
        logger.LogInformation("Found {Count} {ClaimType} claims", items.Count(), claimType);
    }

    /// <summary>
    /// Serialize the EDI message to XML format.
    /// </summary>
    /// <param name="message">EDI message to serialize</param>
    /// <returns>XML representation of the EDI message</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static string SerializeEdiMessageToXml(EdiMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        var serializer = new XmlSerializer(message.GetType());
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, message);
        return stringWriter.ToString();
    }
}