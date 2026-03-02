using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace X12EDI837Ingestion.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterchangeHeaders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    ReceivedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorizationInfoQualifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityInfoQualifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderIdQualifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceiverIdQualifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InterchangeDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ControlNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RawSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    InterchangeControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterchangeHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalGroupHeaders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterchangeHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionalIdCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SenderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceiverCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RawSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FunctionalIdentifierCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalGroupHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalGroupHeaders_InterchangeHeaders_InterchangeHeaderId",
                        column: x => x.InterchangeHeaderId,
                        principalTable: "InterchangeHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionSetHeaders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionalGroupHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionSetIdCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TransactionSetControlNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HierarchicalStructureCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TransactionSetPurposeCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RawStSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RawBhtSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TransactionSetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplementationConvention = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BhtReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSetHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionSetHeaders_FunctionalGroupHeaders_FunctionalGroupHeaderId",
                        column: x => x.FunctionalGroupHeaderId,
                        principalTable: "FunctionalGroupHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionSetHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityIdentifierCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EntityTypeQualifier = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    LastNameOrOrgName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    IdCodeQualifier = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    IdCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RawSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_TransactionSetHeaders_TransactionSetHeaderId",
                        column: x => x.TransactionSetHeaderId,
                        principalTable: "TransactionSetHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionSetHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    SubscriberPartyId = table.Column<long>(type: "bigint", nullable: true),
                    PatientControlNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalClaimChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClaimFilingIndicator = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FacilityTypeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ClaimFrequencyCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    RawSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ClaimSubmitterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Parties_SubscriberPartyId",
                        column: x => x.SubscriberPartyId,
                        principalTable: "Parties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Claims_TransactionSetHeaders_TransactionSetHeaderId",
                        column: x => x.TransactionSetHeaderId,
                        principalTable: "TransactionSetHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimId = table.Column<long>(type: "bigint", nullable: false),
                    ProcedureCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LineItemChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlaceOfService = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RawSegment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UnitOrBasis = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLines_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PatientControlNumber",
                table: "Claims",
                column: "PatientControlNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_SubscriberPartyId",
                table: "Claims",
                column: "SubscriberPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_TransactionSetHeaderId",
                table: "Claims",
                column: "TransactionSetHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalGroupHeaders_InterchangeHeaderId_GroupControlNumber",
                table: "FunctionalGroupHeaders",
                columns: new[] { "InterchangeHeaderId", "GroupControlNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_InterchangeHeaders_ControlNumber",
                table: "InterchangeHeaders",
                column: "ControlNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_TransactionSetHeaderId_Role",
                table: "Parties",
                columns: new[] { "TransactionSetHeaderId", "Role" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLines_ClaimId",
                table: "ServiceLines",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSetHeaders_FunctionalGroupHeaderId_TransactionSetControlNumber",
                table: "TransactionSetHeaders",
                columns: new[] { "FunctionalGroupHeaderId", "TransactionSetControlNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceLines");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "TransactionSetHeaders");

            migrationBuilder.DropTable(
                name: "FunctionalGroupHeaders");

            migrationBuilder.DropTable(
                name: "InterchangeHeaders");
        }
    }
}
