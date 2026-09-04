using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using Microsoft.Extensions.Logging;
using PayerEDI.Data.Helpers;
using PayerEDI.Data.Models;
using PayerEDI.Data.Models.Attachments.Factory;
using PayerEDI.Data.Models.Claims;
using PayerEDI.Data.Models.Claims.Factory;

namespace PayerEDI.Data.Services;

public class EdiFabricEdiProcessor(ILogger<EdiFabricEdiProcessor> logger) : IEdiProcessor
{
    public List<ProcessedEdiTransaction> ProcessEdi(Stream ediStream)
    {
        var claims = new List<ProcessedEdiTransaction>();
        using var edi = new X12Reader(ediStream, X12TypeFactory.GetTypeInfo);
        var transactions = edi.ReadToEnd().ToList();
        // Keep this interface-typed so nested processors share one boxed enumerator instead of advancing copied struct enumerators.
#pragma warning disable CA1859
        IEnumerator<IEdiItem> transactionEnumerator = transactions.GetEnumerator(); // there has to be a better .Net way of handling this than to just tell the compiler to shutup
#pragma warning restore CA1859

        while (transactionEnumerator.MoveNext()) // maybe use a stack with push and pop semantics instead of this advance with the enumerator
        {
            var transaction = transactionEnumerator.Current;

            switch (transaction)
            {
                case ISA isa:
                    ProcessInterchange(claims, transactionEnumerator, isa);
                    break;
                case null:
                    logger.LogTrace("Null detected while processing transaction");
                    continue;
                default:
                    logger.LogInformation(
                        "Unhandled transaction type in process Edi File {Transaction}",
                        transaction
                    );
                    break;
            }
        }

        return claims;
    }

    private void ProcessInterchange(
        List<ProcessedEdiTransaction> claims,
        IEnumerator<IEdiItem> transactionEnumerator,
        ISA isa
    )
    {
        logger.LogInformation("Transaction Start {Transaction}", isa);
        logger.LogInformation("Interchange Date {InterchangeDate}", isa.InterchangeDate_9);

        while (transactionEnumerator.MoveNext())
        {
            var transaction = transactionEnumerator.Current;

            switch (transaction)
            {
                case GS gs:
                    ProcessFunctionalGroup(claims, transactionEnumerator, gs);
                    break;
                case IEA iea:
                    logger.LogInformation("Transaction End {Transaction}", iea);
                    return; // end of document
                default:
                    logger.LogWarning(
                        "Unhandled transaction type in process interchange {Transaction}",
                        transaction
                    );
                    break;
            }
        }
    }

    private void ProcessFunctionalGroup(
        List<ProcessedEdiTransaction> claims,
        IEnumerator<IEdiItem> transactionEnumerator,
        GS gs
    )
    {
        logger.LogInformation("GS transaction Begin {Transaction}", gs);
        logger.LogInformation("Functional Group Date {FunctionalGroupDate}", gs.Date_4);
        logger.LogInformation("Control Group Number {Number}", gs.GroupControlNumber_6);

        while (transactionEnumerator.MoveNext())
        {
            var transaction = transactionEnumerator.Current;

            switch (transaction)
            {
                case TS837P ts837P:
                    logger.LogDebug("837P transaction {Transaction}", ts837P);
                    claims.Add(
                        new ProcessedProfessionalClaim(
                            ts837P,
                            ProfessionalCareClaim.New(gs.Date_4, gs.Time_5, ts837P)
                        )
                    );
                    break;
                case TS837D ts837D:
                    logger.LogDebug("837D transaction {Transaction}", ts837D);
                    claims.Add(
                        new ProcessedDentalClaim(
                            ts837D,
                            DentalCareClaim.New(gs.Date_4, gs.Time_5, ts837D)
                        )
                    );
                    break;
                case TS275 ts275:
                    logger.LogDebug("275 transaction {Transaction}", ts275);
                    claims.Add(
                        new ProcessedAttachmentTransaction(
                            ts275,
                            AttachmentFactory.New(gs.GroupDateTime(), ts275)
                        )
                    );
                    break;
                case ReaderErrorContext errorContext: // FIXME: aggregate and pass back to caller, or stop processing and throw exception?
                    logger.LogError(
                        errorContext.Exception,
                        "Reader error at {ReaderErrorCode}: {ErrorMessage}",
                        errorContext.ReaderErrorCode,
                        errorContext.Exception.Message
                    );
                    break;
                case GE ge:
                    logger.LogInformation("GE transaction End {Transaction}", ge);
                    logger.LogInformation("Control Group Number {Number}", ge.GroupControlNumber_2);
                    return; // end of section
                default:
                    logger.LogWarning(
                        "Unhandled transaction type in functional group {Transaction}",
                        transaction
                    );
                    break;
            }
        }
    }
}
