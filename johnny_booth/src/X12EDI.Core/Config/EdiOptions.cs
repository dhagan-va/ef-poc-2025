using EdiFabric.Core.Model.Edi;

namespace X12EDI.Core.Config
{
    /// <summary>
    /// Represents configuration options for EDI processing.
    /// </summary>
    public class EdiOptions
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to continue processing on error.
        /// </summary>
        public bool ContinueOnError { get; set; } = true;

        public ValidationLevel SNIPLevelValidation { get; set; } = ValidationLevel.SyntaxOnly_SNIP1;

        /// <summary>
        /// Gets or sets the folder path for EDI files.
        /// </summary>
        public string? FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the serial key for EdiFabric.
        /// </summary>
        public string? SerialKey { get; set; }

        #endregion Public Properties
    }
}