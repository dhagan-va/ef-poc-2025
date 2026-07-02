using Edi837Ingestion.Persistence;
using integration.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace integration.SqlServer;

/// <summary>
/// Smoke tests asserting the <see cref="SqlServerFixture"/> stands up a real SQL Server with the
/// app's migrations applied, and that the idempotency ledger round-trips through the real engine.
/// These require Docker (Testcontainers starts the SQL Server image).
/// </summary>
[Collection("SqlServer")]
public sealed class SqlServerSchemaTests(SqlServerFixture sql)
{
    [Fact]
    public async Task Database_IsMigrated()
    {
        await using var context = sql.CreateContext();

        Assert.True(await context.Database.CanConnectAsync());
        Assert.NotEmpty(await context.Database.GetAppliedMigrationsAsync());
    }

    [Fact]
    public async Task Interchange_RoundTripsThroughLedger()
    {
        var contentHash = $"hash-{Guid.NewGuid():N}";

        await using (var write = sql.CreateContext())
        {
            write.Interchanges.Add(new IngestedInterchange
            {
                ContentHash = contentHash,
                SenderId = "SENDER",
                ReceiverId = "RECEIVER",
                InterchangeControlNumber = "000000001",
                ReceivedAt = DateTime.UtcNow,
            });
            await write.SaveChangesAsync();
        }

        // A fresh context proves the row was persisted, not merely tracked in memory.
        await using var read = sql.CreateContext();
        var stored = await read.Interchanges.SingleAsync(i => i.ContentHash == contentHash);

        Assert.True(stored.Id > 0);
        Assert.Equal("SENDER", stored.SenderId);
    }
}
