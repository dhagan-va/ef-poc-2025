using PayerEDI.Data.Database.Tables;

namespace PayerEDI.Data.Database.Repositories;

public class DocumentTableRepository(PayerEdiDbContext context) : IDocumentTableRepository
{
    public void Add(DocumentTable documentTable) => context.Documents.Add(documentTable);

    public Task<int> SaveAsync(
        DocumentTable documentTable,
        CancellationToken cancellationToken = default
    )
    {
        Add(documentTable);
        return context.SaveChangesAsync(cancellationToken);
    }
}
