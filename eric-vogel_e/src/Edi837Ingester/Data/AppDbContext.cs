using Edi837Ingester.Data.Entities;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingester.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TS837P> TS837P { get; set; }
    public DbSet<TS837D> TS837D { get; set; }
    public DbSet<TS837I> TS837I { get; set; }
    public DbSet<ProcessedClaim> ClaimProcesses { get; set; }
    public DbSet<ClaimType> ClaimTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ClaimType>()
           .HasData(
               new ClaimType { Id = 1, Name = "837 Professional", Code = "837P" },
               new ClaimType { Id = 2, Name = "837 Dental", Code = "837D" },
               new ClaimType { Id = 3, Name = "837 Institutional", Code = "837I" }
           );

        modelBuilder
               .Entity<ProcessedClaim>()
               .HasIndex(c => new { c.ProviderNPI, c.TransactionControlNumber, c.ClaimTypeId })
               .IsUnique()
               .HasFilter(@"[ProviderNPI] IS NOT NULL AND [TransactionControlNumber] IS NOT NULL 
                            AND [ClaimTypeId] IS NOT NULL");
    }
}