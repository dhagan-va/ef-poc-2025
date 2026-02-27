
using Microsoft.EntityFrameworkCore;
using X12EDI837Ingestion.Domain.Entities;
namespace X12EDI837Ingestion.Domain;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<InterchangeHeader> Interchanges => Set<InterchangeHeader>();
    public DbSet<FunctionalGroupHeader> FunctionalGroups => Set<FunctionalGroupHeader>();
    public DbSet<TransactionSetHeader> TransactionSets => Set<TransactionSetHeader>();
    public DbSet<Party> Parties => Set<Party>();
    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<ServiceLine> ServiceLines => Set<ServiceLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // InterchangeHeader (ISA)
        modelBuilder.Entity<InterchangeHeader>(e =>
        {
            e.ToTable("InterchangeHeaders");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .UseIdentityColumn(1, 1);

            e.Property(x => x.FileName).HasMaxLength(260);
            e.Property(x => x.ControlNumber).HasMaxLength(20);
            e.Property(x => x.SenderId).HasMaxLength(50);
            e.Property(x => x.ReceiverId).HasMaxLength(50);
            e.Property(x => x.RawSegment).HasMaxLength(2000);
        });

        // FunctionalGroupHeader (GS)
        modelBuilder.Entity<FunctionalGroupHeader>(e =>
        {
            e.ToTable("FunctionalGroupHeaders");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .UseIdentityColumn(1, 1);

            e.HasOne(x => x.InterchangeHeader)
                .WithMany(x => x.FunctionalGroups)
                .HasForeignKey(x => x.InterchangeHeaderId);

            e.Property(x => x.FunctionalIdCode).HasMaxLength(5);
            e.Property(x => x.GroupControlNumber).HasMaxLength(20);
            e.Property(x => x.Version).HasMaxLength(20);
            e.Property(x => x.RawSegment).HasMaxLength(2000);
        });

        // TransactionSetHeader (ST+BHT)
        modelBuilder.Entity<TransactionSetHeader>(e =>
        {
            e.ToTable("TransactionSetHeaders");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .UseIdentityColumn(1, 1);


            e.HasOne(x => x.FunctionalGroupHeader)
                .WithMany(x => x.TransactionSets)
                .HasForeignKey(x => x.FunctionalGroupHeaderId);

            e.Property(x => x.TransactionSetIdCode).HasMaxLength(10);
            e.Property(x => x.TransactionSetControlNumber).HasMaxLength(20);
            e.Property(x => x.ReferenceId).HasMaxLength(50);
            e.Property(x => x.RawStSegment).HasMaxLength(2000);
            e.Property(x => x.RawBhtSegment).HasMaxLength(2000);

            e.HasIndex(x => x.TransactionSetControlNumber);
        });

        // Party (NM1)
        modelBuilder.Entity<Party>(e =>
        {
            e.ToTable("Parties");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .UseIdentityColumn(1, 1);

            e.HasOne(x => x.TransactionSetHeader)
                .WithMany(x => x.Parties)
                .HasForeignKey(x => x.TransactionSetHeaderId);

            e.Property(x => x.Role).HasMaxLength(50);
            e.Property(x => x.LastNameOrOrgName).HasMaxLength(100);
            e.Property(x => x.FirstName).HasMaxLength(60);
            e.Property(x => x.IdCodeQualifier).HasMaxLength(5);
            e.Property(x => x.IdCode).HasMaxLength(50);
            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasIndex(x => new { x.TransactionSetHeaderId, x.Role });
        });

        // Claim (CLM)
        modelBuilder.Entity<Claim>(e =>
        {
            e.ToTable("Claims");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .UseIdentityColumn(1, 1);

            e.HasOne(x => x.TransactionSetHeader)
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.TransactionSetHeaderId);

            e.HasOne(x => x.SubscriberParty)
                .WithMany()
                .HasForeignKey(x => x.SubscriberPartyId)
                .OnDelete(DeleteBehavior.NoAction);

            e.Property(x => x.PatientControlNumber).HasMaxLength(50);
            e.Property(x => x.TotalClaimChargeAmount).HasColumnType("decimal(18,2)");
            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasIndex(x => x.PatientControlNumber);
        });

        // ServiceLine (SV1)
        modelBuilder.Entity<ServiceLine>(e =>
        {
            e.ToTable("ServiceLines");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .UseIdentityColumn(1, 1);

            e.HasOne(x => x.Claim)
                .WithMany(x => x.ServiceLines)
                .HasForeignKey(x => x.ClaimId);

            e.Property(x => x.ProcedureCode).HasMaxLength(30);
            e.Property(x => x.LineItemChargeAmount).HasColumnType("decimal(18,2)");
            e.Property(x => x.UnitCount).HasColumnType("decimal(18,2)");
            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasIndex(x => x.ClaimId);
        });
    }
}
