using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using PayerEDI.Data.Models.Attachments;
using PayerEDI.Data.Models.Claims;

namespace PayerEDI.Data.Models;

/// <summary>Base result for a transaction processed from an X12 interchange.</summary>
/// <param name="Message">The parsed EdiFabric transaction.</param>
public abstract record ProcessedEdiTransaction(EdiMessage Message)
{
    public void Deconstruct(out EdiMessage message, out HealthCareClaim claim)
    {
        message = Message;
        claim = this switch
        {
            ProcessedProfessionalClaim professional => professional.Claim,
            ProcessedDentalClaim dental => dental.Claim,
            _ => throw new InvalidOperationException("The transaction is not an 837 claim."),
        };
    }
}

/// <summary>Processed 837P transaction and mapped professional claim.</summary>
public sealed record ProcessedProfessionalClaim(TS837P EdiMessage, ProfessionalCareClaim Claim)
    : ProcessedEdiTransaction(EdiMessage);

/// <summary>Processed 837D transaction and mapped dental claim.</summary>
public sealed record ProcessedDentalClaim(TS837D EdiMessage, DentalCareClaim Claim)
    : ProcessedEdiTransaction(EdiMessage);

/// <summary>Processed 275 transaction and extracted attachment metadata.</summary>
/// <param name="Mapping">Mapped metadata and extraction errors.</param>
public sealed record ProcessedAttachmentTransaction(TS275 EdiMessage, AttachmentMapping Mapping)
    : ProcessedEdiTransaction(EdiMessage);
