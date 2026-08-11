using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using H2.EdiIngestor.Data;
using Microsoft.Extensions.Logging;

namespace H2.EdiIngestor;

public sealed class X12N837Parser : IX12N837Parser
{
    private const string DateTimeFormat = "yyyyMMdd";
    private readonly ILogger<X12N837Parser> _logger;
    private readonly IX12NFlattener _x12NFlattener;

    public X12N837Parser(ILogger<X12N837Parser> logger, IX12NFlattener x12NFlattener)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _x12NFlattener = x12NFlattener;
    }

    public async IAsyncEnumerable<Claim> ReadFile(
        string fileName,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("fileName must be provided", nameof(fileName));

        await foreach (
            var flattenedClaimFile in _x12NFlattener.GetFlattenedClaimFiles(
                fileName,
                cancellationToken
            )
        )
        {
            yield return MapClaimFile(flattenedClaimFile);
        }
    }

    public static Claim MapClaimFile(
        FlattenedClaimFile flattenedClaimFile,
        Action<Claim, Loop_2300_837P?>? populate2300 = null
    )
    {
        ArgumentNullException.ThrowIfNull(populate2300);
        var result = new Claim();
        result.BillingProviderName = GetName(
            flattenedClaimFile?.Loop2000A?.AllNM1.Loop2010AA.NM1_BillingProviderName
        );
        result.BillingProviderNpi =
            flattenedClaimFile
                ?.Loop2000A
                ?.AllNM1
                .Loop2010AA
                .NM1_BillingProviderName
                .ResponseContactIdentifier_09 ?? string.Empty;
        result.RenderingProviderName = result.BillingProviderName;
        result.RenderingProviderNpi = result.BillingProviderNpi;
        result.SubscriberId =
            flattenedClaimFile
                ?.Loop2000B
                ?.AllNM1
                .Loop2010BA
                .NM1_SubscriberName
                .ResponseContactIdentifier_09 ?? string.Empty;

        if (flattenedClaimFile?.Loop2000C is not null)
            result.PatientName = GetName(flattenedClaimFile?.Loop2000C?.Loop2010CA.NM1_PatientName);
        else
            result.PatientName = GetName(
                flattenedClaimFile?.Loop2000B?.AllNM1.Loop2010BA.NM1_SubscriberName
            );

        populate2300(result, flattenedClaimFile?.Loop2300);

        return result;
    }

    public static void PopulateClaimWith2300Loop(Claim result, Loop_2300_837P? loop2300)
    {
        ArgumentNullException.ThrowIfNull(loop2300);
        PopulateClaimWith2300Loop(result, loop2300, MapService);
    }

    public static void PopulateClaimWith2300Loop(
        Claim result,
        Loop_2300_837P loop2300,
        Func<Loop_2400_837P, HI_DependentHealthCareDiagnosisCode_2, Service>? mapService = null
    )
    {
        ArgumentNullException.ThrowIfNull(loop2300);
        ArgumentNullException.ThrowIfNull(mapService);

        result.ClaimId = loop2300.CLM_ClaimInformation.PatientControlNumber_01;
        if (loop2300.AllNM1?.Loop2310B is not null)
        {
            result.RenderingProviderName = GetName(
                loop2300.AllNM1.Loop2310B.NM1_RenderingProviderName
            );
            result.RenderingProviderNpi = loop2300
                .AllNM1
                .Loop2310B
                .NM1_RenderingProviderName
                .ResponseContactIdentifier_09;
        }

        var services = new List<Service>();

        foreach (var loop2400 in loop2300.Loop2400)
            mapService(loop2400, loop2300.AllHI.HI_HealthCareDiagnosisCode);
        result.Services = services;
    }

    public static Service MapService(
        Loop_2400_837P loop2400,
        HI_DependentHealthCareDiagnosisCode_2 hiSegment
    )
    {
        var result = new Service();
        result.ProcedureCode = loop2400
            .SV1_ProfessionalService
            .CompositeMedicalProcedureIdentifier_01
            .ProcedureCode_02;
        result.StartDate = GetDate(loop2400.AllDTP.DTP_Date_ServiceDate);

        if (
            !decimal.TryParse(
                loop2400.SV1_ProfessionalService.LineItemChargeAmount_02,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var chargeAmount
            )
        )
        {
            chargeAmount = 0m;
        }
        result.ChargeAmount = chargeAmount;

        if (
            !int.TryParse(
                loop2400.SV1_ProfessionalService.ServiceUnitCount_04,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var quantity
            )
        )
        {
            quantity = 0;
        }
        result.Quantity = quantity;
        result.UnitOrBasisForMeasurementCode = loop2400
            .SV1_ProfessionalService
            .UnitorBasisforMeasurementCode_03;
        result.DiagnosisCode1 = GetDiag(
            1,
            hiSegment,
            loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07
        );
        result.DiagnosisCode2 = GetDiag(
            2,
            hiSegment,
            loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07
        );
        result.DiagnosisCode3 = GetDiag(
            3,
            hiSegment,
            loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07
        );
        result.DiagnosisCode4 = GetDiag(
            4,
            hiSegment,
            loop2400.SV1_ProfessionalService.CompositeDiagnosisCodePointer_07
        );
        return result;
    }

    public static string? GetDiag(
        int requestedPointer,
        HI_DependentHealthCareDiagnosisCode_2 claimDiagnosisCodes,
        C004_CompositeDiagnosisCodePointer diagnosisPointers
    )
    {
        return GetDiag(requestedPointer, claimDiagnosisCodes, diagnosisPointers, GetDiag);
    }

    public static string? GetDiag(
        int requestedPointer,
        HI_DependentHealthCareDiagnosisCode_2 claimDiagnosisCodes,
        C004_CompositeDiagnosisCodePointer diagnosisPointers,
        Func<string, HI_DependentHealthCareDiagnosisCode_2, string?>? getDiag = null
    )
    {
        ArgumentNullException.ThrowIfNull(getDiag);

        return requestedPointer switch
        {
            1 => GetDiag(diagnosisPointers.DiagnosisCodePointer_01, claimDiagnosisCodes),
            2 => GetDiag(diagnosisPointers.DiagnosisCodePointer_02, claimDiagnosisCodes),
            3 => GetDiag(diagnosisPointers.DiagnosisCodePointer_03, claimDiagnosisCodes),
            4 => GetDiag(diagnosisPointers.DiagnosisCodePointer_04, claimDiagnosisCodes),
            _
                => throw new ArgumentOutOfRangeException(
                    $"Only four diagnosis codes are supported on a service, but pointer #{requestedPointer} was requested"
                )
        };
    }

    public static string? GetDiag(
        string diagnosisPointer,
        HI_DependentHealthCareDiagnosisCode_2 claimDiagnosisCodes
    )
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
        return DateTime.ParseExact(
            dtpSegment.DateTimePeriod_03.Substring(0, 8),
            DateTimeFormat,
            culture
        );
    }

    public static string GetName(NM1? nameSegment)
    {
        if (nameSegment is null)
            return string.Empty;
        if (nameSegment.EntityTypeQualifier_02 == "2")
            return nameSegment.ResponseContactLastorOrganizationName_03;
        return $"{nameSegment.ResponseContactFirstName_04} {nameSegment.ResponseContactLastorOrganizationName_03}";
    }
}
