using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edi837Ingester.Migrations
{
    /// <inheritdoc />
    public partial class AddedTS837P : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "All_REF_837P_5",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMT_CoordinationofBenefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT_CoordinationofBenefits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMT_CoordinationofBenefits_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT_CoordinationofBenefits_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMT_PatientAmountPaid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT_PatientAmountPaid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMT_PostageClaimedAmount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT_PostageClaimedAmount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMT_RemainingPatientLiability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT_RemainingPatientLiability", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMT_SalesTaxAmount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT_SalesTaxAmount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BHT_BeginningOfHierarchicalTransaction_8",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HierarchicalStructureCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetPurposeCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmitterTransactionIdentifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetCreationDate_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetCreationTime_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionTypeCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHT_BeginningOfHierarchicalTransaction_8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C001_CompositeUnitOfMeasure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitorBasisforMeasurementCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_15 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C001_CompositeUnitOfMeasure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C001_CompositeUnitOfMeasure_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitorBasisforMeasurementCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exponent_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_15 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C001_CompositeUnitOfMeasure_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C002_ActionsIndicated_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaperworkReportActionCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaperworkReportActionCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaperworkReportActionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaperworkReportActionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaperworkReportActionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C002_ActionsIndicated_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C003_CompositeMedicalProcedureIdentifier_12",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductorServiceIDQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_08 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C003_CompositeMedicalProcedureIdentifier_12", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C003_CompositeMedicalProcedureIdentifier_9",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductorServiceIDQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureModifier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_08 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C003_CompositeMedicalProcedureIdentifier_9", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C004_CompositeDiagnosisCodePointer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiagnosisCodePointer_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCodePointer_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCodePointer_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCodePointer_04 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C004_CompositeDiagnosisCodePointer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022_HealthCareCodeInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022_HealthCareCodeInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022_HealthCareCodeInformation_12",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022_HealthCareCodeInformation_12", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022_HealthCareCodeInformation_13",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022_HealthCareCodeInformation_13", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022_HealthCareCodeInformation_15",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022_HealthCareCodeInformation_15", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022_HealthCareCodeInformation_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022_HealthCareCodeInformation_4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022_HealthCareCodeInformation_8",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022_HealthCareCodeInformation_8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C023_HealthCareServiceLocationInformation_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityTypeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityCodeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimFrequencyTypeCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C023_HealthCareServiceLocationInformation_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C024_RelatedCausesInformation_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelatedCausesCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedCausesCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedCausesCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateorProvinceCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C024_RelatedCausesInformation_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C035_ProviderSpecialtyInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderSpecialtyCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyQualifierCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C035_ProviderSpecialtyInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C040_ReferenceIdentifier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C040_ReferenceIdentifier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C040_ReferenceIdentifier_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C040_ReferenceIdentifier_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C040_ReferenceIdentifier_7",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C040_ReferenceIdentifier_7", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CN1_ContractInformation_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractTypeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractPercentage_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsDiscountPercentage_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractVersionIdentifier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CN1_ContractInformation_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR1_AmbulanceTransportInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitorBasisforMeasurementCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientWeight_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbulanceTransportCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbulanceTransportReasonCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportDistance_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressInformation_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressInformation_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoundTripPurposeDescription_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StretcherPurposeDescription_10 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR1_AmbulanceTransportInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR2_SpinalManipulationServiceInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubluxationLevelCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubluxationLevelCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientConditionCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientConditionDescription_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientConditionDescription_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR2_SpinalManipulationServiceInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR3_DurableMedicalEquipmentCertification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificationTypeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurableMedicalEquipmentDuration_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsulinDependentCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_05 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR3_DurableMedicalEquipmentCertification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRC_ConditionIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCategory_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationConditionIndicator_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC_ConditionIndicator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRC_EPSDTReferral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCategory_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationConditionIndicator_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC_EPSDTReferral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRC_HomeboundIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCategory_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationConditionIndicator_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC_HomeboundIndicator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRC_HospiceEmployeeIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCategory_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationConditionIndicator_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC_HospiceEmployeeIndicator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CUR_ForeignCurrencyInformation_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyMarketExchangeCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeQualifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeQualifier_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeQualifier_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time_15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeQualifier_16 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_17 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time_18 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeQualifier_19 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_20 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time_21 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUR_ForeignCurrencyInformation_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DMG_PatientDemographicInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimePeriodFormatQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DependentBirthDate_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DependentGenderCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatusCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitizenshipStatusCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasisofVerificationCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeListQualifierCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DMG_PatientDemographicInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_AccidentDate_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_AccidentDate_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_AcuteManifestation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_AcuteManifestation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_AdmissionDate_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_AdmissionDate_4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_AuthorizedReturntoWork",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_AuthorizedReturntoWork", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_BeginTherapyDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_BeginTherapyDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_CertificationRevision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_CertificationRevision", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_ClaimCheckOrRemittanceDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_ClaimCheckOrRemittanceDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_ClaimLevelServiceDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_ClaimLevelServiceDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_DisabilityDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_DisabilityDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_Discharge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_Discharge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_HearingandVisionPrescriptionDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_HearingandVisionPrescriptionDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_InitialTreatmentDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_InitialTreatmentDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_LastCertificationDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_LastCertificationDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_LastMenstrualPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_LastMenstrualPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_LastSeenDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_LastSeenDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_LastWorked",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_LastWorked", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_LastXrayDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_LastXrayDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_OnsetofCurrentIllnessorSymptom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_OnsetofCurrentIllnessorSymptom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_PropertyandCasualtyDateofFirstContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_PropertyandCasualtyDateofFirstContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_RepricerReceivedDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_RepricerReceivedDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DTP_ShippedDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_ShippedDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HCP_ClaimPricing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricingMethodology_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricedAllowedAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricedSavingAmount_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricingOrganizationIdentifier_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricingPerDiemorFlatRateAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricedApprovedAmbulatoryPatientGroupCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectReasonCode_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyComplianceCode_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionCode_15 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HCP_ClaimPricing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HCP_LinePricing_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricingMethodology_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricedAllowedAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricedSavingAmount_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricingOrganizationIdentifier_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricingPerDiemorFlatRateAmount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepricedApprovedAmbulatoryPatientGroupCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectReasonCode_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyComplianceCode_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionCode_15 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HCP_LinePricing_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HL_BillingProviderHierarchicalLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HierarchicalIDNumber_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalParentIDNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalLevelCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalChildCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL_BillingProviderHierarchicalLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HL_DependentLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HierarchicalIDNumber_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalParentIDNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalLevelCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalChildCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL_DependentLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HL_SubscriberHierarchicalLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HierarchicalIDNumber_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalParentIDNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalLevelCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchicalChildCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL_SubscriberHierarchicalLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LIN_DrugIdentification_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedIdentification_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductorServiceIDQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalDrugCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_16 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_17 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_18 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_19 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_20 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_21 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_22 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_23 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_24 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_25 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_26 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_27 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_28 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_29 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceIDQualifier_30 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductServiceID_31 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIN_DrugIdentification_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LQ_FormIdentificationCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeListQualifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormIdentifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LQ_FormIdentificationCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LX_HeaderNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedNumber_01 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LX_HeaderNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MOA_OutpatientAdjudicationInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReimbursementRate_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HCPCSPayableAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimPaymentRemarkCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimPaymentRemarkCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimPaymentRemarkCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimPaymentRemarkCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimPaymentRemarkCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NonPayableProfessionalComponentBilledAmount_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOA_OutpatientAdjudicationInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "N3_AdditionalPatientInformationContactAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponseContactAddressLine_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactAddressLine_02 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N3_AdditionalPatientInformationContactAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "N4_AdditionalPatientInformationContactCity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdditionalPatientInformationContactCityName_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalPatientInformationContactStateCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalPatientInformationContactPostalZoneorZIPCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationIdentifier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountrySubdivisionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N4_AdditionalPatientInformationContactCity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_AmbulanceDrop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_AmbulanceDrop", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_AmbulancePick",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_AmbulancePick", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_BillingProviderName_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_BillingProviderName_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_InformationReceiverName_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_InformationReceiverName_4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OrderingProviderName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OrderingProviderName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherPayerBillingProvider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherPayerBillingProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherPayerName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherPayerName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherPayerReferringProvider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherPayerReferringProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherPayerRenderingProvider_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherPayerRenderingProvider_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherPayerServiceFacilityLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherPayerServiceFacilityLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherPayerSupervisingProvider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherPayerSupervisingProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_OtherSubscriberName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_OtherSubscriberName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_PatientName_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_PatientName_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_Pay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_Pay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_Pay_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_Pay_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_PurchasedServiceProviderName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_PurchasedServiceProviderName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_ReceiverName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_ReceiverName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_ReferringProviderName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_ReferringProviderName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_RenderingProviderName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_RenderingProviderName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_ServiceFacilityLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_ServiceFacilityLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_SubscriberName_5",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_SubscriberName_5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1_SupervisingProviderName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactLastorOrganizationName_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactFirstName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactMiddleName_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactNameSuffix_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactIdentifier_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLastorOrganizationName_12 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1_SupervisingProviderName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NTE_ClaimNote_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteReferenceCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingNoteText_02 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTE_ClaimNote_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NTE_LineNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteReferenceCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingNoteText_02 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTE_LineNote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NTE_ThirdPartyOrganizationNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteReferenceCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingNoteText_02 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTE_ThirdPartyOrganizationNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OI_OtherInsuranceCoverageInformation_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimFilingIndicatorCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimSubmissionReasonCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefitsAssignmentCertificationIndicator_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSignatureSourceCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderAgreementCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseofInformationCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OI_OtherInsuranceCoverageInformation_2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PAT_PatientInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndividualRelationshipCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientLocationCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentStatusCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStatusCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientDeathDate_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientWeight_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PregnancyIndicator_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAT_PatientInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PAT_PatientInformation_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndividualRelationshipCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientLocationCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentStatusCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStatusCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientDeathDate_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientWeight_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PregnancyIndicator_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAT_PatientInformation_3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PER_PropertyandCasualtyPatientContactInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactFunctionCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactName_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationNumberQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactCommunicationNumber_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationNumberQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactCommunicationNumber_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationNumberQualifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactCommunicationNumber_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactInquiryReference_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PER_PropertyandCasualtyPatientContactInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PS1_PurchasedServiceInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchasedServiceProviderIdentifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasedServiceChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateorProvinceCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS1_PurchasedServiceInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SBR_OtherSubscriberInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayerResponsibilitySequenceNumberCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndividualRelationshipCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuredGrouporPolicyNumber_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherInsuredGroupName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceTypeCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoordinationofBenefitsCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentStatusCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimFilingIndicatorCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBR_OtherSubscriberInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SBR_SubscriberInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayerResponsibilitySequenceNumberCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndividualRelationshipCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuredGrouporPolicyNumber_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherInsuredGroupName_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceTypeCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoordinationofBenefitsCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentStatusCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimFilingIndicatorCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBR_SubscriberInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberofIncludedSegments_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetControlNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionSetIdentifierCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionSetControlNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplementationConventionPreference_03 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "All_AMT_837P_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AMT_CoordinationofBenefits_COB_PayerPaidAmountId = table.Column<int>(type: "int", nullable: true),
                    AMT_CoordinationofBenefits_COB_TotalNon_AmountId = table.Column<int>(type: "int", nullable: true),
                    AMT_RemainingPatientLiabilityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_AMT_837P_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_AMT_837P_2_AMT_CoordinationofBenefits_2_AMT_CoordinationofBenefits_COB_TotalNon_AmountId",
                        column: x => x.AMT_CoordinationofBenefits_COB_TotalNon_AmountId,
                        principalTable: "AMT_CoordinationofBenefits_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_AMT_837P_2_AMT_CoordinationofBenefits_AMT_CoordinationofBenefits_COB_PayerPaidAmountId",
                        column: x => x.AMT_CoordinationofBenefits_COB_PayerPaidAmountId,
                        principalTable: "AMT_CoordinationofBenefits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_AMT_837P_2_AMT_RemainingPatientLiability_AMT_RemainingPatientLiabilityId",
                        column: x => x.AMT_RemainingPatientLiabilityId,
                        principalTable: "AMT_RemainingPatientLiability",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_AMT_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AMT_SalesTaxAmountId = table.Column<int>(type: "int", nullable: true),
                    AMT_PostageClaimedAmountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_AMT_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_AMT_837P_AMT_PostageClaimedAmount_AMT_PostageClaimedAmountId",
                        column: x => x.AMT_PostageClaimedAmountId,
                        principalTable: "AMT_PostageClaimedAmount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_AMT_837P_AMT_SalesTaxAmount_AMT_SalesTaxAmountId",
                        column: x => x.AMT_SalesTaxAmountId,
                        principalTable: "AMT_SalesTaxAmount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QTY_AmbulancePatientCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbulancePatientCount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeUnitOfMeasure_03Id = table.Column<int>(type: "int", nullable: true),
                    FreeformInformation_04 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QTY_AmbulancePatientCount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QTY_AmbulancePatientCount_C001_CompositeUnitOfMeasure_CompositeUnitOfMeasure_03Id",
                        column: x => x.CompositeUnitOfMeasure_03Id,
                        principalTable: "C001_CompositeUnitOfMeasure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QTY_ObstetricAnesthesiaAdditionalUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbulancePatientCount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeUnitOfMeasure_03Id = table.Column<int>(type: "int", nullable: true),
                    FreeformInformation_04 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QTY_ObstetricAnesthesiaAdditionalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QTY_ObstetricAnesthesiaAdditionalUnits_C001_CompositeUnitOfMeasure_CompositeUnitOfMeasure_03Id",
                        column: x => x.CompositeUnitOfMeasure_03Id,
                        principalTable: "C001_CompositeUnitOfMeasure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CTP_DrugQuantity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassofTradeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceIdentifierCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalDrugUnitCount_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeUnitOfMeasure_05Id = table.Column<int>(type: "int", nullable: true),
                    PriceMultiplierQualifier_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Multiplier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasisofUnitPriceCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionValue_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultiplePriceQuantity_11 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTP_DrugQuantity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CTP_DrugQuantity_C001_CompositeUnitOfMeasure_2_CompositeUnitOfMeasure_05Id",
                        column: x => x.CompositeUnitOfMeasure_05Id,
                        principalTable: "C001_CompositeUnitOfMeasure_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentReportTypeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportTransmissionCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportCopiesNeeded_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentControlNumber_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentDescription_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionsIndicated_08Id = table.Column<int>(type: "int", nullable: true),
                    RequestCategoryCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator_C002_ActionsIndicated_2_ActionsIndicated_08Id",
                        column: x => x.ActionsIndicated_08Id,
                        principalTable: "C002_ActionsIndicated_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SVD_LineAdjudicationInformation_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OtherPayerPrimaryIdentifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceLinePaidAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeMedicalProcedureIdentifier_03Id = table.Column<int>(type: "int", nullable: true),
                    ProductServiceID_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidServiceUnitCount_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BundledorUnbundledLineNumber_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SVD_LineAdjudicationInformation_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SVD_LineAdjudicationInformation_3_C003_CompositeMedicalProcedureIdentifier_12_CompositeMedicalProcedureIdentifier_03Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_03Id,
                        principalTable: "C003_CompositeMedicalProcedureIdentifier_12",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV5_DurableMedicalEquipmentService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompositeMedicalProcedureIdentifier_01Id = table.Column<int>(type: "int", nullable: true),
                    UnitorBasisforMeasurementCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LengthofMedicalNecessity_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DMERentalPrice_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DMEPurchasePrice_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentalUnitPriceIndicator_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrognosisCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV5_DurableMedicalEquipmentService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV5_DurableMedicalEquipmentService_C003_CompositeMedicalProcedureIdentifier_9_CompositeMedicalProcedureIdentifier_01Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_01Id,
                        principalTable: "C003_CompositeMedicalProcedureIdentifier_9",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV1_ProfessionalService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompositeMedicalProcedureIdentifier_01Id = table.Column<int>(type: "int", nullable: true),
                    LineItemChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitorBasisforMeasurementCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceUnitCount_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceofServiceCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypeCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeDiagnosisCodePointer_07Id = table.Column<int>(type: "int", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyIndicator_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultipleProcedureCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EPSDTIndicator_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyPlanningIndicator_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewCode_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalorLocalAssignedReviewValue_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoPayStatusCode_15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthCareProfessionalShortageAreaCode_16 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentification_17 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode_18 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonetaryAmount_19 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelofCareCode_20 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderAgreementCode_21 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV1_ProfessionalService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV1_ProfessionalService_C003_CompositeMedicalProcedureIdentifier_12_CompositeMedicalProcedureIdentifier_01Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_01Id,
                        principalTable: "C003_CompositeMedicalProcedureIdentifier_12",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SV1_ProfessionalService_C004_CompositeDiagnosisCodePointer_CompositeDiagnosisCodePointer_07Id",
                        column: x => x.CompositeDiagnosisCodePointer_07Id,
                        principalTable: "C004_CompositeDiagnosisCodePointer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HI_AnesthesiaRelatedProcedure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthCareCodeInformation_01Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_02Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_03Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_04Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_05Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_06Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_07Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_08Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_09Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_10Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_11Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_12Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HI_AnesthesiaRelatedProcedure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_12_HealthCareCodeInformation_01Id",
                        column: x => x.HealthCareCodeInformation_01Id,
                        principalTable: "C022_HealthCareCodeInformation_12",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_15_HealthCareCodeInformation_02Id",
                        column: x => x.HealthCareCodeInformation_02Id,
                        principalTable: "C022_HealthCareCodeInformation_15",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_03Id",
                        column: x => x.HealthCareCodeInformation_03Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_04Id",
                        column: x => x.HealthCareCodeInformation_04Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_05Id",
                        column: x => x.HealthCareCodeInformation_05Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_06Id",
                        column: x => x.HealthCareCodeInformation_06Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_07Id",
                        column: x => x.HealthCareCodeInformation_07Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_08Id",
                        column: x => x.HealthCareCodeInformation_08Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_09Id",
                        column: x => x.HealthCareCodeInformation_09Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_10Id",
                        column: x => x.HealthCareCodeInformation_10Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_11Id",
                        column: x => x.HealthCareCodeInformation_11Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_AnesthesiaRelatedProcedure_C022_HealthCareCodeInformation_HealthCareCodeInformation_12Id",
                        column: x => x.HealthCareCodeInformation_12Id,
                        principalTable: "C022_HealthCareCodeInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HI_DependentHealthCareDiagnosisCode_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthCareCodeInformation_01Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_02Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_03Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_04Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_05Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_06Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_07Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_08Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_09Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_10Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_11Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_12Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HI_DependentHealthCareDiagnosisCode_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_02Id",
                        column: x => x.HealthCareCodeInformation_02Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_03Id",
                        column: x => x.HealthCareCodeInformation_03Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_04Id",
                        column: x => x.HealthCareCodeInformation_04Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_05Id",
                        column: x => x.HealthCareCodeInformation_05Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_06Id",
                        column: x => x.HealthCareCodeInformation_06Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_07Id",
                        column: x => x.HealthCareCodeInformation_07Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_08Id",
                        column: x => x.HealthCareCodeInformation_08Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_09Id",
                        column: x => x.HealthCareCodeInformation_09Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_10Id",
                        column: x => x.HealthCareCodeInformation_10Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_11Id",
                        column: x => x.HealthCareCodeInformation_11Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_4_HealthCareCodeInformation_12Id",
                        column: x => x.HealthCareCodeInformation_12Id,
                        principalTable: "C022_HealthCareCodeInformation_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_DependentHealthCareDiagnosisCode_2_C022_HealthCareCodeInformation_8_HealthCareCodeInformation_01Id",
                        column: x => x.HealthCareCodeInformation_01Id,
                        principalTable: "C022_HealthCareCodeInformation_8",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CLM_ClaimInformation_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientControlNumber_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimFilingIndicatorCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NonInstitutionalClaimTypeCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthCareServiceLocationInformation_05Id = table.Column<int>(type: "int", nullable: true),
                    ProviderorSupplierSignatureIndicator_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentorPlanParticipationCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefitsAssignmentCertificationIndicator_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseofInformationCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSignatureSourceCode_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedCausesInformation_11Id = table.Column<int>(type: "int", nullable: true),
                    SpecialProgramIndicator_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelofServiceCode_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderAgreementCode_16 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimStatusCode_17 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YesNoConditionorResponseCode_18 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredeterminationofBenefitsCode_19 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelayReasonCode_20 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLM_ClaimInformation_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CLM_ClaimInformation_3_C023_HealthCareServiceLocationInformation_2_HealthCareServiceLocationInformation_05Id",
                        column: x => x.HealthCareServiceLocationInformation_05Id,
                        principalTable: "C023_HealthCareServiceLocationInformation_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CLM_ClaimInformation_3_C024_RelatedCausesInformation_3_RelatedCausesInformation_11Id",
                        column: x => x.RelatedCausesInformation_11Id,
                        principalTable: "C024_RelatedCausesInformation_3",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PRV_BillingProviderSpecialtyInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderTaxonomyCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateorProvinceCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderSpecialtyInformation_05Id = table.Column<int>(type: "int", nullable: true),
                    ProviderOrganizationCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRV_BillingProviderSpecialtyInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRV_BillingProviderSpecialtyInformation_C035_ProviderSpecialtyInformation_ProviderSpecialtyInformation_05Id",
                        column: x => x.ProviderSpecialtyInformation_05Id,
                        principalTable: "C035_ProviderSpecialtyInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PRV_RenderingProviderSpecialtyInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentificationQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderTaxonomyCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateorProvinceCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderSpecialtyInformation_05Id = table.Column<int>(type: "int", nullable: true),
                    ProviderOrganizationCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRV_RenderingProviderSpecialtyInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRV_RenderingProviderSpecialtyInformation_C035_ProviderSpecialtyInformation_ProviderSpecialtyInformation_05Id",
                        column: x => x.ProviderSpecialtyInformation_05Id,
                        principalTable: "C035_ProviderSpecialtyInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_AdjustedRepricedClaimNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_AdjustedRepricedClaimNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_AdjustedRepricedClaimNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_AdjustedRepricedLineItemReferenceNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_AdjustedRepricedLineItemReferenceNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_AdjustedRepricedLineItemReferenceNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_BillingProviderTaxIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_BillingProviderTaxIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderTaxIdentification_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_BillingProviderTaxIdentification_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_BillingProviderTaxIdentification_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderTaxIdentification_2_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_CarePlanOversight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_CarePlanOversight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_CarePlanOversight_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries_C040_ReferenceIdentifier_ReferenceIdentifie~",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_ClinicalLaboratoryImprovementAmendment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_ClinicalLaboratoryImprovementAmendment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_ClinicalLaboratoryImprovementAmendment_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_DemonstrationProjectIdentifier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_DemonstrationProjectIdentifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_DemonstrationProjectIdentifier_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_ImmunizationBatchNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_ImmunizationBatchNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_ImmunizationBatchNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_InvestigationalDeviceExemptionNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_InvestigationalDeviceExemptionNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_InvestigationalDeviceExemptionNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_LineItemControlNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_LineItemControlNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_LineItemControlNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_MammographyCertificationNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_MammographyCertificationNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_MammographyCertificationNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_MandatoryMedicare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_MandatoryMedicare", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_MandatoryMedicare_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_MedicalRecordNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_MedicalRecordNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_MedicalRecordNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerClaimAdjustmentIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerClaimAdjustmentIndicator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerClaimAdjustmentIndicator_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerClaimControlNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerClaimControlNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerClaimControlNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerPriorAuthorizationNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerPriorAuthorizationNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerPriorAuthorizationNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerReferralNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerReferralNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerReferralNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherSubscriberSecondaryIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherSubscriberSecondaryIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherSubscriberSecondaryIdentification_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_Pay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_Pay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_Pay_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_PrescriptionorCompoundDrugAssociationNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_PrescriptionorCompoundDrugAssociationNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_PrescriptionorCompoundDrugAssociationNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_PropertyandCasualtyClaimNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_PropertyandCasualtyClaimNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_PropertyandCasualtyClaimNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_ReferringClinicalLaboratoryImprovementAmendment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_ReferringClinicalLaboratoryImprovementAmendment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_ReferringClinicalLaboratoryImprovementAmendment_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_RepricedClaimNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_RepricedClaimNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_RepricedClaimNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_RepricedLineItemReferenceNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_RepricedLineItemReferenceNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_RepricedLineItemReferenceNumber_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_ServiceAuthorizationExceptionCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_ServiceAuthorizationExceptionCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_ServiceAuthorizationExceptionCode_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_PropertyandCasualtyPatientIdentifier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_PropertyandCasualtyPatientIdentifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_PropertyandCasualtyPatientIdentifier_C040_ReferenceIdentifier_7_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier_7",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_CRC_837P_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRC_HomeboundIndicatorId = table.Column<int>(type: "int", nullable: true),
                    CRC_EPSDTReferralId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_CRC_837P_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_CRC_837P_2_CRC_EPSDTReferral_CRC_EPSDTReferralId",
                        column: x => x.CRC_EPSDTReferralId,
                        principalTable: "CRC_EPSDTReferral",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_CRC_837P_2_CRC_HomeboundIndicator_CRC_HomeboundIndicatorId",
                        column: x => x.CRC_HomeboundIndicatorId,
                        principalTable: "CRC_HomeboundIndicator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_CRC_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRC_HospiceEmployeeIndicatorId = table.Column<int>(type: "int", nullable: true),
                    CRC_ConditionIndicator_DurableMedicalEquipmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_CRC_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_CRC_837P_CRC_ConditionIndicator_CRC_ConditionIndicator_DurableMedicalEquipmentId",
                        column: x => x.CRC_ConditionIndicator_DurableMedicalEquipmentId,
                        principalTable: "CRC_ConditionIndicator",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_CRC_837P_CRC_HospiceEmployeeIndicator_CRC_HospiceEmployeeIndicatorId",
                        column: x => x.CRC_HospiceEmployeeIndicatorId,
                        principalTable: "CRC_HospiceEmployeeIndicator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "C056_CompositeRaceOrEthnicityInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceorEthnicityCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeListQualifierCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DMG_PatientDemographicInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C056_CompositeRaceOrEthnicityInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C056_CompositeRaceOrEthnicityInformation_DMG_PatientDemographicInformation_DMG_PatientDemographicInformationId",
                        column: x => x.DMG_PatientDemographicInformationId,
                        principalTable: "DMG_PatientDemographicInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_DTP_837P_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DTP_Date_OnsetofCurrentIllnessorSymptomId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_InitialTreatmentDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastSeenDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_AcuteManifestationId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_AccidentId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastMenstrualPeriodId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastX_DateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_HearingandVisionPrescriptionDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_DisabilityDatesId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastWorkedId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_AuthorizedReturntoWorkId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_AdmissionId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_DischargeId = table.Column<int>(type: "int", nullable: true),
                    DTP_PropertyandCasualtyDateofFirstContactId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_RepricerReceivedDateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_DTP_837P_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_AccidentDate_2_DTP_Date_AccidentId",
                        column: x => x.DTP_Date_AccidentId,
                        principalTable: "DTP_AccidentDate_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_AcuteManifestation_DTP_Date_AcuteManifestationId",
                        column: x => x.DTP_Date_AcuteManifestationId,
                        principalTable: "DTP_AcuteManifestation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_AdmissionDate_4_DTP_Date_AdmissionId",
                        column: x => x.DTP_Date_AdmissionId,
                        principalTable: "DTP_AdmissionDate_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_AuthorizedReturntoWork_DTP_Date_AuthorizedReturntoWorkId",
                        column: x => x.DTP_Date_AuthorizedReturntoWorkId,
                        principalTable: "DTP_AuthorizedReturntoWork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_DisabilityDates_DTP_Date_DisabilityDatesId",
                        column: x => x.DTP_Date_DisabilityDatesId,
                        principalTable: "DTP_DisabilityDates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_Discharge_DTP_Date_DischargeId",
                        column: x => x.DTP_Date_DischargeId,
                        principalTable: "DTP_Discharge",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_HearingandVisionPrescriptionDate_DTP_Date_HearingandVisionPrescriptionDateId",
                        column: x => x.DTP_Date_HearingandVisionPrescriptionDateId,
                        principalTable: "DTP_HearingandVisionPrescriptionDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_InitialTreatmentDate_DTP_Date_InitialTreatmentDateId",
                        column: x => x.DTP_Date_InitialTreatmentDateId,
                        principalTable: "DTP_InitialTreatmentDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_LastMenstrualPeriod_DTP_Date_LastMenstrualPeriodId",
                        column: x => x.DTP_Date_LastMenstrualPeriodId,
                        principalTable: "DTP_LastMenstrualPeriod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_LastSeenDate_DTP_Date_LastSeenDateId",
                        column: x => x.DTP_Date_LastSeenDateId,
                        principalTable: "DTP_LastSeenDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_LastWorked_DTP_Date_LastWorkedId",
                        column: x => x.DTP_Date_LastWorkedId,
                        principalTable: "DTP_LastWorked",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_LastXrayDate_DTP_Date_LastX_DateId",
                        column: x => x.DTP_Date_LastX_DateId,
                        principalTable: "DTP_LastXrayDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_OnsetofCurrentIllnessorSymptom_DTP_Date_OnsetofCurrentIllnessorSymptomId",
                        column: x => x.DTP_Date_OnsetofCurrentIllnessorSymptomId,
                        principalTable: "DTP_OnsetofCurrentIllnessorSymptom",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_PropertyandCasualtyDateofFirstContact_DTP_PropertyandCasualtyDateofFirstContactId",
                        column: x => x.DTP_PropertyandCasualtyDateofFirstContactId,
                        principalTable: "DTP_PropertyandCasualtyDateofFirstContact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_2_DTP_RepricerReceivedDate_DTP_Date_RepricerReceivedDateId",
                        column: x => x.DTP_Date_RepricerReceivedDateId,
                        principalTable: "DTP_RepricerReceivedDate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_DTP_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DTP_Date_ServiceDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_PrescriptionDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_DATE_CertificationRevision_RecertificationDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_BeginTherapyDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastCertificationDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastSeenDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_ShippedDateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_LastX_DateId = table.Column<int>(type: "int", nullable: true),
                    DTP_Date_InitialTreatmentDateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_DTP_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_BeginTherapyDate_DTP_Date_BeginTherapyDateId",
                        column: x => x.DTP_Date_BeginTherapyDateId,
                        principalTable: "DTP_BeginTherapyDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_CertificationRevision_DTP_DATE_CertificationRevision_RecertificationDateId",
                        column: x => x.DTP_DATE_CertificationRevision_RecertificationDateId,
                        principalTable: "DTP_CertificationRevision",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_ClaimLevelServiceDate_DTP_Date_ServiceDateId",
                        column: x => x.DTP_Date_ServiceDateId,
                        principalTable: "DTP_ClaimLevelServiceDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_HearingandVisionPrescriptionDate_DTP_Date_PrescriptionDateId",
                        column: x => x.DTP_Date_PrescriptionDateId,
                        principalTable: "DTP_HearingandVisionPrescriptionDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_InitialTreatmentDate_DTP_Date_InitialTreatmentDateId",
                        column: x => x.DTP_Date_InitialTreatmentDateId,
                        principalTable: "DTP_InitialTreatmentDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_LastCertificationDate_DTP_Date_LastCertificationDateId",
                        column: x => x.DTP_Date_LastCertificationDateId,
                        principalTable: "DTP_LastCertificationDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_LastSeenDate_DTP_Date_LastSeenDateId",
                        column: x => x.DTP_Date_LastSeenDateId,
                        principalTable: "DTP_LastSeenDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_LastXrayDate_DTP_Date_LastX_DateId",
                        column: x => x.DTP_Date_LastX_DateId,
                        principalTable: "DTP_LastXrayDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_DTP_837P_DTP_ShippedDate_DTP_Date_ShippedDateId",
                        column: x => x.DTP_Date_ShippedDateId,
                        principalTable: "DTP_ShippedDate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2310F_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_AmbulanceDrop_LocationId = table.Column<int>(type: "int", nullable: true),
                    N3_AmbulanceDrop_LocationAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_AmbulanceDrop_LocationCity_State_ZipCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2310F_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2310F_837P_N3_AdditionalPatientInformationContactAddress_N3_AmbulanceDrop_LocationAddressId",
                        column: x => x.N3_AmbulanceDrop_LocationAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310F_837P_N4_AdditionalPatientInformationContactCity_N4_AmbulanceDrop_LocationCity_State_ZipCodeId",
                        column: x => x.N4_AmbulanceDrop_LocationCity_State_ZipCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310F_837P_NM1_AmbulanceDrop_NM1_AmbulanceDrop_LocationId",
                        column: x => x.NM1_AmbulanceDrop_LocationId,
                        principalTable: "NM1_AmbulanceDrop",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420H_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_AmbulanceDrop_LocationId = table.Column<int>(type: "int", nullable: true),
                    N3_AmbulanceDrop_LocationAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_AmbulanceDrop_LocationCity_State_ZipCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420H_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420H_837P_N3_AdditionalPatientInformationContactAddress_N3_AmbulanceDrop_LocationAddressId",
                        column: x => x.N3_AmbulanceDrop_LocationAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420H_837P_N4_AdditionalPatientInformationContactCity_N4_AmbulanceDrop_LocationCity_State_ZipCodeId",
                        column: x => x.N4_AmbulanceDrop_LocationCity_State_ZipCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420H_837P_NM1_AmbulanceDrop_NM1_AmbulanceDrop_LocationId",
                        column: x => x.NM1_AmbulanceDrop_LocationId,
                        principalTable: "NM1_AmbulanceDrop",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2310E_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_AmbulancePick_LocationId = table.Column<int>(type: "int", nullable: true),
                    N3_AmbulancePick_LocationAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_AmbulancePick_LocationCity_State_ZipCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2310E_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2310E_837P_N3_AdditionalPatientInformationContactAddress_N3_AmbulancePick_LocationAddressId",
                        column: x => x.N3_AmbulancePick_LocationAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310E_837P_N4_AdditionalPatientInformationContactCity_N4_AmbulancePick_LocationCity_State_ZipCodeId",
                        column: x => x.N4_AmbulancePick_LocationCity_State_ZipCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310E_837P_NM1_AmbulancePick_NM1_AmbulancePick_LocationId",
                        column: x => x.NM1_AmbulancePick_LocationId,
                        principalTable: "NM1_AmbulancePick",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420G_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_AmbulancePick_LocationId = table.Column<int>(type: "int", nullable: true),
                    N3_AmbulancePick_LocationAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_AmbulancePick_LocationCity_State_ZipCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420G_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420G_837P_N3_AdditionalPatientInformationContactAddress_N3_AmbulancePick_LocationAddressId",
                        column: x => x.N3_AmbulancePick_LocationAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420G_837P_N4_AdditionalPatientInformationContactCity_N4_AmbulancePick_LocationCity_State_ZipCodeId",
                        column: x => x.N4_AmbulancePick_LocationCity_State_ZipCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420G_837P_NM1_AmbulancePick_NM1_AmbulancePick_LocationId",
                        column: x => x.NM1_AmbulancePick_LocationId,
                        principalTable: "NM1_AmbulancePick",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_1000A_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_SubmitterNameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_1000A_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_1000A_837P_NM1_InformationReceiverName_4_NM1_SubmitterNameId",
                        column: x => x.NM1_SubmitterNameId,
                        principalTable: "NM1_InformationReceiverName_4",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330G_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherPayerBillingProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330G_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330G_837P_NM1_OtherPayerBillingProvider_NM1_OtherPayerBillingProviderId",
                        column: x => x.NM1_OtherPayerBillingProviderId,
                        principalTable: "NM1_OtherPayerBillingProvider",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2010BB_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_PayerNameId = table.Column<int>(type: "int", nullable: true),
                    N3_PayerAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_PayerCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2010BB_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2010BB_837P_All_REF_837P_5_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BB_837P_N3_AdditionalPatientInformationContactAddress_N3_PayerAddressId",
                        column: x => x.N3_PayerAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BB_837P_N4_AdditionalPatientInformationContactCity_N4_PayerCity_State_ZIPCodeId",
                        column: x => x.N4_PayerCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BB_837P_NM1_OtherPayerName_NM1_PayerNameId",
                        column: x => x.NM1_PayerNameId,
                        principalTable: "NM1_OtherPayerName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330D_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherPayerRenderingProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330D_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330D_837P_NM1_OtherPayerRenderingProvider_2_NM1_OtherPayerRenderingProviderId",
                        column: x => x.NM1_OtherPayerRenderingProviderId,
                        principalTable: "NM1_OtherPayerRenderingProvider_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330E_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherPayerServiceFacilityLocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330E_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330E_837P_NM1_OtherPayerServiceFacilityLocation_NM1_OtherPayerServiceFacilityLocationId",
                        column: x => x.NM1_OtherPayerServiceFacilityLocationId,
                        principalTable: "NM1_OtherPayerServiceFacilityLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330F_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherPayerSupervisingProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330F_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330F_837P_NM1_OtherPayerSupervisingProvider_NM1_OtherPayerSupervisingProviderId",
                        column: x => x.NM1_OtherPayerSupervisingProviderId,
                        principalTable: "NM1_OtherPayerSupervisingProvider",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2010AB_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_Pay_AddressNameId = table.Column<int>(type: "int", nullable: true),
                    N3_Pay_ToAddress_ADDRESSId = table.Column<int>(type: "int", nullable: true),
                    N4_Pay_AddressCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2010AB_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2010AB_837P_N3_AdditionalPatientInformationContactAddress_N3_Pay_ToAddress_ADDRESSId",
                        column: x => x.N3_Pay_ToAddress_ADDRESSId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AB_837P_N4_AdditionalPatientInformationContactCity_N4_Pay_AddressCity_State_ZIPCodeId",
                        column: x => x.N4_Pay_AddressCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AB_837P_NM1_Pay_NM1_Pay_AddressNameId",
                        column: x => x.NM1_Pay_AddressNameId,
                        principalTable: "NM1_Pay",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420B_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_PurchasedServiceProviderNameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420B_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420B_837P_NM1_PurchasedServiceProviderName_NM1_PurchasedServiceProviderNameId",
                        column: x => x.NM1_PurchasedServiceProviderNameId,
                        principalTable: "NM1_PurchasedServiceProviderName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_1000B_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_ReceiverNameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_1000B_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_1000B_837P_NM1_ReceiverName_NM1_ReceiverNameId",
                        column: x => x.NM1_ReceiverNameId,
                        principalTable: "NM1_ReceiverName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420C_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_ServiceFacilityLocationId = table.Column<int>(type: "int", nullable: true),
                    N3_ServiceFacilityLocationAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_ServiceFacilityLocationCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420C_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420C_837P_N3_AdditionalPatientInformationContactAddress_N3_ServiceFacilityLocationAddressId",
                        column: x => x.N3_ServiceFacilityLocationAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420C_837P_N4_AdditionalPatientInformationContactCity_N4_ServiceFacilityLocationCity_State_ZIPCodeId",
                        column: x => x.N4_ServiceFacilityLocationCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420C_837P_NM1_ServiceFacilityLocation_NM1_ServiceFacilityLocationId",
                        column: x => x.NM1_ServiceFacilityLocationId,
                        principalTable: "NM1_ServiceFacilityLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2310D_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_SupervisingProviderNameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2310D_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2310D_837P_NM1_SupervisingProviderName_NM1_SupervisingProviderNameId",
                        column: x => x.NM1_SupervisingProviderNameId,
                        principalTable: "NM1_SupervisingProviderName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420D_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_SupervisingProviderNameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420D_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420D_837P_NM1_SupervisingProviderName_NM1_SupervisingProviderNameId",
                        column: x => x.NM1_SupervisingProviderNameId,
                        principalTable: "NM1_SupervisingProviderName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NTE_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NTE_LineNoteId = table.Column<int>(type: "int", nullable: true),
                    NTE_ThirdPartyOrganizationNotesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NTE_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NTE_837P_NTE_LineNote_NTE_LineNoteId",
                        column: x => x.NTE_LineNoteId,
                        principalTable: "NTE_LineNote",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NTE_837P_NTE_ThirdPartyOrganizationNotes_NTE_ThirdPartyOrganizationNotesId",
                        column: x => x.NTE_ThirdPartyOrganizationNotesId,
                        principalTable: "NTE_ThirdPartyOrganizationNotes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2310C_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_ServiceFacilityLocationNameId = table.Column<int>(type: "int", nullable: true),
                    N3_ServiceFacilityLocationAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_ServiceFacilityLocationCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    PER_ServiceFacilityContactInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2310C_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2310C_837P_N3_AdditionalPatientInformationContactAddress_N3_ServiceFacilityLocationAddressId",
                        column: x => x.N3_ServiceFacilityLocationAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310C_837P_N4_AdditionalPatientInformationContactCity_N4_ServiceFacilityLocationCity_State_ZIPCodeId",
                        column: x => x.N4_ServiceFacilityLocationCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310C_837P_NM1_ServiceFacilityLocation_NM1_ServiceFacilityLocationNameId",
                        column: x => x.NM1_ServiceFacilityLocationNameId,
                        principalTable: "NM1_ServiceFacilityLocation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310C_837P_PER_PropertyandCasualtyPatientContactInformation_PER_ServiceFacilityContactInformationId",
                        column: x => x.PER_ServiceFacilityContactInformationId,
                        principalTable: "PER_PropertyandCasualtyPatientContactInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_QTY_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QTY_AmbulancePatientCountId = table.Column<int>(type: "int", nullable: true),
                    QTY_ObstetricAnesthesiaAdditionalUnitsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_QTY_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_QTY_837P_QTY_AmbulancePatientCount_QTY_AmbulancePatientCountId",
                        column: x => x.QTY_AmbulancePatientCountId,
                        principalTable: "QTY_AmbulancePatientCount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_QTY_837P_QTY_ObstetricAnesthesiaAdditionalUnits_QTY_ObstetricAnesthesiaAdditionalUnitsId",
                        column: x => x.QTY_ObstetricAnesthesiaAdditionalUnitsId,
                        principalTable: "QTY_ObstetricAnesthesiaAdditionalUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_PWK_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicatorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_PWK_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_PWK_837P_PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator_PWK_DurableMedicalEquipmentCertificateofMedic~",
                        column: x => x.PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicatorId,
                        principalTable: "PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_HI_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HI_HealthCareDiagnosisCodeId = table.Column<int>(type: "int", nullable: true),
                    HI_AnesthesiaRelatedProcedureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_HI_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_HI_837P_HI_AnesthesiaRelatedProcedure_HI_AnesthesiaRelatedProcedureId",
                        column: x => x.HI_AnesthesiaRelatedProcedureId,
                        principalTable: "HI_AnesthesiaRelatedProcedure",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_HI_837P_HI_DependentHealthCareDiagnosisCode_2_HI_HealthCareDiagnosisCodeId",
                        column: x => x.HI_HealthCareDiagnosisCodeId,
                        principalTable: "HI_DependentHealthCareDiagnosisCode_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2310B_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_RenderingProviderNameId = table.Column<int>(type: "int", nullable: true),
                    PRV_RenderingProviderSpecialtyInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2310B_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2310B_837P_NM1_RenderingProviderName_NM1_RenderingProviderNameId",
                        column: x => x.NM1_RenderingProviderNameId,
                        principalTable: "NM1_RenderingProviderName",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310B_837P_PRV_RenderingProviderSpecialtyInformation_PRV_RenderingProviderSpecialtyInformationId",
                        column: x => x.PRV_RenderingProviderSpecialtyInformationId,
                        principalTable: "PRV_RenderingProviderSpecialtyInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420A_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_RenderingProviderNameId = table.Column<int>(type: "int", nullable: true),
                    PRV_RenderingProviderSpecialtyInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420A_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420A_837P_NM1_RenderingProviderName_NM1_RenderingProviderNameId",
                        column: x => x.NM1_RenderingProviderNameId,
                        principalTable: "NM1_RenderingProviderName",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420A_837P_PRV_RenderingProviderSpecialtyInformation_PRV_RenderingProviderSpecialtyInformationId",
                        column: x => x.PRV_RenderingProviderSpecialtyInformationId,
                        principalTable: "PRV_RenderingProviderSpecialtyInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P_8",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_BillingProviderTaxIdentificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_8", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_8_REF_BillingProviderTaxIdentification_REF_BillingProviderTaxIdentificationId",
                        column: x => x.REF_BillingProviderTaxIdentificationId,
                        principalTable: "REF_BillingProviderTaxIdentification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_OtherPayerPriorAuthorizationNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_OtherPayerReferralNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_OtherPayerClaimAdjustmentIndicatorId = table.Column<int>(type: "int", nullable: true),
                    REF_OtherPayerClaimControlNumberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_REF_OtherPayerClaimAdjustmentIndicator_REF_OtherPayerClaimAdjustmentIndicatorId",
                        column: x => x.REF_OtherPayerClaimAdjustmentIndicatorId,
                        principalTable: "REF_OtherPayerClaimAdjustmentIndicator",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_REF_OtherPayerClaimControlNumber_REF_OtherPayerClaimControlNumberId",
                        column: x => x.REF_OtherPayerClaimControlNumberId,
                        principalTable: "REF_OtherPayerClaimControlNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_REF_OtherPayerPriorAuthorizationNumber_REF_OtherPayerPriorAuthorizationNumberId",
                        column: x => x.REF_OtherPayerPriorAuthorizationNumberId,
                        principalTable: "REF_OtherPayerPriorAuthorizationNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_REF_OtherPayerReferralNumber_REF_OtherPayerReferralNumberId",
                        column: x => x.REF_OtherPayerReferralNumberId,
                        principalTable: "REF_OtherPayerReferralNumber",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330A_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherSubscriberNameId = table.Column<int>(type: "int", nullable: true),
                    N3_OtherSubscriberAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_OtherSubscriberCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    REF_OtherSubscriberSecondaryIdentificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330A_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330A_837P_N3_AdditionalPatientInformationContactAddress_N3_OtherSubscriberAddressId",
                        column: x => x.N3_OtherSubscriberAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330A_837P_N4_AdditionalPatientInformationContactCity_N4_OtherSubscriberCity_State_ZIPCodeId",
                        column: x => x.N4_OtherSubscriberCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330A_837P_NM1_OtherSubscriberName_NM1_OtherSubscriberNameId",
                        column: x => x.NM1_OtherSubscriberNameId,
                        principalTable: "NM1_OtherSubscriberName",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330A_837P_REF_OtherSubscriberSecondaryIdentification_REF_OtherSubscriberSecondaryIdentificationId",
                        column: x => x.REF_OtherSubscriberSecondaryIdentificationId,
                        principalTable: "REF_OtherSubscriberSecondaryIdentification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_Pay_ToPlanSecondaryIdentificationId = table.Column<int>(type: "int", nullable: true),
                    REF_Pay_ToPlanTaxIdentificationNumberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_3_REF_BillingProviderTaxIdentification_2_REF_Pay_ToPlanTaxIdentificationNumberId",
                        column: x => x.REF_Pay_ToPlanTaxIdentificationNumberId,
                        principalTable: "REF_BillingProviderTaxIdentification_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_3_REF_Pay_REF_Pay_ToPlanSecondaryIdentificationId",
                        column: x => x.REF_Pay_ToPlanSecondaryIdentificationId,
                        principalTable: "REF_Pay",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2410_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LIN_DrugIdentificationId = table.Column<int>(type: "int", nullable: true),
                    CTP_DrugQuantityId = table.Column<int>(type: "int", nullable: true),
                    REF_PrescriptionorCompoundDrugAssociationNumberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2410_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2410_837P_CTP_DrugQuantity_CTP_DrugQuantityId",
                        column: x => x.CTP_DrugQuantityId,
                        principalTable: "CTP_DrugQuantity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2410_837P_LIN_DrugIdentification_2_LIN_DrugIdentificationId",
                        column: x => x.LIN_DrugIdentificationId,
                        principalTable: "LIN_DrugIdentification_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2410_837P_REF_PrescriptionorCompoundDrugAssociationNumber_REF_PrescriptionorCompoundDrugAssociationNumberId",
                        column: x => x.REF_PrescriptionorCompoundDrugAssociationNumberId,
                        principalTable: "REF_PrescriptionorCompoundDrugAssociationNumber",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_SubscriberSecondaryIdentificationId = table.Column<int>(type: "int", nullable: true),
                    REF_PropertyandCasualtyClaimNumberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_4_REF_OtherSubscriberSecondaryIdentification_REF_SubscriberSecondaryIdentificationId",
                        column: x => x.REF_SubscriberSecondaryIdentificationId,
                        principalTable: "REF_OtherSubscriberSecondaryIdentification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_4_REF_PropertyandCasualtyClaimNumber_REF_PropertyandCasualtyClaimNumberId",
                        column: x => x.REF_PropertyandCasualtyClaimNumberId,
                        principalTable: "REF_PropertyandCasualtyClaimNumber",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_RepricedLineItemReferenceNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_AdjustedRepricedLineItemReferenceNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_LineItemControlNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_MammographyCertificationNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId = table.Column<int>(type: "int", nullable: true),
                    REF_ReferringClinicalLaboratoryImprovementAmendment_CLIA_FacilityIdentificationId = table.Column<int>(type: "int", nullable: true),
                    REF_ImmunizationBatchNumberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_AdjustedRepricedLineItemReferenceNumber_REF_AdjustedRepricedLineItemReferenceNumberId",
                        column: x => x.REF_AdjustedRepricedLineItemReferenceNumberId,
                        principalTable: "REF_AdjustedRepricedLineItemReferenceNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_ClinicalLaboratoryImprovementAmendment_REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId",
                        column: x => x.REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId,
                        principalTable: "REF_ClinicalLaboratoryImprovementAmendment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_ImmunizationBatchNumber_REF_ImmunizationBatchNumberId",
                        column: x => x.REF_ImmunizationBatchNumberId,
                        principalTable: "REF_ImmunizationBatchNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_LineItemControlNumber_REF_LineItemControlNumberId",
                        column: x => x.REF_LineItemControlNumberId,
                        principalTable: "REF_LineItemControlNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_MammographyCertificationNumber_REF_MammographyCertificationNumberId",
                        column: x => x.REF_MammographyCertificationNumberId,
                        principalTable: "REF_MammographyCertificationNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_ReferringClinicalLaboratoryImprovementAmendment_REF_ReferringClinicalLaboratoryImprovementAmendment_CLIA_~",
                        column: x => x.REF_ReferringClinicalLaboratoryImprovementAmendment_CLIA_FacilityIdentificationId,
                        principalTable: "REF_ReferringClinicalLaboratoryImprovementAmendment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_2_REF_RepricedLineItemReferenceNumber_REF_RepricedLineItemReferenceNumberId",
                        column: x => x.REF_RepricedLineItemReferenceNumberId,
                        principalTable: "REF_RepricedLineItemReferenceNumber",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P_6",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_ServiceAuthorizationExceptionCodeId = table.Column<int>(type: "int", nullable: true),
                    REF_MandatoryMedicare_Section4081_CrossoverIndicatorId = table.Column<int>(type: "int", nullable: true),
                    REF_MammographyCertificationNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_ReferralNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_PriorAuthorizationId = table.Column<int>(type: "int", nullable: true),
                    REF_PayerClaimControlNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId = table.Column<int>(type: "int", nullable: true),
                    REF_RepricedClaimNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_AdjustedRepricedClaimNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_InvestigationalDeviceExemptionNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_ClaimIdentifierForTransmissionIntermediariesId = table.Column<int>(type: "int", nullable: true),
                    REF_MedicalRecordNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_DemonstrationProjectIdentifierId = table.Column<int>(type: "int", nullable: true),
                    REF_CarePlanOversightId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_6", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_AdjustedRepricedClaimNumber_REF_AdjustedRepricedClaimNumberId",
                        column: x => x.REF_AdjustedRepricedClaimNumberId,
                        principalTable: "REF_AdjustedRepricedClaimNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_CarePlanOversight_REF_CarePlanOversightId",
                        column: x => x.REF_CarePlanOversightId,
                        principalTable: "REF_CarePlanOversight",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries_REF_ClaimIdentifierForTransm~",
                        column: x => x.REF_ClaimIdentifierForTransmissionIntermediariesId,
                        principalTable: "REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_ClinicalLaboratoryImprovementAmendment_REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId",
                        column: x => x.REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId,
                        principalTable: "REF_ClinicalLaboratoryImprovementAmendment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_DemonstrationProjectIdentifier_REF_DemonstrationProjectIdentifierId",
                        column: x => x.REF_DemonstrationProjectIdentifierId,
                        principalTable: "REF_DemonstrationProjectIdentifier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_InvestigationalDeviceExemptionNumber_REF_InvestigationalDeviceExemptionNumberId",
                        column: x => x.REF_InvestigationalDeviceExemptionNumberId,
                        principalTable: "REF_InvestigationalDeviceExemptionNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_MammographyCertificationNumber_REF_MammographyCertificationNumberId",
                        column: x => x.REF_MammographyCertificationNumberId,
                        principalTable: "REF_MammographyCertificationNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_MandatoryMedicare_REF_MandatoryMedicare_Section4081_CrossoverIndicatorId",
                        column: x => x.REF_MandatoryMedicare_Section4081_CrossoverIndicatorId,
                        principalTable: "REF_MandatoryMedicare",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_MedicalRecordNumber_REF_MedicalRecordNumberId",
                        column: x => x.REF_MedicalRecordNumberId,
                        principalTable: "REF_MedicalRecordNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_OtherPayerClaimControlNumber_REF_PayerClaimControlNumberId",
                        column: x => x.REF_PayerClaimControlNumberId,
                        principalTable: "REF_OtherPayerClaimControlNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_OtherPayerPriorAuthorizationNumber_REF_PriorAuthorizationId",
                        column: x => x.REF_PriorAuthorizationId,
                        principalTable: "REF_OtherPayerPriorAuthorizationNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_OtherPayerReferralNumber_REF_ReferralNumberId",
                        column: x => x.REF_ReferralNumberId,
                        principalTable: "REF_OtherPayerReferralNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_RepricedClaimNumber_REF_RepricedClaimNumberId",
                        column: x => x.REF_RepricedClaimNumberId,
                        principalTable: "REF_RepricedClaimNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_6_REF_ServiceAuthorizationExceptionCode_REF_ServiceAuthorizationExceptionCodeId",
                        column: x => x.REF_ServiceAuthorizationExceptionCodeId,
                        principalTable: "REF_ServiceAuthorizationExceptionCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_REF_837P_7",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_PropertyandCasualtyClaimNumberId = table.Column<int>(type: "int", nullable: true),
                    REF_PropertyandCasualtyPatientIdentifierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_REF_837P_7", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_REF_837P_7_REF_PropertyandCasualtyClaimNumber_REF_PropertyandCasualtyClaimNumberId",
                        column: x => x.REF_PropertyandCasualtyClaimNumberId,
                        principalTable: "REF_PropertyandCasualtyClaimNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_REF_837P_7_REF_PropertyandCasualtyPatientIdentifier_REF_PropertyandCasualtyPatientIdentifierId",
                        column: x => x.REF_PropertyandCasualtyPatientIdentifierId,
                        principalTable: "REF_PropertyandCasualtyPatientIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CRC_PatientConditionInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCategory_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationConditionIndicator_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    All_CRC_837P_2Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC_PatientConditionInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CRC_PatientConditionInformation_All_CRC_837P_2_All_CRC_837P_2Id",
                        column: x => x.All_CRC_837P_2Id,
                        principalTable: "All_CRC_837P_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CRC_AmbulanceCertification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCategory_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificationConditionIndicator_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    All_CRC_837PId = table.Column<int>(type: "int", nullable: true),
                    All_CRC_837P_2Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC_AmbulanceCertification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CRC_AmbulanceCertification_All_CRC_837P_2_All_CRC_837P_2Id",
                        column: x => x.All_CRC_837P_2Id,
                        principalTable: "All_CRC_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CRC_AmbulanceCertification_All_CRC_837P_All_CRC_837PId",
                        column: x => x.All_CRC_837PId,
                        principalTable: "All_CRC_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DTP_AssumedandRelinquishedCareDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    All_DTP_837P_2Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_AssumedandRelinquishedCareDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTP_AssumedandRelinquishedCareDates_All_DTP_837P_2_All_DTP_837P_2Id",
                        column: x => x.All_DTP_837P_2Id,
                        principalTable: "All_DTP_837P_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DTP_TestDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    All_DTP_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP_TestDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTP_TestDate_All_DTP_837P_All_DTP_837PId",
                        column: x => x.All_DTP_837PId,
                        principalTable: "All_DTP_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_BillingProviderSecondaryIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    All_REF_837P_5Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2330G_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_BillingProviderSecondaryIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderSecondaryIdentification_All_REF_837P_5_All_REF_837P_5Id",
                        column: x => x.All_REF_837P_5Id,
                        principalTable: "All_REF_837P_5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderSecondaryIdentification_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderSecondaryIdentification_Loop_2330G_837P_Loop_2330G_837PId",
                        column: x => x.Loop_2330G_837PId,
                        principalTable: "Loop_2330G_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NM1_837P_6",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loop1000AId = table.Column<int>(type: "int", nullable: true),
                    Loop1000BId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NM1_837P_6", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_6_Loop_1000A_837P_Loop1000AId",
                        column: x => x.Loop1000AId,
                        principalTable: "Loop_1000A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_6_Loop_1000B_837P_Loop1000BId",
                        column: x => x.Loop1000BId,
                        principalTable: "Loop_1000B_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_BillingProviderSecondaryIdentification_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2420C_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_BillingProviderSecondaryIdentification_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderSecondaryIdentification_2_C040_ReferenceIdentifier_3_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderSecondaryIdentification_2_Loop_2420C_837P_Loop_2420C_837PId",
                        column: x => x.Loop_2420C_837PId,
                        principalTable: "Loop_2420C_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerServiceFacilityLocationSecondaryIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2310C_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2330E_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerServiceFacilityLocationSecondaryIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerServiceFacilityLocationSecondaryIdentification_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerServiceFacilityLocationSecondaryIdentification_Loop_2310C_837P_Loop_2310C_837PId",
                        column: x => x.Loop_2310C_837PId,
                        principalTable: "Loop_2310C_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerServiceFacilityLocationSecondaryIdentification_Loop_2330E_837P_Loop_2330E_837PId",
                        column: x => x.Loop_2330E_837PId,
                        principalTable: "Loop_2330E_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PWK_ClaimSupplementalInformation_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentReportTypeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportTransmissionCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportCopiesNeeded_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentControlNumber_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentDescription_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionsIndicated_08Id = table.Column<int>(type: "int", nullable: true),
                    RequestCategoryCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    All_PWK_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PWK_ClaimSupplementalInformation_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PWK_ClaimSupplementalInformation_2_All_PWK_837P_All_PWK_837PId",
                        column: x => x.All_PWK_837PId,
                        principalTable: "All_PWK_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PWK_ClaimSupplementalInformation_2_C002_ActionsIndicated_2_ActionsIndicated_08Id",
                        column: x => x.ActionsIndicated_08Id,
                        principalTable: "C002_ActionsIndicated_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HI_ConditionInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthCareCodeInformation_01Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_02Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_03Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_04Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_05Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_06Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_07Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_08Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_09Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_10Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_11Id = table.Column<int>(type: "int", nullable: true),
                    HealthCareCodeInformation_12Id = table.Column<int>(type: "int", nullable: true),
                    All_HI_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HI_ConditionInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_All_HI_837P_All_HI_837PId",
                        column: x => x.All_HI_837PId,
                        principalTable: "All_HI_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_01Id",
                        column: x => x.HealthCareCodeInformation_01Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_02Id",
                        column: x => x.HealthCareCodeInformation_02Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_03Id",
                        column: x => x.HealthCareCodeInformation_03Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_04Id",
                        column: x => x.HealthCareCodeInformation_04Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_05Id",
                        column: x => x.HealthCareCodeInformation_05Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_06Id",
                        column: x => x.HealthCareCodeInformation_06Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_07Id",
                        column: x => x.HealthCareCodeInformation_07Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_08Id",
                        column: x => x.HealthCareCodeInformation_08Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_09Id",
                        column: x => x.HealthCareCodeInformation_09Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_10Id",
                        column: x => x.HealthCareCodeInformation_10Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_11Id",
                        column: x => x.HealthCareCodeInformation_11Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_ConditionInformation_C022_HealthCareCodeInformation_13_HealthCareCodeInformation_12Id",
                        column: x => x.HealthCareCodeInformation_12Id,
                        principalTable: "C022_HealthCareCodeInformation_13",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NM1_837P_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loop2310BId = table.Column<int>(type: "int", nullable: true),
                    Loop2310CId = table.Column<int>(type: "int", nullable: true),
                    Loop2310DId = table.Column<int>(type: "int", nullable: true),
                    Loop2310EId = table.Column<int>(type: "int", nullable: true),
                    Loop2310FId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NM1_837P_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_3_Loop_2310B_837P_Loop2310BId",
                        column: x => x.Loop2310BId,
                        principalTable: "Loop_2310B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_3_Loop_2310C_837P_Loop2310CId",
                        column: x => x.Loop2310CId,
                        principalTable: "Loop_2310C_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_3_Loop_2310D_837P_Loop2310DId",
                        column: x => x.Loop2310DId,
                        principalTable: "Loop_2310D_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_3_Loop_2310E_837P_Loop2310EId",
                        column: x => x.Loop2310EId,
                        principalTable: "Loop_2310E_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_3_Loop_2310F_837P_Loop2310FId",
                        column: x => x.Loop2310FId,
                        principalTable: "Loop_2310F_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_AssistantSurgeonSecondaryIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2310B_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2310D_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2330D_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2330F_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_AssistantSurgeonSecondaryIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_Loop_2310B_837P_Loop_2310B_837PId",
                        column: x => x.Loop_2310B_837PId,
                        principalTable: "Loop_2310B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_Loop_2310D_837P_Loop_2310D_837PId",
                        column: x => x.Loop_2310D_837PId,
                        principalTable: "Loop_2310D_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_Loop_2330D_837P_Loop_2330D_837PId",
                        column: x => x.Loop_2330D_837PId,
                        principalTable: "Loop_2330D_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_Loop_2330F_837P_Loop_2330F_837PId",
                        column: x => x.Loop_2330F_837PId,
                        principalTable: "Loop_2330F_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_AssistantSurgeonSecondaryIdentification_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2420A_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2420D_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_AssistantSurgeonSecondaryIdentification_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_2_C040_ReferenceIdentifier_3_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_2_Loop_2420A_837P_Loop_2420A_837PId",
                        column: x => x.Loop_2420A_837PId,
                        principalTable: "Loop_2420A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_AssistantSurgeonSecondaryIdentification_2_Loop_2420D_837P_Loop_2420D_837PId",
                        column: x => x.Loop_2420D_837PId,
                        principalTable: "Loop_2420D_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2010AA_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_BillingProviderNameId = table.Column<int>(type: "int", nullable: true),
                    N3_BillingProviderAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_BillingProviderCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2010AA_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2010AA_837P_All_REF_837P_8_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_8",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AA_837P_N3_AdditionalPatientInformationContactAddress_N3_BillingProviderAddressId",
                        column: x => x.N3_BillingProviderAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AA_837P_N4_AdditionalPatientInformationContactCity_N4_BillingProviderCity_State_ZIPCodeId",
                        column: x => x.N4_BillingProviderCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AA_837P_NM1_BillingProviderName_2_NM1_BillingProviderNameId",
                        column: x => x.NM1_BillingProviderNameId,
                        principalTable: "NM1_BillingProviderName_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_BillingProviderUPIN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    All_REF_837P_8Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_BillingProviderUPIN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderUPIN_All_REF_837P_8_All_REF_837P_8Id",
                        column: x => x.All_REF_837P_8Id,
                        principalTable: "All_REF_837P_8",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_BillingProviderUPIN_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330B_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherPayerNameId = table.Column<int>(type: "int", nullable: true),
                    N3_OtherPayerAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_OtherPayerCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    DTP_ClaimCheckorRemittanceDateId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330B_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330B_837P_All_REF_837P_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330B_837P_DTP_ClaimCheckOrRemittanceDate_DTP_ClaimCheckorRemittanceDateId",
                        column: x => x.DTP_ClaimCheckorRemittanceDateId,
                        principalTable: "DTP_ClaimCheckOrRemittanceDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330B_837P_N3_AdditionalPatientInformationContactAddress_N3_OtherPayerAddressId",
                        column: x => x.N3_OtherPayerAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330B_837P_N4_AdditionalPatientInformationContactCity_N4_OtherPayerCity_State_ZIPCodeId",
                        column: x => x.N4_OtherPayerCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330B_837P_NM1_OtherPayerName_NM1_OtherPayerNameId",
                        column: x => x.NM1_OtherPayerNameId,
                        principalTable: "NM1_OtherPayerName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerSecondaryIdentifier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    All_REF_837PId = table.Column<int>(type: "int", nullable: true),
                    All_REF_837P_5Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerSecondaryIdentifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerSecondaryIdentifier_All_REF_837P_5_All_REF_837P_5Id",
                        column: x => x.All_REF_837P_5Id,
                        principalTable: "All_REF_837P_5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerSecondaryIdentifier_All_REF_837P_All_REF_837PId",
                        column: x => x.All_REF_837PId,
                        principalTable: "All_REF_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerSecondaryIdentifier_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2010AC_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_Pay_ToPlanNameId = table.Column<int>(type: "int", nullable: true),
                    N3_Pay_ToPlanAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_Pay_ToPlanCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2010AC_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2010AC_837P_All_REF_837P_3_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AC_837P_N3_AdditionalPatientInformationContactAddress_N3_Pay_ToPlanAddressId",
                        column: x => x.N3_Pay_ToPlanAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AC_837P_N4_AdditionalPatientInformationContactCity_N4_Pay_ToPlanCity_State_ZIPCodeId",
                        column: x => x.N4_Pay_ToPlanCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010AC_837P_NM1_Pay_3_NM1_Pay_ToPlanNameId",
                        column: x => x.NM1_Pay_ToPlanNameId,
                        principalTable: "NM1_Pay_3",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2010BA_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_SubscriberNameId = table.Column<int>(type: "int", nullable: true),
                    N3_SubscriberAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_SubscriberCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    DMG_SubscriberDemographicInformationId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true),
                    PER_PropertyandCasualtySubscriberContactInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2010BA_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2010BA_837P_All_REF_837P_4_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BA_837P_DMG_PatientDemographicInformation_DMG_SubscriberDemographicInformationId",
                        column: x => x.DMG_SubscriberDemographicInformationId,
                        principalTable: "DMG_PatientDemographicInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BA_837P_N3_AdditionalPatientInformationContactAddress_N3_SubscriberAddressId",
                        column: x => x.N3_SubscriberAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BA_837P_N4_AdditionalPatientInformationContactCity_N4_SubscriberCity_State_ZIPCodeId",
                        column: x => x.N4_SubscriberCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BA_837P_NM1_SubscriberName_5_NM1_SubscriberNameId",
                        column: x => x.NM1_SubscriberNameId,
                        principalTable: "NM1_SubscriberName_5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010BA_837P_PER_PropertyandCasualtyPatientContactInformation_PER_PropertyandCasualtySubscriberContactInformationId",
                        column: x => x.PER_PropertyandCasualtySubscriberContactInformationId,
                        principalTable: "PER_PropertyandCasualtyPatientContactInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerPriorAuthorizationNumber_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    All_REF_837P_2Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerPriorAuthorizationNumber_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerPriorAuthorizationNumber_2_All_REF_837P_2_All_REF_837P_2Id",
                        column: x => x.All_REF_837P_2Id,
                        principalTable: "All_REF_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerPriorAuthorizationNumber_2_C040_ReferenceIdentifier_3_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier_3",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OtherPayerReferralNumber_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    All_REF_837P_2Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OtherPayerReferralNumber_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerReferralNumber_2_All_REF_837P_2_All_REF_837P_2Id",
                        column: x => x.All_REF_837P_2Id,
                        principalTable: "All_REF_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OtherPayerReferralNumber_2_C040_ReferenceIdentifier_3_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier_3",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2010CA_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_PatientNameId = table.Column<int>(type: "int", nullable: true),
                    N3_PatientAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_PatientCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    DMG_PatientDemographicInformationId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true),
                    PER_PropertyandCasualtyPatientContactInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2010CA_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2010CA_837P_All_REF_837P_7_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_7",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010CA_837P_DMG_PatientDemographicInformation_DMG_PatientDemographicInformationId",
                        column: x => x.DMG_PatientDemographicInformationId,
                        principalTable: "DMG_PatientDemographicInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010CA_837P_N3_AdditionalPatientInformationContactAddress_N3_PatientAddressId",
                        column: x => x.N3_PatientAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010CA_837P_N4_AdditionalPatientInformationContactCity_N4_PatientCity_State_ZIPCodeId",
                        column: x => x.N4_PatientCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010CA_837P_NM1_PatientName_3_NM1_PatientNameId",
                        column: x => x.NM1_PatientNameId,
                        principalTable: "NM1_PatientName_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2010CA_837P_PER_PropertyandCasualtyPatientContactInformation_PER_PropertyandCasualtyPatientContactInformationId",
                        column: x => x.PER_PropertyandCasualtyPatientContactInformationId,
                        principalTable: "PER_PropertyandCasualtyPatientContactInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TS837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STId = table.Column<int>(type: "int", nullable: true),
                    BHT_BeginningOfHierarchicalTransactionId = table.Column<int>(type: "int", nullable: true),
                    AllNM1Id = table.Column<int>(type: "int", nullable: true),
                    SEId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TS837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TS837P_All_NM1_837P_6_AllNM1Id",
                        column: x => x.AllNM1Id,
                        principalTable: "All_NM1_837P_6",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TS837P_BHT_BeginningOfHierarchicalTransaction_8_BHT_BeginningOfHierarchicalTransactionId",
                        column: x => x.BHT_BeginningOfHierarchicalTransactionId,
                        principalTable: "BHT_BeginningOfHierarchicalTransaction_8",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TS837P_SE_SEId",
                        column: x => x.SEId,
                        principalTable: "SE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TS837P_ST_STId",
                        column: x => x.STId,
                        principalTable: "ST",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2310A_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_ReferringProviderNameId = table.Column<int>(type: "int", nullable: true),
                    All_NM1_837P_3Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2310A_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2310A_837P_All_NM1_837P_3_All_NM1_837P_3Id",
                        column: x => x.All_NM1_837P_3Id,
                        principalTable: "All_NM1_837P_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2310A_837P_NM1_ReferringProviderName_NM1_ReferringProviderNameId",
                        column: x => x.NM1_ReferringProviderNameId,
                        principalTable: "NM1_ReferringProviderName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PER_BillingProviderContactInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactFunctionCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactName_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationNumberQualifier_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactCommunicationNumber_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationNumberQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactCommunicationNumber_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationNumberQualifier_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContactCommunicationNumber_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactInquiryReference_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loop_1000A_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2010AA_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PER_BillingProviderContactInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PER_BillingProviderContactInformation_Loop_1000A_837P_Loop_1000A_837PId",
                        column: x => x.Loop_1000A_837PId,
                        principalTable: "Loop_1000A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PER_BillingProviderContactInformation_Loop_2010AA_837P_Loop_2010AA_837PId",
                        column: x => x.Loop_2010AA_837PId,
                        principalTable: "Loop_2010AA_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NM1_837P_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loop2330AId = table.Column<int>(type: "int", nullable: true),
                    Loop2330BId = table.Column<int>(type: "int", nullable: true),
                    Loop2330DId = table.Column<int>(type: "int", nullable: true),
                    Loop2330EId = table.Column<int>(type: "int", nullable: true),
                    Loop2330FId = table.Column<int>(type: "int", nullable: true),
                    Loop2330GId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NM1_837P_4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_4_Loop_2330A_837P_Loop2330AId",
                        column: x => x.Loop2330AId,
                        principalTable: "Loop_2330A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_4_Loop_2330B_837P_Loop2330BId",
                        column: x => x.Loop2330BId,
                        principalTable: "Loop_2330B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_4_Loop_2330D_837P_Loop2330DId",
                        column: x => x.Loop2330DId,
                        principalTable: "Loop_2330D_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_4_Loop_2330E_837P_Loop2330EId",
                        column: x => x.Loop2330EId,
                        principalTable: "Loop_2330E_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_4_Loop_2330F_837P_Loop2330FId",
                        column: x => x.Loop2330FId,
                        principalTable: "Loop_2330F_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_4_Loop_2330G_837P_Loop2330GId",
                        column: x => x.Loop2330GId,
                        principalTable: "Loop_2330G_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NM1_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loop2010AAId = table.Column<int>(type: "int", nullable: true),
                    Loop2010ABId = table.Column<int>(type: "int", nullable: true),
                    Loop2010ACId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NM1_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_Loop_2010AA_837P_Loop2010AAId",
                        column: x => x.Loop2010AAId,
                        principalTable: "Loop_2010AA_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_Loop_2010AB_837P_Loop2010ABId",
                        column: x => x.Loop2010ABId,
                        principalTable: "Loop_2010AB_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_Loop_2010AC_837P_Loop2010ACId",
                        column: x => x.Loop2010ACId,
                        principalTable: "Loop_2010AC_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NM1_837P_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loop2010BAId = table.Column<int>(type: "int", nullable: true),
                    Loop2010BBId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NM1_837P_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_2_Loop_2010BA_837P_Loop2010BAId",
                        column: x => x.Loop2010BAId,
                        principalTable: "Loop_2010BA_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_2_Loop_2010BB_837P_Loop2010BBId",
                        column: x => x.Loop2010BBId,
                        principalTable: "Loop_2010BB_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420E_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OrderingProviderNameId = table.Column<int>(type: "int", nullable: true),
                    N3_OrderingProviderAddressId = table.Column<int>(type: "int", nullable: true),
                    N4_OrderingProviderCity_State_ZIPCodeId = table.Column<int>(type: "int", nullable: true),
                    PER_OrderingProviderContactInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420E_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420E_837P_N3_AdditionalPatientInformationContactAddress_N3_OrderingProviderAddressId",
                        column: x => x.N3_OrderingProviderAddressId,
                        principalTable: "N3_AdditionalPatientInformationContactAddress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420E_837P_N4_AdditionalPatientInformationContactCity_N4_OrderingProviderCity_State_ZIPCodeId",
                        column: x => x.N4_OrderingProviderCity_State_ZIPCodeId,
                        principalTable: "N4_AdditionalPatientInformationContactCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420E_837P_NM1_OrderingProviderName_NM1_OrderingProviderNameId",
                        column: x => x.NM1_OrderingProviderNameId,
                        principalTable: "NM1_OrderingProviderName",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420E_837P_PER_BillingProviderContactInformation_PER_OrderingProviderContactInformationId",
                        column: x => x.PER_OrderingProviderContactInformationId,
                        principalTable: "PER_BillingProviderContactInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2330C_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_OtherPayerReferringProviderId = table.Column<int>(type: "int", nullable: true),
                    All_NM1_837P_4Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2330C_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2330C_837P_All_NM1_837P_4_All_NM1_837P_4Id",
                        column: x => x.All_NM1_837P_4Id,
                        principalTable: "All_NM1_837P_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2330C_837P_NM1_OtherPayerReferringProvider_NM1_OtherPayerReferringProviderId",
                        column: x => x.NM1_OtherPayerReferringProviderId,
                        principalTable: "NM1_OtherPayerReferringProvider",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2000A_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HL_BillingProviderHierarchicalLevelId = table.Column<int>(type: "int", nullable: true),
                    PRV_BillingProviderSpecialtyInformationId = table.Column<int>(type: "int", nullable: true),
                    CUR_ForeignCurrencyInformationId = table.Column<int>(type: "int", nullable: true),
                    AllNM1Id = table.Column<int>(type: "int", nullable: true),
                    TS837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2000A_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2000A_837P_All_NM1_837P_AllNM1Id",
                        column: x => x.AllNM1Id,
                        principalTable: "All_NM1_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000A_837P_CUR_ForeignCurrencyInformation_3_CUR_ForeignCurrencyInformationId",
                        column: x => x.CUR_ForeignCurrencyInformationId,
                        principalTable: "CUR_ForeignCurrencyInformation_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000A_837P_HL_BillingProviderHierarchicalLevel_HL_BillingProviderHierarchicalLevelId",
                        column: x => x.HL_BillingProviderHierarchicalLevelId,
                        principalTable: "HL_BillingProviderHierarchicalLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000A_837P_PRV_BillingProviderSpecialtyInformation_PRV_BillingProviderSpecialtyInformationId",
                        column: x => x.PRV_BillingProviderSpecialtyInformationId,
                        principalTable: "PRV_BillingProviderSpecialtyInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000A_837P_TS837P_TS837PId",
                        column: x => x.TS837PId,
                        principalTable: "TS837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "All_NM1_837P_5",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Loop2420AId = table.Column<int>(type: "int", nullable: true),
                    Loop2420BId = table.Column<int>(type: "int", nullable: true),
                    Loop2420CId = table.Column<int>(type: "int", nullable: true),
                    Loop2420DId = table.Column<int>(type: "int", nullable: true),
                    Loop2420EId = table.Column<int>(type: "int", nullable: true),
                    Loop2420GId = table.Column<int>(type: "int", nullable: true),
                    Loop2420HId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_All_NM1_837P_5", x => x.Id);
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420A_837P_Loop2420AId",
                        column: x => x.Loop2420AId,
                        principalTable: "Loop_2420A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420B_837P_Loop2420BId",
                        column: x => x.Loop2420BId,
                        principalTable: "Loop_2420B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420C_837P_Loop2420CId",
                        column: x => x.Loop2420CId,
                        principalTable: "Loop_2420C_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420D_837P_Loop2420DId",
                        column: x => x.Loop2420DId,
                        principalTable: "Loop_2420D_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420E_837P_Loop2420EId",
                        column: x => x.Loop2420EId,
                        principalTable: "Loop_2420E_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420G_837P_Loop2420GId",
                        column: x => x.Loop2420GId,
                        principalTable: "Loop_2420G_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_All_NM1_837P_5_Loop_2420H_837P_Loop2420HId",
                        column: x => x.Loop2420HId,
                        principalTable: "Loop_2420H_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OrderingProviderSecondaryIdentification_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2310A_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2330C_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OrderingProviderSecondaryIdentification_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_2_C040_ReferenceIdentifier_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_2_Loop_2310A_837P_Loop_2310A_837PId",
                        column: x => x.Loop_2310A_837PId,
                        principalTable: "Loop_2310A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_2_Loop_2330C_837P_Loop_2330C_837PId",
                        column: x => x.Loop_2330C_837PId,
                        principalTable: "Loop_2330C_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2000B_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HL_SubscriberHierarchicalLevelId = table.Column<int>(type: "int", nullable: true),
                    SBR_SubscriberInformationId = table.Column<int>(type: "int", nullable: true),
                    PAT_PatientInformationId = table.Column<int>(type: "int", nullable: true),
                    AllNM1Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2000A_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2000B_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2000B_837P_All_NM1_837P_2_AllNM1Id",
                        column: x => x.AllNM1Id,
                        principalTable: "All_NM1_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000B_837P_HL_SubscriberHierarchicalLevel_HL_SubscriberHierarchicalLevelId",
                        column: x => x.HL_SubscriberHierarchicalLevelId,
                        principalTable: "HL_SubscriberHierarchicalLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000B_837P_Loop_2000A_837P_Loop_2000A_837PId",
                        column: x => x.Loop_2000A_837PId,
                        principalTable: "Loop_2000A_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000B_837P_PAT_PatientInformation_3_PAT_PatientInformationId",
                        column: x => x.PAT_PatientInformationId,
                        principalTable: "PAT_PatientInformation_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000B_837P_SBR_SubscriberInformation_SBR_SubscriberInformationId",
                        column: x => x.SBR_SubscriberInformationId,
                        principalTable: "SBR_SubscriberInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2420F_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NM1_ReferringProviderNameId = table.Column<int>(type: "int", nullable: true),
                    All_NM1_837P_5Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2420F_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2420F_837P_All_NM1_837P_5_All_NM1_837P_5Id",
                        column: x => x.All_NM1_837P_5Id,
                        principalTable: "All_NM1_837P_5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2420F_837P_NM1_ReferringProviderName_NM1_ReferringProviderNameId",
                        column: x => x.NM1_ReferringProviderNameId,
                        principalTable: "NM1_ReferringProviderName",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2000C_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HL_PatientHierarchicalLevelId = table.Column<int>(type: "int", nullable: true),
                    PAT_PatientInformationId = table.Column<int>(type: "int", nullable: true),
                    Loop2010CAId = table.Column<int>(type: "int", nullable: true),
                    Loop_2000B_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2000C_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2000C_837P_HL_DependentLevel_HL_PatientHierarchicalLevelId",
                        column: x => x.HL_PatientHierarchicalLevelId,
                        principalTable: "HL_DependentLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000C_837P_Loop_2000B_837P_Loop_2000B_837PId",
                        column: x => x.Loop_2000B_837PId,
                        principalTable: "Loop_2000B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000C_837P_Loop_2010CA_837P_Loop2010CAId",
                        column: x => x.Loop2010CAId,
                        principalTable: "Loop_2010CA_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2000C_837P_PAT_PatientInformation_PAT_PatientInformationId",
                        column: x => x.PAT_PatientInformationId,
                        principalTable: "PAT_PatientInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF_OrderingProviderSecondaryIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberGrouporPolicyNumber_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2420B_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2420E_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2420F_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF_OrderingProviderSecondaryIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_C040_ReferenceIdentifier_3_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040_ReferenceIdentifier_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_Loop_2420B_837P_Loop_2420B_837PId",
                        column: x => x.Loop_2420B_837PId,
                        principalTable: "Loop_2420B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_Loop_2420E_837P_Loop_2420E_837PId",
                        column: x => x.Loop_2420E_837PId,
                        principalTable: "Loop_2420E_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_OrderingProviderSecondaryIdentification_Loop_2420F_837P_Loop_2420F_837PId",
                        column: x => x.Loop_2420F_837PId,
                        principalTable: "Loop_2420F_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2300_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLM_ClaimInformationId = table.Column<int>(type: "int", nullable: true),
                    AllDTPId = table.Column<int>(type: "int", nullable: true),
                    CN1_ContractInformationId = table.Column<int>(type: "int", nullable: true),
                    AMT_PatientAmountPaidId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true),
                    NTE_ClaimNoteId = table.Column<int>(type: "int", nullable: true),
                    CR1_AmbulanceTransportInformationId = table.Column<int>(type: "int", nullable: true),
                    CR2_SpinalManipulationServiceInformationId = table.Column<int>(type: "int", nullable: true),
                    AllCRCId = table.Column<int>(type: "int", nullable: true),
                    AllHIId = table.Column<int>(type: "int", nullable: true),
                    HCP_ClaimPricing_RepricingInformationId = table.Column<int>(type: "int", nullable: true),
                    AllNM1Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2000B_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2000C_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2300_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_AMT_PatientAmountPaid_AMT_PatientAmountPaidId",
                        column: x => x.AMT_PatientAmountPaidId,
                        principalTable: "AMT_PatientAmountPaid",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_All_CRC_837P_2_AllCRCId",
                        column: x => x.AllCRCId,
                        principalTable: "All_CRC_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_All_DTP_837P_2_AllDTPId",
                        column: x => x.AllDTPId,
                        principalTable: "All_DTP_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_All_HI_837P_AllHIId",
                        column: x => x.AllHIId,
                        principalTable: "All_HI_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_All_NM1_837P_3_AllNM1Id",
                        column: x => x.AllNM1Id,
                        principalTable: "All_NM1_837P_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_All_REF_837P_6_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_6",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_CLM_ClaimInformation_3_CLM_ClaimInformationId",
                        column: x => x.CLM_ClaimInformationId,
                        principalTable: "CLM_ClaimInformation_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_CN1_ContractInformation_2_CN1_ContractInformationId",
                        column: x => x.CN1_ContractInformationId,
                        principalTable: "CN1_ContractInformation_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_CR1_AmbulanceTransportInformation_CR1_AmbulanceTransportInformationId",
                        column: x => x.CR1_AmbulanceTransportInformationId,
                        principalTable: "CR1_AmbulanceTransportInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_CR2_SpinalManipulationServiceInformation_CR2_SpinalManipulationServiceInformationId",
                        column: x => x.CR2_SpinalManipulationServiceInformationId,
                        principalTable: "CR2_SpinalManipulationServiceInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_HCP_ClaimPricing_HCP_ClaimPricing_RepricingInformationId",
                        column: x => x.HCP_ClaimPricing_RepricingInformationId,
                        principalTable: "HCP_ClaimPricing",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_Loop_2000B_837P_Loop_2000B_837PId",
                        column: x => x.Loop_2000B_837PId,
                        principalTable: "Loop_2000B_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_Loop_2000C_837P_Loop_2000C_837PId",
                        column: x => x.Loop_2000C_837PId,
                        principalTable: "Loop_2000C_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2300_837P_NTE_ClaimNote_2_NTE_ClaimNoteId",
                        column: x => x.NTE_ClaimNoteId,
                        principalTable: "NTE_ClaimNote_2",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2320_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SBR_OtherSubscriberInformationId = table.Column<int>(type: "int", nullable: true),
                    AllAMTId = table.Column<int>(type: "int", nullable: true),
                    OI_OtherInsuranceCoverageInformationId = table.Column<int>(type: "int", nullable: true),
                    MOA_OutpatientAdjudicationInformationId = table.Column<int>(type: "int", nullable: true),
                    AllNM1Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2300_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2320_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2320_837P_All_AMT_837P_2_AllAMTId",
                        column: x => x.AllAMTId,
                        principalTable: "All_AMT_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2320_837P_All_NM1_837P_4_AllNM1Id",
                        column: x => x.AllNM1Id,
                        principalTable: "All_NM1_837P_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2320_837P_Loop_2300_837P_Loop_2300_837PId",
                        column: x => x.Loop_2300_837PId,
                        principalTable: "Loop_2300_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2320_837P_MOA_OutpatientAdjudicationInformation_MOA_OutpatientAdjudicationInformationId",
                        column: x => x.MOA_OutpatientAdjudicationInformationId,
                        principalTable: "MOA_OutpatientAdjudicationInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2320_837P_OI_OtherInsuranceCoverageInformation_2_OI_OtherInsuranceCoverageInformationId",
                        column: x => x.OI_OtherInsuranceCoverageInformationId,
                        principalTable: "OI_OtherInsuranceCoverageInformation_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2320_837P_SBR_OtherSubscriberInformation_SBR_OtherSubscriberInformationId",
                        column: x => x.SBR_OtherSubscriberInformationId,
                        principalTable: "SBR_OtherSubscriberInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2400_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LX_ServiceLineNumberId = table.Column<int>(type: "int", nullable: true),
                    SV1_ProfessionalServiceId = table.Column<int>(type: "int", nullable: true),
                    SV5_DurableMedicalEquipmentServiceId = table.Column<int>(type: "int", nullable: true),
                    AllPWKId = table.Column<int>(type: "int", nullable: true),
                    CR1_AmbulanceTransportInformationId = table.Column<int>(type: "int", nullable: true),
                    CR3_DurableMedicalEquipmentCertificationId = table.Column<int>(type: "int", nullable: true),
                    AllCRCId = table.Column<int>(type: "int", nullable: true),
                    AllDTPId = table.Column<int>(type: "int", nullable: true),
                    AllQTYId = table.Column<int>(type: "int", nullable: true),
                    CN1_ContractInformationId = table.Column<int>(type: "int", nullable: true),
                    AllREFId = table.Column<int>(type: "int", nullable: true),
                    AllAMTId = table.Column<int>(type: "int", nullable: true),
                    AllNTEId = table.Column<int>(type: "int", nullable: true),
                    PS1_PurchasedServiceInformationId = table.Column<int>(type: "int", nullable: true),
                    HCP_LinePricing_RepricingInformationId = table.Column<int>(type: "int", nullable: true),
                    Loop2410Id = table.Column<int>(type: "int", nullable: true),
                    AllNM1Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2300_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2400_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_AMT_837P_AllAMTId",
                        column: x => x.AllAMTId,
                        principalTable: "All_AMT_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_CRC_837P_AllCRCId",
                        column: x => x.AllCRCId,
                        principalTable: "All_CRC_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_DTP_837P_AllDTPId",
                        column: x => x.AllDTPId,
                        principalTable: "All_DTP_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_NM1_837P_5_AllNM1Id",
                        column: x => x.AllNM1Id,
                        principalTable: "All_NM1_837P_5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_NTE_837P_AllNTEId",
                        column: x => x.AllNTEId,
                        principalTable: "All_NTE_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_PWK_837P_AllPWKId",
                        column: x => x.AllPWKId,
                        principalTable: "All_PWK_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_QTY_837P_AllQTYId",
                        column: x => x.AllQTYId,
                        principalTable: "All_QTY_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_All_REF_837P_2_AllREFId",
                        column: x => x.AllREFId,
                        principalTable: "All_REF_837P_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_CN1_ContractInformation_2_CN1_ContractInformationId",
                        column: x => x.CN1_ContractInformationId,
                        principalTable: "CN1_ContractInformation_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_CR1_AmbulanceTransportInformation_CR1_AmbulanceTransportInformationId",
                        column: x => x.CR1_AmbulanceTransportInformationId,
                        principalTable: "CR1_AmbulanceTransportInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_CR3_DurableMedicalEquipmentCertification_CR3_DurableMedicalEquipmentCertificationId",
                        column: x => x.CR3_DurableMedicalEquipmentCertificationId,
                        principalTable: "CR3_DurableMedicalEquipmentCertification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_HCP_LinePricing_3_HCP_LinePricing_RepricingInformationId",
                        column: x => x.HCP_LinePricing_RepricingInformationId,
                        principalTable: "HCP_LinePricing_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_LX_HeaderNumber_LX_ServiceLineNumberId",
                        column: x => x.LX_ServiceLineNumberId,
                        principalTable: "LX_HeaderNumber",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_Loop_2300_837P_Loop_2300_837PId",
                        column: x => x.Loop_2300_837PId,
                        principalTable: "Loop_2300_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_Loop_2410_837P_Loop2410Id",
                        column: x => x.Loop2410Id,
                        principalTable: "Loop_2410_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_PS1_PurchasedServiceInformation_PS1_PurchasedServiceInformationId",
                        column: x => x.PS1_PurchasedServiceInformationId,
                        principalTable: "PS1_PurchasedServiceInformation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_SV1_ProfessionalService_SV1_ProfessionalServiceId",
                        column: x => x.SV1_ProfessionalServiceId,
                        principalTable: "SV1_ProfessionalService",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2400_837P_SV5_DurableMedicalEquipmentService_SV5_DurableMedicalEquipmentServiceId",
                        column: x => x.SV5_DurableMedicalEquipmentServiceId,
                        principalTable: "SV5_DurableMedicalEquipmentService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PWK_ClaimSupplementalInformation_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentReportTypeCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportTransmissionCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportCopiesNeeded_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityIdentifierCode_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationCodeQualifier_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentControlNumber_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentDescription_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionsIndicated_08Id = table.Column<int>(type: "int", nullable: true),
                    RequestCategoryCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loop_2300_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PWK_ClaimSupplementalInformation_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PWK_ClaimSupplementalInformation_3_C002_ActionsIndicated_2_ActionsIndicated_08Id",
                        column: x => x.ActionsIndicated_08Id,
                        principalTable: "C002_ActionsIndicated_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PWK_ClaimSupplementalInformation_3_Loop_2300_837P_Loop_2300_837PId",
                        column: x => x.Loop_2300_837PId,
                        principalTable: "Loop_2300_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "K3_FileInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixedFormatInformation_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordFormatCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeUnitOfMeasure_03Id = table.Column<int>(type: "int", nullable: true),
                    Loop_2300_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2400_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_K3_FileInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_K3_FileInformation_C001_CompositeUnitOfMeasure_CompositeUnitOfMeasure_03Id",
                        column: x => x.CompositeUnitOfMeasure_03Id,
                        principalTable: "C001_CompositeUnitOfMeasure",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_K3_FileInformation_Loop_2300_837P_Loop_2300_837PId",
                        column: x => x.Loop_2300_837PId,
                        principalTable: "Loop_2300_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_K3_FileInformation_Loop_2400_837P_Loop_2400_837PId",
                        column: x => x.Loop_2400_837PId,
                        principalTable: "Loop_2400_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2430_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SVD_LineAdjudicationInformationId = table.Column<int>(type: "int", nullable: true),
                    DTP_LineCheckorRemittanceDateId = table.Column<int>(type: "int", nullable: true),
                    AMT_RemainingPatientLiabilityId = table.Column<int>(type: "int", nullable: true),
                    Loop_2400_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2430_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2430_837P_AMT_RemainingPatientLiability_AMT_RemainingPatientLiabilityId",
                        column: x => x.AMT_RemainingPatientLiabilityId,
                        principalTable: "AMT_RemainingPatientLiability",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2430_837P_DTP_ClaimCheckOrRemittanceDate_DTP_LineCheckorRemittanceDateId",
                        column: x => x.DTP_LineCheckorRemittanceDateId,
                        principalTable: "DTP_ClaimCheckOrRemittanceDate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2430_837P_Loop_2400_837P_Loop_2400_837PId",
                        column: x => x.Loop_2400_837PId,
                        principalTable: "Loop_2400_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2430_837P_SVD_LineAdjudicationInformation_3_SVD_LineAdjudicationInformationId",
                        column: x => x.SVD_LineAdjudicationInformationId,
                        principalTable: "SVD_LineAdjudicationInformation_3",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_2440_837P",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LQ_FormIdentificationCodeId = table.Column<int>(type: "int", nullable: true),
                    Loop_2400_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_2440_837P", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_2440_837P_LQ_FormIdentificationCode_LQ_FormIdentificationCodeId",
                        column: x => x.LQ_FormIdentificationCodeId,
                        principalTable: "LQ_FormIdentificationCode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_2440_837P_Loop_2400_837P_Loop_2400_837PId",
                        column: x => x.Loop_2400_837PId,
                        principalTable: "Loop_2400_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MEA_TestResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementReferenceIdentificationCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementQualifier_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestResults_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeUnitOfMeasure_04Id = table.Column<int>(type: "int", nullable: true),
                    RangeMinimum_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeMaximum_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementSignificanceCode_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementAttributeCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurfaceLayerPositionCode_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementMethodorDevice_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeListQualifierCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryCode_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loop_2400_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEA_TestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MEA_TestResult_C001_CompositeUnitOfMeasure_CompositeUnitOfMeasure_04Id",
                        column: x => x.CompositeUnitOfMeasure_04Id,
                        principalTable: "C001_CompositeUnitOfMeasure",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MEA_TestResult_Loop_2400_837P_Loop_2400_837PId",
                        column: x => x.Loop_2400_837PId,
                        principalTable: "Loop_2400_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CAS_ClaimLevelAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimAdjustmentGroupCode_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentReasonCode_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentQuantity_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentReasonCode_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount_06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentQuantity_07 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentReasonCode_08 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount_09 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentQuantity_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentReasonCode_11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount_12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentQuantity_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentReasonCode_14 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount_15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentQuantity_16 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentReasonCode_17 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentAmount_18 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentQuantity_19 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loop_2320_837PId = table.Column<int>(type: "int", nullable: true),
                    Loop_2430_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAS_ClaimLevelAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CAS_ClaimLevelAdjustments_Loop_2320_837P_Loop_2320_837PId",
                        column: x => x.Loop_2320_837PId,
                        principalTable: "Loop_2320_837P",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CAS_ClaimLevelAdjustments_Loop_2430_837P_Loop_2430_837PId",
                        column: x => x.Loop_2430_837PId,
                        principalTable: "Loop_2430_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FRM_SupportingDocumentation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionNumberLetter_01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionResponse_02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionResponse_03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionResponse_04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionResponse_05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Loop_2440_837PId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRM_SupportingDocumentation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FRM_SupportingDocumentation_Loop_2440_837P_Loop_2440_837PId",
                        column: x => x.Loop_2440_837PId,
                        principalTable: "Loop_2440_837P",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_All_AMT_837P_AMT_PostageClaimedAmountId",
                table: "All_AMT_837P",
                column: "AMT_PostageClaimedAmountId");

            migrationBuilder.CreateIndex(
                name: "IX_All_AMT_837P_AMT_SalesTaxAmountId",
                table: "All_AMT_837P",
                column: "AMT_SalesTaxAmountId");

            migrationBuilder.CreateIndex(
                name: "IX_All_AMT_837P_2_AMT_CoordinationofBenefits_COB_PayerPaidAmountId",
                table: "All_AMT_837P_2",
                column: "AMT_CoordinationofBenefits_COB_PayerPaidAmountId");

            migrationBuilder.CreateIndex(
                name: "IX_All_AMT_837P_2_AMT_CoordinationofBenefits_COB_TotalNon_AmountId",
                table: "All_AMT_837P_2",
                column: "AMT_CoordinationofBenefits_COB_TotalNon_AmountId");

            migrationBuilder.CreateIndex(
                name: "IX_All_AMT_837P_2_AMT_RemainingPatientLiabilityId",
                table: "All_AMT_837P_2",
                column: "AMT_RemainingPatientLiabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_All_CRC_837P_CRC_ConditionIndicator_DurableMedicalEquipmentId",
                table: "All_CRC_837P",
                column: "CRC_ConditionIndicator_DurableMedicalEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_All_CRC_837P_CRC_HospiceEmployeeIndicatorId",
                table: "All_CRC_837P",
                column: "CRC_HospiceEmployeeIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_All_CRC_837P_2_CRC_EPSDTReferralId",
                table: "All_CRC_837P_2",
                column: "CRC_EPSDTReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_All_CRC_837P_2_CRC_HomeboundIndicatorId",
                table: "All_CRC_837P_2",
                column: "CRC_HomeboundIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_BeginTherapyDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_BeginTherapyDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_DATE_CertificationRevision_RecertificationDateId",
                table: "All_DTP_837P",
                column: "DTP_DATE_CertificationRevision_RecertificationDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_InitialTreatmentDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_InitialTreatmentDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_LastCertificationDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_LastCertificationDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_LastSeenDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_LastSeenDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_LastX_DateId",
                table: "All_DTP_837P",
                column: "DTP_Date_LastX_DateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_PrescriptionDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_PrescriptionDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_ServiceDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_ServiceDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_DTP_Date_ShippedDateId",
                table: "All_DTP_837P",
                column: "DTP_Date_ShippedDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_AccidentId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_AccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_AcuteManifestationId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_AcuteManifestationId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_AdmissionId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_AuthorizedReturntoWorkId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_AuthorizedReturntoWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_DisabilityDatesId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_DisabilityDatesId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_DischargeId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_DischargeId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_HearingandVisionPrescriptionDateId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_HearingandVisionPrescriptionDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_InitialTreatmentDateId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_InitialTreatmentDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_LastMenstrualPeriodId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_LastMenstrualPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_LastSeenDateId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_LastSeenDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_LastWorkedId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_LastWorkedId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_LastX_DateId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_LastX_DateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_OnsetofCurrentIllnessorSymptomId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_OnsetofCurrentIllnessorSymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_Date_RepricerReceivedDateId",
                table: "All_DTP_837P_2",
                column: "DTP_Date_RepricerReceivedDateId");

            migrationBuilder.CreateIndex(
                name: "IX_All_DTP_837P_2_DTP_PropertyandCasualtyDateofFirstContactId",
                table: "All_DTP_837P_2",
                column: "DTP_PropertyandCasualtyDateofFirstContactId");

            migrationBuilder.CreateIndex(
                name: "IX_All_HI_837P_HI_AnesthesiaRelatedProcedureId",
                table: "All_HI_837P",
                column: "HI_AnesthesiaRelatedProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_All_HI_837P_HI_HealthCareDiagnosisCodeId",
                table: "All_HI_837P",
                column: "HI_HealthCareDiagnosisCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_Loop2010AAId",
                table: "All_NM1_837P",
                column: "Loop2010AAId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_Loop2010ABId",
                table: "All_NM1_837P",
                column: "Loop2010ABId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_Loop2010ACId",
                table: "All_NM1_837P",
                column: "Loop2010ACId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_2_Loop2010BAId",
                table: "All_NM1_837P_2",
                column: "Loop2010BAId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_2_Loop2010BBId",
                table: "All_NM1_837P_2",
                column: "Loop2010BBId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_3_Loop2310BId",
                table: "All_NM1_837P_3",
                column: "Loop2310BId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_3_Loop2310CId",
                table: "All_NM1_837P_3",
                column: "Loop2310CId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_3_Loop2310DId",
                table: "All_NM1_837P_3",
                column: "Loop2310DId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_3_Loop2310EId",
                table: "All_NM1_837P_3",
                column: "Loop2310EId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_3_Loop2310FId",
                table: "All_NM1_837P_3",
                column: "Loop2310FId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_4_Loop2330AId",
                table: "All_NM1_837P_4",
                column: "Loop2330AId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_4_Loop2330BId",
                table: "All_NM1_837P_4",
                column: "Loop2330BId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_4_Loop2330DId",
                table: "All_NM1_837P_4",
                column: "Loop2330DId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_4_Loop2330EId",
                table: "All_NM1_837P_4",
                column: "Loop2330EId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_4_Loop2330FId",
                table: "All_NM1_837P_4",
                column: "Loop2330FId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_4_Loop2330GId",
                table: "All_NM1_837P_4",
                column: "Loop2330GId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420AId",
                table: "All_NM1_837P_5",
                column: "Loop2420AId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420BId",
                table: "All_NM1_837P_5",
                column: "Loop2420BId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420CId",
                table: "All_NM1_837P_5",
                column: "Loop2420CId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420DId",
                table: "All_NM1_837P_5",
                column: "Loop2420DId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420EId",
                table: "All_NM1_837P_5",
                column: "Loop2420EId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420GId",
                table: "All_NM1_837P_5",
                column: "Loop2420GId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_5_Loop2420HId",
                table: "All_NM1_837P_5",
                column: "Loop2420HId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_6_Loop1000AId",
                table: "All_NM1_837P_6",
                column: "Loop1000AId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NM1_837P_6_Loop1000BId",
                table: "All_NM1_837P_6",
                column: "Loop1000BId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NTE_837P_NTE_LineNoteId",
                table: "All_NTE_837P",
                column: "NTE_LineNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_All_NTE_837P_NTE_ThirdPartyOrganizationNotesId",
                table: "All_NTE_837P",
                column: "NTE_ThirdPartyOrganizationNotesId");

            migrationBuilder.CreateIndex(
                name: "IX_All_PWK_837P_PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicatorId",
                table: "All_PWK_837P",
                column: "PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_All_QTY_837P_QTY_AmbulancePatientCountId",
                table: "All_QTY_837P",
                column: "QTY_AmbulancePatientCountId");

            migrationBuilder.CreateIndex(
                name: "IX_All_QTY_837P_QTY_ObstetricAnesthesiaAdditionalUnitsId",
                table: "All_QTY_837P",
                column: "QTY_ObstetricAnesthesiaAdditionalUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_REF_OtherPayerClaimAdjustmentIndicatorId",
                table: "All_REF_837P",
                column: "REF_OtherPayerClaimAdjustmentIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_REF_OtherPayerClaimControlNumberId",
                table: "All_REF_837P",
                column: "REF_OtherPayerClaimControlNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_REF_OtherPayerPriorAuthorizationNumberId",
                table: "All_REF_837P",
                column: "REF_OtherPayerPriorAuthorizationNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_REF_OtherPayerReferralNumberId",
                table: "All_REF_837P",
                column: "REF_OtherPayerReferralNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_AdjustedRepricedLineItemReferenceNumberId",
                table: "All_REF_837P_2",
                column: "REF_AdjustedRepricedLineItemReferenceNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId",
                table: "All_REF_837P_2",
                column: "REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_ImmunizationBatchNumberId",
                table: "All_REF_837P_2",
                column: "REF_ImmunizationBatchNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_LineItemControlNumberId",
                table: "All_REF_837P_2",
                column: "REF_LineItemControlNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_MammographyCertificationNumberId",
                table: "All_REF_837P_2",
                column: "REF_MammographyCertificationNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_ReferringClinicalLaboratoryImprovementAmendment_CLIA_FacilityIdentificationId",
                table: "All_REF_837P_2",
                column: "REF_ReferringClinicalLaboratoryImprovementAmendment_CLIA_FacilityIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_2_REF_RepricedLineItemReferenceNumberId",
                table: "All_REF_837P_2",
                column: "REF_RepricedLineItemReferenceNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_3_REF_Pay_ToPlanSecondaryIdentificationId",
                table: "All_REF_837P_3",
                column: "REF_Pay_ToPlanSecondaryIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_3_REF_Pay_ToPlanTaxIdentificationNumberId",
                table: "All_REF_837P_3",
                column: "REF_Pay_ToPlanTaxIdentificationNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_4_REF_PropertyandCasualtyClaimNumberId",
                table: "All_REF_837P_4",
                column: "REF_PropertyandCasualtyClaimNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_4_REF_SubscriberSecondaryIdentificationId",
                table: "All_REF_837P_4",
                column: "REF_SubscriberSecondaryIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_AdjustedRepricedClaimNumberId",
                table: "All_REF_837P_6",
                column: "REF_AdjustedRepricedClaimNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_CarePlanOversightId",
                table: "All_REF_837P_6",
                column: "REF_CarePlanOversightId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_ClaimIdentifierForTransmissionIntermediariesId",
                table: "All_REF_837P_6",
                column: "REF_ClaimIdentifierForTransmissionIntermediariesId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId",
                table: "All_REF_837P_6",
                column: "REF_ClinicalLaboratoryImprovementAmendment_CLIA_NumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_DemonstrationProjectIdentifierId",
                table: "All_REF_837P_6",
                column: "REF_DemonstrationProjectIdentifierId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_InvestigationalDeviceExemptionNumberId",
                table: "All_REF_837P_6",
                column: "REF_InvestigationalDeviceExemptionNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_MammographyCertificationNumberId",
                table: "All_REF_837P_6",
                column: "REF_MammographyCertificationNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_MandatoryMedicare_Section4081_CrossoverIndicatorId",
                table: "All_REF_837P_6",
                column: "REF_MandatoryMedicare_Section4081_CrossoverIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_MedicalRecordNumberId",
                table: "All_REF_837P_6",
                column: "REF_MedicalRecordNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_PayerClaimControlNumberId",
                table: "All_REF_837P_6",
                column: "REF_PayerClaimControlNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_PriorAuthorizationId",
                table: "All_REF_837P_6",
                column: "REF_PriorAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_ReferralNumberId",
                table: "All_REF_837P_6",
                column: "REF_ReferralNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_RepricedClaimNumberId",
                table: "All_REF_837P_6",
                column: "REF_RepricedClaimNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_6_REF_ServiceAuthorizationExceptionCodeId",
                table: "All_REF_837P_6",
                column: "REF_ServiceAuthorizationExceptionCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_7_REF_PropertyandCasualtyClaimNumberId",
                table: "All_REF_837P_7",
                column: "REF_PropertyandCasualtyClaimNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_7_REF_PropertyandCasualtyPatientIdentifierId",
                table: "All_REF_837P_7",
                column: "REF_PropertyandCasualtyPatientIdentifierId");

            migrationBuilder.CreateIndex(
                name: "IX_All_REF_837P_8_REF_BillingProviderTaxIdentificationId",
                table: "All_REF_837P_8",
                column: "REF_BillingProviderTaxIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_C056_CompositeRaceOrEthnicityInformation_DMG_PatientDemographicInformationId",
                table: "C056_CompositeRaceOrEthnicityInformation",
                column: "DMG_PatientDemographicInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CAS_ClaimLevelAdjustments_Loop_2320_837PId",
                table: "CAS_ClaimLevelAdjustments",
                column: "Loop_2320_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_CAS_ClaimLevelAdjustments_Loop_2430_837PId",
                table: "CAS_ClaimLevelAdjustments",
                column: "Loop_2430_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_CLM_ClaimInformation_3_HealthCareServiceLocationInformation_05Id",
                table: "CLM_ClaimInformation_3",
                column: "HealthCareServiceLocationInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_CLM_ClaimInformation_3_RelatedCausesInformation_11Id",
                table: "CLM_ClaimInformation_3",
                column: "RelatedCausesInformation_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_CRC_AmbulanceCertification_All_CRC_837P_2Id",
                table: "CRC_AmbulanceCertification",
                column: "All_CRC_837P_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CRC_AmbulanceCertification_All_CRC_837PId",
                table: "CRC_AmbulanceCertification",
                column: "All_CRC_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_CRC_PatientConditionInformation_All_CRC_837P_2Id",
                table: "CRC_PatientConditionInformation",
                column: "All_CRC_837P_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CTP_DrugQuantity_CompositeUnitOfMeasure_05Id",
                table: "CTP_DrugQuantity",
                column: "CompositeUnitOfMeasure_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_AssumedandRelinquishedCareDates_All_DTP_837P_2Id",
                table: "DTP_AssumedandRelinquishedCareDates",
                column: "All_DTP_837P_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_TestDate_All_DTP_837PId",
                table: "DTP_TestDate",
                column: "All_DTP_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_FRM_SupportingDocumentation_Loop_2440_837PId",
                table: "FRM_SupportingDocumentation",
                column: "Loop_2440_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_01Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_02Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_02Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_03Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_04Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_05Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_06Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_06Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_07Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_07Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_08Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_09Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_09Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_10Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_11Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_AnesthesiaRelatedProcedure_HealthCareCodeInformation_12Id",
                table: "HI_AnesthesiaRelatedProcedure",
                column: "HealthCareCodeInformation_12Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_All_HI_837PId",
                table: "HI_ConditionInformation",
                column: "All_HI_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_01Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_02Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_02Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_03Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_04Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_05Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_06Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_06Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_07Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_07Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_08Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_09Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_09Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_10Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_11Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_ConditionInformation_HealthCareCodeInformation_12Id",
                table: "HI_ConditionInformation",
                column: "HealthCareCodeInformation_12Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_01Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_02Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_02Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_03Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_04Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_05Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_06Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_06Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_07Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_07Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_08Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_09Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_09Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_10Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_11Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_DependentHealthCareDiagnosisCode_2_HealthCareCodeInformation_12Id",
                table: "HI_DependentHealthCareDiagnosisCode_2",
                column: "HealthCareCodeInformation_12Id");

            migrationBuilder.CreateIndex(
                name: "IX_K3_FileInformation_CompositeUnitOfMeasure_03Id",
                table: "K3_FileInformation",
                column: "CompositeUnitOfMeasure_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_K3_FileInformation_Loop_2300_837PId",
                table: "K3_FileInformation",
                column: "Loop_2300_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_K3_FileInformation_Loop_2400_837PId",
                table: "K3_FileInformation",
                column: "Loop_2400_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_1000A_837P_NM1_SubmitterNameId",
                table: "Loop_1000A_837P",
                column: "NM1_SubmitterNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_1000B_837P_NM1_ReceiverNameId",
                table: "Loop_1000B_837P",
                column: "NM1_ReceiverNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000A_837P_AllNM1Id",
                table: "Loop_2000A_837P",
                column: "AllNM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000A_837P_CUR_ForeignCurrencyInformationId",
                table: "Loop_2000A_837P",
                column: "CUR_ForeignCurrencyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000A_837P_HL_BillingProviderHierarchicalLevelId",
                table: "Loop_2000A_837P",
                column: "HL_BillingProviderHierarchicalLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000A_837P_PRV_BillingProviderSpecialtyInformationId",
                table: "Loop_2000A_837P",
                column: "PRV_BillingProviderSpecialtyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000A_837P_TS837PId",
                table: "Loop_2000A_837P",
                column: "TS837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000B_837P_AllNM1Id",
                table: "Loop_2000B_837P",
                column: "AllNM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000B_837P_HL_SubscriberHierarchicalLevelId",
                table: "Loop_2000B_837P",
                column: "HL_SubscriberHierarchicalLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000B_837P_Loop_2000A_837PId",
                table: "Loop_2000B_837P",
                column: "Loop_2000A_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000B_837P_PAT_PatientInformationId",
                table: "Loop_2000B_837P",
                column: "PAT_PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000B_837P_SBR_SubscriberInformationId",
                table: "Loop_2000B_837P",
                column: "SBR_SubscriberInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000C_837P_HL_PatientHierarchicalLevelId",
                table: "Loop_2000C_837P",
                column: "HL_PatientHierarchicalLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000C_837P_Loop_2000B_837PId",
                table: "Loop_2000C_837P",
                column: "Loop_2000B_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000C_837P_Loop2010CAId",
                table: "Loop_2000C_837P",
                column: "Loop2010CAId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2000C_837P_PAT_PatientInformationId",
                table: "Loop_2000C_837P",
                column: "PAT_PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AA_837P_AllREFId",
                table: "Loop_2010AA_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AA_837P_N3_BillingProviderAddressId",
                table: "Loop_2010AA_837P",
                column: "N3_BillingProviderAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AA_837P_N4_BillingProviderCity_State_ZIPCodeId",
                table: "Loop_2010AA_837P",
                column: "N4_BillingProviderCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AA_837P_NM1_BillingProviderNameId",
                table: "Loop_2010AA_837P",
                column: "NM1_BillingProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AB_837P_N3_Pay_ToAddress_ADDRESSId",
                table: "Loop_2010AB_837P",
                column: "N3_Pay_ToAddress_ADDRESSId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AB_837P_N4_Pay_AddressCity_State_ZIPCodeId",
                table: "Loop_2010AB_837P",
                column: "N4_Pay_AddressCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AB_837P_NM1_Pay_AddressNameId",
                table: "Loop_2010AB_837P",
                column: "NM1_Pay_AddressNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AC_837P_AllREFId",
                table: "Loop_2010AC_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AC_837P_N3_Pay_ToPlanAddressId",
                table: "Loop_2010AC_837P",
                column: "N3_Pay_ToPlanAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AC_837P_N4_Pay_ToPlanCity_State_ZIPCodeId",
                table: "Loop_2010AC_837P",
                column: "N4_Pay_ToPlanCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010AC_837P_NM1_Pay_ToPlanNameId",
                table: "Loop_2010AC_837P",
                column: "NM1_Pay_ToPlanNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BA_837P_AllREFId",
                table: "Loop_2010BA_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BA_837P_DMG_SubscriberDemographicInformationId",
                table: "Loop_2010BA_837P",
                column: "DMG_SubscriberDemographicInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BA_837P_N3_SubscriberAddressId",
                table: "Loop_2010BA_837P",
                column: "N3_SubscriberAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BA_837P_N4_SubscriberCity_State_ZIPCodeId",
                table: "Loop_2010BA_837P",
                column: "N4_SubscriberCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BA_837P_NM1_SubscriberNameId",
                table: "Loop_2010BA_837P",
                column: "NM1_SubscriberNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BA_837P_PER_PropertyandCasualtySubscriberContactInformationId",
                table: "Loop_2010BA_837P",
                column: "PER_PropertyandCasualtySubscriberContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BB_837P_AllREFId",
                table: "Loop_2010BB_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BB_837P_N3_PayerAddressId",
                table: "Loop_2010BB_837P",
                column: "N3_PayerAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BB_837P_N4_PayerCity_State_ZIPCodeId",
                table: "Loop_2010BB_837P",
                column: "N4_PayerCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010BB_837P_NM1_PayerNameId",
                table: "Loop_2010BB_837P",
                column: "NM1_PayerNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010CA_837P_AllREFId",
                table: "Loop_2010CA_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010CA_837P_DMG_PatientDemographicInformationId",
                table: "Loop_2010CA_837P",
                column: "DMG_PatientDemographicInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010CA_837P_N3_PatientAddressId",
                table: "Loop_2010CA_837P",
                column: "N3_PatientAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010CA_837P_N4_PatientCity_State_ZIPCodeId",
                table: "Loop_2010CA_837P",
                column: "N4_PatientCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010CA_837P_NM1_PatientNameId",
                table: "Loop_2010CA_837P",
                column: "NM1_PatientNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2010CA_837P_PER_PropertyandCasualtyPatientContactInformationId",
                table: "Loop_2010CA_837P",
                column: "PER_PropertyandCasualtyPatientContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_AllCRCId",
                table: "Loop_2300_837P",
                column: "AllCRCId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_AllDTPId",
                table: "Loop_2300_837P",
                column: "AllDTPId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_AllHIId",
                table: "Loop_2300_837P",
                column: "AllHIId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_AllNM1Id",
                table: "Loop_2300_837P",
                column: "AllNM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_AllREFId",
                table: "Loop_2300_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_AMT_PatientAmountPaidId",
                table: "Loop_2300_837P",
                column: "AMT_PatientAmountPaidId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_CLM_ClaimInformationId",
                table: "Loop_2300_837P",
                column: "CLM_ClaimInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_CN1_ContractInformationId",
                table: "Loop_2300_837P",
                column: "CN1_ContractInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_CR1_AmbulanceTransportInformationId",
                table: "Loop_2300_837P",
                column: "CR1_AmbulanceTransportInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_CR2_SpinalManipulationServiceInformationId",
                table: "Loop_2300_837P",
                column: "CR2_SpinalManipulationServiceInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_HCP_ClaimPricing_RepricingInformationId",
                table: "Loop_2300_837P",
                column: "HCP_ClaimPricing_RepricingInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_Loop_2000B_837PId",
                table: "Loop_2300_837P",
                column: "Loop_2000B_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_Loop_2000C_837PId",
                table: "Loop_2300_837P",
                column: "Loop_2000C_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2300_837P_NTE_ClaimNoteId",
                table: "Loop_2300_837P",
                column: "NTE_ClaimNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310A_837P_All_NM1_837P_3Id",
                table: "Loop_2310A_837P",
                column: "All_NM1_837P_3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310A_837P_NM1_ReferringProviderNameId",
                table: "Loop_2310A_837P",
                column: "NM1_ReferringProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310B_837P_NM1_RenderingProviderNameId",
                table: "Loop_2310B_837P",
                column: "NM1_RenderingProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310B_837P_PRV_RenderingProviderSpecialtyInformationId",
                table: "Loop_2310B_837P",
                column: "PRV_RenderingProviderSpecialtyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310C_837P_N3_ServiceFacilityLocationAddressId",
                table: "Loop_2310C_837P",
                column: "N3_ServiceFacilityLocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310C_837P_N4_ServiceFacilityLocationCity_State_ZIPCodeId",
                table: "Loop_2310C_837P",
                column: "N4_ServiceFacilityLocationCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310C_837P_NM1_ServiceFacilityLocationNameId",
                table: "Loop_2310C_837P",
                column: "NM1_ServiceFacilityLocationNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310C_837P_PER_ServiceFacilityContactInformationId",
                table: "Loop_2310C_837P",
                column: "PER_ServiceFacilityContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310D_837P_NM1_SupervisingProviderNameId",
                table: "Loop_2310D_837P",
                column: "NM1_SupervisingProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310E_837P_N3_AmbulancePick_LocationAddressId",
                table: "Loop_2310E_837P",
                column: "N3_AmbulancePick_LocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310E_837P_N4_AmbulancePick_LocationCity_State_ZipCodeId",
                table: "Loop_2310E_837P",
                column: "N4_AmbulancePick_LocationCity_State_ZipCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310E_837P_NM1_AmbulancePick_LocationId",
                table: "Loop_2310E_837P",
                column: "NM1_AmbulancePick_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310F_837P_N3_AmbulanceDrop_LocationAddressId",
                table: "Loop_2310F_837P",
                column: "N3_AmbulanceDrop_LocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310F_837P_N4_AmbulanceDrop_LocationCity_State_ZipCodeId",
                table: "Loop_2310F_837P",
                column: "N4_AmbulanceDrop_LocationCity_State_ZipCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2310F_837P_NM1_AmbulanceDrop_LocationId",
                table: "Loop_2310F_837P",
                column: "NM1_AmbulanceDrop_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2320_837P_AllAMTId",
                table: "Loop_2320_837P",
                column: "AllAMTId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2320_837P_AllNM1Id",
                table: "Loop_2320_837P",
                column: "AllNM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2320_837P_Loop_2300_837PId",
                table: "Loop_2320_837P",
                column: "Loop_2300_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2320_837P_MOA_OutpatientAdjudicationInformationId",
                table: "Loop_2320_837P",
                column: "MOA_OutpatientAdjudicationInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2320_837P_OI_OtherInsuranceCoverageInformationId",
                table: "Loop_2320_837P",
                column: "OI_OtherInsuranceCoverageInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2320_837P_SBR_OtherSubscriberInformationId",
                table: "Loop_2320_837P",
                column: "SBR_OtherSubscriberInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330A_837P_N3_OtherSubscriberAddressId",
                table: "Loop_2330A_837P",
                column: "N3_OtherSubscriberAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330A_837P_N4_OtherSubscriberCity_State_ZIPCodeId",
                table: "Loop_2330A_837P",
                column: "N4_OtherSubscriberCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330A_837P_NM1_OtherSubscriberNameId",
                table: "Loop_2330A_837P",
                column: "NM1_OtherSubscriberNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330A_837P_REF_OtherSubscriberSecondaryIdentificationId",
                table: "Loop_2330A_837P",
                column: "REF_OtherSubscriberSecondaryIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330B_837P_AllREFId",
                table: "Loop_2330B_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330B_837P_DTP_ClaimCheckorRemittanceDateId",
                table: "Loop_2330B_837P",
                column: "DTP_ClaimCheckorRemittanceDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330B_837P_N3_OtherPayerAddressId",
                table: "Loop_2330B_837P",
                column: "N3_OtherPayerAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330B_837P_N4_OtherPayerCity_State_ZIPCodeId",
                table: "Loop_2330B_837P",
                column: "N4_OtherPayerCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330B_837P_NM1_OtherPayerNameId",
                table: "Loop_2330B_837P",
                column: "NM1_OtherPayerNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330C_837P_All_NM1_837P_4Id",
                table: "Loop_2330C_837P",
                column: "All_NM1_837P_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330C_837P_NM1_OtherPayerReferringProviderId",
                table: "Loop_2330C_837P",
                column: "NM1_OtherPayerReferringProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330D_837P_NM1_OtherPayerRenderingProviderId",
                table: "Loop_2330D_837P",
                column: "NM1_OtherPayerRenderingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330E_837P_NM1_OtherPayerServiceFacilityLocationId",
                table: "Loop_2330E_837P",
                column: "NM1_OtherPayerServiceFacilityLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330F_837P_NM1_OtherPayerSupervisingProviderId",
                table: "Loop_2330F_837P",
                column: "NM1_OtherPayerSupervisingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2330G_837P_NM1_OtherPayerBillingProviderId",
                table: "Loop_2330G_837P",
                column: "NM1_OtherPayerBillingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllAMTId",
                table: "Loop_2400_837P",
                column: "AllAMTId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllCRCId",
                table: "Loop_2400_837P",
                column: "AllCRCId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllDTPId",
                table: "Loop_2400_837P",
                column: "AllDTPId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllNM1Id",
                table: "Loop_2400_837P",
                column: "AllNM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllNTEId",
                table: "Loop_2400_837P",
                column: "AllNTEId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllPWKId",
                table: "Loop_2400_837P",
                column: "AllPWKId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllQTYId",
                table: "Loop_2400_837P",
                column: "AllQTYId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_AllREFId",
                table: "Loop_2400_837P",
                column: "AllREFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_CN1_ContractInformationId",
                table: "Loop_2400_837P",
                column: "CN1_ContractInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_CR1_AmbulanceTransportInformationId",
                table: "Loop_2400_837P",
                column: "CR1_AmbulanceTransportInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_CR3_DurableMedicalEquipmentCertificationId",
                table: "Loop_2400_837P",
                column: "CR3_DurableMedicalEquipmentCertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_HCP_LinePricing_RepricingInformationId",
                table: "Loop_2400_837P",
                column: "HCP_LinePricing_RepricingInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_Loop_2300_837PId",
                table: "Loop_2400_837P",
                column: "Loop_2300_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_Loop2410Id",
                table: "Loop_2400_837P",
                column: "Loop2410Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_LX_ServiceLineNumberId",
                table: "Loop_2400_837P",
                column: "LX_ServiceLineNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_PS1_PurchasedServiceInformationId",
                table: "Loop_2400_837P",
                column: "PS1_PurchasedServiceInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_SV1_ProfessionalServiceId",
                table: "Loop_2400_837P",
                column: "SV1_ProfessionalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2400_837P_SV5_DurableMedicalEquipmentServiceId",
                table: "Loop_2400_837P",
                column: "SV5_DurableMedicalEquipmentServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2410_837P_CTP_DrugQuantityId",
                table: "Loop_2410_837P",
                column: "CTP_DrugQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2410_837P_LIN_DrugIdentificationId",
                table: "Loop_2410_837P",
                column: "LIN_DrugIdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2410_837P_REF_PrescriptionorCompoundDrugAssociationNumberId",
                table: "Loop_2410_837P",
                column: "REF_PrescriptionorCompoundDrugAssociationNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420A_837P_NM1_RenderingProviderNameId",
                table: "Loop_2420A_837P",
                column: "NM1_RenderingProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420A_837P_PRV_RenderingProviderSpecialtyInformationId",
                table: "Loop_2420A_837P",
                column: "PRV_RenderingProviderSpecialtyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420B_837P_NM1_PurchasedServiceProviderNameId",
                table: "Loop_2420B_837P",
                column: "NM1_PurchasedServiceProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420C_837P_N3_ServiceFacilityLocationAddressId",
                table: "Loop_2420C_837P",
                column: "N3_ServiceFacilityLocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420C_837P_N4_ServiceFacilityLocationCity_State_ZIPCodeId",
                table: "Loop_2420C_837P",
                column: "N4_ServiceFacilityLocationCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420C_837P_NM1_ServiceFacilityLocationId",
                table: "Loop_2420C_837P",
                column: "NM1_ServiceFacilityLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420D_837P_NM1_SupervisingProviderNameId",
                table: "Loop_2420D_837P",
                column: "NM1_SupervisingProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420E_837P_N3_OrderingProviderAddressId",
                table: "Loop_2420E_837P",
                column: "N3_OrderingProviderAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420E_837P_N4_OrderingProviderCity_State_ZIPCodeId",
                table: "Loop_2420E_837P",
                column: "N4_OrderingProviderCity_State_ZIPCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420E_837P_NM1_OrderingProviderNameId",
                table: "Loop_2420E_837P",
                column: "NM1_OrderingProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420E_837P_PER_OrderingProviderContactInformationId",
                table: "Loop_2420E_837P",
                column: "PER_OrderingProviderContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420F_837P_All_NM1_837P_5Id",
                table: "Loop_2420F_837P",
                column: "All_NM1_837P_5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420F_837P_NM1_ReferringProviderNameId",
                table: "Loop_2420F_837P",
                column: "NM1_ReferringProviderNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420G_837P_N3_AmbulancePick_LocationAddressId",
                table: "Loop_2420G_837P",
                column: "N3_AmbulancePick_LocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420G_837P_N4_AmbulancePick_LocationCity_State_ZipCodeId",
                table: "Loop_2420G_837P",
                column: "N4_AmbulancePick_LocationCity_State_ZipCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420G_837P_NM1_AmbulancePick_LocationId",
                table: "Loop_2420G_837P",
                column: "NM1_AmbulancePick_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420H_837P_N3_AmbulanceDrop_LocationAddressId",
                table: "Loop_2420H_837P",
                column: "N3_AmbulanceDrop_LocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420H_837P_N4_AmbulanceDrop_LocationCity_State_ZipCodeId",
                table: "Loop_2420H_837P",
                column: "N4_AmbulanceDrop_LocationCity_State_ZipCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2420H_837P_NM1_AmbulanceDrop_LocationId",
                table: "Loop_2420H_837P",
                column: "NM1_AmbulanceDrop_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2430_837P_AMT_RemainingPatientLiabilityId",
                table: "Loop_2430_837P",
                column: "AMT_RemainingPatientLiabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2430_837P_DTP_LineCheckorRemittanceDateId",
                table: "Loop_2430_837P",
                column: "DTP_LineCheckorRemittanceDateId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2430_837P_Loop_2400_837PId",
                table: "Loop_2430_837P",
                column: "Loop_2400_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2430_837P_SVD_LineAdjudicationInformationId",
                table: "Loop_2430_837P",
                column: "SVD_LineAdjudicationInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2440_837P_Loop_2400_837PId",
                table: "Loop_2440_837P",
                column: "Loop_2400_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_2440_837P_LQ_FormIdentificationCodeId",
                table: "Loop_2440_837P",
                column: "LQ_FormIdentificationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_MEA_TestResult_CompositeUnitOfMeasure_04Id",
                table: "MEA_TestResult",
                column: "CompositeUnitOfMeasure_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_MEA_TestResult_Loop_2400_837PId",
                table: "MEA_TestResult",
                column: "Loop_2400_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_PER_BillingProviderContactInformation_Loop_1000A_837PId",
                table: "PER_BillingProviderContactInformation",
                column: "Loop_1000A_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_PER_BillingProviderContactInformation_Loop_2010AA_837PId",
                table: "PER_BillingProviderContactInformation",
                column: "Loop_2010AA_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_PRV_BillingProviderSpecialtyInformation_ProviderSpecialtyInformation_05Id",
                table: "PRV_BillingProviderSpecialtyInformation",
                column: "ProviderSpecialtyInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_PRV_RenderingProviderSpecialtyInformation_ProviderSpecialtyInformation_05Id",
                table: "PRV_RenderingProviderSpecialtyInformation",
                column: "ProviderSpecialtyInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_ClaimSupplementalInformation_2_ActionsIndicated_08Id",
                table: "PWK_ClaimSupplementalInformation_2",
                column: "ActionsIndicated_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_ClaimSupplementalInformation_2_All_PWK_837PId",
                table: "PWK_ClaimSupplementalInformation_2",
                column: "All_PWK_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_ClaimSupplementalInformation_3_ActionsIndicated_08Id",
                table: "PWK_ClaimSupplementalInformation_3",
                column: "ActionsIndicated_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_ClaimSupplementalInformation_3_Loop_2300_837PId",
                table: "PWK_ClaimSupplementalInformation_3",
                column: "Loop_2300_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator_ActionsIndicated_08Id",
                table: "PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator",
                column: "ActionsIndicated_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_QTY_AmbulancePatientCount_CompositeUnitOfMeasure_03Id",
                table: "QTY_AmbulancePatientCount",
                column: "CompositeUnitOfMeasure_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_QTY_ObstetricAnesthesiaAdditionalUnits_CompositeUnitOfMeasure_03Id",
                table: "QTY_ObstetricAnesthesiaAdditionalUnits",
                column: "CompositeUnitOfMeasure_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AdjustedRepricedClaimNumber_ReferenceIdentifier_04Id",
                table: "REF_AdjustedRepricedClaimNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AdjustedRepricedLineItemReferenceNumber_ReferenceIdentifier_04Id",
                table: "REF_AdjustedRepricedLineItemReferenceNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_Loop_2310B_837PId",
                table: "REF_AssistantSurgeonSecondaryIdentification",
                column: "Loop_2310B_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_Loop_2310D_837PId",
                table: "REF_AssistantSurgeonSecondaryIdentification",
                column: "Loop_2310D_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_Loop_2330D_837PId",
                table: "REF_AssistantSurgeonSecondaryIdentification",
                column: "Loop_2330D_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_Loop_2330F_837PId",
                table: "REF_AssistantSurgeonSecondaryIdentification",
                column: "Loop_2330F_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_ReferenceIdentifier_04Id",
                table: "REF_AssistantSurgeonSecondaryIdentification",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_2_Loop_2420A_837PId",
                table: "REF_AssistantSurgeonSecondaryIdentification_2",
                column: "Loop_2420A_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_2_Loop_2420D_837PId",
                table: "REF_AssistantSurgeonSecondaryIdentification_2",
                column: "Loop_2420D_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_AssistantSurgeonSecondaryIdentification_2_ReferenceIdentifier_04Id",
                table: "REF_AssistantSurgeonSecondaryIdentification_2",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderSecondaryIdentification_All_REF_837P_5Id",
                table: "REF_BillingProviderSecondaryIdentification",
                column: "All_REF_837P_5Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderSecondaryIdentification_Loop_2330G_837PId",
                table: "REF_BillingProviderSecondaryIdentification",
                column: "Loop_2330G_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderSecondaryIdentification_ReferenceIdentifier_04Id",
                table: "REF_BillingProviderSecondaryIdentification",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderSecondaryIdentification_2_Loop_2420C_837PId",
                table: "REF_BillingProviderSecondaryIdentification_2",
                column: "Loop_2420C_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderSecondaryIdentification_2_ReferenceIdentifier_04Id",
                table: "REF_BillingProviderSecondaryIdentification_2",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderTaxIdentification_ReferenceIdentifier_04Id",
                table: "REF_BillingProviderTaxIdentification",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderTaxIdentification_2_ReferenceIdentifier_04Id",
                table: "REF_BillingProviderTaxIdentification_2",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderUPIN_All_REF_837P_8Id",
                table: "REF_BillingProviderUPIN",
                column: "All_REF_837P_8Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_BillingProviderUPIN_ReferenceIdentifier_04Id",
                table: "REF_BillingProviderUPIN",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_CarePlanOversight_ReferenceIdentifier_04Id",
                table: "REF_CarePlanOversight",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries_ReferenceIdentifier_04Id",
                table: "REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_ClinicalLaboratoryImprovementAmendment_ReferenceIdentifier_04Id",
                table: "REF_ClinicalLaboratoryImprovementAmendment",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_DemonstrationProjectIdentifier_ReferenceIdentifier_04Id",
                table: "REF_DemonstrationProjectIdentifier",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_ImmunizationBatchNumber_ReferenceIdentifier_04Id",
                table: "REF_ImmunizationBatchNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_InvestigationalDeviceExemptionNumber_ReferenceIdentifier_04Id",
                table: "REF_InvestigationalDeviceExemptionNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_LineItemControlNumber_ReferenceIdentifier_04Id",
                table: "REF_LineItemControlNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_MammographyCertificationNumber_ReferenceIdentifier_04Id",
                table: "REF_MammographyCertificationNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_MandatoryMedicare_ReferenceIdentifier_04Id",
                table: "REF_MandatoryMedicare",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_MedicalRecordNumber_ReferenceIdentifier_04Id",
                table: "REF_MedicalRecordNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_Loop_2420B_837PId",
                table: "REF_OrderingProviderSecondaryIdentification",
                column: "Loop_2420B_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_Loop_2420E_837PId",
                table: "REF_OrderingProviderSecondaryIdentification",
                column: "Loop_2420E_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_Loop_2420F_837PId",
                table: "REF_OrderingProviderSecondaryIdentification",
                column: "Loop_2420F_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_ReferenceIdentifier_04Id",
                table: "REF_OrderingProviderSecondaryIdentification",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_2_Loop_2310A_837PId",
                table: "REF_OrderingProviderSecondaryIdentification_2",
                column: "Loop_2310A_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_2_Loop_2330C_837PId",
                table: "REF_OrderingProviderSecondaryIdentification_2",
                column: "Loop_2330C_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OrderingProviderSecondaryIdentification_2_ReferenceIdentifier_04Id",
                table: "REF_OrderingProviderSecondaryIdentification_2",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerClaimAdjustmentIndicator_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerClaimAdjustmentIndicator",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerClaimControlNumber_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerClaimControlNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerPriorAuthorizationNumber_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerPriorAuthorizationNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerPriorAuthorizationNumber_2_All_REF_837P_2Id",
                table: "REF_OtherPayerPriorAuthorizationNumber_2",
                column: "All_REF_837P_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerPriorAuthorizationNumber_2_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerPriorAuthorizationNumber_2",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerReferralNumber_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerReferralNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerReferralNumber_2_All_REF_837P_2Id",
                table: "REF_OtherPayerReferralNumber_2",
                column: "All_REF_837P_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerReferralNumber_2_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerReferralNumber_2",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerSecondaryIdentifier_All_REF_837P_5Id",
                table: "REF_OtherPayerSecondaryIdentifier",
                column: "All_REF_837P_5Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerSecondaryIdentifier_All_REF_837PId",
                table: "REF_OtherPayerSecondaryIdentifier",
                column: "All_REF_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerSecondaryIdentifier_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerSecondaryIdentifier",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerServiceFacilityLocationSecondaryIdentification_Loop_2310C_837PId",
                table: "REF_OtherPayerServiceFacilityLocationSecondaryIdentification",
                column: "Loop_2310C_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerServiceFacilityLocationSecondaryIdentification_Loop_2330E_837PId",
                table: "REF_OtherPayerServiceFacilityLocationSecondaryIdentification",
                column: "Loop_2330E_837PId");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherPayerServiceFacilityLocationSecondaryIdentification_ReferenceIdentifier_04Id",
                table: "REF_OtherPayerServiceFacilityLocationSecondaryIdentification",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_OtherSubscriberSecondaryIdentification_ReferenceIdentifier_04Id",
                table: "REF_OtherSubscriberSecondaryIdentification",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Pay_ReferenceIdentifier_04Id",
                table: "REF_Pay",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_PrescriptionorCompoundDrugAssociationNumber_ReferenceIdentifier_04Id",
                table: "REF_PrescriptionorCompoundDrugAssociationNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_PropertyandCasualtyClaimNumber_ReferenceIdentifier_04Id",
                table: "REF_PropertyandCasualtyClaimNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_PropertyandCasualtyPatientIdentifier_ReferenceIdentifier_04Id",
                table: "REF_PropertyandCasualtyPatientIdentifier",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_ReferringClinicalLaboratoryImprovementAmendment_ReferenceIdentifier_04Id",
                table: "REF_ReferringClinicalLaboratoryImprovementAmendment",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_RepricedClaimNumber_ReferenceIdentifier_04Id",
                table: "REF_RepricedClaimNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_RepricedLineItemReferenceNumber_ReferenceIdentifier_04Id",
                table: "REF_RepricedLineItemReferenceNumber",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_ServiceAuthorizationExceptionCode_ReferenceIdentifier_04Id",
                table: "REF_ServiceAuthorizationExceptionCode",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV1_ProfessionalService_CompositeDiagnosisCodePointer_07Id",
                table: "SV1_ProfessionalService",
                column: "CompositeDiagnosisCodePointer_07Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV1_ProfessionalService_CompositeMedicalProcedureIdentifier_01Id",
                table: "SV1_ProfessionalService",
                column: "CompositeMedicalProcedureIdentifier_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV5_DurableMedicalEquipmentService_CompositeMedicalProcedureIdentifier_01Id",
                table: "SV5_DurableMedicalEquipmentService",
                column: "CompositeMedicalProcedureIdentifier_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_SVD_LineAdjudicationInformation_3_CompositeMedicalProcedureIdentifier_03Id",
                table: "SVD_LineAdjudicationInformation_3",
                column: "CompositeMedicalProcedureIdentifier_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_TS837P_AllNM1Id",
                table: "TS837P",
                column: "AllNM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TS837P_BHT_BeginningOfHierarchicalTransactionId",
                table: "TS837P",
                column: "BHT_BeginningOfHierarchicalTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TS837P_SEId",
                table: "TS837P",
                column: "SEId");

            migrationBuilder.CreateIndex(
                name: "IX_TS837P_STId",
                table: "TS837P",
                column: "STId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "C056_CompositeRaceOrEthnicityInformation");

            migrationBuilder.DropTable(
                name: "CAS_ClaimLevelAdjustments");

            migrationBuilder.DropTable(
                name: "CRC_AmbulanceCertification");

            migrationBuilder.DropTable(
                name: "CRC_PatientConditionInformation");

            migrationBuilder.DropTable(
                name: "DTP_AssumedandRelinquishedCareDates");

            migrationBuilder.DropTable(
                name: "DTP_TestDate");

            migrationBuilder.DropTable(
                name: "FRM_SupportingDocumentation");

            migrationBuilder.DropTable(
                name: "HI_ConditionInformation");

            migrationBuilder.DropTable(
                name: "K3_FileInformation");

            migrationBuilder.DropTable(
                name: "MEA_TestResult");

            migrationBuilder.DropTable(
                name: "PWK_ClaimSupplementalInformation_2");

            migrationBuilder.DropTable(
                name: "PWK_ClaimSupplementalInformation_3");

            migrationBuilder.DropTable(
                name: "REF_AssistantSurgeonSecondaryIdentification");

            migrationBuilder.DropTable(
                name: "REF_AssistantSurgeonSecondaryIdentification_2");

            migrationBuilder.DropTable(
                name: "REF_BillingProviderSecondaryIdentification");

            migrationBuilder.DropTable(
                name: "REF_BillingProviderSecondaryIdentification_2");

            migrationBuilder.DropTable(
                name: "REF_BillingProviderUPIN");

            migrationBuilder.DropTable(
                name: "REF_OrderingProviderSecondaryIdentification");

            migrationBuilder.DropTable(
                name: "REF_OrderingProviderSecondaryIdentification_2");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerPriorAuthorizationNumber_2");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerReferralNumber_2");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerSecondaryIdentifier");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerServiceFacilityLocationSecondaryIdentification");

            migrationBuilder.DropTable(
                name: "Loop_2320_837P");

            migrationBuilder.DropTable(
                name: "Loop_2430_837P");

            migrationBuilder.DropTable(
                name: "Loop_2440_837P");

            migrationBuilder.DropTable(
                name: "C022_HealthCareCodeInformation_13");

            migrationBuilder.DropTable(
                name: "Loop_2420F_837P");

            migrationBuilder.DropTable(
                name: "Loop_2310A_837P");

            migrationBuilder.DropTable(
                name: "Loop_2330C_837P");

            migrationBuilder.DropTable(
                name: "C040_ReferenceIdentifier_3");

            migrationBuilder.DropTable(
                name: "All_AMT_837P_2");

            migrationBuilder.DropTable(
                name: "MOA_OutpatientAdjudicationInformation");

            migrationBuilder.DropTable(
                name: "OI_OtherInsuranceCoverageInformation_2");

            migrationBuilder.DropTable(
                name: "SBR_OtherSubscriberInformation");

            migrationBuilder.DropTable(
                name: "SVD_LineAdjudicationInformation_3");

            migrationBuilder.DropTable(
                name: "LQ_FormIdentificationCode");

            migrationBuilder.DropTable(
                name: "Loop_2400_837P");

            migrationBuilder.DropTable(
                name: "NM1_ReferringProviderName");

            migrationBuilder.DropTable(
                name: "All_NM1_837P_4");

            migrationBuilder.DropTable(
                name: "NM1_OtherPayerReferringProvider");

            migrationBuilder.DropTable(
                name: "AMT_CoordinationofBenefits_2");

            migrationBuilder.DropTable(
                name: "AMT_CoordinationofBenefits");

            migrationBuilder.DropTable(
                name: "AMT_RemainingPatientLiability");

            migrationBuilder.DropTable(
                name: "All_AMT_837P");

            migrationBuilder.DropTable(
                name: "All_CRC_837P");

            migrationBuilder.DropTable(
                name: "All_DTP_837P");

            migrationBuilder.DropTable(
                name: "All_NM1_837P_5");

            migrationBuilder.DropTable(
                name: "All_NTE_837P");

            migrationBuilder.DropTable(
                name: "All_PWK_837P");

            migrationBuilder.DropTable(
                name: "All_QTY_837P");

            migrationBuilder.DropTable(
                name: "All_REF_837P_2");

            migrationBuilder.DropTable(
                name: "CR3_DurableMedicalEquipmentCertification");

            migrationBuilder.DropTable(
                name: "HCP_LinePricing_3");

            migrationBuilder.DropTable(
                name: "LX_HeaderNumber");

            migrationBuilder.DropTable(
                name: "Loop_2300_837P");

            migrationBuilder.DropTable(
                name: "Loop_2410_837P");

            migrationBuilder.DropTable(
                name: "PS1_PurchasedServiceInformation");

            migrationBuilder.DropTable(
                name: "SV1_ProfessionalService");

            migrationBuilder.DropTable(
                name: "SV5_DurableMedicalEquipmentService");

            migrationBuilder.DropTable(
                name: "Loop_2330A_837P");

            migrationBuilder.DropTable(
                name: "Loop_2330B_837P");

            migrationBuilder.DropTable(
                name: "Loop_2330D_837P");

            migrationBuilder.DropTable(
                name: "Loop_2330E_837P");

            migrationBuilder.DropTable(
                name: "Loop_2330F_837P");

            migrationBuilder.DropTable(
                name: "Loop_2330G_837P");

            migrationBuilder.DropTable(
                name: "AMT_PostageClaimedAmount");

            migrationBuilder.DropTable(
                name: "AMT_SalesTaxAmount");

            migrationBuilder.DropTable(
                name: "CRC_ConditionIndicator");

            migrationBuilder.DropTable(
                name: "CRC_HospiceEmployeeIndicator");

            migrationBuilder.DropTable(
                name: "DTP_BeginTherapyDate");

            migrationBuilder.DropTable(
                name: "DTP_CertificationRevision");

            migrationBuilder.DropTable(
                name: "DTP_ClaimLevelServiceDate");

            migrationBuilder.DropTable(
                name: "DTP_LastCertificationDate");

            migrationBuilder.DropTable(
                name: "DTP_ShippedDate");

            migrationBuilder.DropTable(
                name: "Loop_2420A_837P");

            migrationBuilder.DropTable(
                name: "Loop_2420B_837P");

            migrationBuilder.DropTable(
                name: "Loop_2420C_837P");

            migrationBuilder.DropTable(
                name: "Loop_2420D_837P");

            migrationBuilder.DropTable(
                name: "Loop_2420E_837P");

            migrationBuilder.DropTable(
                name: "Loop_2420G_837P");

            migrationBuilder.DropTable(
                name: "Loop_2420H_837P");

            migrationBuilder.DropTable(
                name: "NTE_LineNote");

            migrationBuilder.DropTable(
                name: "NTE_ThirdPartyOrganizationNotes");

            migrationBuilder.DropTable(
                name: "PWK_DurableMedicalEquipmentCertificateofMedicalNecessityIndicator");

            migrationBuilder.DropTable(
                name: "QTY_AmbulancePatientCount");

            migrationBuilder.DropTable(
                name: "QTY_ObstetricAnesthesiaAdditionalUnits");

            migrationBuilder.DropTable(
                name: "REF_AdjustedRepricedLineItemReferenceNumber");

            migrationBuilder.DropTable(
                name: "REF_ImmunizationBatchNumber");

            migrationBuilder.DropTable(
                name: "REF_LineItemControlNumber");

            migrationBuilder.DropTable(
                name: "REF_ReferringClinicalLaboratoryImprovementAmendment");

            migrationBuilder.DropTable(
                name: "REF_RepricedLineItemReferenceNumber");

            migrationBuilder.DropTable(
                name: "AMT_PatientAmountPaid");

            migrationBuilder.DropTable(
                name: "All_CRC_837P_2");

            migrationBuilder.DropTable(
                name: "All_DTP_837P_2");

            migrationBuilder.DropTable(
                name: "All_HI_837P");

            migrationBuilder.DropTable(
                name: "All_NM1_837P_3");

            migrationBuilder.DropTable(
                name: "All_REF_837P_6");

            migrationBuilder.DropTable(
                name: "CLM_ClaimInformation_3");

            migrationBuilder.DropTable(
                name: "CN1_ContractInformation_2");

            migrationBuilder.DropTable(
                name: "CR1_AmbulanceTransportInformation");

            migrationBuilder.DropTable(
                name: "CR2_SpinalManipulationServiceInformation");

            migrationBuilder.DropTable(
                name: "HCP_ClaimPricing");

            migrationBuilder.DropTable(
                name: "Loop_2000C_837P");

            migrationBuilder.DropTable(
                name: "NTE_ClaimNote_2");

            migrationBuilder.DropTable(
                name: "CTP_DrugQuantity");

            migrationBuilder.DropTable(
                name: "LIN_DrugIdentification_2");

            migrationBuilder.DropTable(
                name: "REF_PrescriptionorCompoundDrugAssociationNumber");

            migrationBuilder.DropTable(
                name: "C003_CompositeMedicalProcedureIdentifier_12");

            migrationBuilder.DropTable(
                name: "C004_CompositeDiagnosisCodePointer");

            migrationBuilder.DropTable(
                name: "C003_CompositeMedicalProcedureIdentifier_9");

            migrationBuilder.DropTable(
                name: "NM1_OtherSubscriberName");

            migrationBuilder.DropTable(
                name: "All_REF_837P");

            migrationBuilder.DropTable(
                name: "DTP_ClaimCheckOrRemittanceDate");

            migrationBuilder.DropTable(
                name: "NM1_OtherPayerRenderingProvider_2");

            migrationBuilder.DropTable(
                name: "NM1_OtherPayerServiceFacilityLocation");

            migrationBuilder.DropTable(
                name: "NM1_OtherPayerSupervisingProvider");

            migrationBuilder.DropTable(
                name: "NM1_OtherPayerBillingProvider");

            migrationBuilder.DropTable(
                name: "NM1_PurchasedServiceProviderName");

            migrationBuilder.DropTable(
                name: "NM1_OrderingProviderName");

            migrationBuilder.DropTable(
                name: "PER_BillingProviderContactInformation");

            migrationBuilder.DropTable(
                name: "C002_ActionsIndicated_2");

            migrationBuilder.DropTable(
                name: "C001_CompositeUnitOfMeasure");

            migrationBuilder.DropTable(
                name: "CRC_EPSDTReferral");

            migrationBuilder.DropTable(
                name: "CRC_HomeboundIndicator");

            migrationBuilder.DropTable(
                name: "DTP_AccidentDate_2");

            migrationBuilder.DropTable(
                name: "DTP_AcuteManifestation");

            migrationBuilder.DropTable(
                name: "DTP_AdmissionDate_4");

            migrationBuilder.DropTable(
                name: "DTP_AuthorizedReturntoWork");

            migrationBuilder.DropTable(
                name: "DTP_DisabilityDates");

            migrationBuilder.DropTable(
                name: "DTP_Discharge");

            migrationBuilder.DropTable(
                name: "DTP_HearingandVisionPrescriptionDate");

            migrationBuilder.DropTable(
                name: "DTP_InitialTreatmentDate");

            migrationBuilder.DropTable(
                name: "DTP_LastMenstrualPeriod");

            migrationBuilder.DropTable(
                name: "DTP_LastSeenDate");

            migrationBuilder.DropTable(
                name: "DTP_LastWorked");

            migrationBuilder.DropTable(
                name: "DTP_LastXrayDate");

            migrationBuilder.DropTable(
                name: "DTP_OnsetofCurrentIllnessorSymptom");

            migrationBuilder.DropTable(
                name: "DTP_PropertyandCasualtyDateofFirstContact");

            migrationBuilder.DropTable(
                name: "DTP_RepricerReceivedDate");

            migrationBuilder.DropTable(
                name: "HI_AnesthesiaRelatedProcedure");

            migrationBuilder.DropTable(
                name: "HI_DependentHealthCareDiagnosisCode_2");

            migrationBuilder.DropTable(
                name: "Loop_2310B_837P");

            migrationBuilder.DropTable(
                name: "Loop_2310C_837P");

            migrationBuilder.DropTable(
                name: "Loop_2310D_837P");

            migrationBuilder.DropTable(
                name: "Loop_2310E_837P");

            migrationBuilder.DropTable(
                name: "Loop_2310F_837P");

            migrationBuilder.DropTable(
                name: "REF_AdjustedRepricedClaimNumber");

            migrationBuilder.DropTable(
                name: "REF_CarePlanOversight");

            migrationBuilder.DropTable(
                name: "REF_ClaimIdentificationNumberForClearinghousesAndOtherTransmissionIntermediaries");

            migrationBuilder.DropTable(
                name: "REF_ClinicalLaboratoryImprovementAmendment");

            migrationBuilder.DropTable(
                name: "REF_DemonstrationProjectIdentifier");

            migrationBuilder.DropTable(
                name: "REF_InvestigationalDeviceExemptionNumber");

            migrationBuilder.DropTable(
                name: "REF_MammographyCertificationNumber");

            migrationBuilder.DropTable(
                name: "REF_MandatoryMedicare");

            migrationBuilder.DropTable(
                name: "REF_MedicalRecordNumber");

            migrationBuilder.DropTable(
                name: "REF_RepricedClaimNumber");

            migrationBuilder.DropTable(
                name: "REF_ServiceAuthorizationExceptionCode");

            migrationBuilder.DropTable(
                name: "C023_HealthCareServiceLocationInformation_2");

            migrationBuilder.DropTable(
                name: "C024_RelatedCausesInformation_3");

            migrationBuilder.DropTable(
                name: "HL_DependentLevel");

            migrationBuilder.DropTable(
                name: "Loop_2000B_837P");

            migrationBuilder.DropTable(
                name: "Loop_2010CA_837P");

            migrationBuilder.DropTable(
                name: "PAT_PatientInformation");

            migrationBuilder.DropTable(
                name: "C001_CompositeUnitOfMeasure_2");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerClaimAdjustmentIndicator");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerClaimControlNumber");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerPriorAuthorizationNumber");

            migrationBuilder.DropTable(
                name: "REF_OtherPayerReferralNumber");

            migrationBuilder.DropTable(
                name: "C022_HealthCareCodeInformation_12");

            migrationBuilder.DropTable(
                name: "C022_HealthCareCodeInformation_15");

            migrationBuilder.DropTable(
                name: "C022_HealthCareCodeInformation");

            migrationBuilder.DropTable(
                name: "C022_HealthCareCodeInformation_4");

            migrationBuilder.DropTable(
                name: "C022_HealthCareCodeInformation_8");

            migrationBuilder.DropTable(
                name: "NM1_RenderingProviderName");

            migrationBuilder.DropTable(
                name: "PRV_RenderingProviderSpecialtyInformation");

            migrationBuilder.DropTable(
                name: "NM1_ServiceFacilityLocation");

            migrationBuilder.DropTable(
                name: "NM1_SupervisingProviderName");

            migrationBuilder.DropTable(
                name: "NM1_AmbulancePick");

            migrationBuilder.DropTable(
                name: "NM1_AmbulanceDrop");

            migrationBuilder.DropTable(
                name: "All_NM1_837P_2");

            migrationBuilder.DropTable(
                name: "HL_SubscriberHierarchicalLevel");

            migrationBuilder.DropTable(
                name: "Loop_2000A_837P");

            migrationBuilder.DropTable(
                name: "PAT_PatientInformation_3");

            migrationBuilder.DropTable(
                name: "SBR_SubscriberInformation");

            migrationBuilder.DropTable(
                name: "All_REF_837P_7");

            migrationBuilder.DropTable(
                name: "NM1_PatientName_3");

            migrationBuilder.DropTable(
                name: "Loop_2010BA_837P");

            migrationBuilder.DropTable(
                name: "Loop_2010BB_837P");

            migrationBuilder.DropTable(
                name: "All_NM1_837P");

            migrationBuilder.DropTable(
                name: "CUR_ForeignCurrencyInformation_3");

            migrationBuilder.DropTable(
                name: "HL_BillingProviderHierarchicalLevel");

            migrationBuilder.DropTable(
                name: "PRV_BillingProviderSpecialtyInformation");

            migrationBuilder.DropTable(
                name: "TS837P");

            migrationBuilder.DropTable(
                name: "REF_PropertyandCasualtyPatientIdentifier");

            migrationBuilder.DropTable(
                name: "All_REF_837P_4");

            migrationBuilder.DropTable(
                name: "DMG_PatientDemographicInformation");

            migrationBuilder.DropTable(
                name: "NM1_SubscriberName_5");

            migrationBuilder.DropTable(
                name: "PER_PropertyandCasualtyPatientContactInformation");

            migrationBuilder.DropTable(
                name: "All_REF_837P_5");

            migrationBuilder.DropTable(
                name: "NM1_OtherPayerName");

            migrationBuilder.DropTable(
                name: "Loop_2010AA_837P");

            migrationBuilder.DropTable(
                name: "Loop_2010AB_837P");

            migrationBuilder.DropTable(
                name: "Loop_2010AC_837P");

            migrationBuilder.DropTable(
                name: "C035_ProviderSpecialtyInformation");

            migrationBuilder.DropTable(
                name: "All_NM1_837P_6");

            migrationBuilder.DropTable(
                name: "BHT_BeginningOfHierarchicalTransaction_8");

            migrationBuilder.DropTable(
                name: "SE");

            migrationBuilder.DropTable(
                name: "ST");

            migrationBuilder.DropTable(
                name: "C040_ReferenceIdentifier_7");

            migrationBuilder.DropTable(
                name: "REF_OtherSubscriberSecondaryIdentification");

            migrationBuilder.DropTable(
                name: "REF_PropertyandCasualtyClaimNumber");

            migrationBuilder.DropTable(
                name: "All_REF_837P_8");

            migrationBuilder.DropTable(
                name: "NM1_BillingProviderName_2");

            migrationBuilder.DropTable(
                name: "NM1_Pay");

            migrationBuilder.DropTable(
                name: "All_REF_837P_3");

            migrationBuilder.DropTable(
                name: "N3_AdditionalPatientInformationContactAddress");

            migrationBuilder.DropTable(
                name: "N4_AdditionalPatientInformationContactCity");

            migrationBuilder.DropTable(
                name: "NM1_Pay_3");

            migrationBuilder.DropTable(
                name: "Loop_1000A_837P");

            migrationBuilder.DropTable(
                name: "Loop_1000B_837P");

            migrationBuilder.DropTable(
                name: "REF_BillingProviderTaxIdentification");

            migrationBuilder.DropTable(
                name: "REF_BillingProviderTaxIdentification_2");

            migrationBuilder.DropTable(
                name: "REF_Pay");

            migrationBuilder.DropTable(
                name: "NM1_InformationReceiverName_4");

            migrationBuilder.DropTable(
                name: "NM1_ReceiverName");

            migrationBuilder.DropTable(
                name: "C040_ReferenceIdentifier");
        }
    }
}
