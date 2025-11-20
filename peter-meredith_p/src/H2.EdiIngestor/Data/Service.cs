using Microsoft.EntityFrameworkCore;

namespace H2.EdiIngestor.Data;

public class Service
{
    public int Id { get; set; }

    public string? ProcedureCode { get; set; }
    public DateTime StartDate { get; set; }

    [Precision(9, 2)]
    public decimal ChargeAmount { get; set; }
    public int Quantity { get; set; }
    public string? UnitOrBasisForMeasurementCode { get; set; }
    public string? DiagnosisCode1 { get; set; }
    public string? DiagnosisCode2 { get; set; }
    public string? DiagnosisCode3 { get; set; }
    public string? DiagnosisCode4 { get; set; }

    public int? ClaimId { get; set; }
    public Claim? Claim { get; set; }
}
