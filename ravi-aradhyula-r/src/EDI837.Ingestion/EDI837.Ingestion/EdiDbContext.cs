using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;


public class EdiDbContext : DbContext
{
    private readonly string _connectionString;


    public EdiDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<Claim> Claims { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
