## EDI 837 Ingestetion

A command-line tool built with .NET 8 and EdiFabric to parse EDI 837 files and save them to SQL Server.

Features

- ✅ Parse EDI 837 files with EdiFabric
- ✅ Save 837 files to SQLite
- ✅ Unit tests

## Prerequisites

- .NET 8 SDK
- EdiFabric license (trial or commercial)

## EdiFabric License Setup

EdiFabric requires a valid license key to function. 

### Get a License Key

1. **Trial License** (Free): Visit https://www.edifabric.com/trial.html
2. **Commercial License**: Visit https://www.edifabric.com/ for production use

### Set the License Key

All parameters and variables are stored in appsettings.json file

## Installation

```bash
cd andrey-s-837-ingestion/EDIParser/src/EDIParser
dotnet restore
dotnet build
```

## Usage

### Basic Usage

Parse an EDI 837 file:

```bash
dotnet run
```

### Configuration

All configurations including running mode (EDI local file/S3 bucket file download) and validation modes are configured in appsettings.json
Assuming the desired files are uploaded to S3 bucket, change the following setting as shown:

Parse local an EDI 837 file:

''' Set IsS3Mode to FALSE to process file indicated in "TestFilePath" setting
"TestFilePath: "complete file path"
"IsS3Mode": "False"

''' Set IsS3Mode to TRUE to download and process EDI file name in "TestFilePath" setting from AWS S3 bucket
"TestFilePath: "complete file path(path does not matter)"
"IsS3Mode": "True"


Parse a local or S3 EDI 837 file with setting SNIP level is controlled by setting "SNIPValidationLevel" as follows:

"SNIPValidationLevel": "1" - will validate to SyntaxOnly_SNIP1
"SNIPValidationLevel": "2" - will validate to LimitsAndCodes_SNIP2
"SNIPValidationLevel": "3" - will validate to Balancing_SNIP3
"SNIPValidationLevel": "4" - will validate to InterSegment_SNIP4

Start the process in command line:

```bash
dotnet run 
```

### Moto.py S3

Run S3 Server with Moto.py (tested on boto3-1.42.50, Python 3.14.3, pip 26.0.1):

```bash
```Create and activate a virtual environment (recommended)
python3 -m venv .venv
source .venv/bin/activate
```Start the server
python start_moto.py
```

Upload sample files to Moto.py S3 server:

```bash
python uploadFile.py
```

## Output

The tool produces:

1. **Console Summary** - Overview of parsed data
2. **Log files** - Details of the executed operation in /Logs 


### Example Output

