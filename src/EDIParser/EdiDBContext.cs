using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EdiFabric.Templates.Hipaa5010;
using EdiFabric.Templates.X12004010;
using Microsoft.Extensions.Logging;

namespace EdiParser
{
    public class EdiDBContext : DbContext
    {
        //private readonly IConfiguration _configuration;

        // Optional: Add a constructor that takes DbContextOptions for flexibility (e.g., for testing or dependency injection setups).
        public EdiDBContext(DbContextOptions<EdiDBContext> options) : base(options){}
        
        public EdiDBContext(){}
        public DbSet<TS837> TS837 { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Check if the options have already been configured (e.g., via dependency injection)
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=EDIEntities.db");
            }
        }


    }

}
