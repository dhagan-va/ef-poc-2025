namespace EDI837.Ingestion {
    public class ClaimStaging
    {
        public int Id { get; set; }

        public string? ProviderNPI { get; set; }  // optional — see below
        public string? TransactionControlNumber { get; set; }  // ST02

        public string ClaimXml { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
    }

}