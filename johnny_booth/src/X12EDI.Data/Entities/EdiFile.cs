namespace X12EDI.Data.Entities
{
    public class EdiFile
    {
        #region Public Properties

        public EdiEnvelope? Envelope { get; set; }
        public ICollection<EdiError> Errors { get; set; } = new List<EdiError>();
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public DateTime IngestedAt { get; set; }
        public ICollection<EdiTransaction> Transactions { get; set; } = new List<EdiTransaction>();

        #endregion Public Properties
    }
}