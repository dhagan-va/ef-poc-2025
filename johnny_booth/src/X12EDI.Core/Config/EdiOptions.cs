namespace X12EDI.Core.Config
{
    public class EdiOptions
    {
        #region Public Properties

        public bool ContinueOnError { get; set; } = true;
        public string? FolderPath { get; set; }
        public string? SerialKey { get; set; }

        #endregion Public Properties
    }
}