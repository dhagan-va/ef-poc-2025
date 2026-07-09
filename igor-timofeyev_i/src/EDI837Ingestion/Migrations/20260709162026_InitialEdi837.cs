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
                name: "Interchanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsaControlNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayerIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Npi = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawEdiFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawEdiFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FunctionalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterchangeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalGroups_Interchanges_InterchangeId",
                        column: x => x.InterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionControlNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterchangeId = table.Column<int>(type: "int", nullable: true),
                    FunctionalGroupId = table.Column<int>(type: "int", nullable: true),
                    RawEdiFileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionSets_FunctionalGroups_FunctionalGroupId",
                        column: x => x.FunctionalGroupId,
                        principalTable: "FunctionalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TransactionSets_Interchanges_InterchangeId",
                        column: x => x.InterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionSets_RawEdiFiles_RawEdiFileId",
                        column: x => x.RawEdiFileId,
                        principalTable: "RawEdiFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionSetId = table.Column<int>(type: "int", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    PayerId = table.Column<int>(type: "int", nullable: true),
                    BillingProviderId = table.Column<int>(type: "int", nullable: true),
                    RenderingProviderId = table.Column<int>(type: "int", nullable: true),
                    TotalChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClaimDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Claims_Payers_PayerId",
                        column: x => x.PayerId,
                        principalTable: "Payers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Claims_Providers_BillingProviderId",
                        column: x => x.BillingProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Claims_Providers_RenderingProviderId",
                        column: x => x.RenderingProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Claims_TransactionSets_TransactionSetId",
                        column: x => x.TransactionSetId,
                        principalTable: "TransactionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimLineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimId = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    ServiceDateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceDateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modifiers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Units = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimLineItems_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidationIssues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionSetId = table.Column<int>(type: "int", nullable: true),
                    ClaimId = table.Column<int>(type: "int", nullable: true),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationIssues_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationIssues_TransactionSets_TransactionSetId",
                        column: x => x.TransactionSetId,
                        principalTable: "TransactionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimLineItems_ClaimId",
                table: "ClaimLineItems",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_BillingProviderId",
                table: "Claims",
                column: "BillingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PatientId",
                table: "Claims",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PayerId",
                table: "Claims",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_RenderingProviderId",
                table: "Claims",
                column: "RenderingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_TransactionSetId",
                table: "Claims",
                column: "TransactionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_ClaimId",
                table: "Diagnoses",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalGroups_InterchangeId",
                table: "FunctionalGroups",
                column: "InterchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Interchanges_IsaControlNumber_SenderId_ReceiverId",
                table: "Interchanges",
                columns: new[] { "IsaControlNumber", "SenderId", "ReceiverId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payers_PayerIdentifier",
                table: "Payers",
                column: "PayerIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_Npi",
                table: "Providers",
                column: "Npi");

            migrationBuilder.CreateIndex(
                name: "IX_RawEdiFiles_FileName",
                table: "RawEdiFiles",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSets_FunctionalGroupId",
                table: "TransactionSets",
                column: "FunctionalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSets_InterchangeId",
                table: "TransactionSets",
                column: "InterchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSets_RawEdiFileId",
                table: "TransactionSets",
                column: "RawEdiFileId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSets_TransactionControlNumber",
                table: "TransactionSets",
                column: "TransactionControlNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationIssues_ClaimId",
                table: "ValidationIssues",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationIssues_TransactionSetId",
                table: "ValidationIssues",
                column: "TransactionSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimLineItems");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "ValidationIssues");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Payers");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "TransactionSets");

            migrationBuilder.DropTable(
                name: "FunctionalGroups");

            migrationBuilder.DropTable(
                name: "RawEdiFiles");

            migrationBuilder.DropTable(
                name: "Interchanges");
        }
    }
}
