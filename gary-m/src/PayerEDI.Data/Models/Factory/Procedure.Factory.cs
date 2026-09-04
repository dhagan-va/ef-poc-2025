using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Helpers;

namespace PayerEDI.Data.Models.Factory;

public static class ProcedureFactory
{
    extension(Procedure)
    {
        public static Procedure New(Loop_2400_837P line)
        {
            var service = line.SV1_ProfessionalService!;
            var procedure = service.CompositeMedicalProcedureIdentifier_01;
            var pointer = service.CompositeDiagnosisCodePointer_07;
            var date = line.AllDTP?.DTP_Date_ServiceDate;

            return new Procedure(
                ServiceLineNumber: line.LX_ServiceLineNumber?.AssignedNumber_01.EdiValue(),
                ProcedureIdQualifier: procedure?.ProductorServiceIDQualifier_01.EdiValue(),
                ProcedureCode: procedure?.ProcedureCode_02.EdiValue(),
                ProductServiceId: procedure?.ProductServiceID_08.EdiValue(),
                Modifier1: procedure?.ProcedureModifier_03.EdiValue(),
                Modifier2: procedure?.ProcedureModifier_04.EdiValue(),
                Modifier3: procedure?.ProcedureModifier_05.EdiValue(),
                Modifier4: procedure?.ProcedureModifier_06.EdiValue(),
                Description: procedure?.Description_07.EdiValue(),
                ChargeAmount: service.LineItemChargeAmount_02.EdiValue(),
                UnitOrBasisForMeasurementCode: service.UnitorBasisforMeasurementCode_03.EdiValue(),
                ServiceUnitCount: service.ServiceUnitCount_04.EdiValue(),
                PlaceOfServiceCode: service.PlaceofServiceCode_05.EdiValue(),
                ServiceTypeCode: service.ServiceTypeCode_06.EdiValue(),
                DiagnosisPointer1: pointer?.DiagnosisCodePointer_01.EdiValue(),
                DiagnosisPointer2: pointer?.DiagnosisCodePointer_02.EdiValue(),
                DiagnosisPointer3: pointer?.DiagnosisCodePointer_03.EdiValue(),
                DiagnosisPointer4: pointer?.DiagnosisCodePointer_04.EdiValue(),
                ServiceDateQualifier: date?.DateTimeQualifier_01.EdiValue(),
                ServiceDateFormatQualifier: date?.DateTimePeriodFormatQualifier_02.EdiValue(),
                ServiceDate: date?.DateTimePeriod_03.EdiValue(),
                EmergencyIndicator: service.EmergencyIndicator_09.EdiValue(),
                MultipleProcedureCode: service.MultipleProcedureCode_10.EdiValue(),
                EpsdtIndicator: service.EPSDTIndicator_11.EdiValue(),
                FamilyPlanningIndicator: service.FamilyPlanningIndicator_12.EdiValue(),
                ReviewCode: service.ReviewCode_13.EdiValue(),
                CopayStatusCode: service.CoPayStatusCode_15.EdiValue(),
                ReferenceIdentification: service.ReferenceIdentification_17.EdiValue(),
                MonetaryAmount: service.MonetaryAmount_08.EdiValue(),
                NationalOrLocalAssignedReviewValue: service.NationalorLocalAssignedReviewValue_14.EdiValue(),
                HealthCareProfessionalShortageAreaCode: service.HealthCareProfessionalShortageAreaCode_16.EdiValue(),
                PostalCode: service.PostalCode_18.EdiValue(),
                SecondaryMonetaryAmount: service.MonetaryAmount_19.EdiValue(),
                LevelOfCareCode: service.LevelofCareCode_20.EdiValue(),
                ProviderAgreementCode: service.ProviderAgreementCode_21.EdiValue()
            );
        }

