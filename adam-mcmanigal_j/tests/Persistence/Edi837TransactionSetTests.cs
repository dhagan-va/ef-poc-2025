using Edi837Ingestion.Parsing;
using Edi837Ingestion.Persistence;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace test.Persistence;

/// <summary>
/// Covers the per-variant transaction-set entities without a database or the EdiFabric license (the
/// POCOs are constructed directly): the <c>From</c> factories project a parsed transaction set onto a
/// row, and the JSON value-converter configured on the model round-trips the EdiFabric POCO faithfully.
/// </summary>
public sealed class Edi837TransactionSetTests
{
    private static Edi837DbContext NewContext() =>
        new(new DbContextOptionsBuilder<Edi837DbContext>()
            .UseSqlServer("Server=(local);Database=edi;TrustServerCertificate=True;")
            .Options);

    [Fact]
    public void From_Professional_ProjectsTransactionSet_OntoRow()
    {
        var interchange = new IngestedInterchange();
        var message = new TS837P();
        var transactionSet = new ParsedTransactionSet<TS837P>("101", "0001", message);

        var row = Edi837ProfessionalTransactionSet.From(transactionSet, interchange);

        Assert.Same(interchange, row.Interchange);
        Assert.Equal("101", row.GroupControlNumber);
        Assert.Equal("0001", row.TransactionSetControlNumber);
        Assert.Same(message, row.Message);
    }

    [Fact]
    public void From_Institutional_ProjectsTransactionSet_OntoRow()
    {
        var interchange = new IngestedInterchange();
        var message = new TS837I();

        var row = Edi837InstitutionalTransactionSet.From(
            new ParsedTransactionSet<TS837I>("101", "987654", message), interchange);

        Assert.Same(interchange, row.Interchange);
        Assert.Equal("987654", row.TransactionSetControlNumber);
        Assert.Same(message, row.Message);
    }

    [Fact]
    public void From_Dental_ProjectsTransactionSet_OntoRow()
    {
        var interchange = new IngestedInterchange();
        var message = new TS837D();

        var row = Edi837DentalTransactionSet.From(
            new ParsedTransactionSet<TS837D>("101", "3456", message), interchange);

        Assert.Same(interchange, row.Interchange);
        Assert.Equal("3456", row.TransactionSetControlNumber);
        Assert.Same(message, row.Message);
    }

    [Fact]
    public void From_Throws_OnNullArguments()
    {
        var transactionSet = new ParsedTransactionSet<TS837P>("101", "0001", new TS837P());

        Assert.Throws<ArgumentNullException>(
            () => Edi837ProfessionalTransactionSet.From(null!, new IngestedInterchange()));
        Assert.Throws<ArgumentNullException>(
            () => Edi837ProfessionalTransactionSet.From(transactionSet, null!));
    }

    [Fact]
    public void ConfiguredConverter_RoundTripsMessage_ThroughJson()
    {
        using var context = NewContext();
        var converter = context.Model
            .FindEntityType(typeof(Edi837ProfessionalTransactionSet))!
            .FindProperty("Message")!
            .GetValueConverter()!;

        var message = new TS837P { ST = new ST { TransactionSetControlNumber_02 = "0001" } };

        var json = Assert.IsType<string>(converter.ConvertToProvider(message));
        var restored = Assert.IsType<TS837P>(converter.ConvertFromProvider(json));

        Assert.Contains("0001", json);
        Assert.Equal("0001", restored.ST?.TransactionSetControlNumber_02);
    }
}
