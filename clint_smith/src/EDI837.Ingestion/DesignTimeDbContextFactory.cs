using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EDI837.Ingestion
{
    public class HIPAA_5010_837P_ContextFactory : IDesignTimeDbContextFactory<HIPAA_5010_837P_Context>
    {
        public HIPAA_5010_837P_Context CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var conn = config["Database:ConnectionStrings:HIPAA_5010_837P"];

            var options = new DbContextOptionsBuilder<HIPAA_5010_837P_Context>()
                .UseSqlServer(conn)
                .Options;

            return new HIPAA_5010_837P_Context(options);
        }
    }

    public class ClaimStagingContextFactory : IDesignTimeDbContextFactory<ClaimStagingContext>
    {
        public ClaimStagingContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var conn = config["Database:ConnectionStrings:ClaimStaging"];

            var options = new DbContextOptionsBuilder<ClaimStagingContext>()
                .UseSqlServer(conn)
                .Options;

            return new ClaimStagingContext(options);
        }
    }
}
