using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X12EDI.Data.Entities;

namespace X12EDI.Data.DBContext
{
    public class EdiDbContext : DbContext
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

        #endregion Public Properties

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