using H2.EdiIngestor.Data;

using Microsoft.EntityFrameworkCore;

public class EdiIngestorDbContext : DbContext
{
    public EdiIngestorDbContext(DbContextOptions<EdiIngestorDbContext> options)
        : base(options) { }

    public DbSet<Claim> Claims { get; set; }
    public DbSet<Service> Services { get; set; }
}
