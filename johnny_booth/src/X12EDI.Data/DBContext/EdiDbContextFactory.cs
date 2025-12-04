using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace X12EDI.Data.DBContext
{
    /// <summary>
    /// A factory for creating <see cref="EdiDbContext"/> instances during design time.
    /// This is used by Entity Framework Core tools for tasks like creating and applying migrations.
    /// </summary>
    public class EdiDbContextFactory : IDesignTimeDbContextFactory<EdiDbContext>
    {
        #region Public Methods

        /// <summary>
        /// Creates a new instance of the <see cref="EdiDbContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service. Not used in this implementation.</param>
        /// <returns>A new instance of <see cref="EdiDbContext"/>.</returns>
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

        #endregion Public Methods
    }
}