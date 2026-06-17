using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using X12EDI837Ingestion.Domain.Entities;
using X12EDI837Ingestion.Infrastructure.Repositories;
using X12EDI837Ingestion.Application.Interfaces;
using X12EDI837Ingestion.Application.Services;
using X12EDI837Ingestion.Domain;
using EdiFabric.Templates.X12004010;


namespace X12EDI837Ingestion.Tests;

public class X12837IngestionTests
{

    private static AppDbContext CreateInMemoryDbContext(string inMemoryDBName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
         
            .UseInMemoryDatabase(inMemoryDBName)
            .Options;

        var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static ServiceProvider CreateServiceProvider(string inMemoryDBName)
    {
        var services = new ServiceCollection();

        
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseInMemoryDatabase(inMemoryDBName));

        services.AddLogging();

        //IX12EDI837IngestionService
        services.AddScoped<IX12EDI837IngestRepo, X12EDI837IngestRepo>();
        services.AddScoped<IX12EDI837IngestionService, X12EDI837IngestionService>();

        return services.BuildServiceProvider();
    }

    // ---------- Test Cases ----------

    [Fact]
    public void InMemory_DbContext_Should_Be_Created()
    {
        using var db = CreateInMemoryDbContext("EDIIngestion");

        Assert.NotNull(db);
        Assert.NotNull(db.Database);
    }

    [Fact]
    public async Task InMemory_DbContext_Should_Insert_Interchange()
    {
        await using var db = CreateInMemoryDbContext("EDIIngestion");
      
        var interchangeHdr = new InterchangeHeader();

        db.Interchanges.Add(interchangeHdr);
        await db.SaveChangesAsync();

        var count = await db.Interchanges.CountAsync();
        Assert.True(count >= 1, "Expected at least 1 interchange row to be inserted into InMemory DB.");
    }

    [Fact]
    public void DI_Should_Resolve_Repository()
    {
        using var provider = CreateServiceProvider("EDIIngestion");

        var repo = provider.GetService<IX12EDI837IngestRepo>();

        Assert.NotNull(repo);
    }

    [Fact]
    public void DI_Should_Resolve_Service()
    {
        using var provider = CreateServiceProvider("EDIIngestion");

        var svc = provider.GetService<IX12EDI837IngestionService>();

        Assert.NotNull(svc);
    }

    [Fact]
    public void Interfaces_Should_Contain_AtLeast_One_Public_Method()
    {
        //Get all public instance methods of the interface
        var svcMethods = typeof(IX12EDI837IngestionService)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance);

        //Get all public instance methods of the interface
        var repoMethods = typeof(IX12EDI837IngestRepo)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance);

        Assert.True(svcMethods.Length > 0, "Service interface should expose at least one public method.");
        Assert.True(repoMethods.Length > 0, "Repository interface should expose at least one public method.");
    }

    /*
     Create Mock Repository
        ↓
    Setup Behavior
        ↓
    Call Method
        ↓
    Verify Method Call
    */

    [Fact]
    public async Task Moq_Should_Verify_Repository_Method_Call()
    {
      
        var repoMock = new Mock<IX12EDI837IngestRepo>();
        
        repoMock
            .Setup(r => r.InsertInterchangeAsync(It.IsAny<InterchangeHeader>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1l);

        //Create a dummy interchange to pass to the repo method
        var isa = new InterchangeHeader();

        await repoMock.Object.InsertInterchangeAsync(isa, CancellationToken.None);

        //Verify that the method was called once with any InterchangeHeader and any CancellationToken
        repoMock.Verify(
            r => r.InsertInterchangeAsync(It.IsAny<InterchangeHeader>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}