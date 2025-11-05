using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ef837Ingest.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionalIdCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppSenderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppReceiverCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ISA",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterchangeControlNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SegmentCount = table.Column<int>(type: "int", nullable: false),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ST",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionSetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TS837P",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TS837P", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TS850",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TS850", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GS");

            migrationBuilder.DropTable(
                name: "ISA");

            migrationBuilder.DropTable(
                name: "SE");

            migrationBuilder.DropTable(
                name: "ST");

            migrationBuilder.DropTable(
                name: "TS837P");

            migrationBuilder.DropTable(
                name: "TS850");
        }
    }
}
