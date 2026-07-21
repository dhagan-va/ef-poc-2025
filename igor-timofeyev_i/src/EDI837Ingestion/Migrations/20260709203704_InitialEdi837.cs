using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDI837Ingestion.Migrations
{
    /// <inheritdoc />
    public partial class InitialEdi837 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillingProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Npi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterchangeControls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterchangeControls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberPatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberPatients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterchangeControlId = table.Column<int>(type: "int", nullable: false),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalGroups_InterchangeControls_InterchangeControlId",
                        column: x => x.InterchangeControlId,
                        principalTable: "InterchangeControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionalGroupId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmitterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmitterContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimBatches_FunctionalGroups_FunctionalGroupId",
                        column: x => x.FunctionalGroupId,
                        principalTable: "FunctionalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimBatchId = table.Column<int>(type: "int", nullable: false),
                    ClaimSubmitterIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalClaimChargeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FacilityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillingProviderId = table.Column<int>(type: "int", nullable: false),
                    SubscriberPatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalClaims_ClaimBatches_ClaimBatchId",
                        column: x => x.ClaimBatchId,
                        principalTable: "ClaimBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimDiagnoses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalClaimId = table.Column<int>(type: "int", nullable: false),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnosisType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimDiagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimDiagnoses_MedicalClaims_MedicalClaimId",
                        column: x => x.MedicalClaimId,
                        principalTable: "MedicalClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimServiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalClaimId = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineChargeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitCount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimServiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimServiceLines_MedicalClaims_MedicalClaimId",
                        column: x => x.MedicalClaimId,
                        principalTable: "MedicalClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBatches_FunctionalGroupId",
                table: "ClaimBatches",
                column: "FunctionalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDiagnoses_MedicalClaimId",
                table: "ClaimDiagnoses",
                column: "MedicalClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimServiceLines_MedicalClaimId",
                table: "ClaimServiceLines",
                column: "MedicalClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalGroups_InterchangeControlId",
                table: "FunctionalGroups",
                column: "InterchangeControlId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalClaims_ClaimBatchId",
                table: "MedicalClaims",
                column: "ClaimBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingProviders");

            migrationBuilder.DropTable(
                name: "ClaimDiagnoses");

            migrationBuilder.DropTable(
                name: "ClaimServiceLines");

            migrationBuilder.DropTable(
                name: "SubscriberPatients");

            migrationBuilder.DropTable(
                name: "MedicalClaims");

            migrationBuilder.DropTable(
                name: "ClaimBatches");

            migrationBuilder.DropTable(
                name: "FunctionalGroups");

            migrationBuilder.DropTable(
                name: "InterchangeControls");
        }
    }
}
