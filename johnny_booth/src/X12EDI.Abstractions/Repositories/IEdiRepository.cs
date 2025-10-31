namespace X12EDI.Abstractions.Repositories
{
    public interface IEdiRepository
    {
        Task<bool> SaveFileAsync(string identifier, IEnumerable<object> items, CancellationToken cancellationToken);
    }
}

