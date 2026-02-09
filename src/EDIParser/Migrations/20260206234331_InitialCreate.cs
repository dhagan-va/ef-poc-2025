using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDIParser.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BHT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HierarchicalStructureCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionSetPurposeCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Time_05 = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionTypeCode_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C001",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitorBasisforMeasurementCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Exponent_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier_03 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Exponent_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier_06 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    Exponent_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier_09 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    Exponent_11 = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier_12 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_13 = table.Column<string>(type: "TEXT", nullable: true),
                    Exponent_14 = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier_15 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C001", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C002",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PaperworkReportActionCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    PaperworkReportActionCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    PaperworkReportActionCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    PaperworkReportActionCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    PaperworkReportActionCode_05 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C002", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C003",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductServiceIDQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ProcedureModifier_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ProcedureModifier_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ProcedureModifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ProcedureModifier_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_07 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C003", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C004",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiagnosisCodePointer_01 = table.Column<string>(type: "TEXT", nullable: true),
                    DiagnosisCodePointer_02 = table.Column<string>(type: "TEXT", nullable: true),
                    DiagnosisCodePointer_03 = table.Column<string>(type: "TEXT", nullable: true),
                    DiagnosisCodePointer_04 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C004", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C005",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToothSurfaceCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ToothSurfaceCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ToothSurfaceCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ToothSurfaceCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ToothSurfaceCode_05 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C005", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C006",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OralCavityDesignationCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    OralCavityDesignationCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    OralCavityDesignationCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    OralCavityDesignationCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    OralCavityDesignationCode_05 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C006", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C022",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeListQualifierCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    VersionIdentifier_07 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C022", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C023",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FacilityCodeValue_01 = table.Column<string>(type: "TEXT", nullable: true),
                    FacilityCodeQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimFrequencyTypeCode_03 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C023", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C024",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RelatedCausesCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    RelatedCausesCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    RelatedCausesCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    StateorProvinceCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode_05 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C024", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C035",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderSpecialtyCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    AgencyQualifierCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_03 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C035", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C040",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentificationQualifier_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentificationQualifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C040", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CL1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdmissionTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    AdmissionSourceCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    PatientStatusCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    NursingHomeResidentialStatusCode_04 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CL1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CN1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Percent_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "TEXT", nullable: true),
                    TermsDiscountPercent_05 = table.Column<string>(type: "TEXT", nullable: true),
                    VersionIdentifier_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CN1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitorBasisforMeasurementCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Weight_02 = table.Column<string>(type: "TEXT", nullable: true),
                    AmbulanceTransportCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    AmbulanceTransportReasonCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressInformation_07 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressInformation_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_09 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_10 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CertificationTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_03 = table.Column<string>(type: "TEXT", nullable: true),
                    InsulinDependentCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_05 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR5",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CertificationTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenEquipmentTypeCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenEquipmentTypeCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_07 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_09 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_10 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_11 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenTestConditionCode_12 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenTestFindingsCode_13 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenTestFindingsCode_14 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenTestFindingsCode_15 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_16 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenDeliverySystemCode_17 = table.Column<string>(type: "TEXT", nullable: true),
                    OxygenEquipmentTypeCode_18 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR6",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PrognosisCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_02 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_03 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_05 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    CertificationTypeCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_09 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_10 = table.Column<string>(type: "TEXT", nullable: true),
                    MedicalCodeValue_11 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_12 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_13 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_14 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_15 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_16 = table.Column<string>(type: "TEXT", nullable: true),
                    PatientLocationCode_17 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_18 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_19 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_20 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_21 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR6", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR7",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisciplineTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Number_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Number_03 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR7", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR8",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImplantTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ImplantStatusCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_07 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CUR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityIdentifierCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencyCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeRate_03 = table.Column<string>(type: "TEXT", nullable: true),
                    EntityIdentifierCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencyCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencyMarketExchangeCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimeQualifier_07 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Time_09 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimeQualifier_10 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_11 = table.Column<string>(type: "TEXT", nullable: true),
                    Time_12 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimeQualifier_13 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_14 = table.Column<string>(type: "TEXT", nullable: true),
                    Time_15 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimeQualifier_16 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_17 = table.Column<string>(type: "TEXT", nullable: true),
                    Time_18 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimeQualifier_19 = table.Column<string>(type: "TEXT", nullable: true),
                    Date_20 = table.Column<string>(type: "TEXT", nullable: true),
                    Time_21 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DMG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTimePeriodFormatQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_02 = table.Column<string>(type: "TEXT", nullable: true),
                    GenderCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    MaritalStatusCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    RaceorEthnicityCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    CitizenshipStatusCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    BasisofVerificationCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_09 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DMG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DN1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_04 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DN1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DSB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisabilityTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    OccupationCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    WorkIntensityCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductOptionCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_07 = table.Column<string>(type: "TEXT", nullable: true),
                    MedicalCodeValue_08 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DSB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HCP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PricingMethodology_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Rate_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_07 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_08 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_09 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_10 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_11 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_12 = table.Column<string>(type: "TEXT", nullable: true),
                    RejectReasonCode_13 = table.Column<string>(type: "TEXT", nullable: true),
                    PolicyComplianceCode_14 = table.Column<string>(type: "TEXT", nullable: true),
                    ExceptionCode_15 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HCP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HierarchicalIDNumber_01 = table.Column<string>(type: "TEXT", nullable: true),
                    HierarchicalParentIDNumber_02 = table.Column<string>(type: "TEXT", nullable: true),
                    HierarchicalLevelCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    HierarchicalChildCode_04 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LIN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignedIdentification_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_07 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_08 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_09 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_10 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_11 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_12 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_13 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_14 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_15 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_16 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_17 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_18 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_19 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_20 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_21 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_22 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_23 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_24 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_25 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_26 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_27 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_28 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_29 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceIDQualifier_30 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductServiceID_31 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LX",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignedNumber_01 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LX", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_03 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_05 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_06 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_07 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_09 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_10 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_11 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_12 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_13 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_14 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_15 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_16 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_17 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_18 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_19 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_20 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_21 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_22 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_23 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_24 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MIA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MOA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Percent_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_07 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_09 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "N4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityName_01 = table.Column<string>(type: "TEXT", nullable: true),
                    StateorProvinceCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    LocationQualifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    LocationIdentifier_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NM1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityIdentifierCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    EntityTypeQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    NameLastorOrganizationName_03 = table.Column<string>(type: "TEXT", nullable: true),
                    NameFirst_04 = table.Column<string>(type: "TEXT", nullable: true),
                    NameMiddle_05 = table.Column<string>(type: "TEXT", nullable: true),
                    NamePrefix_06 = table.Column<string>(type: "TEXT", nullable: true),
                    NameSuffix_07 = table.Column<string>(type: "TEXT", nullable: true),
                    IdentificationCodeQualifier_08 = table.Column<string>(type: "TEXT", nullable: true),
                    IdentificationCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    EntityRelationshipCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    EntityIdentifierCode_11 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NM1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimFilingIndicatorCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimSubmissionReasonCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    PatientSignatureSourceCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderAgreementCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ReleaseofInformationCode_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PAT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndividualRelationshipCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    PatientLocationCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    EmploymentStatusCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    StudentStatusCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_06 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    Weight_08 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PS1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceIdentification_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    StateorProvinceCode_03 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PS1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SBR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PayerResponsibilitySequenceNumberCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    IndividualRelationshipCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Name_04 = table.Column<string>(type: "TEXT", nullable: true),
                    InsuranceTypeCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    CoordinationofBenefitsCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    EmploymentStatusCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimFilingIndicatorCode_09 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberofIncludedSegments_01 = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionSetControlNumber_02 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionSetIdentifierCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionSetControlNumber_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ImplementationConventionPreference_03 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SV7",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceIdentification_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_02 = table.Column<string>(type: "TEXT", nullable: true),
                    PrescriptionDenialOverrideCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    CoverageLevelCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ProductProcessCharacteristicCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV7", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApprovalCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CTP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassofTradeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    PriceIdentifierCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitPrice_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_04 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeUnitofMeasure_05Id = table.Column<int>(type: "INTEGER", nullable: true),
                    PriceMultiplierQualifier_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Multiplier_07 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "TEXT", nullable: true),
                    BasisofUnitPriceCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionValue_10 = table.Column<string>(type: "TEXT", nullable: true),
                    MultiplePriceQuantity_11 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CTP_C001_CompositeUnitofMeasure_05Id",
                        column: x => x.CompositeUnitofMeasure_05Id,
                        principalTable: "C001",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductServiceID_01 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeMedicalProcedureIdentifier_02Id = table.Column<int>(type: "INTEGER", nullable: true),
                    MonetaryAmount_03 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_05 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitRate_06 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_07 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    NursingHomeResidentialStatusCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    LevelofCareCode_10 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV2_C003_CompositeMedicalProcedureIdentifier_02Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_02Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceIdentification_01 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeMedicalProcedureIdentifier_02Id = table.Column<int>(type: "INTEGER", nullable: true),
                    ReferenceIdentification_03 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    DispenseasWrittenCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    LevelofServiceCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    PrescriptionOriginCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_08 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitDoseCode_11 = table.Column<string>(type: "TEXT", nullable: true),
                    BasisofCostDeterminationCode_12 = table.Column<string>(type: "TEXT", nullable: true),
                    BasisofDaysSupplyDeterminationCode_13 = table.Column<string>(type: "TEXT", nullable: true),
                    DosageFormCode_14 = table.Column<string>(type: "TEXT", nullable: true),
                    CopayStatusCode_15 = table.Column<string>(type: "TEXT", nullable: true),
                    PatientLocationCode_16 = table.Column<string>(type: "TEXT", nullable: true),
                    LevelofCareCode_17 = table.Column<string>(type: "TEXT", nullable: true),
                    PriorAuthorizationTypeCode_18 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV4_C003_CompositeMedicalProcedureIdentifier_02Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_02Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV5",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompositeMedicalProcedureIdentifier_01Id = table.Column<int>(type: "INTEGER", nullable: true),
                    UnitorBasisforMeasurementCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_03 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_04 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_05 = table.Column<string>(type: "TEXT", nullable: true),
                    FrequencyCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    PrognosisCode_07 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV5", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV5_C003_CompositeMedicalProcedureIdentifier_01Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_01Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SVD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdentificationCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeMedicalProcedureIdentifier_03Id = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductServiceID_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_05 = table.Column<string>(type: "TEXT", nullable: true),
                    AssignedNumber_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SVD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SVD_C003_CompositeMedicalProcedureIdentifier_03Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_03Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompositeMedicalProcedureIdentifier_01Id = table.Column<int>(type: "INTEGER", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_04 = table.Column<string>(type: "TEXT", nullable: true),
                    FacilityCodeValue_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceTypeCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeDiagnosisCodePointer_07Id = table.Column<int>(type: "INTEGER", nullable: true),
                    MonetaryAmount_08 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    MultipleProcedureCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_11 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_12 = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewCode_13 = table.Column<string>(type: "TEXT", nullable: true),
                    NationalorLocalAssignedReviewValue_14 = table.Column<string>(type: "TEXT", nullable: true),
                    CopayStatusCode_15 = table.Column<string>(type: "TEXT", nullable: true),
                    HealthCareProfessionalShortageAreaCode_16 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_17 = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode_18 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_19 = table.Column<string>(type: "TEXT", nullable: true),
                    LevelofCareCode_20 = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderAgreementCode_21 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV1_C003_CompositeMedicalProcedureIdentifier_01Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_01Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SV1_C004_CompositeDiagnosisCodePointer_07Id",
                        column: x => x.CompositeDiagnosisCodePointer_07Id,
                        principalTable: "C004",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV6",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompositeMedicalProcedureIdentifier_01Id = table.Column<int>(type: "INTEGER", nullable: true),
                    FacilityCodeQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    FacilityCodeValue_03 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_04 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeDiagnosisCodePointer_05Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_07 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV6", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV6_C003_CompositeMedicalProcedureIdentifier_01Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_01Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SV6_C004_CompositeDiagnosisCodePointer_05Id",
                        column: x => x.CompositeDiagnosisCodePointer_05Id,
                        principalTable: "C004",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SV3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompositeMedicalProcedureIdentifier_01Id = table.Column<int>(type: "INTEGER", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    FacilityCodeValue_03 = table.Column<string>(type: "TEXT", nullable: true),
                    OralCavityDesignation_04Id = table.Column<int>(type: "INTEGER", nullable: true),
                    ProsthesisCrownorInlayCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_07 = table.Column<string>(type: "TEXT", nullable: true),
                    CopayStatusCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderAgreementCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeDiagnosisCodePointer_11Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SV3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SV3_C003_CompositeMedicalProcedureIdentifier_01Id",
                        column: x => x.CompositeMedicalProcedureIdentifier_01Id,
                        principalTable: "C003",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SV3_C004_CompositeDiagnosisCodePointer_11Id",
                        column: x => x.CompositeDiagnosisCodePointer_11Id,
                        principalTable: "C004",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SV3_C006_OralCavityDesignation_04Id",
                        column: x => x.OralCavityDesignation_04Id,
                        principalTable: "C006",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CLM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimSubmittersIdentifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimFilingIndicatorCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    NonInstitutionalClaimTypeCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    HealthCareServiceLocationInformation_05Id = table.Column<int>(type: "INTEGER", nullable: true),
                    YesNoConditionorResponseCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderAcceptAssignmentCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    ReleaseofInformationCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    PatientSignatureSourceCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    RelatedCausesInformation_11Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SpecialProgramCode_12 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_13 = table.Column<string>(type: "TEXT", nullable: true),
                    LevelofServiceCode_14 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_15 = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderAgreementCode_16 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimStatusCode_17 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_18 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimSubmissionReasonCode_19 = table.Column<string>(type: "TEXT", nullable: true),
                    DelayReasonCode_20 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CLM_C023_HealthCareServiceLocationInformation_05Id",
                        column: x => x.HealthCareServiceLocationInformation_05Id,
                        principalTable: "C023",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CLM_C024_RelatedCausesInformation_11Id",
                        column: x => x.RelatedCausesInformation_11Id,
                        principalTable: "C024",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PRV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentificationQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_03 = table.Column<string>(type: "TEXT", nullable: true),
                    StateorProvinceCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderSpecialtyInformation_05Id = table.Column<int>(type: "INTEGER", nullable: true),
                    ProviderOrganizationCode_06 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRV_C035_ProviderSpecialtyInformation_05Id",
                        column: x => x.ProviderSpecialtyInformation_05Id,
                        principalTable: "C035",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TS837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    STId = table.Column<int>(type: "INTEGER", nullable: true),
                    BHTId = table.Column<int>(type: "INTEGER", nullable: true),
                    SEId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TS837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TS837_BHT_BHTId",
                        column: x => x.BHTId,
                        principalTable: "BHT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TS837_SE_SEId",
                        column: x => x.SEId,
                        principalTable: "SE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TS837_ST_STId",
                        column: x => x.STId,
                        principalTable: "ST",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_HL_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HLId = table.Column<int>(type: "INTEGER", nullable: true),
                    PRVId = table.Column<int>(type: "INTEGER", nullable: true),
                    SBRId = table.Column<int>(type: "INTEGER", nullable: true),
                    PATId = table.Column<int>(type: "INTEGER", nullable: true),
                    CURId = table.Column<int>(type: "INTEGER", nullable: true),
                    TS837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_HL_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_HL_837_CUR_CURId",
                        column: x => x.CURId,
                        principalTable: "CUR",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_HL_837_HL_HLId",
                        column: x => x.HLId,
                        principalTable: "HL",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_HL_837_PAT_PATId",
                        column: x => x.PATId,
                        principalTable: "PAT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_HL_837_PRV_PRVId",
                        column: x => x.PRVId,
                        principalTable: "PRV",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_HL_837_SBR_SBRId",
                        column: x => x.SBRId,
                        principalTable: "SBR",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_HL_837_TS837_TS837Id",
                        column: x => x.TS837Id,
                        principalTable: "TS837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_NM1_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NM1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    N4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    TS837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_NM1_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_N4_N4Id",
                        column: x => x.N4Id,
                        principalTable: "N4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_NM1_NM1Id",
                        column: x => x.NM1Id,
                        principalTable: "NM1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_TS837_TS837Id",
                        column: x => x.TS837Id,
                        principalTable: "TS837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_NM1_837_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NM1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    N4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    DMGId = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_HL_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_NM1_837_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_2_DMG_DMGId",
                        column: x => x.DMGId,
                        principalTable: "DMG",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_2_Loop_HL_837_Loop_HL_837Id",
                        column: x => x.Loop_HL_837Id,
                        principalTable: "Loop_HL_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_2_N4_N4Id",
                        column: x => x.N4Id,
                        principalTable: "N4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_2_NM1_NM1Id",
                        column: x => x.NM1Id,
                        principalTable: "NM1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AMT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmountQualifierCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_02 = table.Column<string>(type: "TEXT", nullable: true),
                    CreditDebitFlagCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_SBR_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimAdjustmentGroupCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimAdjustmentReasonCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimAdjustmentReasonCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_07 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimAdjustmentReasonCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_09 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_10 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimAdjustmentReasonCode_11 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_12 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_13 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimAdjustmentReasonCode_14 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_15 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_16 = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimAdjustmentReasonCode_17 = table.Column<string>(type: "TEXT", nullable: true),
                    MonetaryAmount_18 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_19 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_SBR_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_SVD_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CR2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    SubluxationLevelCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    SubluxationLevelCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_07 = table.Column<string>(type: "TEXT", nullable: true),
                    NatureofConditionCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_10 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_11 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_12 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loop_CLM_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CLMId = table.Column<int>(type: "INTEGER", nullable: true),
                    CL1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    DN1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CN1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    DSBId = table.Column<int>(type: "INTEGER", nullable: true),
                    URId = table.Column<int>(type: "INTEGER", nullable: true),
                    CR1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR5Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR6Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR8Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HCPId = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_HL_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_CLM_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CL1_CL1Id",
                        column: x => x.CL1Id,
                        principalTable: "CL1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CLM_CLMId",
                        column: x => x.CLMId,
                        principalTable: "CLM",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CN1_CN1Id",
                        column: x => x.CN1Id,
                        principalTable: "CN1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CR1_CR1Id",
                        column: x => x.CR1Id,
                        principalTable: "CR1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CR2_CR2Id",
                        column: x => x.CR2Id,
                        principalTable: "CR2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CR3_CR3Id",
                        column: x => x.CR3Id,
                        principalTable: "CR3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CR5_CR5Id",
                        column: x => x.CR5Id,
                        principalTable: "CR5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CR6_CR6Id",
                        column: x => x.CR6Id,
                        principalTable: "CR6",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_CR8_CR8Id",
                        column: x => x.CR8Id,
                        principalTable: "CR8",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_DN1_DN1Id",
                        column: x => x.DN1Id,
                        principalTable: "DN1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_DSB_DSBId",
                        column: x => x.DSBId,
                        principalTable: "DSB",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_HCP_HCPId",
                        column: x => x.HCPId,
                        principalTable: "HCP",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_Loop_HL_837_Loop_HL_837Id",
                        column: x => x.Loop_HL_837Id,
                        principalTable: "Loop_HL_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CLM_837_UR_URId",
                        column: x => x.URId,
                        principalTable: "UR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DN2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceIdentification_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ToothStatusCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_03 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_04 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DN2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DN2_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_CR7_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CR7Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_CR7_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_CR7_837_CR7_CR7Id",
                        column: x => x.CR7Id,
                        principalTable: "CR7",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_CR7_837_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_SBR_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SBRId = table.Column<int>(type: "INTEGER", nullable: true),
                    DMGId = table.Column<int>(type: "INTEGER", nullable: true),
                    OIId = table.Column<int>(type: "INTEGER", nullable: true),
                    MIAId = table.Column<int>(type: "INTEGER", nullable: true),
                    MOAId = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_SBR_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_SBR_837_DMG_DMGId",
                        column: x => x.DMGId,
                        principalTable: "DMG",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_SBR_837_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_SBR_837_MIA_MIAId",
                        column: x => x.MIAId,
                        principalTable: "MIA",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_SBR_837_MOA_MOAId",
                        column: x => x.MOAId,
                        principalTable: "MOA",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_SBR_837_OI_OIId",
                        column: x => x.OIId,
                        principalTable: "OI",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_SBR_837_SBR_SBRId",
                        column: x => x.SBRId,
                        principalTable: "SBR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HSD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuantityQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    SampleSelectionModulus_04 = table.Column<string>(type: "TEXT", nullable: true),
                    TimePeriodQualifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    NumberofPeriods_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ShipDeliveryorCalendarPatternCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    ShipDeliveryPatternTimeCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CR7_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HSD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HSD_Loop_CR7_837_Loop_CR7_837Id",
                        column: x => x.Loop_CR7_837Id,
                        principalTable: "Loop_CR7_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_NM1_837_4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NM1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    N4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_SBR_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_NM1_837_4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_4_Loop_SBR_837_Loop_SBR_837Id",
                        column: x => x.Loop_SBR_837Id,
                        principalTable: "Loop_SBR_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_4_N4_N4Id",
                        column: x => x.N4Id,
                        principalTable: "N4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_4_NM1_NM1Id",
                        column: x => x.NM1Id,
                        principalTable: "NM1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_LX_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LXId = table.Column<int>(type: "INTEGER", nullable: true),
                    SV1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SV2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SV3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SV4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SV5Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SV6Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SV7Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CR5Id = table.Column<int>(type: "INTEGER", nullable: true),
                    CN1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    PS1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HSDId = table.Column<int>(type: "INTEGER", nullable: true),
                    HCPId = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_LX_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_CN1_CN1Id",
                        column: x => x.CN1Id,
                        principalTable: "CN1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_CR1_CR1Id",
                        column: x => x.CR1Id,
                        principalTable: "CR1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_CR3_CR3Id",
                        column: x => x.CR3Id,
                        principalTable: "CR3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_CR5_CR5Id",
                        column: x => x.CR5Id,
                        principalTable: "CR5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_HCP_HCPId",
                        column: x => x.HCPId,
                        principalTable: "HCP",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_HSD_HSDId",
                        column: x => x.HSDId,
                        principalTable: "HSD",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_LX_LXId",
                        column: x => x.LXId,
                        principalTable: "LX",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_PS1_PS1Id",
                        column: x => x.PS1Id,
                        principalTable: "PS1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV1_SV1Id",
                        column: x => x.SV1Id,
                        principalTable: "SV1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV2_SV2Id",
                        column: x => x.SV2Id,
                        principalTable: "SV2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV3_SV3Id",
                        column: x => x.SV3Id,
                        principalTable: "SV3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV4_SV4Id",
                        column: x => x.SV4Id,
                        principalTable: "SV4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV5_SV5Id",
                        column: x => x.SV5Id,
                        principalTable: "SV5",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV6_SV6Id",
                        column: x => x.SV6Id,
                        principalTable: "SV6",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LX_837_SV7_SV7Id",
                        column: x => x.SV7Id,
                        principalTable: "SV7",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CR4",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    YesNoConditionorResponseCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    CertificationTypeCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_04 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_06 = table.Column<string>(type: "TEXT", nullable: true),
                    NonVisitCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_09 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_10 = table.Column<string>(type: "TEXT", nullable: true),
                    Height_11 = table.Column<string>(type: "TEXT", nullable: true),
                    UnitorBasisforMeasurementCode_12 = table.Column<string>(type: "TEXT", nullable: true),
                    Weight_13 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_14 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_15 = table.Column<string>(type: "TEXT", nullable: true),
                    NutrientAdministrationMethodCode_16 = table.Column<string>(type: "TEXT", nullable: true),
                    NutrientAdministrationTechniqueCode_17 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_18 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_19 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_20 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_21 = table.Column<string>(type: "TEXT", nullable: true),
                    Percent_22 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_23 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_24 = table.Column<string>(type: "TEXT", nullable: true),
                    Percent_25 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_26 = table.Column<string>(type: "TEXT", nullable: true),
                    Percent_27 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_28 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_29 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CR4_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CR4_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CRC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeCategory_01 = table.Column<string>(type: "TEXT", nullable: true),
                    YesNoConditionorResponseCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionIndicator_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionIndicator_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionIndicator_05 = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionIndicator_06 = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionIndicator_07 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CRC_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CRC_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HealthCareCodeInformation_01Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_02Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_03Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_04Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_05Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_06Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_07Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_08Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_09Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_10Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_11Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HealthCareCodeInformation_12Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_01Id",
                        column: x => x.HealthCareCodeInformation_01Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_02Id",
                        column: x => x.HealthCareCodeInformation_02Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_03Id",
                        column: x => x.HealthCareCodeInformation_03Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_04Id",
                        column: x => x.HealthCareCodeInformation_04Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_05Id",
                        column: x => x.HealthCareCodeInformation_05Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_06Id",
                        column: x => x.HealthCareCodeInformation_06Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_07Id",
                        column: x => x.HealthCareCodeInformation_07Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_08Id",
                        column: x => x.HealthCareCodeInformation_08Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_09Id",
                        column: x => x.HealthCareCodeInformation_09Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_10Id",
                        column: x => x.HealthCareCodeInformation_10Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_11Id",
                        column: x => x.HealthCareCodeInformation_11Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_C022_HealthCareCodeInformation_12Id",
                        column: x => x.HealthCareCodeInformation_12Id,
                        principalTable: "C022",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HI_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IMM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImmunizationTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ImmunizationStatusCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    ReportTypeCode_05 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMM_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "K3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FixedFormatInformation_01 = table.Column<string>(type: "TEXT", nullable: true),
                    RecordFormatCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeUnitofMeasure_03Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_K3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_K3_C001_CompositeUnitofMeasure_03Id",
                        column: x => x.CompositeUnitofMeasure_03Id,
                        principalTable: "C001",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_K3_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_K3_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_NM1_837_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NM1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    PRVId = table.Column<int>(type: "INTEGER", nullable: true),
                    N4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_NM1_837_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_3_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_3_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_3_N4_N4Id",
                        column: x => x.N4Id,
                        principalTable: "N4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_3_NM1_NM1Id",
                        column: x => x.NM1Id,
                        principalTable: "NM1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_NM1_837_3_PRV_PRVId",
                        column: x => x.PRVId,
                        principalTable: "PRV",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_SVD_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SVDId = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_SVD_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_SVD_837_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_SVD_837_SVD_SVDId",
                        column: x => x.SVDId,
                        principalTable: "SVD",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MEA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MeasurementReferenceIDCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    MeasurementQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    MeasurementValue_03 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeUnitofMeasure_04Id = table.Column<int>(type: "INTEGER", nullable: true),
                    RangeMinimum_05 = table.Column<string>(type: "TEXT", nullable: true),
                    RangeMaximum_06 = table.Column<string>(type: "TEXT", nullable: true),
                    MeasurementSignificanceCode_07 = table.Column<string>(type: "TEXT", nullable: true),
                    MeasurementAttributeCode_08 = table.Column<string>(type: "TEXT", nullable: true),
                    SurfaceLayerPositionCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    MeasurementMethodorDevice_10 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MEA_C001_CompositeUnitofMeasure_04Id",
                        column: x => x.CompositeUnitofMeasure_04Id,
                        principalTable: "C001",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MEA_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteReferenceCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NTE_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NTE_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PWK",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReportTypeCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ReportTransmissionCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ReportCopiesNeeded_03 = table.Column<string>(type: "TEXT", nullable: true),
                    EntityIdentifierCode_04 = table.Column<string>(type: "TEXT", nullable: true),
                    IdentificationCodeQualifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    IdentificationCode_06 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_07 = table.Column<string>(type: "TEXT", nullable: true),
                    ActionsIndicated_08Id = table.Column<int>(type: "INTEGER", nullable: true),
                    RequestCategoryCode_09 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PWK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PWK_C002_ActionsIndicated_08Id",
                        column: x => x.ActionsIndicated_08Id,
                        principalTable: "C002",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PWK_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PWK_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QTY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuantityQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity_02 = table.Column<string>(type: "TEXT", nullable: true),
                    CompositeUnitofMeasure_03Id = table.Column<int>(type: "INTEGER", nullable: true),
                    FreeFormMessage_04 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QTY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QTY_C001_CompositeUnitofMeasure_03Id",
                        column: x => x.CompositeUnitofMeasure_03Id,
                        principalTable: "C001",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QTY_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QTY_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TOO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeListQualifierCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    IndustryCode_02 = table.Column<string>(type: "TEXT", nullable: true),
                    ToothSurface_03Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TOO_C005_ToothSurface_03Id",
                        column: x => x.ToothSurface_03Id,
                        principalTable: "C005",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TOO_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "N2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Name_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_NM1_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_4Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_N2_Loop_NM1_837_2_Loop_NM1_837_2Id",
                        column: x => x.Loop_NM1_837_2Id,
                        principalTable: "Loop_NM1_837_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_N2_Loop_NM1_837_3_Loop_NM1_837_3Id",
                        column: x => x.Loop_NM1_837_3Id,
                        principalTable: "Loop_NM1_837_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_N2_Loop_NM1_837_4_Loop_NM1_837_4Id",
                        column: x => x.Loop_NM1_837_4Id,
                        principalTable: "Loop_NM1_837_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_N2_Loop_NM1_837_Loop_NM1_837Id",
                        column: x => x.Loop_NM1_837Id,
                        principalTable: "Loop_NM1_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "N3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressInformation_01 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressInformation_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_NM1_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_4Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_N3_Loop_NM1_837_2_Loop_NM1_837_2Id",
                        column: x => x.Loop_NM1_837_2Id,
                        principalTable: "Loop_NM1_837_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_N3_Loop_NM1_837_3_Loop_NM1_837_3Id",
                        column: x => x.Loop_NM1_837_3Id,
                        principalTable: "Loop_NM1_837_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_N3_Loop_NM1_837_4_Loop_NM1_837_4Id",
                        column: x => x.Loop_NM1_837_4Id,
                        principalTable: "Loop_NM1_837_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_N3_Loop_NM1_837_Loop_NM1_837Id",
                        column: x => x.Loop_NM1_837Id,
                        principalTable: "Loop_NM1_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContactFunctionCode_01 = table.Column<string>(type: "TEXT", nullable: true),
                    Name_02 = table.Column<string>(type: "TEXT", nullable: true),
                    CommunicationNumberQualifier_03 = table.Column<string>(type: "TEXT", nullable: true),
                    CommunicationNumber_04 = table.Column<string>(type: "TEXT", nullable: true),
                    CommunicationNumberQualifier_05 = table.Column<string>(type: "TEXT", nullable: true),
                    CommunicationNumber_06 = table.Column<string>(type: "TEXT", nullable: true),
                    CommunicationNumberQualifier_07 = table.Column<string>(type: "TEXT", nullable: true),
                    CommunicationNumber_08 = table.Column<string>(type: "TEXT", nullable: true),
                    ContactInquiryReference_09 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_NM1_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_4Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PER_Loop_NM1_837_2_Loop_NM1_837_2Id",
                        column: x => x.Loop_NM1_837_2Id,
                        principalTable: "Loop_NM1_837_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PER_Loop_NM1_837_3_Loop_NM1_837_3Id",
                        column: x => x.Loop_NM1_837_3Id,
                        principalTable: "Loop_NM1_837_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PER_Loop_NM1_837_4_Loop_NM1_837_4Id",
                        column: x => x.Loop_NM1_837_4Id,
                        principalTable: "Loop_NM1_837_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PER_Loop_NM1_837_Loop_NM1_837Id",
                        column: x => x.Loop_NM1_837Id,
                        principalTable: "Loop_NM1_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "REF",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceIdentificationQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentification_02 = table.Column<string>(type: "TEXT", nullable: true),
                    Description_03 = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceIdentifier_04Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_3Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    TS837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REF", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REF_C040_ReferenceIdentifier_04Id",
                        column: x => x.ReferenceIdentifier_04Id,
                        principalTable: "C040",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_Loop_NM1_837_2_Loop_NM1_837_2Id",
                        column: x => x.Loop_NM1_837_2Id,
                        principalTable: "Loop_NM1_837_2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_Loop_NM1_837_3_Loop_NM1_837_3Id",
                        column: x => x.Loop_NM1_837_3Id,
                        principalTable: "Loop_NM1_837_3",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_Loop_NM1_837_4_Loop_NM1_837_4Id",
                        column: x => x.Loop_NM1_837_4Id,
                        principalTable: "Loop_NM1_837_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_Loop_NM1_837_Loop_NM1_837Id",
                        column: x => x.Loop_NM1_837Id,
                        principalTable: "Loop_NM1_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_REF_TS837_TS837Id",
                        column: x => x.TS837Id,
                        principalTable: "TS837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DTP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTimeQualifier_01 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriodFormatQualifier_02 = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimePeriod_03 = table.Column<string>(type: "TEXT", nullable: true),
                    Loop_CLM_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_HL_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_NM1_837_4Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_SVD_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTP_Loop_CLM_837_Loop_CLM_837Id",
                        column: x => x.Loop_CLM_837Id,
                        principalTable: "Loop_CLM_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DTP_Loop_HL_837_Loop_HL_837Id",
                        column: x => x.Loop_HL_837Id,
                        principalTable: "Loop_HL_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DTP_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DTP_Loop_NM1_837_4_Loop_NM1_837_4Id",
                        column: x => x.Loop_NM1_837_4Id,
                        principalTable: "Loop_NM1_837_4",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DTP_Loop_SVD_837_Loop_SVD_837Id",
                        column: x => x.Loop_SVD_837Id,
                        principalTable: "Loop_SVD_837",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loop_LIN_837",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LINId = table.Column<int>(type: "INTEGER", nullable: true),
                    CTPId = table.Column<int>(type: "INTEGER", nullable: true),
                    REFId = table.Column<int>(type: "INTEGER", nullable: true),
                    Loop_LX_837Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loop_LIN_837", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loop_LIN_837_CTP_CTPId",
                        column: x => x.CTPId,
                        principalTable: "CTP",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LIN_837_LIN_LINId",
                        column: x => x.LINId,
                        principalTable: "LIN",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LIN_837_Loop_LX_837_Loop_LX_837Id",
                        column: x => x.Loop_LX_837Id,
                        principalTable: "Loop_LX_837",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Loop_LIN_837_REF_REFId",
                        column: x => x.REFId,
                        principalTable: "REF",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AMT_Loop_CLM_837Id",
                table: "AMT",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_AMT_Loop_LX_837Id",
                table: "AMT",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_AMT_Loop_SBR_837Id",
                table: "AMT",
                column: "Loop_SBR_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CAS_Loop_SBR_837Id",
                table: "CAS",
                column: "Loop_SBR_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CAS_Loop_SVD_837Id",
                table: "CAS",
                column: "Loop_SVD_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CLM_HealthCareServiceLocationInformation_05Id",
                table: "CLM",
                column: "HealthCareServiceLocationInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_CLM_RelatedCausesInformation_11Id",
                table: "CLM",
                column: "RelatedCausesInformation_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_CR2_Loop_LX_837Id",
                table: "CR2",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CR4_Loop_CLM_837Id",
                table: "CR4",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CR4_Loop_LX_837Id",
                table: "CR4",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CRC_Loop_CLM_837Id",
                table: "CRC",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CRC_Loop_LX_837Id",
                table: "CRC",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_CTP_CompositeUnitofMeasure_05Id",
                table: "CTP",
                column: "CompositeUnitofMeasure_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_DN2_Loop_CLM_837Id",
                table: "DN2",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_Loop_CLM_837Id",
                table: "DTP",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_Loop_HL_837Id",
                table: "DTP",
                column: "Loop_HL_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_Loop_LX_837Id",
                table: "DTP",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_Loop_NM1_837_4Id",
                table: "DTP",
                column: "Loop_NM1_837_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_DTP_Loop_SVD_837Id",
                table: "DTP",
                column: "Loop_SVD_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_01Id",
                table: "HI",
                column: "HealthCareCodeInformation_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_02Id",
                table: "HI",
                column: "HealthCareCodeInformation_02Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_03Id",
                table: "HI",
                column: "HealthCareCodeInformation_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_04Id",
                table: "HI",
                column: "HealthCareCodeInformation_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_05Id",
                table: "HI",
                column: "HealthCareCodeInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_06Id",
                table: "HI",
                column: "HealthCareCodeInformation_06Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_07Id",
                table: "HI",
                column: "HealthCareCodeInformation_07Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_08Id",
                table: "HI",
                column: "HealthCareCodeInformation_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_09Id",
                table: "HI",
                column: "HealthCareCodeInformation_09Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_10Id",
                table: "HI",
                column: "HealthCareCodeInformation_10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_11Id",
                table: "HI",
                column: "HealthCareCodeInformation_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_HealthCareCodeInformation_12Id",
                table: "HI",
                column: "HealthCareCodeInformation_12Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_Loop_CLM_837Id",
                table: "HI",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_HI_Loop_LX_837Id",
                table: "HI",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_HSD_Loop_CR7_837Id",
                table: "HSD",
                column: "Loop_CR7_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_IMM_Loop_LX_837Id",
                table: "IMM",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_K3_CompositeUnitofMeasure_03Id",
                table: "K3",
                column: "CompositeUnitofMeasure_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_K3_Loop_CLM_837Id",
                table: "K3",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_K3_Loop_LX_837Id",
                table: "K3",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CL1Id",
                table: "Loop_CLM_837",
                column: "CL1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CLMId",
                table: "Loop_CLM_837",
                column: "CLMId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CN1Id",
                table: "Loop_CLM_837",
                column: "CN1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CR1Id",
                table: "Loop_CLM_837",
                column: "CR1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CR2Id",
                table: "Loop_CLM_837",
                column: "CR2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CR3Id",
                table: "Loop_CLM_837",
                column: "CR3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CR5Id",
                table: "Loop_CLM_837",
                column: "CR5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CR6Id",
                table: "Loop_CLM_837",
                column: "CR6Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_CR8Id",
                table: "Loop_CLM_837",
                column: "CR8Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_DN1Id",
                table: "Loop_CLM_837",
                column: "DN1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_DSBId",
                table: "Loop_CLM_837",
                column: "DSBId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_HCPId",
                table: "Loop_CLM_837",
                column: "HCPId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_Loop_HL_837Id",
                table: "Loop_CLM_837",
                column: "Loop_HL_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CLM_837_URId",
                table: "Loop_CLM_837",
                column: "URId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CR7_837_CR7Id",
                table: "Loop_CR7_837",
                column: "CR7Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_CR7_837_Loop_CLM_837Id",
                table: "Loop_CR7_837",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_HL_837_CURId",
                table: "Loop_HL_837",
                column: "CURId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_HL_837_HLId",
                table: "Loop_HL_837",
                column: "HLId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_HL_837_PATId",
                table: "Loop_HL_837",
                column: "PATId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_HL_837_PRVId",
                table: "Loop_HL_837",
                column: "PRVId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_HL_837_SBRId",
                table: "Loop_HL_837",
                column: "SBRId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_HL_837_TS837Id",
                table: "Loop_HL_837",
                column: "TS837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LIN_837_CTPId",
                table: "Loop_LIN_837",
                column: "CTPId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LIN_837_LINId",
                table: "Loop_LIN_837",
                column: "LINId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LIN_837_Loop_LX_837Id",
                table: "Loop_LIN_837",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LIN_837_REFId",
                table: "Loop_LIN_837",
                column: "REFId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_CN1Id",
                table: "Loop_LX_837",
                column: "CN1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_CR1Id",
                table: "Loop_LX_837",
                column: "CR1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_CR3Id",
                table: "Loop_LX_837",
                column: "CR3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_CR5Id",
                table: "Loop_LX_837",
                column: "CR5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_HCPId",
                table: "Loop_LX_837",
                column: "HCPId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_HSDId",
                table: "Loop_LX_837",
                column: "HSDId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_Loop_CLM_837Id",
                table: "Loop_LX_837",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_LXId",
                table: "Loop_LX_837",
                column: "LXId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_PS1Id",
                table: "Loop_LX_837",
                column: "PS1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV1Id",
                table: "Loop_LX_837",
                column: "SV1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV2Id",
                table: "Loop_LX_837",
                column: "SV2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV3Id",
                table: "Loop_LX_837",
                column: "SV3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV4Id",
                table: "Loop_LX_837",
                column: "SV4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV5Id",
                table: "Loop_LX_837",
                column: "SV5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV6Id",
                table: "Loop_LX_837",
                column: "SV6Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_LX_837_SV7Id",
                table: "Loop_LX_837",
                column: "SV7Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_N4Id",
                table: "Loop_NM1_837",
                column: "N4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_NM1Id",
                table: "Loop_NM1_837",
                column: "NM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_TS837Id",
                table: "Loop_NM1_837",
                column: "TS837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_2_DMGId",
                table: "Loop_NM1_837_2",
                column: "DMGId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_2_Loop_HL_837Id",
                table: "Loop_NM1_837_2",
                column: "Loop_HL_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_2_N4Id",
                table: "Loop_NM1_837_2",
                column: "N4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_2_NM1Id",
                table: "Loop_NM1_837_2",
                column: "NM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_3_Loop_CLM_837Id",
                table: "Loop_NM1_837_3",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_3_Loop_LX_837Id",
                table: "Loop_NM1_837_3",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_3_N4Id",
                table: "Loop_NM1_837_3",
                column: "N4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_3_NM1Id",
                table: "Loop_NM1_837_3",
                column: "NM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_3_PRVId",
                table: "Loop_NM1_837_3",
                column: "PRVId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_4_Loop_SBR_837Id",
                table: "Loop_NM1_837_4",
                column: "Loop_SBR_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_4_N4Id",
                table: "Loop_NM1_837_4",
                column: "N4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_NM1_837_4_NM1Id",
                table: "Loop_NM1_837_4",
                column: "NM1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SBR_837_DMGId",
                table: "Loop_SBR_837",
                column: "DMGId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SBR_837_Loop_CLM_837Id",
                table: "Loop_SBR_837",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SBR_837_MIAId",
                table: "Loop_SBR_837",
                column: "MIAId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SBR_837_MOAId",
                table: "Loop_SBR_837",
                column: "MOAId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SBR_837_OIId",
                table: "Loop_SBR_837",
                column: "OIId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SBR_837_SBRId",
                table: "Loop_SBR_837",
                column: "SBRId");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SVD_837_Loop_LX_837Id",
                table: "Loop_SVD_837",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_Loop_SVD_837_SVDId",
                table: "Loop_SVD_837",
                column: "SVDId");

            migrationBuilder.CreateIndex(
                name: "IX_MEA_CompositeUnitofMeasure_04Id",
                table: "MEA",
                column: "CompositeUnitofMeasure_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_MEA_Loop_LX_837Id",
                table: "MEA",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_N2_Loop_NM1_837_2Id",
                table: "N2",
                column: "Loop_NM1_837_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_N2_Loop_NM1_837_3Id",
                table: "N2",
                column: "Loop_NM1_837_3Id");

            migrationBuilder.CreateIndex(
                name: "IX_N2_Loop_NM1_837_4Id",
                table: "N2",
                column: "Loop_NM1_837_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_N2_Loop_NM1_837Id",
                table: "N2",
                column: "Loop_NM1_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_N3_Loop_NM1_837_2Id",
                table: "N3",
                column: "Loop_NM1_837_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_N3_Loop_NM1_837_3Id",
                table: "N3",
                column: "Loop_NM1_837_3Id");

            migrationBuilder.CreateIndex(
                name: "IX_N3_Loop_NM1_837_4Id",
                table: "N3",
                column: "Loop_NM1_837_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_N3_Loop_NM1_837Id",
                table: "N3",
                column: "Loop_NM1_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_NTE_Loop_CLM_837Id",
                table: "NTE",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_NTE_Loop_LX_837Id",
                table: "NTE",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_PER_Loop_NM1_837_2Id",
                table: "PER",
                column: "Loop_NM1_837_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PER_Loop_NM1_837_3Id",
                table: "PER",
                column: "Loop_NM1_837_3Id");

            migrationBuilder.CreateIndex(
                name: "IX_PER_Loop_NM1_837_4Id",
                table: "PER",
                column: "Loop_NM1_837_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_PER_Loop_NM1_837Id",
                table: "PER",
                column: "Loop_NM1_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_PRV_ProviderSpecialtyInformation_05Id",
                table: "PRV",
                column: "ProviderSpecialtyInformation_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_ActionsIndicated_08Id",
                table: "PWK",
                column: "ActionsIndicated_08Id");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_Loop_CLM_837Id",
                table: "PWK",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_PWK_Loop_LX_837Id",
                table: "PWK",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_QTY_CompositeUnitofMeasure_03Id",
                table: "QTY",
                column: "CompositeUnitofMeasure_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_QTY_Loop_CLM_837Id",
                table: "QTY",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_QTY_Loop_LX_837Id",
                table: "QTY",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Loop_CLM_837Id",
                table: "REF",
                column: "Loop_CLM_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Loop_LX_837Id",
                table: "REF",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Loop_NM1_837_2Id",
                table: "REF",
                column: "Loop_NM1_837_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Loop_NM1_837_3Id",
                table: "REF",
                column: "Loop_NM1_837_3Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Loop_NM1_837_4Id",
                table: "REF",
                column: "Loop_NM1_837_4Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_Loop_NM1_837Id",
                table: "REF",
                column: "Loop_NM1_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_ReferenceIdentifier_04Id",
                table: "REF",
                column: "ReferenceIdentifier_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_REF_TS837Id",
                table: "REF",
                column: "TS837Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV1_CompositeDiagnosisCodePointer_07Id",
                table: "SV1",
                column: "CompositeDiagnosisCodePointer_07Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV1_CompositeMedicalProcedureIdentifier_01Id",
                table: "SV1",
                column: "CompositeMedicalProcedureIdentifier_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV2_CompositeMedicalProcedureIdentifier_02Id",
                table: "SV2",
                column: "CompositeMedicalProcedureIdentifier_02Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV3_CompositeDiagnosisCodePointer_11Id",
                table: "SV3",
                column: "CompositeDiagnosisCodePointer_11Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV3_CompositeMedicalProcedureIdentifier_01Id",
                table: "SV3",
                column: "CompositeMedicalProcedureIdentifier_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV3_OralCavityDesignation_04Id",
                table: "SV3",
                column: "OralCavityDesignation_04Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV4_CompositeMedicalProcedureIdentifier_02Id",
                table: "SV4",
                column: "CompositeMedicalProcedureIdentifier_02Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV5_CompositeMedicalProcedureIdentifier_01Id",
                table: "SV5",
                column: "CompositeMedicalProcedureIdentifier_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV6_CompositeDiagnosisCodePointer_05Id",
                table: "SV6",
                column: "CompositeDiagnosisCodePointer_05Id");

            migrationBuilder.CreateIndex(
                name: "IX_SV6_CompositeMedicalProcedureIdentifier_01Id",
                table: "SV6",
                column: "CompositeMedicalProcedureIdentifier_01Id");

            migrationBuilder.CreateIndex(
                name: "IX_SVD_CompositeMedicalProcedureIdentifier_03Id",
                table: "SVD",
                column: "CompositeMedicalProcedureIdentifier_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_TOO_Loop_LX_837Id",
                table: "TOO",
                column: "Loop_LX_837Id");

            migrationBuilder.CreateIndex(
                name: "IX_TOO_ToothSurface_03Id",
                table: "TOO",
                column: "ToothSurface_03Id");

            migrationBuilder.CreateIndex(
                name: "IX_TS837_BHTId",
                table: "TS837",
                column: "BHTId");

            migrationBuilder.CreateIndex(
                name: "IX_TS837_SEId",
                table: "TS837",
                column: "SEId");

            migrationBuilder.CreateIndex(
                name: "IX_TS837_STId",
                table: "TS837",
                column: "STId");

            migrationBuilder.AddForeignKey(
                name: "FK_AMT_Loop_CLM_837_Loop_CLM_837Id",
                table: "AMT",
                column: "Loop_CLM_837Id",
                principalTable: "Loop_CLM_837",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AMT_Loop_LX_837_Loop_LX_837Id",
                table: "AMT",
                column: "Loop_LX_837Id",
                principalTable: "Loop_LX_837",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AMT_Loop_SBR_837_Loop_SBR_837Id",
                table: "AMT",
                column: "Loop_SBR_837Id",
                principalTable: "Loop_SBR_837",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CAS_Loop_SBR_837_Loop_SBR_837Id",
                table: "CAS",
                column: "Loop_SBR_837Id",
                principalTable: "Loop_SBR_837",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CAS_Loop_SVD_837_Loop_SVD_837Id",
                table: "CAS",
                column: "Loop_SVD_837Id",
                principalTable: "Loop_SVD_837",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CR2_Loop_LX_837_Loop_LX_837Id",
                table: "CR2",
                column: "Loop_LX_837Id",
                principalTable: "Loop_LX_837",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loop_CR7_837_Loop_CLM_837_Loop_CLM_837Id",
                table: "Loop_CR7_837");

            migrationBuilder.DropForeignKey(
                name: "FK_Loop_LX_837_Loop_CLM_837_Loop_CLM_837Id",
                table: "Loop_LX_837");

            migrationBuilder.DropTable(
                name: "AMT");

            migrationBuilder.DropTable(
                name: "CAS");

            migrationBuilder.DropTable(
                name: "CR4");

            migrationBuilder.DropTable(
                name: "CRC");

            migrationBuilder.DropTable(
                name: "DN2");

            migrationBuilder.DropTable(
                name: "DTP");

            migrationBuilder.DropTable(
                name: "HI");

            migrationBuilder.DropTable(
                name: "IMM");

            migrationBuilder.DropTable(
                name: "K3");

            migrationBuilder.DropTable(
                name: "Loop_LIN_837");

            migrationBuilder.DropTable(
                name: "MEA");

            migrationBuilder.DropTable(
                name: "N2");

            migrationBuilder.DropTable(
                name: "N3");

            migrationBuilder.DropTable(
                name: "NTE");

            migrationBuilder.DropTable(
                name: "PER");

            migrationBuilder.DropTable(
                name: "PWK");

            migrationBuilder.DropTable(
                name: "QTY");

            migrationBuilder.DropTable(
                name: "TOO");

            migrationBuilder.DropTable(
                name: "Loop_SVD_837");

            migrationBuilder.DropTable(
                name: "C022");

            migrationBuilder.DropTable(
                name: "CTP");

            migrationBuilder.DropTable(
                name: "LIN");

            migrationBuilder.DropTable(
                name: "REF");

            migrationBuilder.DropTable(
                name: "C002");

            migrationBuilder.DropTable(
                name: "C005");

            migrationBuilder.DropTable(
                name: "SVD");

            migrationBuilder.DropTable(
                name: "C001");

            migrationBuilder.DropTable(
                name: "C040");

            migrationBuilder.DropTable(
                name: "Loop_NM1_837_2");

            migrationBuilder.DropTable(
                name: "Loop_NM1_837_3");

            migrationBuilder.DropTable(
                name: "Loop_NM1_837_4");

            migrationBuilder.DropTable(
                name: "Loop_NM1_837");

            migrationBuilder.DropTable(
                name: "Loop_SBR_837");

            migrationBuilder.DropTable(
                name: "N4");

            migrationBuilder.DropTable(
                name: "NM1");

            migrationBuilder.DropTable(
                name: "DMG");

            migrationBuilder.DropTable(
                name: "MIA");

            migrationBuilder.DropTable(
                name: "MOA");

            migrationBuilder.DropTable(
                name: "OI");

            migrationBuilder.DropTable(
                name: "Loop_CLM_837");

            migrationBuilder.DropTable(
                name: "CL1");

            migrationBuilder.DropTable(
                name: "CLM");

            migrationBuilder.DropTable(
                name: "CR2");

            migrationBuilder.DropTable(
                name: "CR6");

            migrationBuilder.DropTable(
                name: "CR8");

            migrationBuilder.DropTable(
                name: "DN1");

            migrationBuilder.DropTable(
                name: "DSB");

            migrationBuilder.DropTable(
                name: "Loop_HL_837");

            migrationBuilder.DropTable(
                name: "UR");

            migrationBuilder.DropTable(
                name: "C023");

            migrationBuilder.DropTable(
                name: "C024");

            migrationBuilder.DropTable(
                name: "Loop_LX_837");

            migrationBuilder.DropTable(
                name: "CUR");

            migrationBuilder.DropTable(
                name: "HL");

            migrationBuilder.DropTable(
                name: "PAT");

            migrationBuilder.DropTable(
                name: "PRV");

            migrationBuilder.DropTable(
                name: "SBR");

            migrationBuilder.DropTable(
                name: "TS837");

            migrationBuilder.DropTable(
                name: "CN1");

            migrationBuilder.DropTable(
                name: "CR1");

            migrationBuilder.DropTable(
                name: "CR3");

            migrationBuilder.DropTable(
                name: "CR5");

            migrationBuilder.DropTable(
                name: "HCP");

            migrationBuilder.DropTable(
                name: "HSD");

            migrationBuilder.DropTable(
                name: "LX");

            migrationBuilder.DropTable(
                name: "PS1");

            migrationBuilder.DropTable(
                name: "SV1");

            migrationBuilder.DropTable(
                name: "SV2");

            migrationBuilder.DropTable(
                name: "SV3");

            migrationBuilder.DropTable(
                name: "SV4");

            migrationBuilder.DropTable(
                name: "SV5");

            migrationBuilder.DropTable(
                name: "SV6");

            migrationBuilder.DropTable(
                name: "SV7");

            migrationBuilder.DropTable(
                name: "C035");

            migrationBuilder.DropTable(
                name: "BHT");

            migrationBuilder.DropTable(
                name: "SE");

            migrationBuilder.DropTable(
                name: "ST");

            migrationBuilder.DropTable(
                name: "Loop_CR7_837");

            migrationBuilder.DropTable(
                name: "C006");

            migrationBuilder.DropTable(
                name: "C003");

            migrationBuilder.DropTable(
                name: "C004");

            migrationBuilder.DropTable(
                name: "CR7");
        }
    }
}
