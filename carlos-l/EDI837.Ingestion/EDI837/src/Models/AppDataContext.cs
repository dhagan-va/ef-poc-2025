using Microsoft.EntityFrameworkCore;

namespace EDI837.src.Models
{
    public class AppDataContext: DbContext
    {
        public AppDataContext(DbContextOptions options) : base(options) 
        { 
        
        }
       
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
