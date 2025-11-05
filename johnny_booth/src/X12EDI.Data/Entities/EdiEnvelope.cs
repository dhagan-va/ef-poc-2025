namespace X12EDI.Data.Entities
{
    public class EdiEnvelope
    {
        #region Public Properties

        public string? CodeIdentifyingInformationType { get; set; }
        public EdiFile EdiFile { get; set; } = default!;
        public int EdiFileId { get; set; }
        public string? GroupControlNumber { get; set; }
        public int Id { get; set; }

        public string? InterchangeControlNumber { get; set; }  // ISA13
        public string? ReceiverId { get; set; }
        public string? SenderId { get; set; }                  // ISA06

        // ISA08
        // GS06
        // GS01
        public DateTime Timestamp { get; set; }

        #endregion Public Properties
    }
}