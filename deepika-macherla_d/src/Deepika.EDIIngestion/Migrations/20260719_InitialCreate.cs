using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deepika.EDIIngestion.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interchanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsaControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GsControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BhtReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawIsa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawGs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawSt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawBht = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interchanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interchanges_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterchangeId = table.Column<int>(type: "int", nullable: true),
                    ProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Interchanges_InterchangeId",
                        column: x => x.InterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Claims_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_InterchangeId",
                table: "Claims",
                column: "InterchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ProviderId",
                table: "Claims",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Interchanges_ProviderId",
                table: "Interchanges",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Interchanges");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
