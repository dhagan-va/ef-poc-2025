namespace EdiConsole.Domain.Claims;

/// <summary>
/// One diagnosis code from the HI segment (up to 12 per claim).
/// </summary>
public class ClaimDiagnosis
{
    public long Id { get; set; }
    public long ClaimId { get; set; }

    /// <summary>
    /// Position in the HI segment (1–12), not a running counter:
    /// service lines point to this number (SV107).
    /// </summary>
    public byte Sequence { get; set; }

    public required string Qualifier { get; set; }  // ABK/ABF = ICD-10 principal/other, BK/BF = ICD-9
    public required string Code { get; set; }       // without the dot, e.g. I10
}
