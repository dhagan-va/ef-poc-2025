
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
            e.Property(x => x.Id).UseIdentityColumn(1, 1);

            e.Property(x => x.FileName).HasMaxLength(260);
            e.Property(x => x.ReceivedAtUtc).HasColumnType("datetime2");

            e.Property(x => x.SenderId).HasMaxLength(50);
            e.Property(x => x.ReceiverId).HasMaxLength(50);

            e.Property(x => x.InterchangeDateTime).HasColumnType("datetime2");
            e.Property(x => x.ControlNumber).HasMaxLength(20); // ISA13

            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasMany(x => x.FunctionalGroups)
                .WithOne(x => x.InterchangeHeader)
                .HasForeignKey(x => x.InterchangeHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.ControlNumber);
        });

        // FunctionalGroupHeader (GS)
        modelBuilder.Entity<FunctionalGroupHeader>(e =>
        {
            e.ToTable("FunctionalGroupHeaders");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn(1, 1);

            e.HasOne(x => x.InterchangeHeader)
                .WithMany(x => x.FunctionalGroups)
                .HasForeignKey(x => x.InterchangeHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(x => x.FunctionalIdCode).HasMaxLength(5); // GS01 (HC)
            e.Property(x => x.SenderCode).HasMaxLength(50);      // GS02
            e.Property(x => x.ReceiverCode).HasMaxLength(50);    // GS03

            e.Property(x => x.GroupDateTime).HasColumnType("datetime2"); // GS04+GS05
            e.Property(x => x.GroupControlNumber).HasMaxLength(20);      // GS06
            e.Property(x => x.Version).HasMaxLength(20);                 // GS08

            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasMany(x => x.TransactionSets)
                .WithOne(x => x.FunctionalGroupHeader)
                .HasForeignKey(x => x.FunctionalGroupHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.InterchangeHeaderId, x.GroupControlNumber });
        });

        // TransactionSetHeader (ST + BHT)
        modelBuilder.Entity<TransactionSetHeader>(e =>
        {
            e.ToTable("TransactionSetHeaders");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);

            e.HasOne(x => x.FunctionalGroupHeader)
                .WithMany(x => x.TransactionSets)
                .HasForeignKey(x => x.FunctionalGroupHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ST
            e.Property(x => x.TransactionSetIdCode).HasMaxLength(10);        // ST01
            e.Property(x => x.TransactionSetControlNumber).HasMaxLength(20); // ST02
            e.Property(x => x.ImplementationConvention).HasMaxLength(20);    // ST03

            // BHT
            e.Property(x => x.HierarchicalStructureCode).HasMaxLength(10); // BHT01
            e.Property(x => x.TransactionSetPurposeCode).HasMaxLength(5);  // BHT02
            e.Property(x => x.ReferenceId).HasMaxLength(50);               // BHT03
            e.Property(x => x.TransactionDateTime).HasColumnType("datetime2");

            e.Property(x => x.RawStSegment).HasMaxLength(2000);
            e.Property(x => x.RawBhtSegment).HasMaxLength(2000);

            e.HasMany(x => x.Parties)
                .WithOne(x => x.TransactionSetHeader)
                .HasForeignKey(x => x.TransactionSetHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Claims)
                .WithOne(x => x.TransactionSetHeader)
                .HasForeignKey(x => x.TransactionSetHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.FunctionalGroupHeaderId, x.TransactionSetControlNumber });
        });

        // Party (NM1)
        modelBuilder.Entity<Party>(e =>
        {
            e.ToTable("Parties");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn(1, 1);

            e.HasOne(x => x.TransactionSetHeader)
                .WithMany(x => x.Parties)
                .HasForeignKey(x => x.TransactionSetHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(x => x.Role).IsRequired().HasMaxLength(50);

            e.Property(x => x.EntityIdentifierCode).HasMaxLength(5);
            e.Property(x => x.EntityTypeQualifier).HasMaxLength(2);
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
            e.Property(x => x.Id).UseIdentityColumn(1, 1);

            e.HasOne(x => x.TransactionSetHeader)
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.TransactionSetHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.SubscriberParty)
                .WithMany()
                .HasForeignKey(x => x.SubscriberPartyId)
                .OnDelete(DeleteBehavior.NoAction);

            e.Property(x => x.PatientControlNumber).HasMaxLength(50);           // CLM01
            e.Property(x => x.TotalClaimChargeAmount).HasColumnType("decimal(18,2)"); // CLM02
            e.Property(x => x.FacilityTypeCode).HasMaxLength(10);              // CLM05-1 (simplified)
            e.Property(x => x.ClaimFrequencyCode).HasMaxLength(5);             // CLM05-3

            e.Property(x => x.ClaimFilingIndicator).HasMaxLength(10);

            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasIndex(x => x.PatientControlNumber);
        });

        // ServiceLine (SV1)
        modelBuilder.Entity<ServiceLine>(e =>
        {
            e.ToTable("ServiceLines");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn(1, 1);

            e.HasOne(x => x.Claim)
                .WithMany(x => x.ServiceLines)
                .HasForeignKey(x => x.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(x => x.ProcedureCode).HasMaxLength(30);
            e.Property(x => x.LineItemChargeAmount).HasColumnType("decimal(18,2)");
            e.Property(x => x.UnitOrBasis).HasMaxLength(5);
            e.Property(x => x.UnitCount).HasColumnType("decimal(18,2)");
            e.Property(x => x.PlaceOfService).HasMaxLength(10);

            e.Property(x => x.RawSegment).HasMaxLength(2000);

            e.HasIndex(x => x.ClaimId);
        });
    }
}
