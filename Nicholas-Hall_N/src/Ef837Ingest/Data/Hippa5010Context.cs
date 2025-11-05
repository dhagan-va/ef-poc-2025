using Ef837Ingest.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edi837Ingestion.Data;

public class Hipaa5010Context(DbContextOptions<Hipaa5010Context> options) : DbContext(options)
{
    public DbSet<IsaEntity> ISA => Set<IsaEntity>();
    public DbSet<GsEntity> GS => Set<GsEntity>();
    public DbSet<StEntity> ST => Set<StEntity>();
    public DbSet<SeEntity> SE => Set<SeEntity>();

    public DbSet<Ts837pEntity> TS837P => Set<Ts837pEntity>();
    public DbSet<Ts850Entity> TS850 => Set<Ts850Entity>();
    public DbSet<ValidationIssue> ValidationIssues => Set<ValidationIssue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Hipaa5010Context).Assembly);
    }
}
