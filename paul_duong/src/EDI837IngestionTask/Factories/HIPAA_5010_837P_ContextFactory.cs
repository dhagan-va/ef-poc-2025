
using EDI837IngestionTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EDI837IngestionTask.Factories
{
    public class HIPAA_5010_837P_ContextFactory : IDesignTimeDbContextFactory<HIPAA_5010_837P_Context>
    {
        public HIPAA_5010_837P_Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HIPAA_5010_837P_Context>();
            optionsBuilder.UseSqlServer(EnvSetup.GetDbConnection());
            return new HIPAA_5010_837P_Context(optionsBuilder.Options);
        }
    }
}