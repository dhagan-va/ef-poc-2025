using System.Globalization;
using EdiConsole.Domain.Claims;
using EdiConsole.Domain.Parties;
using EdiFabric.Templates.Hipaa5010;

namespace EdiConsole.Mapping;

/// <summary>
/// Maps a validated 837P transaction to domain claims.
/// </summary>
/// <remarks>
/// <para>
/// Claims sit under the subscriber (2000B → 2300) when the subscriber is the patient,
/// and under a dependent (2000B → 2000C → 2300) otherwise. Both places are walked.
/// </para>
/// <para>
/// Required loops and segments are read directly: if one is missing, the code fails right there.
/// A missing required value throws <see cref="InvalidDataException"/> naming its X12 location,
/// because a null string would otherwise slip into a required property and only fail when saved.
/// Numbers and dates are parsed with the invariant culture: X12 always uses "." as the decimal mark.
/// </para>
/// </remarks>
public sealed class ClaimMapper : IClaimMapper
{
    public IReadOnlyList<Claim> Map(TS837P transaction)
    {
        var transactionSetControlNumber = transaction.ST.TransactionSetControlNumber_02
            ?? throw new InvalidDataException("ST02 (transaction set control number) is missing.");
        var claims = new List<Claim>();

        foreach (var loop2000A in transaction.Loop2000A)            // billing provider (required)
        {
            var billingProvider = MapBillingProvider(loop2000A);    // once per 2000A, shared by its claims

            foreach (var loop2000B in loop2000A.Loop2000B)          // subscriber (required)
            {
                var subscriber = MapSubscriber(loop2000B);          // once per 2000B, shared by its claims
                var subscriberContext = new ClaimContext(
                    transactionSetControlNumber,
                    billingProvider,
                    subscriber,
                    MapPayer(loop2000B),
                    MapPatient(null, subscriber));                  // the subscriber is the patient

                // The subscriber is the patient: claims sit directly under 2000B (situational).
                foreach (var loop2300 in loop2000B.Loop2300 ?? [])
                    claims.Add(MapClaim(subscriberContext, loop2300));

                // A dependent is the patient: claims sit under 2000C (situational).
                foreach (var loop2000C in loop2000B.Loop2000C ?? [])
                {
                    // Same parties, different patient: a copy of the context with one value replaced.
                    var dependentContext = subscriberContext with { Patient = MapPatient(loop2000C, subscriber) };

                    foreach (var loop2300 in loop2000C.Loop2300)    // required inside 2000C
                        claims.Add(MapClaim(dependentContext, loop2300));
                }
            }
        }

        return claims;
    }

    /// <summary>
    /// What every claim under one subscriber, or one dependent, shares. Each party is mapped once,
    /// at the loop where it lives, and the same immutable instance is reused by all those claims.
    /// </summary>
    private sealed record ClaimContext(
        string TransactionSetControlNumber,                         // ST02
        BillingProvider BillingProvider,                            // 2010AA
        Subscriber Subscriber,                                      // 2000B + 2010BA
        Payer Payer,                                                // 2010BB
        Patient Patient);                                           // 2000C + 2010CA, or the subscriber

    // 2300: CLM + HI + the 2400 service lines
    private static Claim MapClaim(ClaimContext context, Loop_2300_837P loop2300)
    {
        var clm = loop2300.CLM_ClaimInformation;

        var claim = new Claim
        {
            TransactionSetControlNumber = context.TransactionSetControlNumber,
            PatientControlNumber = clm.PatientControlNumber_01
                ?? throw new InvalidDataException("2300 CLM01 (patient control number) is missing."),
            TotalCharge = decimal.Parse(clm.TotalClaimChargeAmount_02, CultureInfo.InvariantCulture),
            PlaceOfServiceCode = clm.HealthCareServiceLocationInformation_05.FacilityTypeCode_01
                ?? throw new InvalidDataException("2300 CLM05-1 (place of service) is missing."),
            FrequencyCode = clm.HealthCareServiceLocationInformation_05.ClaimFrequencyTypeCode_03
                ?? throw new InvalidDataException("2300 CLM05-3 (claim frequency) is missing."),
            BillingProvider = context.BillingProvider,
            Subscriber = context.Subscriber,
            Patient = context.Patient,
            Payer = context.Payer,
        };

        claim.Diagnoses.AddRange(MapDiagnoses(loop2300));
        foreach (var loop2400 in loop2300.Loop2400)                 // service lines (required, 1–50)
            claim.ServiceLines.Add(MapServiceLine(loop2400));

        return claim;
    }

