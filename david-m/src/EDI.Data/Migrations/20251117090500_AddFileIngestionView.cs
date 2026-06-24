using EDI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDI.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(EdiDbContext))]
    [Migration("20251117090500_AddFileIngestionView")]
    public partial class AddFileIngestionView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlScripts.EdiFileIngestionView);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID('dbo.vw_EdiFileIngestion', 'V') IS NOT NULL
                    DROP VIEW dbo.vw_EdiFileIngestion;
                """);
        }

        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            new SnapshotInvoker().Invoke(modelBuilder);
        }

        private sealed class SnapshotInvoker : EdiDbContextModelSnapshot
        {
            public void Invoke(ModelBuilder modelBuilder) => base.BuildModel(modelBuilder);
        }
    }
}
