using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PayerEDI.Data.Database;

public sealed class PayerEdiDbContextFactory : IDesignTimeDbContextFactory<PayerEdiDbContext>
{
    public PayerEdiDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("EDI_PROCESSOR_CONNECTIONSTRINGS__MIGRATION")
            ?? throw new InvalidOperationException(
                "Set EDI_PROCESSOR_CONNECTIONSTRINGS__MIGRATION before running EF migrations."
            );

        var options = new DbContextOptionsBuilder<PayerEdiDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new PayerEdiDbContext(options);
    }
}
