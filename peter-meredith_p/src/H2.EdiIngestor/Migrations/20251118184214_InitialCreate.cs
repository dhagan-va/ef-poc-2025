using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H2.EdiIngestor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillingProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingProviderNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenderingProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenderingProviderNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitOrBasisForMeasurementCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCode1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCode2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCode3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCode4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ClaimId",
                table: "Services",
                column: "ClaimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Claims");
        }
    }
}
