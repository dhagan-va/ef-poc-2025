namespace X12EDI.Data.Entities
{
    public class EdiError
    {
        #region Public Properties

        public EdiFile EdiFile { get; set; } = null!;
        public int EdiFileId { get; set; }
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;

        #endregion Public Properties
    }
}