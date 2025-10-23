namespace EDI837.Ingestion {
    public class ClaimStaging
    {
        public int Id { get; set; }
        public required string ProviderNPI { get; set; }
        public required string PatientControlNumber { get; set; }
        public required DateTime ReceivedAt { get; set; }
        public required string ClaimXml { get; set; }
    }

}