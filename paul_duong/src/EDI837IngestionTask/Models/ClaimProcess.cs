namespace EDI837IngestionTask.Models
{
    public class ClaimProcess
    {
        public int Id { get; set; }
        public string? PBSEDINumber { get; set; }
        public string? KICEDINumber { get; set; }
        public string? SubmitterId { get; set; }
        public string? ClaimCreatedDt { get; set; }
        public string? ProviderNPI { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderZIP { get; set; }
        public string? Tin { get; set; }
        public string? TransactionControlNumber { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}