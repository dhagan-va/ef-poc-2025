using EdiFabric.Framework.Writers;
using EdiFabric.Templates.Hipaa5010;
using EdiMettle.Common;
using EdiMettle.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle.BLL
{
    public class GhostWriter
    {
        public static void WriteToFile(InstitutionalClaim claim, InterchangeHeader header, GroupSegment groupSeg)
        {
            //const int controlNumber = 987654;
            //var transaction = BuildInstitutionalClaim(controlNumber);
            //InterchangeHeader headseg = new(controlNumber: 1);
            //GroupSegment groupseg = new(controlNumber: "1", senderId: "SENDER1", receiverId: "RECEIVER1", version: "005010X223A2");

            using var stream = new MemoryStream();
            using (var writer = new X12Writer(stream))
            {
                writer.Write(header.ToInsanity());
                writer.Write(groupSeg.ToInsanity());
                writer.Write(claim.ToInsanity());
            }

            var ediString = stream.LoadToString();
            throw new NotImplementedException("TODO:  Write to text file");
        }

        #region giant claim info comment
        /// The patient is a different person than the subscriber. The payer is a commercial health insurance company.
        /// Scenario:
        /// PRIMARY PAYER SUBSCRIBER: John T Doe
        /// SUBSCRIBER ADDRESS: 125 City Avenue, Centerville, PA 17111
        /// SEX: M
        /// DOB: 11/11/1926
        /// MEDICARE INSURANCE ID#: 030005074A
        /// PAYER ID #: 00435
        /// PATIENT: Same as Primary Subscriber
        /// DESTINATION PAYER: Medicare B
        /// SUBMITTER: Jones Hospital
        /// EDI#: 12345
        /// RECEIVER: Medicare
        /// EDI#: 00120
        /// BILLING PROVIDER: Jones Hospital
        /// NPI: 9876540809
        /// TIN: 567891234
        /// MEDICARE PROVIDER: #330127
        /// ADDRESS: 225 Main Street Barkley Building, Centerville, PA 17111
        /// ATTENDING PHYSICIAN: John J Jones
        /// UPIN #: B99937
        /// PATIENT ACCOUNT NUMBER: 756048Q
        /// DATE OF ADMISSION: 09/11/96
        /// STATEMENT PERIOD DATE: 09/11/96 - 09/11/96
        /// PLACE OF SERVICE: Inpatient Hospital
        /// Occurrence Codes and Dates:
        /// A1 11/11/26
        /// A2 11/01/91
        /// B1 11/11/26
        /// B2 01/01/87
        /// Condition Codes: 09
        /// Value Codes: A2 $15.31
        /// PRINCIPAL DIAGNOSIS CODE: 366.9
        /// SECONDARY DIAGNOSIS CODES:
        /// 401.9
        /// 794.31
        /// NUMBER OF COVERED DAYS: 1
        /// SERVICES:
        /// INSTITUTIONAL SERVICES RENDERED:
        /// REVENUE CODE: 0305 HCPCS Procedure Code: 85025 Unit: 1 Price $13.39
        /// REVENUE CODE: 0730 HCPCS Procedure Code: 93005 Unit: 1 Price: $76.54
        /// TOTAL CHARGES: $89.93
        /// SECONDARY PAYER SUBSCRIBER: Jane S Doe(wife)
        /// SUBSCRIBER ADDRESS: 125 City Avenue, Centerville, PA 17111
        /// SEX: F
        /// DOB: 12/11/1927
        /// STATE TEACHERS ID#: 222004433
        /// PAYER ID #: 1135
#endregion

        /// <summary>
        /// Build institutional claim.
        /// Original from http://www.x12.org/examples/005010X223/institutional/837-institutional-claim/
        /// </summary>
        static TS837I BuildInstitutionalClaim(int controlNumber)
        {
            var result = new TS837I
            {
                //  Occurrence of NM1 Loops in any order
                AllNM1 = new All_NM1_837I_6
                {
                    //  Begin 1000A Loop SUBMITTER NAME
                    Loop1000A = new Loop_1000A_837I
                    {
                        //  NM1 SUBMITTER NAME
                        NM1_SubmitterName = new NM1_InformationReceiverName_4
                        {
                            EntityIdentifierCode_01 = "41",
                            EntityTypeQualifier_02 = "2",
                            ResponseContactLastorOrganizationName_03 = "JONES HOSPITAL",
                            IdentificationCodeQualifier_08 = "46",
                            ResponseContactIdentifier_09 = "12345"
                        },

                        //  Repeating PER Patient information
                        PER_SubmitterEDIContactInformation = []
                    }
                }
            };

            //  PER SUBMITTER EDI CONTACT INFORMATION
            var per1 = new PER_BillingProviderContactInformation
            {
                ContactFunctionCode_01 = "IC",
                ResponseContactName_02 = "JANE DOE",
                CommunicationNumberQualifier_03 = "TE",
                ResponseContactCommunicationNumber_04 = "9005555555"
            };
            result.AllNM1.Loop1000A.PER_SubmitterEDIContactInformation.Add(per1);

            //  End 1000A Loop

            //  Begin 1000B Loop RECEIVER NAME
            result.AllNM1.Loop1000B = new Loop_1000B_837I
            {
                //  NM1 RECEIVER NAME
                NM1_ReceiverName = new NM1_ReceiverName
                {
                    EntityIdentifierCode_01 = "40",
                    EntityTypeQualifier_02 = "2",
                    ResponseContactLastorOrganizationName_03 = "MEDICARE",
                    IdentificationCodeQualifier_08 = "46",
                    ResponseContactIdentifier_09 = "00120"
                }
            };

            //  End 1000B Loop

            //  Repeating 2000A Loops
            result.Loop2000A = [];

            //  Begin 2000A Loop BILLING PROVIDER
            var loop2000A = new Loop_2000A_837I
            {
                //  HL BILLING PROVIDER HIERARCHICAL LEVEL
                HL_BillingProviderHierarchicalLevel = new HL_BillingProviderHierarchicalLevel
                {
                    HierarchicalIDNumber_01 = "1",
                    HierarchicalLevelCode_03 = "20",
                    HierarchicalChildCode_04 = "1"
                },

                //  PRV BILLING PROVIDER SPECIALTY
                PRV_BillingProviderSpecialtyInformation = new PRV_BillingProviderSpecialtyInformation
                {
                    ProviderCode_01 = "BI",
                    ReferenceIdentificationQualifier_02 = "PXC",
                    ProviderTaxonomyCode_03 = "203BA0200N"
                },

                //  Occurrence of NM1 Loops in any order
                AllNM1 = new All_NM1_837I
                {
                    //  Begin 2010AA Loop BILLING PROVIDER NAME
                    Loop2010AA = new Loop_2010AA_837I
                    {
                        //  NM1 BILLING PROVIDER NAME INCLUDING NATIONAL PROVIDER ID
                        NM1_BillingProviderName = new NM1_BillingProviderName_3
                        {
                            EntityIdentifierCode_01 = "85",
                            EntityTypeQualifier_02 = "2",
                            ResponseContactLastorOrganizationName_03 = "JONES HOSPITAL",
                            IdentificationCodeQualifier_08 = "XX",
                            ResponseContactIdentifier_09 = "9876540809"
                        },

                        //  N3 BILLING PROVIDER ADDRESS
                        N3_BillingProviderAddress = new N3_AdditionalPatientInformationContactAddress
                        {
                            ResponseContactAddressLine_01 = "225 MAIN STREET BARKLEY BUILDING"
                        },

                        //  N4 BILLING PROVIDER LOCATION
                        N4_BillingProviderCity_State_ZIPCode = new N4_AdditionalPatientInformationContactCity
                        {
                            AdditionalPatientInformationContactCityName_01 = "CENTERVILLE",
                            AdditionalPatientInformationContactStateCode_02 = "PA",
                            AdditionalPatientInformationContactPostalZoneorZIPCode_03 = "17111"
                        },

                        //  REF BILLING PROVIDER TAX IDENTIFICATION NUMBER
                        REF_BillingProviderTaxIdentification = new REF_BillingProviderTaxIdentification_2
                        {
                            ReferenceIdentificationQualifier_01 = "EI",
                            MemberGrouporPolicyNumber_02 = "567891234"
                        },

                        //  Repeating PER billing provider contact information
                        PER_BillingProviderContactInformation = []
                    }
                }
            };

            //  PER BILLING PROVIDER CONTACT INFORMATION
            var per2 = new PER_BillingProviderContactInformation
            {
                ContactFunctionCode_01 = "IC",
                ResponseContactName_02 = "CONNIE",
                CommunicationNumberQualifier_03 = "TE",
                ResponseContactCommunicationNumber_04 = "3055551234"
            };
            loop2000A.AllNM1.Loop2010AA.PER_BillingProviderContactInformation.Add(per2);

            //  End 2010AA Loop

            //  Repeating 2000B Loops
            loop2000A.Loop2000B = [];

            //  Begin 2000B Loop SUBSCRIBER HL LOOP
            var loop2000B = new Loop_2000B_837I
            {
                //  HL SUBSCRIBER HIERARCHICAL LEVEL
                HL_SubscriberHierarchicalLevel = new HL_SubscriberHierarchicalLevel
                {
                    HierarchicalIDNumber_01 = "2",
                    HierarchicalParentIDNumber_02 = "1",
                    HierarchicalLevelCode_03 = "22",
                    HierarchicalChildCode_04 = "0"
                },

                //  SBR SUBSCRIBER INFORMATION
                SBR_SubscriberInformation = new SBR_SubscriberInformation_2
                {
                    PayerResponsibilitySequenceNumberCode_01 = "P",
                    IndividualRelationshipCode_02 = "18",
                    ClaimFilingIndicatorCode_09 = "MB"
                },

                //  Occurrence of NM1 Loops in any order
                AllNM1 = new All_NM1_837I_2
                {
                    //  Begin 2010BA Loop SUBSCRIBER NAME LOOP
                    Loop2010BA = new Loop_2010BA_837I
                    {
                        //  NM1 SUBSCRIBER NAME
                        NM1_SubscriberName = new NM1_SubscriberName_5
                        {
                            EntityIdentifierCode_01 = "IL",
                            EntityTypeQualifier_02 = "1",
                            ResponseContactLastorOrganizationName_03 = "DOE",
                            ResponseContactFirstName_04 = "JOHN",
                            ResponseContactMiddleName_05 = "T",
                            IdentificationCodeQualifier_08 = "MI",
                            ResponseContactIdentifier_09 = "030005074A"
                        },

                        //  N3 SUBSCRIBER ADDRESS
                        N3_SubscriberAddress = new N3_AdditionalPatientInformationContactAddress
                        {
                            ResponseContactAddressLine_01 = "125 CITY AVENUE"
                        },

                        //  N4 SUBSCRIBER LOCATION
                        N4_SubscriberCity_State_ZIPCode = new N4_AdditionalPatientInformationContactCity
                        {
                            AdditionalPatientInformationContactCityName_01 = "CENTERVILLE",
                            AdditionalPatientInformationContactStateCode_02 = "PA",
                            AdditionalPatientInformationContactPostalZoneorZIPCode_03 = "17111"
                        },

                        //  DMG SUBSCRIBER DEMOGRAPHIC INFORMATION
                        DMG_SubscriberDemographicInformation = new DMG_PatientDemographicInformation
                        {
                            DateTimePeriodFormatQualifier_01 = "D8",
                            DependentBirthDate_02 = "19261111",
                            DependentGenderCode_03 = "M"
                        }
                    },

                    //  End 2010BA Loop

                    //  Begin 2010BB Loop PAYER NAME LOOP
                    Loop2010BB = new Loop_2010BB_837I
                    {
                        //  NM1 PAYER NAME
                        NM1_PayerName = new NM1_OtherPayerName
                        {
                            EntityIdentifierCode_01 = "PR",
                            EntityTypeQualifier_02 = "2",
                            ResponseContactLastorOrganizationName_03 = "MEDICARE B",
                            IdentificationCodeQualifier_08 = "PI",
                            ResponseContactIdentifier_09 = "00435"
                        },

                        //  Occurrence of REF Segments in any order
                        AllREF = new All_REF_837I_3
                        {
                            //  REF BILLING PROVIDER SECONDARY IDENTIFICATION
                            REF_BillingProviderSecondaryIdentification = new REF_BillingProviderSecondaryIdentification
                            {
                                ReferenceIdentificationQualifier_01 = "G2",
                                MemberGrouporPolicyNumber_02 = "330127"
                            }
                        }
                    }
                },

                //  End 2010BB Loop

                //  Repeating 2300 Loops
                Loop2300 = []
            };

            //  Begin 2300 Loop CLAIM INFORMATION
            var loop2300 = new Loop_2300_837I
            {
                //  CLM CLAIM LEVEL INFORMATION
                CLM_ClaimInformation = new CLM_ClaimInformation_2
                {
                    PatientControlNumber_01 = "756048Q",
                    TotalClaimChargeAmount_02 = "89.93",
                    NonInstitutionalClaimTypeCode_04 = "14:A:1",
                    HealthCareServiceLocationInformation_05 = new C023_HealthCareServiceLocationInformation_3()
                }
            };
            loop2300.CLM_ClaimInformation.HealthCareServiceLocationInformation_05.FacilityTypeCode_01 = "A";
            loop2300.CLM_ClaimInformation.ProviderorSupplierSignatureIndicator_06 = "Y";
            loop2300.CLM_ClaimInformation.AssignmentorPlanParticipationCode_07 = "Y";

            //  Occurrence of DTP Segments in any order
            loop2300.AllDTP = new All_DTP_837I
            {
                //  DTP STATEMENT DATES
                DTP_StatementDates = new DTP_StatementDates
                {
                    DateTimeQualifier_01 = "434",
                    DateTimePeriodFormatQualifier_02 = "RD8",
                    DateTimePeriod_03 = "19960911"
                }
            };

            //  CL1 INSTITUTIONAL CLAIM CODE
            loop2300.CL1_InstitutionalClaimCode = new CL1_InstitutionalClaimCode
            {
                AdmissionTypeCode_01 = "3",
                PatientStatusCode_03 = "01"
            };

            //  Occurrence of HI Segments in any order
            loop2300.AllHI = new All_HI_837I
            {
                //  HI PRINCIPAL DIAGNOSIS CODES
                HI_PrincipalDiagnosis = new HI_PrincipalDiagnosis
                {
                    HealthCareCodeInformation_01 = new C022_HealthCareCodeInformation_8()
                }
            };
            loop2300.AllHI.HI_PrincipalDiagnosis.HealthCareCodeInformation_01.CodeListQualifierCode_01 = "BK";
            loop2300.AllHI.HI_PrincipalDiagnosis.HealthCareCodeInformation_01.IndustryCode_02 = "3669";

            //  Repeatable HI Other Diagnostics
            loop2300.AllHI.HI_OtherDiagnosisInformation = [];

            //  HI OTHER DIAGNOSIS INFORMATION
            var hi1 = new HI_OtherDiagnosisInformation
            {
                HealthCareCodeInformation_01 = new C022_HealthCareCodeInformation_4
                {
                    CodeListQualifierCode_01 = "BF",
                    IndustryCode_02 = "4019"
                },
                HealthCareCodeInformation_02 = new C022_HealthCareCodeInformation_4
                {
                    CodeListQualifierCode_01 = "BF",
                    IndustryCode_02 = "79431"
                }
            };
            loop2300.AllHI.HI_OtherDiagnosisInformation.Add(hi1);

            //  Repeatable HI Occurrence Information
            loop2300.AllHI.HI_OccurrenceInformation = [];

            //  HI OCCURRENCE INFORMATION
            var hi3 = new HI_OccurrenceInformation
            {
                HealthCareCodeInformation_01 = new C022_HealthCareCodeInformation_6
                {
                    CodeListQualifierCode_01 = "BH",
                    IndustryCode_02 = "A1",
                    DateTimePeriodFormatQualifier_03 = "D8",
                    DateTimePeriod_04 = "19261111"
                },
                HealthCareCodeInformation_02 = new C022_HealthCareCodeInformation_6
                {
                    CodeListQualifierCode_01 = "BH",
                    IndustryCode_02 = "A2",
                    DateTimePeriodFormatQualifier_03 = "D8",
                    DateTimePeriod_04 = "19911101"
                },
                HealthCareCodeInformation_03 = new C022_HealthCareCodeInformation_6()
            };
            hi3.HealthCareCodeInformation_03 = new C022_HealthCareCodeInformation_6
            {
                CodeListQualifierCode_01 = "BH",
                IndustryCode_02 = "B1",
                DateTimePeriodFormatQualifier_03 = "D8",
                DateTimePeriod_04 = "19261111"
            };
            hi3.HealthCareCodeInformation_04 = new C022_HealthCareCodeInformation_6
            {
                CodeListQualifierCode_01 = "BH",
                IndustryCode_02 = "B2",
                DateTimePeriodFormatQualifier_03 = "D8",
                DateTimePeriod_04 = "19870101"
            };
            loop2300.AllHI.HI_OccurrenceInformation.Add(hi3);

            //  Repeatable HI Value Information
            loop2300.AllHI.HI_ValueInformation = [];

            //  HI VALUE INFORMATION
            var hi7 = new HI_ValueInformation
            {
                HealthCareCodeInformation_01 = new C022_HealthCareCodeInformation_7
                {
                    CodeListQualifierCode_01 = "BE",
                    IndustryCode_02 = "A2",
                    MonetaryAmount_05 = "15.31"
                }
            };
            loop2300.AllHI.HI_ValueInformation.Add(hi7);

            //  Repeatable HI Condition Information
            loop2300.AllHI.HI_ConditionInformation = [];

            //  HI CONDITION INFORMATION
            var hi8 = new HI_ConditionInformation
            {
                HealthCareCodeInformation_01 = new C022_HealthCareCodeInformation_13
                {
                    CodeListQualifierCode_01 = "BG",
                    IndustryCode_02 = "09"
                }
            };
            loop2300.AllHI.HI_ConditionInformation.Add(hi8);

            //  Occurrence of NM1 Loops in any order
            loop2300.AllNM1 = new All_NM1_837I_3
            {
                //  Begin 2310A Loop ATTENDING PROVIDER NAME
                Loop2310A = new Loop_2310A_837I
                {
                    //  NM1 ATTENDING PROVIDER
                    NM1_AttendingProviderName = new NM1_AttendingProviderName
                    {
                        EntityIdentifierCode_01 = "71",
                        EntityTypeQualifier_02 = "1",
                        ResponseContactLastorOrganizationName_03 = "JONES",
                        ResponseContactFirstName_04 = "JOHN",
                        ResponseContactMiddleName_05 = "J"
                    },

                    //  Repeating REF Secondary Identification
                    REF_AttendingProviderSecondaryIdentification = []
                }
            };

            //  REF ATTENDING PROVIDER SECONDARY IDENTIFICATION
            var ref1 = new REF_AssistantSurgeonSecondaryIdentification
            {
                ReferenceIdentificationQualifier_01 = "1G",
                MemberGrouporPolicyNumber_02 = "B99937"
            };
            loop2300.AllNM1.Loop2310A.REF_AttendingProviderSecondaryIdentification.Add(ref1);

            //  End 2310A Loop

            //  Repeating a Loops
            loop2300.Loop2320 = [];

            //  Begin 2320 Loop OTHER SUBSCRIBER INFORMATION
            var loop2320 = new Loop_2320_837I
            {
                //  SBR OTHER SUBSCRIBER INFORMATION
                SBR_OtherSubscriberInformation = new SBR_OtherSubscriberInformation_2
                {
                    PayerResponsibilitySequenceNumberCode_01 = "S",
                    IndividualRelationshipCode_02 = "01",
                    InsuredGrouporPolicyNumber_03 = "351630",
                    OtherInsuredGroupName_04 = "STATE TEACHERS",
                    ClaimFilingIndicatorCode_09 = "CI"
                },

                //  OI OTHER INSURANCE COVERAGE INFORMATION
                OI_OtherInsuranceCoverageInformation = new OI_OtherInsuranceCoverageInformation_2
                {
                    BenefitsAssignmentCertificationIndicator_03 = "Y",
                    ReleaseofInformationCode_06 = "Y"
                },

                //  Occurrence of NM1 Loops in any order
                AllNM1 = new All_NM1_837I_4
                {
                    //  Begin 2330A Loop OTHER SUBSCRIBER NAME
                    Loop2330A = new Loop_2330A_837I
                    {
                        //  NM1 OTHER SUBSCRIBER NAME
                        NM1_OtherSubscriberName = new NM1_OtherSubscriberName
                        {
                            EntityIdentifierCode_01 = "IL",
                            EntityTypeQualifier_02 = "1",
                            ResponseContactLastorOrganizationName_03 = "DOE",
                            ResponseContactFirstName_04 = "JANE",
                            ResponseContactMiddleName_05 = "S",
                            IdentificationCodeQualifier_08 = "MI",
                            ResponseContactIdentifier_09 = "222004433"
                        },

                        //  N3 OTHER SUBSCRIBER ADDRESS
                        N3_OtherSubscriberAddress = new N3_AdditionalPatientInformationContactAddress
                        {
                            ResponseContactAddressLine_01 = "125 CITY AVENUE"
                        },

                        //  N4 OTHER SUBSCRIBER CITY, STATE, ZIP CODE
                        N4_OtherSubscriberCity_State_ZIPCode = new N4_AdditionalPatientInformationContactCity
                        {
                            AdditionalPatientInformationContactCityName_01 = "CENTERVILLE",
                            AdditionalPatientInformationContactStateCode_02 = "PA",
                            AdditionalPatientInformationContactPostalZoneorZIPCode_03 = "17111"
                        }
                    },

                    //  End 2330A Loop

                    //  Begin 2330B Loop OTHER PAYER NAME
                    Loop2330B = new Loop_2330B_837I
                    {
                        //  NM1 OTHER PAYER NAME
                        NM1_OtherPayerName = new NM1_OtherPayerName
                        {
                            EntityIdentifierCode_01 = "PR",
                            EntityTypeQualifier_02 = "2",
                            ResponseContactLastorOrganizationName_03 = "STATE TEACHERS",
                            IdentificationCodeQualifier_08 = "PI",
                            ResponseContactIdentifier_09 = "1135"
                        }
                    }
                }
            };

            //  End 2330B Loop

            //  End 2320 Loop
            loop2300.Loop2320.Add(loop2320);

            //  Repeating 2400 Loops
            loop2300.Loop2400 = [];

            //  Begin 2400 Loop 1 SERVICE LINE
            var loop24001 = new Loop_2400_837I
            {
                //  LX SERVICE LINE COUNTER
                LX_ServiceLineNumber = new LX_HeaderNumber
                {
                    AssignedNumber_01 = "1"
                },

                //  SV2 INSTITUTIONAL SERVICE
                SV2_InstitutionalServiceLine = new SV2_InstitutionalServiceLine
                {
                    ServiceLineRevenueCode_01 = "0305",
                    CompositeMedicalProcedureIdentifier_02 = new C003_CompositeMedicalProcedureIdentifier_8()
                }
            };
            loop24001.SV2_InstitutionalServiceLine.CompositeMedicalProcedureIdentifier_02.ProductorServiceIDQualifier_01 = "HC";
            loop24001.SV2_InstitutionalServiceLine.CompositeMedicalProcedureIdentifier_02.ProcedureCode_02 = "85025";
            loop24001.SV2_InstitutionalServiceLine.LineItemChargeAmount_03 = "13.39";
            loop24001.SV2_InstitutionalServiceLine.UnitorBasisforMeasurementCode_04 = "UN";
            loop24001.SV2_InstitutionalServiceLine.ServiceUnitCount_05 = "1";

            //  DTP DATE - SERVICE DATES
            loop24001.DTP_Date_ServiceDate = new DTP_ClaimLevelServiceDate
            {
                DateTimeQualifier_01 = "472",
                DateTimePeriodFormatQualifier_02 = "D8",
                DateTimePeriod_03 = "19960911"
            };

            //  End 2400 Loop 1
            loop2300.Loop2400.Add(loop24001);

            //  Begin 2400 Loop 2 SERVICE LINE
            var loop24002 = new Loop_2400_837I
            {
                //  LX SERVICE LINE COUNTER
                LX_ServiceLineNumber = new LX_HeaderNumber
                {
                    AssignedNumber_01 = "2"
                },

                //  SV2 INSTITUTIONAL SERVICE
                SV2_InstitutionalServiceLine = new SV2_InstitutionalServiceLine
                {
                    ServiceLineRevenueCode_01 = "0730",
                    CompositeMedicalProcedureIdentifier_02 = new C003_CompositeMedicalProcedureIdentifier_8()
                }
            };
            loop24002.SV2_InstitutionalServiceLine.CompositeMedicalProcedureIdentifier_02.ProductorServiceIDQualifier_01 = "HC";
            loop24002.SV2_InstitutionalServiceLine.CompositeMedicalProcedureIdentifier_02.ProcedureCode_02 = "93005";
            loop24002.SV2_InstitutionalServiceLine.LineItemChargeAmount_03 = "76.54";
            loop24002.SV2_InstitutionalServiceLine.UnitorBasisforMeasurementCode_04 = "UN";
            loop24002.SV2_InstitutionalServiceLine.ServiceUnitCount_05 = "3";

            //  DTP DATE - SERVICE DATES
            loop24002.DTP_Date_ServiceDate = new DTP_ClaimLevelServiceDate
            {
                DateTimeQualifier_01 = "472",
                DateTimePeriodFormatQualifier_02 = "D8",
                DateTimePeriod_03 = "19960911"
            };

            //  End 2400 Loop 2
            loop2300.Loop2400.Add(loop24002);

            //  End 2300 Loop
            loop2000B.Loop2300.Add(loop2300);

            //  End 2000B Loop
            loop2000A.Loop2000B.Add(loop2000B);

            //  End 2000A Loop
            result.Loop2000A.Add(loop2000A);

            return result;
        }
    }
}
