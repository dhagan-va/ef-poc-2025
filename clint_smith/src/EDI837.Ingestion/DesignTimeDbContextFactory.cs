
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System;
using System.IO;

namespace EDI837.Ingestion
{
    public class HIPAA_5010_837P_ContextFactory
        : IDesignTimeDbContextFactory<HIPAA_5010_837P_Context>
    {
        public HIPAA_5010_837P_Context CreateDbContext(string[] args)
        {
            // Always load .env manually for CLI tools
            Env.Load(Path.Combine(AppContext.BaseDirectory, "../../.env"));
            var conn = Environment.GetEnvironmentVariable("SQL_CONN_STRING");

            var options = new DbContextOptionsBuilder<HIPAA_5010_837P_Context>()
                .UseSqlServer(conn)
                .Options;

            return new HIPAA_5010_837P_Context(options);
        }
    }

    public class ClaimStagingContextFactory
        : IDesignTimeDbContextFactory<ClaimStagingContext>
    {
        public ClaimStagingContext CreateDbContext(string[] args)
        {
            Console.WriteLine(Path.Combine(AppContext.BaseDirectory, "../../.env"));
            Env.Load(Path.Combine(AppContext.BaseDirectory, "../../.env"));
            var conn = Environment.GetEnvironmentVariable("STAGING_CONN_STRING");

            var options = new DbContextOptionsBuilder<ClaimStagingContext>()
                .UseSqlServer(conn)
                .Options;

            return new ClaimStagingContext(options);
        }
    }
}
