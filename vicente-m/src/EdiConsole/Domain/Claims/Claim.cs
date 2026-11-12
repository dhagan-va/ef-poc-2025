using EdiConsole.Domain.Parties;

namespace EdiConsole.Domain.Claims;

/// <summary>
/// One claim: loop 2300 (one CLM segment).
/// Belongs to an <see cref="Tracking.IngestedFile"/> and owns its diagnoses and service lines.
/// </summary>
public class Claim
{
    public long Id { get; set; }

    /// <summary>
    /// Foreign key to the file this claim came from. EF Core fills it when the file is saved
    /// with its <see cref="Tracking.IngestedFile.Claims"/>. There's no navigation back to the file:
    /// the relationship is one-way (file → claims), which is all we need.
    /// </summary>
    public long IngestedFileId { get; set; }

    public required string TransactionSetControlNumber { get; set; }  // ST02
    public required string PatientControlNumber { get; set; }         // CLM01: unique per provider only, not globally
    public decimal TotalCharge { get; set; }                          // CLM02
    public required string PlaceOfServiceCode { get; set; }           // CLM05-1, e.g. 11 = office
    public required string FrequencyCode { get; set; }                // CLM05-3: 1 original, 7 replacement, 8 void

    public required BillingProvider BillingProvider { get; set; }    // 2010AA
    public required Subscriber Subscriber { get; set; }              // 2000B + 2010BA
    public required Patient Patient { get; set; }                    // 2000C + 2010CA, or the subscriber
    public required Payer Payer { get; set; }                        // 2010BB

    public List<ClaimDiagnosis> Diagnoses { get; } = [];
    public List<ServiceLine> ServiceLines { get; } = [];
}
