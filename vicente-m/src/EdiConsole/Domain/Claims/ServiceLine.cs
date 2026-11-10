namespace EdiConsole.Domain.Claims;

/// <summary>
/// One service line: loop 2400 (LX + SV1 + DTP*472). Up to 50 per claim.
/// </summary>
public class ServiceLine
{
    public long Id { get; set; }
    public long ClaimId { get; set; }

    public int LineNumber { get; set; }                  // LX01
    public required string ProcedureCode { get; set; }   // SV101-2 (SV101-1 = HC: CPT/HCPCS)
    public string? Modifier1 { get; set; }               // SV101-3
    public string? Modifier2 { get; set; }               // SV101-4
    public string? Modifier3 { get; set; }               // SV101-5
    public string? Modifier4 { get; set; }               // SV101-6
    public decimal Charge { get; set; }                  // SV102
    public required string UnitBasis { get; set; }       // SV103: UN = units, MJ = minutes
    public decimal Units { get; set; }                   // SV104
    public string? PlaceOfServiceCode { get; set; }      // SV105, only when different from the claim's

    public byte DiagnosisPointer1 { get; set; }          // SV107-1 → ClaimDiagnosis.Sequence (primary for this line)
    public byte? DiagnosisPointer2 { get; set; }         // SV107-2
    public byte? DiagnosisPointer3 { get; set; }         // SV107-3
    public byte? DiagnosisPointer4 { get; set; }         // SV107-4

    public DateOnly ServiceDateFrom { get; set; }        // DTP03: the D8 date, or start of an RD8 range
    public DateOnly? ServiceDateTo { get; set; }         // DTP03: end of an RD8 range
}
