using Microsoft.EntityFrameworkCore;
using EdiFabric.Templates.Hipaa5010;

namespace EDI837.src.Models
{
    public class AppDataContext: DbContext
    {
        public AppDataContext(DbContextOptions options) : base(options) 
        { 
        
        }
       
        public DbSet<TS837P> TS837Ps{ get; set; }
        public DbSet<ProcessedClaim> ProcessedClaims { get; set; }
    }
}
