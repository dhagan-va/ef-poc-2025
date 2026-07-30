using Microsoft.EntityFrameworkCore;
using Deepika.EDIIngestion.Models;

namespace Deepika.EDIIngestion.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EdiInterchange> Interchanges { get; set; } = null!;
        public DbSet<Provider> Providers { get; set; } = null!;
        public DbSet<Claim> Claims { get; set; } = null!;
        public DbSet<ErrorLog> ErrorLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EdiInterchange>(b =>
            {
                b.HasOne(e => e.Provider).WithMany().HasForeignKey(e => e.ProviderId);
                b.HasMany(e => e.Claims).WithOne(c => c.Interchange).HasForeignKey(c => c.InterchangeId);
            });

            modelBuilder.Entity<Claim>(b =>
            {
                // Ensure SQL precision/scale matches existing migration to avoid silent truncation warnings
                b.Property(c => c.TotalChargeAmount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ErrorLog>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.FileName).HasMaxLength(260);
                b.Property(e => e.OccurredAt).IsRequired();
                b.Property(e => e.ErrorMessage).HasMaxLength(2000);
                b.Property(e => e.ErrorType).HasMaxLength(250);
            });
        }
    }
}
