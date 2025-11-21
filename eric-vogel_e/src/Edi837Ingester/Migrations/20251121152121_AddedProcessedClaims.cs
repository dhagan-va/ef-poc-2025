using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Edi837Ingester.Migrations
{
    /// <inheritdoc />
    public partial class AddedProcessedClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimTypeId = table.Column<int>(type: "int", nullable: false),
                    ProviderNPI = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransactionControlNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Received = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimProcesses_ClaimTypes_ClaimTypeId",
                        column: x => x.ClaimTypeId,
                        principalTable: "ClaimTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClaimTypes",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "837P", "837 Professional" },
                    { 2, "837D", "837 Dental" },
                    { 3, "837I", "837 Institutional" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProcesses_ClaimTypeId",
                table: "ClaimProcesses",
                column: "ClaimTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProcesses_ProviderNPI_TransactionControlNumber_ClaimTypeId",
                table: "ClaimProcesses",
                columns: new[] { "ProviderNPI", "TransactionControlNumber", "ClaimTypeId" },
                unique: true,
                filter: "[ProviderNPI] IS NOT NULL AND [TransactionControlNumber] IS NOT NULL \r\n                            AND [ClaimTypeId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimProcesses");

            migrationBuilder.DropTable(
                name: "ClaimTypes");
        }
    }
}
