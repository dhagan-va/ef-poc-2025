namespace X12EDI.Abstractions.Services
{
    /// <summary>
    /// Defines the contract for a service that ingests and processes files.
    /// </summary>
    public interface IFileIngestionService
    {
        #region Public Methods

        /// <summary>
        /// Asynchronously ingests all available files.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task IngestAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously ingests a specific file.
        /// </summary>
        /// <param name="subpath">The path to the file to ingest.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task IngestAsync(string subpath, CancellationToken cancellationToken = default);

        #endregion Public Methods
    }
}