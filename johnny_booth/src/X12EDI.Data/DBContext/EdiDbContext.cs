using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using X12EDI.Data.Entities;

namespace X12EDI.Data.DBContext
{
    public class EdiDbContext : DbContext, IEdiDbContext
    {
        #region Public Constructors

        public EdiDbContext(DbContextOptions<EdiDbContext> options) : base(options)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public DbSet<EdiError> EdiErrors => Set<EdiError>();
        public DbSet<EdiFile> EdiFiles => Set<EdiFile>();
        public DbSet<EdiTransaction> EdiTransactions => Set<EdiTransaction>();
        public DbSet<TS837P> TS837Ps => Set<TS837P>();

        #endregion Public Properties

        #region Public Methods

        public void AddEntity(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            base.Add(entity);
        }

        public void AddRangeEntities(IEnumerable<object> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            base.AddRange(entities);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EdiTransaction>()
                .HasIndex(t => t.Checksum)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }

        #endregion Protected Methods
    }
}