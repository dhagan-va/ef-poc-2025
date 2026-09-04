using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayerEDI.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class Persist275Attachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "document_attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientMemberId = table.Column<string>(
                        type: "nvarchar(80)",
                        maxLength: 80,
                        nullable: true
                    ),
                    PatientMemberIdQualifier = table.Column<string>(
                        type: "nvarchar(3)",
                        maxLength: 3,
                        nullable: true
                    ),
                    ClaimReference = table.Column<string>(
                        type: "nvarchar(80)",
                        maxLength: 80,
                        nullable: true
                    ),
                    ClaimReferenceQualifier = table.Column<string>(
                        type: "nvarchar(3)",
                        maxLength: 3,
                        nullable: true
                    ),
                    SequenceNumber = table.Column<string>(
                        type: "nvarchar(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    FileName = table.Column<string>(
                        type: "nvarchar(255)",
                        maxLength: 255,
                        nullable: true
                    ),
                    ContentType = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: true
                    ),
                    DeclaredLength = table.Column<string>(
                        type: "nvarchar(32)",
                        maxLength: 32,
                        nullable: true
                    ),
                    StorageLocation = table.Column<string>(
                        type: "nvarchar(2048)",
                        maxLength: 2048,
                        nullable: true
                    ),
                    Status = table.Column<string>(
                        type: "nvarchar(32)",
                        maxLength: 32,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_document_attachment_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_document_attachment_ClaimReference",
                table: "document_attachment",
                column: "ClaimReference"
            );

            migrationBuilder.CreateIndex(
                name: "IX_document_attachment_DocumentId",
                table: "document_attachment",
                column: "DocumentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_document_attachment_PatientMemberId",
                table: "document_attachment",
                column: "PatientMemberId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "document_attachment");
        }
    }
}
