using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edi837Ingestion.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionSetTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DentalTransactionSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngestedInterchangeId = table.Column<int>(type: "int", nullable: false),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    TransactionSetControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalTransactionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DentalTransactionSets_Interchanges_IngestedInterchangeId",
                        column: x => x.IngestedInterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionalTransactionSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngestedInterchangeId = table.Column<int>(type: "int", nullable: false),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    TransactionSetControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionalTransactionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionalTransactionSets_Interchanges_IngestedInterchangeId",
                        column: x => x.IngestedInterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalTransactionSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngestedInterchangeId = table.Column<int>(type: "int", nullable: false),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    TransactionSetControlNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalTransactionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessionalTransactionSets_Interchanges_IngestedInterchangeId",
                        column: x => x.IngestedInterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DentalTransactionSets_IngestedInterchangeId",
                table: "DentalTransactionSets",
                column: "IngestedInterchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalTransactionSets_IngestedInterchangeId",
                table: "InstitutionalTransactionSets",
                column: "IngestedInterchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalTransactionSets_IngestedInterchangeId",
                table: "ProfessionalTransactionSets",
                column: "IngestedInterchangeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentalTransactionSets");

            migrationBuilder.DropTable(
                name: "InstitutionalTransactionSets");

            migrationBuilder.DropTable(
                name: "ProfessionalTransactionSets");
        }
    }
}
