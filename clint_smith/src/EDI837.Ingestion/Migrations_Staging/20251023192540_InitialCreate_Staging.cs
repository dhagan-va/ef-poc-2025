using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDI837.Ingestion.Migrations_Staging
{
    /// <inheritdoc />
    public partial class InitialCreate_Staging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimStagings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderNPI = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransactionControlNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimXml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStagings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStagings_ProviderNPI_TransactionControlNumber",
                table: "ClaimStagings",
                columns: new[] { "ProviderNPI", "TransactionControlNumber" },
                unique: true,
                filter: "[ProviderNPI] IS NOT NULL AND [TransactionControlNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimStagings");
        }
    }
}
