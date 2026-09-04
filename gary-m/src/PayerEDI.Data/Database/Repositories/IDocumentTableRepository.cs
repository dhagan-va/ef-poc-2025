using PayerEDI.Data.Database.Tables;

namespace PayerEDI.Data.Database.Repositories;

public interface IDocumentTableRepository
{
    void Add(DocumentTable documentTable);

    Task<int> SaveAsync(DocumentTable documentTable, CancellationToken cancellationToken = default);
}
