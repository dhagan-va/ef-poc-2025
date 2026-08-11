namespace H2.EdiIngestor.Data;

public class Claim
{
    public int Id { get; set; }

    public string? BillingProviderName { get; set; }
    public string? BillingProviderNpi { get; set; }
    public string? RenderingProviderName { get; set; }
    public string? RenderingProviderNpi { get; set; }
    public string? ClaimId { get; set; }
    public string? PatientName { get; set; }
    public string? SubscriberId { get; set; }

    public IEnumerable<Service>? Services { get; set; }
}
