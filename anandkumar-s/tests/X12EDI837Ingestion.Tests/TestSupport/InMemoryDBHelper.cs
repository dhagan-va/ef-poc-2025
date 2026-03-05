
    using global::X12EDI837Ingestion.Domain;
    using Microsoft.EntityFrameworkCore;

    namespace X12EDI837Ingestion.Tests.TestSupport;

    public static class InMemoryDbHelper
    {
        public static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }
    }