    // 2010AA
    private static BillingProvider MapBillingProvider(Loop_2000A_837P loop2000A)
    {
        var loop2010AA = loop2000A.AllNM1.Loop2010AA;
        var nm1 = loop2010AA.NM1_BillingProviderName;
        var n3 = loop2010AA.N3_BillingProviderAddress;
        var n4 = loop2010AA.N4_BillingProviderCity_State_ZIPCode;

        return new BillingProvider
        {
            Npi = nm1.ResponseContactIdentifier_09
                ?? throw new InvalidDataException("2010AA NM109 (billing provider NPI) is missing."),
            Name = new PartyName
            {
                LastOrOrganizationName = nm1.ResponseContactLastorOrganizationName_03
                    ?? throw new InvalidDataException("2010AA NM103 (billing provider name) is missing."),
                FirstName = nm1.ResponseContactFirstName_04,
                MiddleName = nm1.ResponseContactMiddleName_05,
            },
            TaxId = loop2010AA.AllREF.REF_BillingProviderTaxIdentification.MemberGrouporPolicyNumber_02
                ?? throw new InvalidDataException("2010AA REF02 (billing provider tax ID) is missing."),
            Address = new Address
            {
                Line1 = n3.ResponseContactAddressLine_01,
                Line2 = n3.ResponseContactAddressLine_02,
                City = n4.AdditionalPatientInformationContactCityName_01,
                State = n4.AdditionalPatientInformationContactStateCode_02,
                PostalCode = n4.AdditionalPatientInformationContactPostalZoneorZIPCode_03,
            },
        };
    }

    // 2000B (SBR) + 2010BA
    private static Subscriber MapSubscriber(Loop_2000B_837P loop2000B)
    {
        var sbr = loop2000B.SBR_SubscriberInformation;
        var loop2010BA = loop2000B.AllNM1.Loop2010BA;
        var nm1 = loop2010BA.NM1_SubscriberName;
        var n3 = loop2010BA.N3_SubscriberAddress;                   // situational: sent when the subscriber is the patient
        var n4 = loop2010BA.N4_SubscriberCity_State_ZIPCode;
        var dmg = loop2010BA.DMG_SubscriberDemographicInformation;  // situational, same rule
        var birthDate = dmg?.DependentBirthDate_02;  // misleading naming, but verified right for subscriber birthday

        return new Subscriber
        {
            MemberId = nm1.ResponseContactIdentifier_09
                ?? throw new InvalidDataException("2010BA NM109 (member ID) is missing."),
            Name = new PartyName
            {
                LastOrOrganizationName = nm1.ResponseContactLastorOrganizationName_03
                    ?? throw new InvalidDataException("2010BA NM103 (subscriber name) is missing."),
                FirstName = nm1.ResponseContactFirstName_04,
                MiddleName = nm1.ResponseContactMiddleName_05,
            },
            DateOfBirth = birthDate is null
                ? null
                : DateOnly.ParseExact(birthDate, "yyyyMMdd", CultureInfo.InvariantCulture),
            Gender = dmg?.DependentGenderCode_03,                                           // misleading naming, but verified right for subscriber gender
            PayerResponsibility = sbr.PayerResponsibilitySequenceNumberCode_01
                ?? throw new InvalidDataException("2000B SBR01 (payer responsibility) is missing."),
            GroupNumber = sbr.InsuredGrouporPolicyNumber_03,
            ClaimFilingIndicator = sbr.ClaimFilingIndicatorCode_09,
            Address = new Address
            {
                Line1 = n3?.ResponseContactAddressLine_01,
                Line2 = n3?.ResponseContactAddressLine_02,
                City = n4?.AdditionalPatientInformationContactCityName_01,
                State = n4?.AdditionalPatientInformationContactStateCode_02,
                PostalCode = n4?.AdditionalPatientInformationContactPostalZoneorZIPCode_03,
            },
        };
    }

    // 2010BB
    private static Payer MapPayer(Loop_2000B_837P loop2000B)
    {
        var nm1 = loop2000B.AllNM1.Loop2010BB.NM1_PayerName;

        return new Payer
        {
            PayerId = nm1.ResponseContactIdentifier_09
                ?? throw new InvalidDataException("2010BB NM109 (payer ID) is missing."),
            Name = nm1.ResponseContactLastorOrganizationName_03
                ?? throw new InvalidDataException("2010BB NM103 (payer name) is missing."),
        };
    }

    // 2000C (PAT) + 2010CA. Without a 2000C, the subscriber is the patient.
    private static Patient MapPatient(Loop_2000C_837P? loop2000C, Subscriber subscriber)
    {
        if (loop2000C is null)
        {
            return new Patient
            {
                RelationshipToSubscriber = Patient.SelfRelationshipCode,
                Name = subscriber.Name,
                DateOfBirth = subscriber.DateOfBirth,
                Gender = subscriber.Gender,
            };
        }

        var nm1 = loop2000C.Loop2010CA.NM1_PatientName;
        var dmg = loop2000C.Loop2010CA.DMG_PatientDemographicInformation;
        var birthDate = dmg.DependentBirthDate_02; // misleading naming

        return new Patient
        {
            RelationshipToSubscriber = loop2000C.PAT_PatientInformation.IndividualRelationshipCode_01
                ?? throw new InvalidDataException("2000C PAT01 (relationship to subscriber) is missing."),
            Name = new PartyName
            {
                LastOrOrganizationName = nm1.ResponseContactLastorOrganizationName_03
                    ?? throw new InvalidDataException("2010CA NM103 (patient name) is missing."),
                FirstName = nm1.ResponseContactFirstName_04,
                MiddleName = nm1.ResponseContactMiddleName_05,
            },
            DateOfBirth = birthDate is null
                ? null
                : DateOnly.ParseExact(birthDate, "yyyyMMdd", CultureInfo.InvariantCulture),
            Gender = dmg.DependentGenderCode_03, // misleading naming
        };
    }

