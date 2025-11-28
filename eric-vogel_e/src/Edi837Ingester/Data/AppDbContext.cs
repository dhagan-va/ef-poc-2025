using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingester.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TS837P> TS837P { get; set; }
    public DbSet<TS837D> TS837D { get; set; }
    public DbSet<TS837I> TS837I { get; set; }
}