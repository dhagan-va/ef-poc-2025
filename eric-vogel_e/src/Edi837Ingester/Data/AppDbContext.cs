using Microsoft.EntityFrameworkCore;

namespace Edi837Ingester.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    
}