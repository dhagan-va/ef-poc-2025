namespace PayerEDI.Data.Models;

/// <summary>
/// A health-care service line from an X12 837P professional or 837D dental claim.
/// </summary>
/// <remarks>
/// <para>
/// For 837P, values are mapped from the 2400 loop's SV1 Professional Service
/// segment and its Composite Diagnosis Code Pointer, together with the loop's
/// DTP Date Service segment. For 837D, values are mapped from the 2400 loop's
/// SV3 Dental Service segment, its Composite Diagnosis Code Pointer and Oral
/// Cavity Designation composites, together with the loop's DTP Date Service
/// segment.
/// </para>
/// <para>
/// The model uses strings because EDI values are retained in their parsed
/// representation and are not converted to domain-specific numeric or date
/// types. Fields that are not defined for a transaction type, are omitted from
/// the payload, or are not available in the corresponding EdiFabric segment
/// remain null. The factories also call <c>EdiValue()</c>, so wrapper values are
/// exposed as their EDI text rather than as EdiFabric wrapper objects.
/// </para>
/// </remarks>
/// <param name="ServiceLineNumber">2400 LX01 service line number.</param>
/// <param name="ProcedureIdQualifier">SV1-01 or SV3-01 composite component 1 procedure identifier qualifier.</param>
/// <param name="ProcedureCode">SV1-01 or SV3-01 composite component 2 procedure code.</param>
/// <param name="ProductServiceId">SV1-01 or SV3-01 composite component 8 product/service ID.</param>
/// <param name="Modifier1">SV1-01 or SV3-01 composite component 3 first procedure modifier.</param>
/// <param name="Modifier2">SV1-01 or SV3-01 composite component 4 second procedure modifier.</param>
/// <param name="Modifier3">SV1-01 or SV3-01 composite component 5 third procedure modifier.</param>
/// <param name="Modifier4">SV1-01 or SV3-01 composite component 6 fourth procedure modifier.</param>
/// <param name="Description">SV1-01 or SV3-01 composite component 7 procedure description.</param>
/// <param name="ChargeAmount">SV1-02 or SV3-02 line-item charge amount.</param>
/// <param name="UnitOrBasisForMeasurementCode">837P SV1-03 unit or basis-for-measurement code; not used by SV3.</param>
/// <param name="ServiceUnitCount">837P SV1-04 service unit count; not used by SV3.</param>
/// <param name="PlaceOfServiceCode">SV1-05 or SV3-03 place-of-service code.</param>
/// <param name="ServiceTypeCode">837P SV1-06 service type code; not used by SV3.</param>
/// <param name="DiagnosisPointer1">SV1-07 or SV3-11 composite diagnosis pointer, first value.</param>
/// <param name="DiagnosisPointer2">SV1-07 or SV3-11 composite diagnosis pointer, second value.</param>
/// <param name="DiagnosisPointer3">SV1-07 or SV3-11 composite diagnosis pointer, third value.</param>
/// <param name="DiagnosisPointer4">SV1-07 or SV3-11 composite diagnosis pointer, fourth value.</param>
/// <param name="ServiceDateQualifier">2400 DTP01 date/time qualifier from the Date Service segment.</param>
/// <param name="ServiceDateFormatQualifier">2400 DTP02 date/time period format qualifier.</param>
/// <param name="ServiceDate">2400 DTP03 service date or date range value.</param>
/// <param name="EmergencyIndicator">837P SV1-09 emergency indicator; not used by SV3.</param>
/// <param name="MultipleProcedureCode">837P SV1-10 multiple-procedure code; not used by SV3.</param>
/// <param name="EpsdtIndicator">837P SV1-11 EPSDT indicator; not used by SV3.</param>
/// <param name="FamilyPlanningIndicator">837P SV1-12 family-planning indicator; not used by SV3.</param>
/// <param name="ReviewCode">837P SV1-13 review code; not used by SV3.</param>
/// <param name="CopayStatusCode">SV1-15 or SV3-08 copay status code.</param>
/// <param name="ProviderAgreementCode">SV1-21 or SV3-09 provider agreement code.</param>
/// <param name="YesNoConditionOrResponseCode">837D SV3-10 yes/no condition or response code; not used by SV1.</param>
/// <param name="ReferenceIdentification">837P SV1-17 reference identification; not used by SV3.</param>
/// <param name="MonetaryAmount">837P SV1-08 monetary amount; not used by SV3.</param>
/// <param name="NationalOrLocalAssignedReviewValue">837P SV1-14 national or local assigned review value; not used by SV3.</param>
/// <param name="HealthCareProfessionalShortageAreaCode">837P SV1-16 health-care professional shortage area code; not used by SV3.</param>
/// <param name="PostalCode">837P SV1-18 postal code; not used by SV3.</param>
/// <param name="SecondaryMonetaryAmount">837P SV1-19 secondary monetary amount; not used by SV3.</param>
/// <param name="LevelOfCareCode">837P SV1-20 level-of-care code; not used by SV3.</param>
/// <param name="OralCavityDesignation1">837D SV3-04 oral cavity designation, first value; not used by SV1.</param>
/// <param name="OralCavityDesignation2">837D SV3-04 oral cavity designation, second value; not used by SV1.</param>
/// <param name="OralCavityDesignation3">837D SV3-04 oral cavity designation, third value; not used by SV1.</param>
/// <param name="OralCavityDesignation4">837D SV3-04 oral cavity designation, fourth value; not used by SV1.</param>
/// <param name="OralCavityDesignation5">837D SV3-04 oral cavity designation, fifth value; not used by SV1.</param>
/// <param name="ProsthesisCrownOrInlayCode">837D SV3-05 prosthesis, crown, or inlay code; not used by SV1.</param>
/// <param name="ProcedureCount">837D SV3-06 procedure count; not used by SV1.</param>
public record Procedure(
    string? ServiceLineNumber = null,
    string? ProcedureIdQualifier = null,
    string? ProcedureCode = null,
    string? ProductServiceId = null,
    string? Modifier1 = null,
    string? Modifier2 = null,
    string? Modifier3 = null,
    string? Modifier4 = null,
    string? Description = null,
    string? ChargeAmount = null,
    string? UnitOrBasisForMeasurementCode = null,
    string? ServiceUnitCount = null,
    string? PlaceOfServiceCode = null,
    string? ServiceTypeCode = null,
    string? DiagnosisPointer1 = null,
    string? DiagnosisPointer2 = null,
    string? DiagnosisPointer3 = null,
    string? DiagnosisPointer4 = null,
    string? ServiceDateQualifier = null,
    string? ServiceDateFormatQualifier = null,
    string? ServiceDate = null,
    string? EmergencyIndicator = null,
    string? MultipleProcedureCode = null,
    string? EpsdtIndicator = null,
    string? FamilyPlanningIndicator = null,
    string? ReviewCode = null,
    string? CopayStatusCode = null,
    string? ProviderAgreementCode = null,
    string? YesNoConditionOrResponseCode = null,
    string? ReferenceIdentification = null,
    string? MonetaryAmount = null,
    string? NationalOrLocalAssignedReviewValue = null,
    string? HealthCareProfessionalShortageAreaCode = null,
    string? PostalCode = null,
    string? SecondaryMonetaryAmount = null,
    string? LevelOfCareCode = null,
    string? OralCavityDesignation1 = null,
    string? OralCavityDesignation2 = null,
    string? OralCavityDesignation3 = null,
    string? OralCavityDesignation4 = null,
    string? OralCavityDesignation5 = null,
    string? ProsthesisCrownOrInlayCode = null,
    string? ProcedureCount = null
);
