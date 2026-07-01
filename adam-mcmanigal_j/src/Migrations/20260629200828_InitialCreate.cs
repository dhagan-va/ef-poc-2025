using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edi837Ingestion.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interchanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    InterchangeControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayloadS3Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interchanges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interchanges_ContentHash",
                table: "Interchanges",
                column: "ContentHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interchanges");
        }
    }
}
