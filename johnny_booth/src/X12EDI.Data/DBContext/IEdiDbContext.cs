using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using X12EDI.Data.Entities;

namespace X12EDI.Data.DBContext
{
    public interface IEdiDbContext
    {
        #region Public Properties

        DbSet<EdiError> EdiErrors { get; }
        DbSet<EdiFile> EdiFiles { get; }
        DbSet<EdiTransaction> EdiTransactions { get; }
        DbSet<TS837P> TS837Ps { get; }

        #endregion Public Properties

        #region Public Methods

        // Allow repository to add arbitrary template entities without depending on concrete DbContext
        void AddEntity(object entity);

        void AddRangeEntities(IEnumerable<object> entities);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        #endregion Public Methods
    }
}