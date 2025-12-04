namespace X12EDI.Data.Entities
{
    /// <summary>
    /// Represents a processed EDI file.
    /// </summary>
    public class EdiFile
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the envelope of the EDI file.
        /// </summary>
        public EdiEnvelope? Envelope { get; set; }

        /// <summary>
        /// Gets or sets the collection of errors associated with the file.
        /// </summary>
        public ICollection<EdiError> Errors { get; set; } = [];

        /// <summary>
        /// Gets or sets the unique identifier for the file.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the original identifier of the file (e.g., filename).
        /// </summary>
        public string Identifier { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the file was ingested.
        /// </summary>
        public DateTime IngestedAt { get; set; }

        /// <summary>
        /// Gets or sets the collection of transactions contained within the file.
        /// </summary>
        public ICollection<EdiTransaction> Transactions { get; set; } = [];

        #endregion Public Properties
    }
}