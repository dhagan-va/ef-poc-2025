using EDI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EDI.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(EdiDbContext))]
    [Migration("20251118090000_AddFileIngestionDetailsView")]
    public partial class AddFileIngestionDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlScripts.EdiFileIngestionDetailsView);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID('dbo.vw_EdiFileIngestionDetails', 'V') IS NOT NULL
                    DROP VIEW dbo.vw_EdiFileIngestionDetails;
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
