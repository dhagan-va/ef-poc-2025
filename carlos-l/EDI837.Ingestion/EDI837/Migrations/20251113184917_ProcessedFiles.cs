using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDI837.Migrations
{
    /// <inheritdoc />
    public partial class ProcessedFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
                name: "ProcessedClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimConventionReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Claim = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedClaims", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedClaims");

  
        }
    }
}
