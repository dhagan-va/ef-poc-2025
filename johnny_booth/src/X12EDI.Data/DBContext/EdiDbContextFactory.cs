using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace X12EDI.Data.DBContext
{
    public class EdiDbContextFactory : IDesignTimeDbContextFactory<EdiDbContext>
    {
        public EdiDbContext CreateDbContext(string[] args)
        {
            // build config so we can read appsettings.json from the startup project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EdiDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("EdiDb"));

            return new EdiDbContext(optionsBuilder.Options);
        }
    }
}
