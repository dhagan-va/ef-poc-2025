using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X12EDI.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EdiFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IngestedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EdiEnvelope",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeIdentifyingInformationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EdiFileId = table.Column<int>(type: "int", nullable: false),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterchangeControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiEnvelope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdiEnvelope_EdiFiles_EdiFileId",
                        column: x => x.EdiFileId,
                        principalTable: "EdiFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdiErrors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EdiFileId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdiErrors_EdiFiles_EdiFileId",
                        column: x => x.EdiFileId,
                        principalTable: "EdiFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EdiTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Checksum = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EdiFileId = table.Column<int>(type: "int", nullable: false),
                    Raw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdiTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdiTransactions_EdiFiles_EdiFileId",
                        column: x => x.EdiFileId,
                        principalTable: "EdiFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EdiEnvelope_EdiFileId",
                table: "EdiEnvelope",
                column: "EdiFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EdiErrors_EdiFileId",
                table: "EdiErrors",
                column: "EdiFileId");

            migrationBuilder.CreateIndex(
                name: "IX_EdiTransactions_Checksum",
                table: "EdiTransactions",
                column: "Checksum",
                unique: true,
                filter: "[Checksum] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EdiTransactions_EdiFileId",
                table: "EdiTransactions",
                column: "EdiFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EdiEnvelope");

            migrationBuilder.DropTable(
                name: "EdiErrors");

            migrationBuilder.DropTable(
                name: "EdiTransactions");

            migrationBuilder.DropTable(
                name: "EdiFiles");
        }
    }
}
