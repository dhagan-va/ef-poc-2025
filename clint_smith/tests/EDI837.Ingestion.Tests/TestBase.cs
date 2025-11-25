using Microsoft.Extensions.Configuration;

namespace EDI837.Ingestion.Tests
{
    public abstract class TestBase
    {
        protected readonly IConfiguration Configuration;

        public TestBase()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var ediToken = Configuration["EDI:Token"];
            EdiFabric.SerialKey.Set(ediToken);
        }
    }
}
