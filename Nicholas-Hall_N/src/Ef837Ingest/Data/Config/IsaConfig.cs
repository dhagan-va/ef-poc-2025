using Ef837Ingest.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edi837Ingestion.Data.Config;

public class IsaConfig : IEntityTypeConfiguration<IsaEntity>
{
    public void Configure(EntityTypeBuilder<IsaEntity> b)
    {
        b.ToTable("ISA");
        b.HasKey(x => x.Id);
        b.Property(x => x.InterchangeControlNumber).HasMaxLength(20);
        b.Property(x => x.SenderId).HasMaxLength(30);
        b.Property(x => x.ReceiverId).HasMaxLength(30);
        b.Property(x => x.RawJson).HasColumnType("nvarchar(max)");
    }
}
