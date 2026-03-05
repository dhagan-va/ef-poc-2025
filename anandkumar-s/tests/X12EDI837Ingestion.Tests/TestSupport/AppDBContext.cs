using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X12EDI837Ingestion.Domain;

namespace X12EDI837Ingestion.Tests.TestSupport
{

    public static class AppDBContext
    {
        public static AppDbContext CreateInMemoryDbContext(string? dbName = null)
        {
            dbName ??= Guid.NewGuid().ToString("N");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .EnableSensitiveDataLogging()
                .Options;

            var ctx = new AppDbContext(options);
            return ctx;
        }
    }
}
