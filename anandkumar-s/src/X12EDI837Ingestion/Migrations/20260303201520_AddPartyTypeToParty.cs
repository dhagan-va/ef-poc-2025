using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X12EDI837Ingestion.Migrations
{
    /// <inheritdoc />
    public partial class AddPartyTypeToParty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartyType",
                table: "Parties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartyType",
                table: "Parties");
        }
    }
}
