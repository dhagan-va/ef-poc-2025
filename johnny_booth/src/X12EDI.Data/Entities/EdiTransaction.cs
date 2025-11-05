namespace X12EDI.Data.Entities
{

    public class EdiTransaction
    {
        #region Public Properties

        public string? Checksum { get; set; }
        public string? ControlNumber { get; set; }
        public EdiFile EdiFile { get; set; } = default!;
        public int EdiFileId { get; set; }
        public int Id { get; set; }
        public string? Raw { get; set; }
        public string? TransactionType { get; set; }

        #endregion Public Properties
    }
}