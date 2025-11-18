using Edi837Ingester.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingester.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Edi837Document> Edi837Documents { get; set; }
}