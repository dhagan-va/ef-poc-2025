using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using EdiParser.Entities;
using Microsoft.Extensions.Logging;

namespace EdiParser;

public interface IEdiDBContext
{
   public DbSet<TS837P> TS837P { get; set; }
   public DbSet<TS837I> TS837I { get; set; }
   public DbSet<TS837D> TS837D { get; set; }

   int SaveChanges();
   Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}

public class EdiDBContext : DbContext, IEdiDBContext
{
    // Optional: Add a constructor that takes DbContextOptions for flexibility (e.g., for testing or dependency injection setups).
    public EdiDBContext(DbContextOptions<EdiDBContext> options) : base(options){}
    
    public EdiDBContext(){}
    //public DbSet<TS837> TS837 { get; set; 
    public DbSet<TS837P> TS837P { get; set; }
    public DbSet<TS837I> TS837I { get; set; }
    public DbSet<TS837D> TS837D { get; set; }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Check if the options have already been configured (e.g., via dependency injection)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=EDIEntities.db");
        }
    }
    
   
    // Explicitly implement the interface method by calling the base class method
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}


