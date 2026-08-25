using System;
using System.Collections.Generic;
using EdiMettle.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EdiMettle.Database;

public partial class MettleEntities : DbContext
{
    public virtual DbSet<InstitutionalClaim> InstitutionalClaims { get; set; }
    public virtual DbSet<TransactionSetHeader> TransactionSetHeaders { get; set; }
    public virtual DbSet<GroupSegment> GroupSegments { get; set; }
    public virtual DbSet<BeginningHierarchicalTransaction> BeginningHierarchicals { get; set; }

    public MettleEntities()
    {
    }

    public MettleEntities(DbContextOptions<MettleEntities> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=True;Application Name=EdiMettle");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
