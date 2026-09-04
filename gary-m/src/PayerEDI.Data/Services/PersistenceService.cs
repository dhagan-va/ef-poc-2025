using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using Microsoft.Extensions.Logging;
using PayerEDI.Data.Database;
using PayerEDI.Data.Database.Repositories;
using PayerEDI.Data.Database.Tables;
using PayerEDI.Data.Models;
using PayerEDI.Data.Models.Attachments;
using PayerEDI.Data.Models.Claims;

namespace PayerEDI.Data.Services;

public class PersistenceService(
    ILogger<PersistenceService> logger,
    PayerEdiDbContext context,
    IDocumentTableRepository documentTableRepository,
    IPatientRepository patientRepository
) : IPersistenceService
{
    public Task Save(
        TS837P ts837P,
        ProfessionalCareClaim professionalCareClaim,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogTrace("Saving professional care claim {Claim}", professionalCareClaim);

        return SaveClaimAsync(
            ts837P.CreateDocument(professionalCareClaim.TransactedAt),
            GetPatients(professionalCareClaim.Subscribers),
            ts837P.ErrorContext,
            cancellationToken
        );
    }

    public Task Save(
        TS275 ts275,
        AttachmentMapping mapping,
        DateTime transactionDateTime,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogTrace(
            "Saving 275 attachment transaction with {AttachmentCount} attachments",
            mapping.Transaction.Attachments.Count
        );

        var document = ts275.CreateDocument(transactionDateTime);
        var attachments = mapping.Transaction.Attachments.Select(
            attachment => new DocumentAttachmentTable(
                document.Id,
                attachment.PatientMemberId,
                attachment.PatientMemberIdQualifier,
                attachment.ClaimReferences.FirstOrDefault()?.Value,
                attachment.ClaimReferences.FirstOrDefault()?.Qualifier,
                attachment.SequenceNumber,
                attachment.FileName,
                attachment.ContentType,
                attachment.DeclaredLength,
                attachment.StorageLocation,
                attachment.Status.ToString()
            )
        );

        return SaveAttachmentAsync(
            document,
            attachments,
            ts275.ErrorContext,
            mapping.Errors,
            cancellationToken
        );
    }

    public Task Save(
        TS837D ts837D,
        DentalCareClaim dentalCareClaim,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogTrace("Saving dental care claim {Claim}", dentalCareClaim);

        return SaveClaimAsync(
            ts837D.CreateDocument(dentalCareClaim.TransactedAt),
            GetPatients(dentalCareClaim.Subscribers),
            ts837D.ErrorContext,
            cancellationToken
        );
    }

    private static PatientTable[] GetPatients( // TODO: page this somehow
        IEnumerable<Subscriber> subscribers
    ) =>
        subscribers
            .SelectMany(subscriber => new[] { subscriber.Primary }.Concat(subscriber.Dependents))
            .Select(PatientTable.ToPatientTable)
            .ToArray();

    private async Task SaveClaimAsync(
        DocumentTable documentTable,
        IReadOnlyCollection<PatientTable> patients,
        MessageErrorContext? errorContext,
        CancellationToken cancellationToken
    )
    {
        // Keep the document and its patients atomic: a failed claim must not leave only half of
        // its records persisted. The transaction also uses this scope's single DbContext instance.
        await using var transaction = await context.Database.BeginTransactionAsync( // this is how EF Core handles transactions, Spring does this with an annotation on the method
            cancellationToken
        );

        documentTableRepository.Add(documentTable);
        patientRepository.AddRange(patients);
        if (errorContext is not null)
        {
            context.EdiErrors.Add(errorContext.CreateEdiError(documentTable.Id));
        }
        // Add and AddRange only stage both entity sets in the same DbContext; this single
        // SaveChangesAsync call is the only database commit for the entire claim.
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task SaveAttachmentAsync(
        DocumentTable document,
        IEnumerable<DocumentAttachmentTable> attachments,
        MessageErrorContext? parserError,
        IReadOnlyCollection<AttachmentMappingError> mappingErrors,
        CancellationToken cancellationToken
    )
    {
        await using var transaction = await context.Database.BeginTransactionAsync(
            cancellationToken
        );

        documentTableRepository.Add(document);
        context.DocumentAttachments.AddRange(attachments);

        if (parserError is not null)
        {
            context.EdiErrors.Add(parserError.CreateEdiError(document.Id));
        }

        if (mappingErrors.Count > 0)
        {
            var error = new EdiErrorTable
            {
                DocumentId = document.Id,
                Name = "AttachmentExtraction",
                Message = "One or more 275 attachments failed extraction.",
                Codes = mappingErrors.Select(item => item.Code).Distinct().ToArray(),
            };

            foreach (var mappingError in mappingErrors)
            {
                error.Errors.Add(
                    new EdiSegmentErrorTable
                    {
                        EdiErrorId = error.Id,
                        Name = mappingError.SegmentName,
                        LoopId = mappingError.LoopId,
                        Message = mappingError.Message,
                        SpecRef = mappingError.SpecReference,
                        Codes = [mappingError.Code],
                    }
                );
            }

            context.EdiErrors.Add(error);
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