```
2026-02-12 15:13:14.398 -05:00 [INF] Starting EDI parser...
2026-02-12 15:13:14.399 -05:00 [INF] Testing file in '../../../../../samples/837File.edi' is used
2026-02-12 15:13:14.401 -05:00 [INF] Reading EdiFabric.Templates.X12 file /Users/asychev/RiderProjects/EDIParser/samples/837File.edi
2026-02-12 15:13:14.401 -05:00 [INF] Parsing EDI file /Users/asychev/RiderProjects/EDIParser/samples/837File.edi...
2026-02-12 15:13:15.662 -05:00 [INF] Read EDI file /Users/asychev/RiderProjects/EDIParser/samples/837File.edi successfully...
2026-02-12 15:13:42.358 -05:00 [INF] Read 1 TS837 items from /Users/asychev/RiderProjects/EDIParser/samples/837File.edi...
2026-02-12 15:13:46.751 -05:00 [INF] Comitting 1 edi data...
2026-02-12 15:15:03.640 -05:00 [INF] Executed DbCommand (169ms) [Parameters=[@p0='?' (Size = 8), @p1='?' (Size = 4), @p2='?' (Size = 3), @p3='?' (Size = 4), @p4='?' (Size = 2), @p5='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "BHT" ("Date_04", "HierarchicalStructureCode_01", "ReferenceIdentification_03", "Time_05", "TransactionSetPurposeCode_02", "TransactionTypeCode_06")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:03.918 -05:00 [INF] Executed DbCommand (2ms) [Parameters=[@p0='?', @p1='?', @p2='?', @p3='?', @p4='?', @p5='?' (Size = 2), @p6='?' (Size = 5)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C003" ("Description_07", "ProcedureModifier_03", "ProcedureModifier_04", "ProcedureModifier_05", "ProcedureModifier_06", "ProductServiceIDQualifier_01", "ProductServiceID_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)
RETURNING "Id";
2026-02-12 15:15:03.924 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?', @p1='?', @p2='?', @p3='?', @p4='?', @p5='?' (Size = 2), @p6='?' (Size = 5)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C003" ("Description_07", "ProcedureModifier_03", "ProcedureModifier_04", "ProcedureModifier_05", "ProcedureModifier_06", "ProductServiceIDQualifier_01", "ProductServiceID_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)
RETURNING "Id";
2026-02-12 15:15:03.938 -05:00 [INF] Executed DbCommand (12ms) [Parameters=[@p0='?' (Size = 1), @p1='?', @p2='?', @p3='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C004" ("DiagnosisCodePointer_01", "DiagnosisCodePointer_02", "DiagnosisCodePointer_03", "DiagnosisCodePointer_04")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:03.947 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 1), @p1='?', @p2='?', @p3='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C004" ("DiagnosisCodePointer_01", "DiagnosisCodePointer_02", "DiagnosisCodePointer_03", "DiagnosisCodePointer_04")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:03.966 -05:00 [INF] Executed DbCommand (18ms) [Parameters=[@p0='?' (Size = 3), @p1='?', @p2='?', @p3='?' (Size = 3), @p4='?', @p5='?', @p6='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C022" ("CodeListQualifierCode_01", "DateTimePeriodFormatQualifier_03", "DateTimePeriod_04", "IndustryCode_02", "MonetaryAmount_05", "Quantity_06", "VersionIdentifier_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)
RETURNING "Id";
2026-02-12 15:15:03.976 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 3), @p1='?', @p2='?', @p3='?' (Size = 3), @p4='?', @p5='?', @p6='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C022" ("CodeListQualifierCode_01", "DateTimePeriodFormatQualifier_03", "DateTimePeriod_04", "IndustryCode_02", "MonetaryAmount_05", "Quantity_06", "VersionIdentifier_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)
RETURNING "Id";
2026-02-12 15:15:03.981 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 1), @p1='?' (Size = 1), @p2='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C023" ("ClaimFrequencyTypeCode_03", "FacilityCodeQualifier_02", "FacilityCodeValue_01")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
2026-02-12 15:15:04.003 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 1), @p1='?' (Size = 1), @p2='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "C023" ("ClaimFrequencyTypeCode_03", "FacilityCodeQualifier_02", "FacilityCodeValue_01")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
2026-02-12 15:15:04.017 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?', @p1='?', @p2='?', @p3='?' (Size = 2), @p4='?' (Size = 8), @p5='?' (Size = 1), @p6='?', @p7='?', @p8='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "DMG" ("BasisofVerificationCode_08", "CitizenshipStatusCode_06", "CountryCode_07", "DateTimePeriodFormatQualifier_01", "DateTimePeriod_02", "GenderCode_03", "MaritalStatusCode_04", "Quantity_09", "RaceorEthnicityCode_05")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)
RETURNING "Id";
2026-02-12 15:15:04.069 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?', @p1='?', @p2='?', @p3='?' (Size = 2), @p4='?' (Size = 8), @p5='?' (Size = 1), @p6='?', @p7='?', @p8='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "DMG" ("BasisofVerificationCode_08", "CitizenshipStatusCode_06", "CountryCode_07", "DateTimePeriodFormatQualifier_01", "DateTimePeriod_02", "GenderCode_03", "MaritalStatusCode_04", "Quantity_09", "RaceorEthnicityCode_05")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)
RETURNING "Id";
2026-02-12 15:15:04.072 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 1), @p1='?' (Size = 1), @p2='?' (Size = 2), @p3='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "HL" ("HierarchicalChildCode_04", "HierarchicalIDNumber_01", "HierarchicalLevelCode_03", "HierarchicalParentIDNumber_02")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.087 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 1), @p1='?' (Size = 1), @p2='?' (Size = 2), @p3='?' (Size = 1)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "HL" ("HierarchicalChildCode_04", "HierarchicalIDNumber_01", "HierarchicalLevelCode_03", "HierarchicalParentIDNumber_02")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.088 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 1), @p1='?' (Size = 1), @p2='?' (Size = 2), @p3='?' (Size = 1)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "HL" ("HierarchicalChildCode_04", "HierarchicalIDNumber_01", "HierarchicalLevelCode_03", "HierarchicalParentIDNumber_02")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.091 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 1)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "LX" ("AssignedNumber_01")
VALUES (@p0)
RETURNING "Id";
2026-02-12 15:15:04.098 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 1)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "LX" ("AssignedNumber_01")
VALUES (@p0)
RETURNING "Id";
2026-02-12 15:15:04.099 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 7), @p1='?', @p2='?', @p3='?', @p4='?' (Size = 9), @p5='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N4" ("CityName_01", "CountryCode_04", "LocationIdentifier_06", "LocationQualifier_05", "PostalCode_03", "StateorProvinceCode_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.105 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 7), @p1='?', @p2='?', @p3='?', @p4='?' (Size = 9), @p5='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N4" ("CityName_01", "CountryCode_04", "LocationIdentifier_06", "LocationQualifier_05", "PostalCode_03", "StateorProvinceCode_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.107 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 8), @p1='?', @p2='?', @p3='?', @p4='?' (Size = 9), @p5='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N4" ("CityName_01", "CountryCode_04", "LocationIdentifier_06", "LocationQualifier_05", "PostalCode_03", "StateorProvinceCode_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.109 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 7), @p1='?', @p2='?', @p3='?', @p4='?' (Size = 9), @p5='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N4" ("CityName_01", "CountryCode_04", "LocationIdentifier_06", "LocationQualifier_05", "PostalCode_03", "StateorProvinceCode_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.110 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 8), @p1='?', @p2='?', @p3='?', @p4='?' (Size = 9), @p5='?' (Size = 2)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N4" ("CityName_01", "CountryCode_04", "LocationIdentifier_06", "LocationQualifier_05", "PostalCode_03", "StateorProvinceCode_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.112 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 10), @p6='?', @p7='?' (Size = 16), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.116 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 10), @p6='?' (Size = 3), @p7='?' (Size = 6), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.117 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 5), @p6='?', @p7='?' (Size = 5), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.119 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 10), @p6='?' (Size = 4), @p7='?' (Size = 6), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.121 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 5), @p6='?', @p7='?' (Size = 5), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.122 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 6), @p6='?', @p7='?' (Size = 9), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.170 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 1), @p4='?' (Size = 2), @p5='?' (Size = 5), @p6='?', @p7='?' (Size = 8), @p8='?', @p9='?', @p10='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "NM1" ("EntityIdentifierCode_01", "EntityIdentifierCode_11", "EntityRelationshipCode_10", "EntityTypeQualifier_02", "IdentificationCodeQualifier_08", "IdentificationCode_09", "NameFirst_04", "NameLastorOrganizationName_03", "NameMiddle_05", "NamePrefix_06", "NameSuffix_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.172 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 2), @p4='?', @p5='?', @p6='?' (Size = 1), @p7='?', @p8='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "SBR" ("ClaimFilingIndicatorCode_09", "CoordinationofBenefitsCode_06", "EmploymentStatusCode_08", "IndividualRelationshipCode_02", "InsuranceTypeCode_05", "Name_04", "PayerResponsibilitySequenceNumberCode_01", "ReferenceIdentification_03", "YesNoConditionorResponseCode_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)
RETURNING "Id";
2026-02-12 15:15:04.175 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 2), @p4='?', @p5='?', @p6='?' (Size = 1), @p7='?', @p8='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "SBR" ("ClaimFilingIndicatorCode_09", "CoordinationofBenefitsCode_06", "EmploymentStatusCode_08", "IndividualRelationshipCode_02", "InsuranceTypeCode_05", "Name_04", "PayerResponsibilitySequenceNumberCode_01", "ReferenceIdentification_03", "YesNoConditionorResponseCode_07")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)
RETURNING "Id";
2026-02-12 15:15:04.177 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (Size = 2), @p1='?' (Size = 4)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "SE" ("NumberofIncludedSegments_01", "TransactionSetControlNumber_02")
VALUES (@p0, @p1)
RETURNING "Id";
2026-02-12 15:15:04.183 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 12), @p1='?' (Size = 4), @p2='?' (Size = 3)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "ST" ("ImplementationConventionPreference_03", "TransactionSetControlNumber_02", "TransactionSetIdentifierCode_01")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
2026-02-12 15:15:04.192 -05:00 [INF] Executed DbCommand (3ms) [Parameters=[@p3='?', @p4='?', @p5='?', @p6='?' (Size = 5), @p7='?', @p8='?' (DbType = Int32), @p9='?', @p10='?' (Size = 3), @p11='?', @p12='?', @p13='?' (Size = 1), @p14='?', @p15='?' (DbType = Int32), @p16='?' (Size = 1), @p17='?', @p18='?' (Size = 1), @p19='?' (Size = 1), @p20='?', @p21='?', @p22='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "CLM" ("ClaimFilingIndicatorCode_03", "ClaimStatusCode_17", "ClaimSubmissionReasonCode_19", "ClaimSubmittersIdentifier_01", "DelayReasonCode_20", "HealthCareServiceLocationInformation_05Id", "LevelofServiceCode_14", "MonetaryAmount_02", "NonInstitutionalClaimTypeCode_04", "PatientSignatureSourceCode_10", "ProviderAcceptAssignmentCode_07", "ProviderAgreementCode_16", "RelatedCausesInformation_11Id", "ReleaseofInformationCode_09", "SpecialProgramCode_12", "YesNoConditionorResponseCode_06", "YesNoConditionorResponseCode_08", "YesNoConditionorResponseCode_13", "YesNoConditionorResponseCode_15", "YesNoConditionorResponseCode_18")
VALUES (@p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22)
RETURNING "Id";
2026-02-12 15:15:04.199 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?', @p1='?', @p2='?', @p3='?' (Size = 5), @p4='?', @p5='?' (DbType = Int32), @p6='?', @p7='?' (Size = 3), @p8='?', @p9='?', @p10='?' (Size = 1), @p11='?', @p12='?' (DbType = Int32), @p13='?' (Size = 1), @p14='?', @p15='?' (Size = 1), @p16='?' (Size = 1), @p17='?', @p18='?', @p19='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "CLM" ("ClaimFilingIndicatorCode_03", "ClaimStatusCode_17", "ClaimSubmissionReasonCode_19", "ClaimSubmittersIdentifier_01", "DelayReasonCode_20", "HealthCareServiceLocationInformation_05Id", "LevelofServiceCode_14", "MonetaryAmount_02", "NonInstitutionalClaimTypeCode_04", "PatientSignatureSourceCode_10", "ProviderAcceptAssignmentCode_07", "ProviderAgreementCode_16", "RelatedCausesInformation_11Id", "ReleaseofInformationCode_09", "SpecialProgramCode_12", "YesNoConditionorResponseCode_06", "YesNoConditionorResponseCode_08", "YesNoConditionorResponseCode_13", "YesNoConditionorResponseCode_15", "YesNoConditionorResponseCode_18")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19)
RETURNING "Id";
2026-02-12 15:15:04.203 -05:00 [INF] Executed DbCommand (3ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?', @p3='?', @p4='?', @p5='?', @p6='?' (Size = 3), @p7='?', @p8='?', @p9='?', @p10='?', @p11='?', @p12='?', @p13='?' (Size = 1), @p14='?', @p15='?', @p16='?', @p17='?' (Size = 2), @p18='?', @p19='?', @p20='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "SV1" ("CompositeDiagnosisCodePointer_07Id", "CompositeMedicalProcedureIdentifier_01Id", "CopayStatusCode_15", "FacilityCodeValue_05", "HealthCareProfessionalShortageAreaCode_16", "LevelofCareCode_20", "MonetaryAmount_02", "MonetaryAmount_08", "MonetaryAmount_19", "MultipleProcedureCode_10", "NationalorLocalAssignedReviewValue_14", "PostalCode_18", "ProviderAgreementCode_21", "Quantity_04", "ReferenceIdentification_17", "ReviewCode_13", "ServiceTypeCode_06", "UnitorBasisforMeasurementCode_03", "YesNoConditionorResponseCode_09", "YesNoConditionorResponseCode_11", "YesNoConditionorResponseCode_12")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20)
RETURNING "Id";
2026-02-12 15:15:04.206 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?', @p3='?', @p4='?', @p5='?', @p6='?' (Size = 3), @p7='?', @p8='?', @p9='?', @p10='?', @p11='?', @p12='?', @p13='?' (Size = 1), @p14='?', @p15='?', @p16='?', @p17='?' (Size = 2), @p18='?', @p19='?', @p20='?'], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "SV1" ("CompositeDiagnosisCodePointer_07Id", "CompositeMedicalProcedureIdentifier_01Id", "CopayStatusCode_15", "FacilityCodeValue_05", "HealthCareProfessionalShortageAreaCode_16", "LevelofCareCode_20", "MonetaryAmount_02", "MonetaryAmount_08", "MonetaryAmount_19", "MultipleProcedureCode_10", "NationalorLocalAssignedReviewValue_14", "PostalCode_18", "ProviderAgreementCode_21", "Quantity_04", "ReferenceIdentification_17", "ReviewCode_13", "ServiceTypeCode_06", "UnitorBasisforMeasurementCode_03", "YesNoConditionorResponseCode_09", "YesNoConditionorResponseCode_11", "YesNoConditionorResponseCode_12")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20)
RETURNING "Id";
2026-02-12 15:15:04.209 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "TS837" ("BHTId", "SEId", "STId")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
2026-02-12 15:15:04.215 -05:00 [INF] Executed DbCommand (2ms) [Parameters=[@p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_HL_837" ("CURId", "HLId", "PATId", "PRVId", "SBRId", "TS837Id")
VALUES (@p3, @p4, @p5, @p6, @p7, @p8)
RETURNING "Id";
2026-02-12 15:15:04.219 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_HL_837" ("CURId", "HLId", "PATId", "PRVId", "SBRId", "TS837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.221 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_HL_837" ("CURId", "HLId", "PATId", "PRVId", "SBRId", "TS837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.224 -05:00 [INF] Executed DbCommand (2ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837" ("N4Id", "NM1Id", "TS837Id")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
2026-02-12 15:15:04.228 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837" ("N4Id", "NM1Id", "TS837Id")
VALUES (@p0, @p1, @p2)
RETURNING "Id";
2026-02-12 15:15:04.235 -05:00 [INF] Executed DbCommand (5ms) [Parameters=[@p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32), @p11='?' (DbType = Int32), @p12='?' (DbType = Int32), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32), @p15='?' (DbType = Int32), @p16='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_CLM_837" ("CL1Id", "CLMId", "CN1Id", "CR1Id", "CR2Id", "CR3Id", "CR5Id", "CR6Id", "CR8Id", "DN1Id", "DSBId", "HCPId", "Loop_HL_837Id", "URId")
VALUES (@p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16)
RETURNING "Id";
2026-02-12 15:15:04.240 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32), @p11='?' (DbType = Int32), @p12='?' (DbType = Int32), @p13='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_CLM_837" ("CL1Id", "CLMId", "CN1Id", "CR1Id", "CR2Id", "CR3Id", "CR5Id", "CR6Id", "CR8Id", "DN1Id", "DSBId", "HCPId", "Loop_HL_837Id", "URId")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13)
RETURNING "Id";
2026-02-12 15:15:04.245 -05:00 [INF] Executed DbCommand (2ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837_2" ("DMGId", "Loop_HL_837Id", "N4Id", "NM1Id")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.376 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837_2" ("DMGId", "Loop_HL_837Id", "N4Id", "NM1Id")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.378 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837_2" ("DMGId", "Loop_HL_837Id", "N4Id", "NM1Id")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.379 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837_2" ("DMGId", "Loop_HL_837Id", "N4Id", "NM1Id")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.381 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_NM1_837_2" ("DMGId", "Loop_HL_837Id", "N4Id", "NM1Id")
VALUES (@p0, @p1, @p2, @p3)
RETURNING "Id";
2026-02-12 15:15:04.418 -05:00 [INF] Executed DbCommand (36ms) [Parameters=[@p0='?' (Size = 2), @p1='?', @p2='?', @p3='?' (Size = 10), @p4='?', @p5='?', @p6='?' (Size = 2), @p7='?', @p8='?' (DbType = Int32), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32), @p11='?' (DbType = Int32), @p12='?' (Size = 9)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "PER" ("CommunicationNumberQualifier_03", "CommunicationNumberQualifier_05", "CommunicationNumberQualifier_07", "CommunicationNumber_04", "CommunicationNumber_06", "CommunicationNumber_08", "ContactFunctionCode_01", "ContactInquiryReference_09", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id", "Name_02")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12)
RETURNING "Id";
2026-02-12 15:15:04.429 -05:00 [INF] Executed DbCommand (7ms) [Parameters=[@p13='?' (DbType = Int32), @p14='?' (DbType = Int32), @p15='?' (DbType = Int32), @p16='?' (DbType = Int32), @p17='?' (DbType = Int32), @p18='?' (DbType = Int32), @p19='?' (DbType = Int32), @p20='?' (DbType = Int32), @p21='?' (DbType = Int32), @p22='?' (DbType = Int32), @p23='?' (DbType = Int32), @p24='?' (DbType = Int32), @p25='?' (DbType = Int32), @p26='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "HI" ("HealthCareCodeInformation_01Id", "HealthCareCodeInformation_02Id", "HealthCareCodeInformation_03Id", "HealthCareCodeInformation_04Id", "HealthCareCodeInformation_05Id", "HealthCareCodeInformation_06Id", "HealthCareCodeInformation_07Id", "HealthCareCodeInformation_08Id", "HealthCareCodeInformation_09Id", "HealthCareCodeInformation_10Id", "HealthCareCodeInformation_11Id", "HealthCareCodeInformation_12Id", "Loop_CLM_837Id", "Loop_LX_837Id")
VALUES (@p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26)
RETURNING "Id";
2026-02-12 15:15:04.440 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32), @p11='?' (DbType = Int32), @p12='?' (DbType = Int32), @p13='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "HI" ("HealthCareCodeInformation_01Id", "HealthCareCodeInformation_02Id", "HealthCareCodeInformation_03Id", "HealthCareCodeInformation_04Id", "HealthCareCodeInformation_05Id", "HealthCareCodeInformation_06Id", "HealthCareCodeInformation_07Id", "HealthCareCodeInformation_08Id", "HealthCareCodeInformation_09Id", "HealthCareCodeInformation_10Id", "HealthCareCodeInformation_11Id", "HealthCareCodeInformation_12Id", "Loop_CLM_837Id", "Loop_LX_837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13)
RETURNING "Id";
2026-02-12 15:15:04.448 -05:00 [INF] Executed DbCommand (6ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32), @p11='?' (DbType = Int32), @p12='?' (DbType = Int32), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32), @p15='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_LX_837" ("CN1Id", "CR1Id", "CR3Id", "CR5Id", "HCPId", "HSDId", "LXId", "Loop_CLM_837Id", "PS1Id", "SV1Id", "SV2Id", "SV3Id", "SV4Id", "SV5Id", "SV6Id", "SV7Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15)
RETURNING "Id";
2026-02-12 15:15:04.456 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?' (DbType = Int32), @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32), @p8='?' (DbType = Int32), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32), @p11='?' (DbType = Int32), @p12='?' (DbType = Int32), @p13='?' (DbType = Int32), @p14='?' (DbType = Int32), @p15='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "Loop_LX_837" ("CN1Id", "CR1Id", "CR3Id", "CR5Id", "HCPId", "HSDId", "LXId", "Loop_CLM_837Id", "PS1Id", "SV1Id", "SV2Id", "SV3Id", "SV4Id", "SV5Id", "SV6Id", "SV7Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15)
RETURNING "Id";
2026-02-12 15:15:04.463 -05:00 [INF] Executed DbCommand (2ms) [Parameters=[@p0='?' (Size = 14), @p1='?', @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N3" ("AddressInformation_01", "AddressInformation_02", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.465 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 11), @p1='?', @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N3" ("AddressInformation_01", "AddressInformation_02", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.466 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 11), @p1='?', @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N3" ("AddressInformation_01", "AddressInformation_02", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.467 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 13), @p1='?', @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N3" ("AddressInformation_01", "AddressInformation_02", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.469 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 11), @p1='?', @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "N3" ("AddressInformation_01", "AddressInformation_02", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
RETURNING "Id";
2026-02-12 15:15:04.475 -05:00 [INF] Executed DbCommand (5ms) [Parameters=[@p0='?', @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (Size = 2), @p8='?' (Size = 9), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "REF" ("Description_03", "Loop_CLM_837Id", "Loop_LX_837Id", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id", "ReferenceIdentificationQualifier_01", "ReferenceIdentification_02", "ReferenceIdentifier_04Id", "TS837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.478 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?', @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (Size = 2), @p8='?' (Size = 5), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "REF" ("Description_03", "Loop_CLM_837Id", "Loop_LX_837Id", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id", "ReferenceIdentificationQualifier_01", "ReferenceIdentification_02", "ReferenceIdentifier_04Id", "TS837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.481 -05:00 [INF] Executed DbCommand (1ms) [Parameters=[@p0='?', @p1='?' (DbType = Int32), @p2='?' (DbType = Int32), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (Size = 2), @p8='?' (Size = 5), @p9='?' (DbType = Int32), @p10='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "REF" ("Description_03", "Loop_CLM_837Id", "Loop_LX_837Id", "Loop_NM1_837Id", "Loop_NM1_837_2Id", "Loop_NM1_837_3Id", "Loop_NM1_837_4Id", "ReferenceIdentificationQualifier_01", "ReferenceIdentification_02", "ReferenceIdentifier_04Id", "TS837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)
RETURNING "Id";
2026-02-12 15:15:04.486 -05:00 [INF] Executed DbCommand (3ms) [Parameters=[@p11='?' (Size = 2), @p12='?' (Size = 8), @p13='?' (Size = 3), @p14='?' (DbType = Int32), @p15='?' (DbType = Int32), @p16='?' (DbType = Int32), @p17='?' (DbType = Int32), @p18='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "DTP" ("DateTimePeriodFormatQualifier_02", "DateTimePeriod_03", "DateTimeQualifier_01", "Loop_CLM_837Id", "Loop_HL_837Id", "Loop_LX_837Id", "Loop_NM1_837_4Id", "Loop_SVD_837Id")
VALUES (@p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18)
RETURNING "Id";
2026-02-12 15:15:04.514 -05:00 [INF] Executed DbCommand (0ms) [Parameters=[@p0='?' (Size = 2), @p1='?' (Size = 8), @p2='?' (Size = 3), @p3='?' (DbType = Int32), @p4='?' (DbType = Int32), @p5='?' (DbType = Int32), @p6='?' (DbType = Int32), @p7='?' (DbType = Int32)], CommandType='"Text"', CommandTimeout='30']
INSERT INTO "DTP" ("DateTimePeriodFormatQualifier_02", "DateTimePeriod_03", "DateTimeQualifier_01", "Loop_CLM_837Id", "Loop_HL_837Id", "Loop_LX_837Id", "Loop_NM1_837_4Id", "Loop_SVD_837Id")
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)
RETURNING "Id";
2026-02-12 15:15:28.418 -05:00 [INF] Console app shutting down...

============================================================
```

## Sample EDI 837 Files

Sample files are included at `samples/'
ClaimPayment.edi - 837P
DentalClaim.edi - 837D
InstitutionalClaim.edi - 837I

## How It Works

1. **Read EDI File** - Uses EdiFabric's X12Reader to parse the file
2. **Save Data to Database** - Saves parsed EDI data to SQLite using Entity Framework Core
3. **Outputs processed transactions count** - 


## Troubleshooting

## License

MIT License

## Credits

Built with:
- [EdiFabric](https://www.edifabric.com/) - EDI parsing library
- [System.CommandLine](https://github.com/dotnet/command-line-api) - Command-line interface
- .NET 8
