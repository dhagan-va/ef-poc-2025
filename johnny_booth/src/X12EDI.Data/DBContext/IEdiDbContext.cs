using Microsoft.EntityFrameworkCore;
using X12EDI.Data.Entities;

namespace X12EDI.Data.DBContext
{
    public interface IEdiDbContext
    {
        DbSet<EdiError> EdiErrors { get; }
        DbSet<EdiFile> EdiFiles { get; }
        DbSet<EdiTransaction> EdiTransactions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}