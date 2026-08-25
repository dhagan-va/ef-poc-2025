using EdiFabric.Templates.Hipaa5010;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle
{
    public interface ILoop
    {
        // 1000A
        // NM1_InformationReceiverName_4 NM1_SubmitterName
        // List<PER_BillingProviderContactInformation> PER_SubmitterEDIContactInformation

        // 1000B
        // NM1_ReceiverName

        // 2000A
        // HL_BillingProviderHierarchicalLevel
        // PRV_BillingProviderSpecialtyInformation
        // CUR_ForeignCurrencyInformation_3
        // All_NM1_837I
        // List<Loop_2000B_837I>

        //// 2010AA
        //// NM1_BillingProviderName_3
        //// N3_AdditionalPatientInformationContactAddress
        //// N4_AdditionalPatientInformationContactCity
        //// REF_BillingProviderTaxIdentification_2
        //// List<PER_BillingProviderContactInformation>

        // 2000B
        // HL_SubscriberHierarchicalLevel
        // SBR_SubscriberInformation_2
        // All_NM1_837I_2
        // List<Loop_2300_837I>
        // List<Loop_2000C_837I>

        //// 2010BA
        //// NM1_SubscriberName_5
        //// N3_AdditionalPatientInformationContactAddress
        //// N4_AdditionalPatientInformationContactCity
        //// DMG_PatientDemographicInformation
        //// All_REF_837I_2

        //// 2010BB
        //// NM1_OtherPayerName
        //// N3_AdditionalPatientInformationContactAddress
        //// N4_AdditionalPatientInformationContactCity
        //// All_REF_837I_3

        //// 2300
        //// Seems to have validation??
        //// AMT_PatientEstimatedAmountDue AMT_PatientEstimatedAmountDue
        //// All_NM1_837I_3 AllNM1
        //// HCP_ClaimPricing_2 HCP_ClaimPricing_RepricingInformation
        //// All_HI_837I AllHI
        //// CRC_EPSDTReferral CRC_EPSDTReferral
        //// All_NTE_837I AllNTE
        //// List<K3_FileInformation>
        //// All_REF_837I_4 AllREF
        //// List<Loop_2400_837I>
        //// CN1_ContractInformation_2 CN1_ContractInformation
        //// List<PWK_ClaimSupplementalInformation_2>
        //// CL1_InstitutionalClaimCode CL1_InstitutionalClaimCode
        //// All_DTP_837I AllDTP
        //// CLM_ClaimInformation_2 CLM_ClaimInformation

        ////// 2310A
        ////// NM1_AttendingProviderName
        ////// PRV_AttendingProviderSpecialtyInformation
        ////// List<REF_AssistantSurgeonSecondaryIdentification>

        ////// 2320
        ////// SBR_OtherSubscriberInformation_2 SBR_OtherSubscriberInformation
        ////// List<CAS_ClaimLevelAdjustments> CAS_ClaimLevelAdjustments
        ////// All_AMT_837I_2 AllAMT
        ////// OI_OtherInsuranceCoverageInformation_2 OI_OtherInsuranceCoverageInformation
        ////// MIA_InpatientAdjudicationInformation MIA_InpatientAdjudicationInformation
        ////// MOA_OutpatientAdjudicationInformation MOA_OutpatientAdjudicationInformation
        ////// All_NM1_837I_4 AllNM1

        ////// 2400
        ////// LX_HeaderNumber LX_ServiceLineNumber
        ////// SV2_InstitutionalServiceLine SV2_InstitutionalServiceLine
        ////// List<PWK_ClaimSupplementalInformation_2> PWK_LineSupplementalInformation
        ////// DTP_ClaimLevelServiceDate DTP_Date_ServiceDate
        ////// All_REF_837I AllREF
        ////// All_AMT_837I AllAMT
        ////// NTE_ThirdPartyOrganizationNotes NTE_ThirdPartyOrganizationNotes
        ////// HCP_LinePricing_2 HCP_LinePricing_RepricingInformation
        ////// Loop_2410_837I Loop2410
        ////// All_NM1_837I_5 AllNM1
        ////// List<Loop_2430_837I> Loop2430

    }
}
