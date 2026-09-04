using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PayerEDI.Data.Database.Tables;

namespace PayerEDI.Data.Database;

public class PayerEdiDbContext(DbContextOptions<PayerEdiDbContext> options) : DbContext(options)
{
    public DbSet<DocumentTable> Documents => Set<DocumentTable>();
    public DbSet<EdiErrorTable> EdiErrors => Set<EdiErrorTable>();
    public DbSet<EdiSegmentErrorTable> EdiSegmentErrors => Set<EdiSegmentErrorTable>();
    public DbSet<PatientTable> Patients => Set<PatientTable>();
    public DbSet<DocumentAttachmentTable> DocumentAttachments => Set<DocumentAttachmentTable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var document = modelBuilder.Entity<DocumentTable>();

        document.ToTable("documents");
        document.HasKey(item => item.Id);
        document.Property(item => item.Id).HasColumnType("uniqueidentifier").ValueGeneratedNever();
        document.Property(item => item.EdiMessageType).HasMaxLength(128).IsRequired();
        document.Property(item => item.TransactionDateTime).HasColumnType("datetime2").IsRequired();
        document.Property(item => item.Xml).HasColumnType("xml").IsRequired();

        var attachment = modelBuilder.Entity<DocumentAttachmentTable>();

        attachment.ToTable("document_attachment");
        attachment.HasKey(item => item.Id);
        attachment
            .Property(item => item.Id)
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();
        attachment.Property(item => item.DocumentId).HasColumnType("uniqueidentifier");
        attachment.Property(item => item.PatientMemberId).HasMaxLength(80);
        attachment.Property(item => item.PatientMemberIdQualifier).HasMaxLength(3);
        attachment.Property(item => item.ClaimReference).HasMaxLength(80);
        attachment.Property(item => item.ClaimReferenceQualifier).HasMaxLength(3);
        attachment.Property(item => item.SequenceNumber).HasMaxLength(20);
        attachment.Property(item => item.FileName).HasMaxLength(255);
        attachment.Property(item => item.ContentType).HasMaxLength(128);
        attachment.Property(item => item.DeclaredLength).HasMaxLength(32);
        attachment.Property(item => item.StorageLocation).HasMaxLength(2048);
        attachment.Property(item => item.Status).HasMaxLength(32).IsRequired();
        attachment.HasIndex(item => item.PatientMemberId);
        attachment.HasIndex(item => item.ClaimReference);
        attachment
            .HasOne<DocumentTable>()
            .WithMany()
            .HasForeignKey(item => item.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        var ediError = modelBuilder.Entity<EdiErrorTable>();

        ediError.ToTable("edi_error");
        ediError.HasKey(item => item.Id);
        ediError.Property(item => item.Id).HasColumnType("uniqueidentifier").ValueGeneratedNever();
        ediError.Property(item => item.DocumentId).HasColumnType("uniqueidentifier");
        ediError.Property(item => item.Name).HasMaxLength(128).IsRequired();
        ediError.Property(item => item.ControlNumber).HasMaxLength(128);
        ediError.Property(item => item.Edition).HasMaxLength(64);
        ediError.Property(item => item.Release).HasMaxLength(128);
        ediError.Property(item => item.Message).HasColumnType("nvarchar(max)");
        ConfigureCodes(ediError.Property(item => item.Codes));
        ediError
            .HasOne<DocumentTable>()
            .WithMany()
            .HasForeignKey(item => item.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        var ediSegmentError = modelBuilder.Entity<EdiSegmentErrorTable>();

        ediSegmentError.ToTable("edi_segment_error");
        ediSegmentError.HasKey(item => item.Id);
        ediSegmentError
            .Property(item => item.Id)
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();
        ediSegmentError.Property(item => item.EdiErrorId).HasColumnType("uniqueidentifier");
        ediSegmentError.Property(item => item.Message).HasColumnType("nvarchar(max)").IsRequired();
        ediSegmentError.Property(item => item.Name).HasMaxLength(128).IsRequired();
        ediSegmentError.Property(item => item.LoopId).HasMaxLength(128);
        ediSegmentError.Property(item => item.Value).HasColumnType("nvarchar(max)");
        ediSegmentError.Property(item => item.SpecRef).HasMaxLength(256);
        ConfigureCodes(ediSegmentError.Property(item => item.Codes));
        ediSegmentError
            .HasOne<EdiErrorTable>()
            .WithMany(item => item.Errors)
            .HasForeignKey(item => item.EdiErrorId)
            .OnDelete(DeleteBehavior.Cascade);

        var patient = modelBuilder.Entity<PatientTable>();

        patient.ToTable("Patients");
        patient.HasKey(item => item.Id);
        patient.Property(item => item.Id).HasColumnType("uniqueidentifier").ValueGeneratedNever();

        patient.Property(item => item.EntityType).HasMaxLength(10).IsRequired();
        patient.Property(item => item.EntityIdentifierCode).HasMaxLength(3);
        patient.Property(item => item.IdentificationCodeQualifier).HasMaxLength(2);
        patient.Property(item => item.ResponseContactIdentifier).HasMaxLength(80);
        patient.Property(item => item.LastName).HasMaxLength(60);
        patient.Property(item => item.SecondLastName).HasMaxLength(60);
        patient.Property(item => item.FirstName).HasMaxLength(35);
        patient.Property(item => item.MiddleName).HasMaxLength(25);
        patient.Property(item => item.Prefix).HasMaxLength(10);
        patient.Property(item => item.Suffix).HasMaxLength(10);
        patient.Property(item => item.OrganizationName).HasMaxLength(60);
        patient.Property(item => item.AdditionalOrganizationName).HasMaxLength(60);
        patient.Property(item => item.Relationship).HasMaxLength(2);
    }

    private static void ConfigureCodes(PropertyBuilder<string[]> property)
    {
        var converter = new ValueConverter<string[], string>(
            value => JsonSerializer.Serialize(value, (JsonSerializerOptions?)null),
            value => JsonSerializer.Deserialize<string[]>(value) ?? Array.Empty<string>()
        );
        var comparer = new ValueComparer<string[]>(
            (left, right) => left.SequenceEqual(right),
            value => value.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
            value => value.ToArray()
        );

        property.HasConversion(converter, comparer).HasColumnType("nvarchar(max)").IsRequired();
    }
}
