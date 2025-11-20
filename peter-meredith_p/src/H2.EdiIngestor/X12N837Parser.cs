using System.Reflection;

using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

using H2.EdiIngestor.Data;

namespace H2.EdiIngestor;

public sealed class X12N837Parser : IX12N837Parser
{
    private const string DateTimeFormat = "yyyyMMdd";
    public async IAsyncEnumerable<Claim> ReadFile(string fileName)
    {
        await foreach (var flattenedClaimFile in GetFlattenedClaimFiles(fileName))
        {
            yield return MapClaimFile(flattenedClaimFile);
        }
    }

    public static Claim MapClaimFile(FlattenedClaimFile flattenedClaimFile)
    {
        var result = new Claim();
        result.BillingProviderName = GetName(flattenedClaimFile?.Loop2000A?.AllNM1.Loop2010AA.NM1_BillingProviderName);
        result.BillingProviderNpi = flattenedClaimFile?.Loop2000A?.AllNM1.Loop2010AA.NM1_BillingProviderName.ResponseContactIdentifier_09 ?? string.Empty;
        result.RenderingProviderName = result.BillingProviderName;
        result.RenderingProviderNpi = result.BillingProviderNpi;
        result.SubscriberId = flattenedClaimFile?.Loop2000B?.AllNM1.Loop2010BA.NM1_SubscriberName.ResponseContactIdentifier_09 ?? string.Empty;

        if (flattenedClaimFile?.Loop2000C is not null)
            result.PatientName = GetName(flattenedClaimFile?.Loop2000C?.Loop2010CA.NM1_PatientName);
        else
            result.PatientName = GetName(flattenedClaimFile?.Loop2000B?.AllNM1.Loop2010BA.NM1_SubscriberName);

        Process2300Loop(flattenedClaimFile?.Loop2300);

        void Process2300Loop(Loop_2300_837P? loop2300)
        {
            if (loop2300 is null)
                throw new ArgumentNullException("loop2300");
            result.ClaimId = loop2300.CLM_ClaimInformation.PatientControlNumber_01;

            if (loop2300.AllNM1?.Loop2310B is not null)
            {
                result.RenderingProviderName = GetName(loop2300.AllNM1.Loop2310B.NM1_RenderingProviderName);
                result.RenderingProviderNpi = loop2300.AllNM1.Loop2310B.NM1_RenderingProviderName.ResponseContactIdentifier_09;
            }

            var services = new List<Service>();
            foreach (var loop2400 in loop2300.Loop2400)
            {
                var service = new Service();
                service.ProcedureCode = loop2400.SV1_ProfessionalService.CompositeMedicalProcedureIdentifier_01.ProcedureCode_02;
                service.StartDate = GetDate(loop2400.AllDTP.DTP_Date_ServiceDate);
                service.ChargeAmount = decimal.Parse(loop2400.SV1_ProfessionalService.LineItemChargeAmount_02);
                service.Quantity = int.Parse(loop2400.SV1_ProfessionalService.ServiceUnitCount_04);
                service.UnitOrBasisForMeasurementCode = loop2400.SV1_ProfessionalService.UnitorBasisforMeasurementCode_03;
                service.DiagnosisCode1 = GetDiag(1, loop2300.AllHI.HI_HealthCareDiagnosisCode, loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07);
                service.DiagnosisCode2 = GetDiag(2, loop2300.AllHI.HI_HealthCareDiagnosisCode, loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07);
                service.DiagnosisCode3 = GetDiag(3, loop2300.AllHI.HI_HealthCareDiagnosisCode, loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07);
                service.DiagnosisCode4 = GetDiag(4, loop2300.AllHI.HI_HealthCareDiagnosisCode, loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07);
                services.Add(service);
            }
            result.Services = services;
        }

        return result;
    }

    public static string? GetDiag(int requestedPointer, HI_DependentHealthCareDiagnosisCode_2 claimDiagnosisCodes, C004_CompositeDiagnosisCodePointer diagnosisPointers)
    {
        return requestedPointer switch
        {
            1 => GetDiag(diagnosisPointers.DiagnosisCodePointer_01, claimDiagnosisCodes),
            2 => GetDiag(diagnosisPointers.DiagnosisCodePointer_02, claimDiagnosisCodes),
            3 => GetDiag(diagnosisPointers.DiagnosisCodePointer_03, claimDiagnosisCodes),
            4 => GetDiag(diagnosisPointers.DiagnosisCodePointer_04, claimDiagnosisCodes),
            _ => throw new ArgumentOutOfRangeException($"Only four diagnosis codes are supported on a service, but pointer #{requestedPointer} was requested")
        };
    }

