//using Microsoft.EntityFrameworkCore;
using EdiFabric.Core.Model.Telco;
using EdiFabric.Templates.TelcoD0;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EDI_NCPDP_Ingestion
{
    public class NCPDPContext : DbContext
    {
        public NCPDPContext() : base("name=NCDPDdb")
        {
            Configuration.AutoDetectChangesEnabled = true;
            
        }

        public NCPDPContext(object options)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<TransactionHeader> transactionHeader { get; set; }
        public DbSet<AM04> AM04 { get; set; }
        public DbSet<AM01> AM01 { get; set; }
        public DbSet<AM07> AM07 { get; set; }
        public DbSet<AM11> AM11 { get; set; }
        public DbSet<AM02> AM02 { get; set; }
        public DbSet<AM03> AM03 { get; set; }
        public DbSet<AM05> AM05 { get; set; }
        public DbSet<AM06> AM06 { get; set; }
        public DbSet<AM08> AM08 { get; set; }
        public DbSet<AM09> AM09 { get; set; }
        public DbSet<AM10> AM10 { get; set; }
        public DbSet<AM13> AM13 { get; set; }
        public DbSet<AM14> AM14 { get; set; }
        public DbSet<AM15> AM15 { get; set; }
        public DbSet<AM16> AM16 { get; set; }
        public DbSet<C2G> C2G { get; set; }
        public DbSet<C2Z> C2Z { get; set; }
        public DbSet<C4C> C4C { get; set; }
        public DbSet<C5E> C5E { get; set; }
        public DbSet<C7E> C7E { get; set; }
        public DbSet<CEC> CEC { get; set; }
        public DbSet<CH7> CH7 { get; set; }
        public DbSet<CHB> CHB { get; set; }
        public DbSet<CMU> CMU { get; set; }
        public DbSet<CNR> CNR { get; set; }
        public DbSet<CNX> CNRX { get; set; }
        public DbSet<CSE> CSE { get; set; }
        public DbSet<CVE> CVE { get; set; }
        public DbSet<CXE> CXE { get; set; }
    }
}