    // HI01–HI12. EdiFabric gives position 1 (the principal diagnosis) and positions 2–12 different
    // classes, but all derive from C022, which declares the qualifier and the code.
    private static List<ClaimDiagnosis> MapDiagnoses(Loop_2300_837P loop2300)
    {
        var hi = loop2300.AllHI.HI_HealthCareDiagnosisCode;
        C022?[] positions =
        [
            hi.HealthCareCodeInformation_01, hi.HealthCareCodeInformation_02, hi.HealthCareCodeInformation_03,
            hi.HealthCareCodeInformation_04, hi.HealthCareCodeInformation_05, hi.HealthCareCodeInformation_06,
            hi.HealthCareCodeInformation_07, hi.HealthCareCodeInformation_08, hi.HealthCareCodeInformation_09,
            hi.HealthCareCodeInformation_10, hi.HealthCareCodeInformation_11, hi.HealthCareCodeInformation_12,
        ];

        var diagnoses = new List<ClaimDiagnosis>();
        for (var i = 0; i < positions.Length; i++)
        {
            if (positions[i] is not { } position)
                continue;                                           // position not sent

            diagnoses.Add(new ClaimDiagnosis
            {
                Sequence = (byte)(i + 1),                           // the HI position, which SV107 pointers refer to
                Qualifier = position.CodeListQualifierCode_01
                    ?? throw new InvalidDataException($"2300 HI{i + 1:00}-1 (diagnosis qualifier) is missing."),
                Code = position.IndustryCode_02
                    ?? throw new InvalidDataException($"2300 HI{i + 1:00}-2 (diagnosis code) is missing."),
            });
        }

        return diagnoses;
    }

    // 2400: LX + SV1 + DTP*472
    private static ServiceLine MapServiceLine(Loop_2400_837P loop2400)
    {
        var sv1 = loop2400.SV1_ProfessionalService;
        var procedure = sv1.CompositeMedicalProcedureIdentifier_01;
        var pointers = sv1.CompositeDiagnosisCodePointer_07;
        var serviceDate = loop2400.AllDTP.DTP_Date_ServiceDate;
        var isRange = serviceDate.DateTimePeriodFormatQualifier_02 == "RD8";
        var dates = serviceDate.DateTimePeriod_03.Split('-');      // D8: "20151124"; RD8: "20151124-20151125"

        return new ServiceLine
        {
            LineNumber = int.Parse(loop2400.LX_ServiceLineNumber.AssignedNumber_01, CultureInfo.InvariantCulture),
            ProcedureCode = procedure.ProcedureCode_02
                ?? throw new InvalidDataException("2400 SV101-2 (procedure code) is missing."),
            Modifier1 = procedure.ProcedureModifier_03,
            Modifier2 = procedure.ProcedureModifier_04,
            Modifier3 = procedure.ProcedureModifier_05,
            Modifier4 = procedure.ProcedureModifier_06,
            Charge = decimal.Parse(sv1.LineItemChargeAmount_02, CultureInfo.InvariantCulture),
            UnitBasis = sv1.UnitorBasisforMeasurementCode_03
                ?? throw new InvalidDataException("2400 SV103 (unit basis) is missing."),
            Units = decimal.Parse(sv1.ServiceUnitCount_04, CultureInfo.InvariantCulture),
            PlaceOfServiceCode = sv1.PlaceofServiceCode_05,
            DiagnosisPointer1 = byte.Parse(pointers.DiagnosisCodePointer_01, CultureInfo.InvariantCulture),
            DiagnosisPointer2 = pointers.DiagnosisCodePointer_02 is null
                ? null
                : byte.Parse(pointers.DiagnosisCodePointer_02, CultureInfo.InvariantCulture),
            DiagnosisPointer3 = pointers.DiagnosisCodePointer_03 is null
                ? null
                : byte.Parse(pointers.DiagnosisCodePointer_03, CultureInfo.InvariantCulture),
            DiagnosisPointer4 = pointers.DiagnosisCodePointer_04 is null
                ? null
                : byte.Parse(pointers.DiagnosisCodePointer_04, CultureInfo.InvariantCulture),
            ServiceDateFrom = DateOnly.ParseExact(dates[0], "yyyyMMdd", CultureInfo.InvariantCulture),
            ServiceDateTo = isRange
                ? DateOnly.ParseExact(dates[1], "yyyyMMdd", CultureInfo.InvariantCulture)
                : null,
        };
    }
}
