using Edi837Ingester.Data.Entities;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingester.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TS837P> TS837P { get; set; }
    public DbSet<TS837D> TS837D { get; set; }
    public DbSet<TS837I> TS837I { get; set; }
    public DbSet<Entities.ProcessedClaim> ProcessedClaims { get; set; }
    public DbSet<ClaimType> ClaimTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClaimType>().HasData(
            new ClaimType() { Id = (int)ClaimTypeEnum.Professional, Name = ClaimTypeEnum.Professional.ToString()},
            new ClaimType() { Id = (int)ClaimTypeEnum.Institutional, Name = ClaimTypeEnum.Institutional.ToString()},
            new ClaimType() { Id = (int)ClaimTypeEnum.Dental, Name = ClaimTypeEnum.Dental.ToString() }
            );

        base.OnModelCreating(modelBuilder);
    }
}