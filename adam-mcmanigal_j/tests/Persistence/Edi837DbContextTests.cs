using Edi837Ingestion.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace test.Persistence;

/// <summary>
/// Builds the <see cref="Edi837DbContext"/> model (no database is touched) and asserts the mapping:
/// the <see cref="IngestedInterchange"/> idempotency ledger (one row per file, unique content hash,
/// no claim payload) plus the three per-variant transaction-set tables, each owning its envelope FK
/// and storing the EdiFabric POCO in a single JSON column.
/// </summary>
public sealed class Edi837DbContextTests
{
    private static Edi837DbContext NewContext() =>
        new(new DbContextOptionsBuilder<Edi837DbContext>()
            .UseSqlServer("Server=(local);Database=edi;TrustServerCertificate=True;")
            .Options);

    // The three concrete transaction-set entities — the abstract generic base is not itself mapped.
    public static TheoryData<Type> TransactionSetEntities =>
    [
        typeof(Edi837ProfessionalTransactionSet),
        typeof(Edi837InstitutionalTransactionSet),
        typeof(Edi837DentalTransactionSet),
    ];

    [Fact]
    public void Model_MapsLedgerPlusThreeTransactionSetTables()
    {
        using var context = NewContext();

        var entities = context.Model.GetEntityTypes().Select(e => e.ClrType).ToHashSet();

        Assert.Equal(
            new HashSet<Type>
            {
                typeof(IngestedInterchange),
                typeof(Edi837ProfessionalTransactionSet),
                typeof(Edi837InstitutionalTransactionSet),
                typeof(Edi837DentalTransactionSet),
            },
            entities);
    }

    [Fact]
    public void Model_HasUniqueIndex_OnContentHash()
    {
        using var context = NewContext();
        var entity = context.Model.FindEntityType(typeof(IngestedInterchange))!;

        var uniqueIndex = Assert.Single(entity.GetIndexes(), i => i.IsUnique);
        Assert.Equal(
            new[] { nameof(IngestedInterchange.ContentHash) },
            uniqueIndex.Properties.Select(p => p.Name));
    }

    [Fact]
    public void Model_Interchange_StoresNoConvertedPayload()
    {
        using var context = NewContext();
        var entity = context.Model.FindEntityType(typeof(IngestedInterchange))!;

        // The ledger row itself stays thin: claim data lives in the per-variant tables, not here.
        Assert.DoesNotContain(entity.GetProperties(), p => p.GetValueConverter() is not null);
    }

    [Fact]
    public void Model_PayloadS3Key_IsNullable()
    {
        using var context = NewContext();
        var entity = context.Model.FindEntityType(typeof(IngestedInterchange))!;

        Assert.True(entity.FindProperty(nameof(IngestedInterchange.PayloadS3Key))!.IsNullable);
    }

    [Theory]
    [MemberData(nameof(TransactionSetEntities))]
    public void TransactionSetTable_PersistsMessage_AsJsonColumn(Type entityType)
    {
        using var context = NewContext();
        var message = context.Model.FindEntityType(entityType)!.FindProperty("Message")!;

        Assert.NotNull(message.GetValueConverter());
        Assert.Equal("nvarchar(max)", message.GetColumnType());
        Assert.False(message.IsNullable);
    }

    [Theory]
    [MemberData(nameof(TransactionSetEntities))]
    public void TransactionSetTable_HasRequiredCascadingFk_ToInterchange(Type entityType)
    {
        using var context = NewContext();
        var entity = context.Model.FindEntityType(entityType)!;

        var fk = Assert.Single(entity.GetForeignKeys());
        Assert.Equal(typeof(IngestedInterchange), fk.PrincipalEntityType.ClrType);
        Assert.True(fk.IsRequired);
        Assert.Equal(DeleteBehavior.Cascade, fk.DeleteBehavior);
    }

    [Theory]
    [MemberData(nameof(TransactionSetEntities))]
    public void TransactionSetTable_ControlNumbers_AreLengthCapped(Type entityType)
    {
        using var context = NewContext();
        var entity = context.Model.FindEntityType(entityType)!;

        Assert.Equal(9, entity.FindProperty("GroupControlNumber")!.GetMaxLength());
        Assert.Equal(9, entity.FindProperty("TransactionSetControlNumber")!.GetMaxLength());
    }
}
