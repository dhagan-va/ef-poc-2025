namespace X12EDI.Data.Entities
{
    /// <summary>
    /// Represents an error encountered during EDI file processing.
    /// </summary>
    public class EdiError
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the EDI file associated with this error.
        /// </summary>
        public EdiFile EdiFile { get; set; } = null!;

        /// <summary>
        /// Gets or sets the foreign key for the associated EDI file.
        /// </summary>
        public int EdiFileId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the error.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        #endregion Public Properties
    }
}