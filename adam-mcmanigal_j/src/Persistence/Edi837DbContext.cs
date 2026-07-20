using EdiFabric.Templates.Hipaa5010;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingestion.Persistence;

/// <summary>
/// EF Core context for 837 ingestion. <see cref="IngestedInterchange"/> is the idempotency ledger —
/// one row per ingested interchange (837 file), keyed by a unique SHA-256 content hash so a re-sent
/// file is rejected by the database; the raw file is archived in S3
/// (<see cref="IngestedInterchange.PayloadS3Key"/>). The per-variant transaction-set tables
/// (<see cref="Edi837ProfessionalTransactionSet"/>, <see cref="Edi837InstitutionalTransactionSet"/>,
/// <see cref="Edi837DentalTransactionSet"/>) hold a structured, queryable copy of the parsed
/// transaction sets: each row carries its envelope FK + GS06/ST02 and the EdiFabric POCO as a single
/// JSON column, rather than shredding the deep claim graph into hundreds of relational tables.
/// </summary>
public sealed class Edi837DbContext(DbContextOptions<Edi837DbContext> options) : DbContext(options)
{
    public DbSet<IngestedInterchange> Interchanges => Set<IngestedInterchange>();

    public DbSet<Edi837ProfessionalTransactionSet> ProfessionalTransactionSets => Set<Edi837ProfessionalTransactionSet>();

    public DbSet<Edi837InstitutionalTransactionSet> InstitutionalTransactionSets => Set<Edi837InstitutionalTransactionSet>();

    public DbSet<Edi837DentalTransactionSet> DentalTransactionSets => Set<Edi837DentalTransactionSet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IngestedInterchangeConfiguration());

        // Each variant's table is mapped by the shared generic Edi837TransactionSetConfiguration<,>,
        // closed over the concrete entity and its EdiFabric message type.
        modelBuilder.ApplyConfiguration(new Edi837TransactionSetConfiguration<Edi837ProfessionalTransactionSet, TS837P>());
        modelBuilder.ApplyConfiguration(new Edi837TransactionSetConfiguration<Edi837InstitutionalTransactionSet, TS837I>());
        modelBuilder.ApplyConfiguration(new Edi837TransactionSetConfiguration<Edi837DentalTransactionSet, TS837D>());
    }
}
