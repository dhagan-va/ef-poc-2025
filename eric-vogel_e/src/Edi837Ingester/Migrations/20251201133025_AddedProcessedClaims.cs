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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimXml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimTypeId = table.Column<int>(type: "int", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedClaims_ClaimTypes_ClaimTypeId",
                        column: x => x.ClaimTypeId,
                        principalTable: "ClaimTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClaimTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Professional" },
                    { 2, "Institutional" },
                    { 3, "Dental" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedClaims_ClaimTypeId",
                table: "ProcessedClaims",
                column: "ClaimTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedClaims");

            migrationBuilder.DropTable(
                name: "ClaimTypes");
        }
    }
}