    public static string? GetDiag(string diagnosisPointer, HI_DependentHealthCareDiagnosisCode_2 claimDiagnosisCodes)
    {
        return diagnosisPointer switch
        {
            null => null,
            "" => null,
            "1" => claimDiagnosisCodes.HealthCareCodeInformation_01.IndustryCode_02,
            "2" => claimDiagnosisCodes.HealthCareCodeInformation_02.IndustryCode_02,
            "3" => claimDiagnosisCodes.HealthCareCodeInformation_03.IndustryCode_02,
            "4" => claimDiagnosisCodes.HealthCareCodeInformation_04.IndustryCode_02,
            "5" => claimDiagnosisCodes.HealthCareCodeInformation_05.IndustryCode_02,
            "6" => claimDiagnosisCodes.HealthCareCodeInformation_06.IndustryCode_02,
            "7" => claimDiagnosisCodes.HealthCareCodeInformation_07.IndustryCode_02,
            "8" => claimDiagnosisCodes.HealthCareCodeInformation_08.IndustryCode_02,
            "9" => claimDiagnosisCodes.HealthCareCodeInformation_09.IndustryCode_02,
            "10" => claimDiagnosisCodes.HealthCareCodeInformation_10.IndustryCode_02,
            "11" => claimDiagnosisCodes.HealthCareCodeInformation_11.IndustryCode_02,
            "12" => claimDiagnosisCodes.HealthCareCodeInformation_12.IndustryCode_02,
            _ => throw new ArgumentOutOfRangeException("diagnosisPointer")
        };
    }

    public static DateTime GetDate(DTP dtpSegment)
    {
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        return DateTime.ParseExact(dtpSegment.DateTimePeriod_03.Substring(0, 8), DateTimeFormat, culture);
    }

    public static string GetName(NM1? nameSegment)
    {
        if (nameSegment is null)
            return string.Empty;
        if (nameSegment.EntityTypeQualifier_02 == "2")
            return nameSegment.ResponseContactLastorOrganizationName_03;
        return $"{nameSegment.ResponseContactFirstName_04} {nameSegment.ResponseContactLastorOrganizationName_03}";
    }

    public async IAsyncEnumerable<FlattenedClaimFile> GetFlattenedClaimFiles(string fileName)
    {
        using Stream edi = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize: 4096, useAsync: true);
        using var ediReader = new X12Reader(edi, TypeFactory);

        while (await ediReader.ReadAsync())
        {
            var readerErrorContext = ediReader.Item as ReaderErrorContext;
            if (readerErrorContext != null)
                throw new InvalidX12NFileException(
                    fileName,
                    readerErrorContext.MessageErrorContext.Flatten()
                );

            var claimFile = ediReader.Item as TS837P;
            if (claimFile == null)
                continue;

            MessageErrorContext messageErrorContext;
            if (!claimFile.IsValid(out messageErrorContext))
                throw new InvalidX12NFileException(fileName, messageErrorContext.Flatten());

            var claimTemplate = new FlattenedClaimFile();
            claimTemplate.Loop1000A = claimFile.AllNM1.Loop1000A;
            claimTemplate.Loop1000B = claimFile.AllNM1.Loop1000B;
            foreach (var loop2000A in claimFile.Loop2000A)
            {
                claimTemplate.Loop2000A = loop2000A;
                foreach (var loop2000B in loop2000A.Loop2000B)
                {
                    claimTemplate.Loop2000B = loop2000B;

                    if (loop2000B.Loop2000C.IsEmpty())
                    {
                        foreach (var result in Process23000Loop(loop2000B.Loop2300))
                            yield return result;
                    }
                    else
                    {
                        foreach (var loop2000C in loop2000B.Loop2000C)
                        {
                            claimTemplate.Loop2000C = loop2000C;
                            foreach (var result in Process23000Loop(loop2000C.Loop2300))
                                yield return result;
                        }
                    }

                    IEnumerable<FlattenedClaimFile> Process23000Loop(IEnumerable<Loop_2300_837P> loop2300s)
                    {
                        foreach (var loop2300 in loop2300s)
                        {
                            claimTemplate.Loop2300 = loop2300;
                            claimTemplate.Loop2400 = [.. loop2300.Loop2400];
                            yield return claimTemplate.ShallowCopy();
                        }
                    }
                    ;
                }
            }
        }
    }

    public static TypeInfo TypeFactory(ISA isa, GS gs, ST st)
    {
        if (st.TransactionSetIdentifierCode_01 == "837" && st.ImplementationConventionPreference_03 == "005010X222A1")
            return typeof(TS837P).GetTypeInfo();

        throw new Exception("Unsupported transaction.");
    }
}
