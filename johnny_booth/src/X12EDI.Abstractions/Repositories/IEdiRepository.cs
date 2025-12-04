namespace X12EDI.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for a repository that handles EDI data persistence.
    /// </summary>
    public interface IEdiRepository
    {
        #region Public Methods

        /// <summary>
        /// Asynchronously saves a file and its contained EDI items.
        /// </summary>
        /// <param name="identifier">A unique identifier for the file.</param>
        /// <param name="items">The collection of EDI items parsed from the file.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation, containing a boolean indicating success.</returns>
        Task<bool> SaveFileAsync(string identifier, IEnumerable<object> items, CancellationToken cancellationToken);

        #endregion Public Methods
    }
}