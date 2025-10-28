namespace EDI837IngestionTask.Models
{
    public class ClaimProcess
    {
        public int Id { get; set; }
        public string? ProviderNPI { get; set; }
        public string? TransactionControlNumber { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}