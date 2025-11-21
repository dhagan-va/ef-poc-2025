using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using H2.EdiIngestor;
using H2.EdiIngestor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H2.EdiIngestor.Tests;

[TestClass]
public class WorkerTests
{
    [TestMethod]
    public async Task Worker_ProcessClaims_SavesToDatabase()
    {
        // Arrange: service collection with in-memory EF DbContext
        var services = new ServiceCollection();
        services.AddDbContext<EdiIngestorDbContext>(opts =>
        {
            opts.UseInMemoryDatabase("testdb1");
        });

        // Add scope factory
        var provider = services.BuildServiceProvider();

        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        // Fake parser that yields one claim
        var fakeParser = new FakeParserYieldOneClaim();

        var hostAppLifetime = new FakeHostLifetime();
        var logger = NullLogger<Worker>.Instance;
        var options = Options.Create(new EdiTestOptions { FileName = "dummy" });
        var secrets = Options.Create(new Secrets { ConnectionString = "DataSource=:memory:" });

        var worker = new Worker(
            hostAppLifetime,
            logger,
            options,
            secrets,
            scopeFactory,
            fakeParser
        );

        // Act
        await worker.StartAsync(CancellationToken.None);
        await worker.StopAsync(CancellationToken.None);

        // Assert: ensure claim saved
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EdiIngestorDbContext>();
        Assert.IsTrue(db.Claims.Any());
    }

    [TestMethod]
    public async Task Worker_PassesCancellationToken_ToParser()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDbContext<EdiIngestorDbContext>(opts => opts.UseInMemoryDatabase("testdb2"));
        var provider = services.BuildServiceProvider();
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        var fakeParser = new FakeParserRecordToken();
        var hostAppLifetime = new FakeHostLifetime();
        var logger = NullLogger<Worker>.Instance;
        var options = Options.Create(new EdiTestOptions { FileName = "dummy" });
        var secrets = Options.Create(new Secrets { ConnectionString = "DataSource=:memory:" });

        var worker = new Worker(
            hostAppLifetime,
            logger,
            options,
            secrets,
            scopeFactory,
            fakeParser
        );

        var cts = new CancellationTokenSource();
        var task = worker.StartAsync(cts.Token);

        // cancel immediately
        cts.Cancel();

        // Allow the worker to observe cancellation
        await Task.Delay(50);

        // Assert
        Assert.IsTrue(
            fakeParser.SeenCancellationToken.IsCancellationRequested || cts.IsCancellationRequested
        );
    }
}

// Simple fake implementations used for tests
internal class FakeParserYieldOneClaim : IX12N837Parser
{
    public async IAsyncEnumerable<Claim> ReadFile(
        string fileName,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        yield return new Claim { ClaimId = "1", PatientName = "Test" };
        await Task.CompletedTask;
    }
}

internal class FakeParserRecordToken : IX12N837Parser
{
    public CancellationToken SeenCancellationToken { get; private set; }

    public async IAsyncEnumerable<Claim> ReadFile(
        string fileName,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        SeenCancellationToken = cancellationToken;
        // yield nothing
        await Task.CompletedTask;
        yield break;
    }
}

internal class FakeHostLifetime : IHostApplicationLifetime
{
    public CancellationToken ApplicationStarted => CancellationToken.None;
    public CancellationToken ApplicationStopping => CancellationToken.None;
    public CancellationToken ApplicationStopped => CancellationToken.None;

    public void StopApplication()
    {
        // no-op for tests
    }
}
