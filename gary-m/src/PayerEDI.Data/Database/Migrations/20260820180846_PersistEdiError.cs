using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayerEDI.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class PersistEdiError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "edi_error",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: false
                    ),
                    ControlNumber = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: true
                    ),
                    Edition = table.Column<string>(
                        type: "nvarchar(64)",
                        maxLength: 64,
                        nullable: true
                    ),
                    Release = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: true
                    ),
                    Index = table.Column<int>(type: "int", nullable: false),
                    ValidatedSegmentsCount = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_edi_error", x => x.Id);
                    table.ForeignKey(
                        name: "FK_edi_error_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "edi_segment_error",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdiErrorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: false
                    ),
                    Position = table.Column<int>(type: "int", nullable: false),
                    LoopId = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: true
                    ),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecRef = table.Column<string>(
                        type: "nvarchar(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    Codes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_edi_segment_error", x => x.Id);
                    table.ForeignKey(
                        name: "FK_edi_segment_error_edi_error_EdiErrorId",
                        column: x => x.EdiErrorId,
                        principalTable: "edi_error",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_edi_error_DocumentId",
                table: "edi_error",
                column: "DocumentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_edi_segment_error_EdiErrorId",
                table: "edi_segment_error",
                column: "EdiErrorId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "edi_segment_error");

            migrationBuilder.DropTable(name: "edi_error");
        }
    }
}