        public static Procedure New(Loop_2400_837D line)
        {
            var service = line.SV3_DentalService!;
            var procedure = service.CompositeMedicalProcedureIdentifier_01;
            var pointer = service.CompositeDiagnosisCodePointer_11;
            var oral = service.OralCavityDesignation_04;
            var date = line.AllDTP?.DTP_Date_ServiceDate;

            return new Procedure(
                ServiceLineNumber: line.LX_ServiceLineNumber?.AssignedNumber_01.EdiValue(),
                ProcedureIdQualifier: procedure?.ProductorServiceIDQualifier_01.EdiValue(),
                ProcedureCode: procedure?.ProcedureCode_02.EdiValue(),
                ProductServiceId: procedure?.ProductServiceID_08.EdiValue(),
                Modifier1: procedure?.ProcedureModifier_03.EdiValue(),
                Modifier2: procedure?.ProcedureModifier_04.EdiValue(),
                Modifier3: procedure?.ProcedureModifier_05.EdiValue(),
                Modifier4: procedure?.ProcedureModifier_06.EdiValue(),
                Description: procedure?.Description_07.EdiValue(),
                ChargeAmount: service.LineItemChargeAmount_02.EdiValue(),
                PlaceOfServiceCode: service.PlaceofServiceCode_03.EdiValue(),
                DiagnosisPointer1: pointer?.DiagnosisCodePointer_01.EdiValue(),
                DiagnosisPointer2: pointer?.DiagnosisCodePointer_02.EdiValue(),
                DiagnosisPointer3: pointer?.DiagnosisCodePointer_03.EdiValue(),
                DiagnosisPointer4: pointer?.DiagnosisCodePointer_04.EdiValue(),
                ServiceDateQualifier: date?.DateTimeQualifier_01.EdiValue(),
                ServiceDateFormatQualifier: date?.DateTimePeriodFormatQualifier_02.EdiValue(),
                ServiceDate: date?.DateTimePeriod_03.EdiValue(),
                CopayStatusCode: service.CopayStatusCode_08.EdiValue(),
                ProviderAgreementCode: service.ProviderAgreementCode_09.EdiValue(),
                OralCavityDesignation1: oral?.OralCavityDesignationCode_01.EdiValue(),
                OralCavityDesignation2: oral?.OralCavityDesignationCode_02.EdiValue(),
                OralCavityDesignation3: oral?.OralCavityDesignationCode_03.EdiValue(),
                OralCavityDesignation4: oral?.OralCavityDesignationCode_04.EdiValue(),
                OralCavityDesignation5: oral?.OralCavityDesignationCode_05.EdiValue(),
                ProsthesisCrownOrInlayCode: service.ProsthesisCrownorInlayCode_05.EdiValue(),
                ProcedureCount: service.ProcedureCount_06.EdiValue(),
                YesNoConditionOrResponseCode: service.YesNoConditionorResponseCode_10.EdiValue()
            );
        }

        public static List<Procedure> New(TS837P claim) =>
            claim
                .Loop2000A.SelectMany(provider => provider.Loop2000B)
                .SelectMany(subscriber =>
                    (subscriber.Loop2300 ?? []).Concat(
                        (subscriber.Loop2000C ?? []).SelectMany(dependent =>
                            dependent.Loop2300 ?? []
                        )
                    )
                )
                .SelectMany(claimLoop => claimLoop.Loop2400)
                .Where(line => line.SV1_ProfessionalService is not null)
                .Select(Procedure.New)
                .ToList();

        public static List<Procedure> New(TS837D claim) =>
            claim
                .Loop2000A.SelectMany(provider => provider.Loop2000B)
                .SelectMany(subscriber =>
                    (subscriber.Loop2300 ?? []).Concat(
                        (subscriber.Loop2000C ?? []).SelectMany(dependent =>
                            dependent.Loop2300 ?? []
                        )
                    )
                )
                .SelectMany(claimLoop => claimLoop.Loop2400)
                .Where(line => line.SV3_DentalService is not null)
                .Select(Procedure.New)
                .ToList();
    }
}
