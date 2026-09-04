using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayerEDI.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class PersistDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdiMessageType = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: false
                    ),
                    Xml = table.Column<string>(type: "xml", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.Id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "documents");
        }
    }
}
