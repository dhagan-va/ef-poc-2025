using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edi837Ingestion.Data
{
    public class Hipaa5010ContextFactory : IDesignTimeDbContextFactory<Hipaa5010Context>
    {
        public Hipaa5010Context CreateDbContext(string[] args)
        {
            // Find appsettings.json relative to the working dir when running `dotnet ef`
            var basePath = Directory.GetCurrentDirectory();

            // If your csproj is in src/Edi837Ingestion, migrations are usually run from that folder.
            // Adjust if you run the command from the solution root.
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = config.GetConnectionString("X12")
                     ?? "Server=.\\SQLEXPRESS;Database=Hipaa5010;Trusted_Connection=True;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<Hipaa5010Context>()
                .UseSqlServer(cs);

            return new Hipaa5010Context(optionsBuilder.Options);
        }
    }
}
