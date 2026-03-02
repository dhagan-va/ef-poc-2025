using System.Globalization;
using System.Reflection;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using Microsoft.Extensions.Logging;
using X12EDI837Ingestion.Application.Interfaces;
using X12EDI837Ingestion.Domain.Entities;
using X12EDI837Ingestion.Infrastructure.Repositories;

public sealed class X12EDI837IngestionService : IX12EDI837IngestionService
{
    private readonly ILogger<X12EDI837IngestionService> _logger;
    private readonly IX12EDI837IngestRepo _repo;

    public X12EDI837IngestionService(
        ILogger<X12EDI837IngestionService> logger,
        IX12EDI837IngestRepo repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task ProcessIngestionAsync(string filePath, CancellationToken ct = default)
    {
        _logger.LogInformation("ProcessIngestionAsync started. File={File}", filePath);

     
        await using var stream = File.OpenRead(filePath);

        // Load HIPAA templates by assembly name 
        using var reader = new X12Reader(stream, "EdiFabric.Templates.Hipaa5010");

        
        InterchangeHeader? interchange = null;
        FunctionalGroupHeader? currentGs = null;
      
        // Stream read to keep memory stable and preserve proper GS context
        while (reader.Read())
        {
            ct.ThrowIfCancellationRequested();
            var item = reader.Item;
            // Fail fast on parse errors emitted by the reader.
            if (item is ErrorContext err)
                throw new InvalidOperationException($"EDI parse error: {err.Message}");

            
            if (HasErrors(item))
                throw new InvalidOperationException($"EDI parse error: item {item.GetType().Name} contains errors.");

            switch (item)
            {
                case ISA isa:
                    {
                        interchange = MapIsa(isa, filePath);
                        break;
                    }

                case GS gs:
                    {
                        EnsureInterchange(interchange);

                        currentGs = MapGs(gs);
                        interchange!.FunctionalGroups.Add(currentGs);
                        break;
                    }

                case ST st:
                    {
                        // This ST is the generic ST segment. We log it for diagnostics.
                        // ST*837*0021*005010X222A1~
                        _logger.LogInformation("ST01={Id} ST02={Ctrl} ST03={Impl}",
                            st.TransactionSetIdentifierCode_01,
                            st.TransactionSetControlNumber_02,
                            st.ImplementationConventionPreference_03);

                        break;
                    }

                default:
                    {
                        // Typed 837 appears as TS837 or TS837P depending on template.
                        var typeName = item.GetType().Name;
                        if (typeName is "TS837" or "TS837P")
                        {
                            EnsureInterchange(interchange);

                            if (currentGs is null)
                                throw new InvalidOperationException("GS not found. Cannot attach transaction sets.");

                            dynamic tx = item;

                            string? st02 = null;
                            try
                            {
                                st02 = (string?)tx?.ST_TransactionSetHeader?.TransactionSetControlNumber_2;

                                var stHeader = MapTransactionSetFrom837(tx);
                                currentGs.TransactionSets.Add(stHeader);

                                MapClaimsAndLinesFrom837(tx, stHeader);

                                if (stHeader.Claims.Count == 0)
                                {
                                    _logger.LogWarning(
                                        "No claims found while mapping 837 transaction. Type={Type} ST02={Ctrl}. " +
                                        "Template version or input structure may differ.",
                                        typeName, st02);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex,
                                    "Failed mapping 837 transaction. Type={Type} ST02={Ctrl}",
                                    typeName, st02);

                                throw;
                            }
                        }

                        break;
                    }
            }
        }

        EnsureInterchange(interchange);

        // Persist as one unit (repo already has transaction + correct insert order)
        var interchangeId = await _repo.InsertInterchangeAsync(interchange, ct);

        _logger.LogInformation("ProcessIngestionAsync completed. InterchangeId={Id}", interchangeId);
    }

    // ---------------- Mapping helpers ----------------

    private static InterchangeHeader MapIsa(ISA isa, string filePath)
        => new()
        {
            SourceFile = Path.GetFileName(filePath),
            InterchangeControlNumber = isa.InterchangeControlNumber_13,
            SenderId = isa.InterchangeSenderID_6,
            ReceiverId = isa.InterchangeReceiverID_8,
            Date = isa.InterchangeDate_9,
            Time = isa.InterchangeTime_10
        };

    private static FunctionalGroupHeader MapGs(GS gs)
        => new()
        {
            FunctionalIdentifierCode = gs.CodeIdentifyingInformationType_1, // HC
            SenderCode = gs.SenderIDCode_2,
            ReceiverCode = gs.ReceiverIDCode_3,
            Date = gs.Date_4,
            Time = gs.Time_5,
            GroupControlNumber = gs.GroupControlNumber_6, // e.g., 101
            Version = gs.VersionAndRelease_8              // e.g., 005010X222A1
        };

    private static TransactionSetHeader MapTransactionSetFrom837(dynamic ts837)
        => new()
        {
            TransactionSetId =
                ts837.ST_TransactionSetHeader?.TransactionSetIdentifierCode_1, // 837
            TransactionSetControlNumber =
                ts837.ST_TransactionSetHeader?.TransactionSetControlNumber_2,  // 0021
            ImplementationConvention =
                ts837.ST_TransactionSetHeader?.ImplementationConventionReference_3, // 005010X222A1
            BhtReferenceId =
                ts837.BHT_BeginningOfHierarchicalTransaction?.ReferenceIdentification_3 // e.g., 244579
        };

    private static void MapClaimsAndLinesFrom837(dynamic ts837, TransactionSetHeader st)
    {
        // Your sample has claim(s) in Loop2300 and service lines in Loop2400.
        // Structure differs slightly by version, but dynamic + null checks keeps it resilient enough.

        var loop2000A = ts837.Loop2000A;
        if (loop2000A == null) return;

        foreach (var a in loop2000A)
        {
            var loop2000B = a.Loop2000B;
            if (loop2000B == null) continue;

            foreach (var b in loop2000B)
            {
                var loop2000C = b.Loop2000C;
                if (loop2000C == null) continue;

                foreach (var c in loop2000C)
                {
                    var loop2000D = c.Loop2000D;
                    if (loop2000D == null) continue;

                    foreach (var d in loop2000D)
                    {
                        var loop2000E = d.Loop2000E;
                        if (loop2000E == null) continue;

                        foreach (var e in loop2000E)
                        {
                            var loop2300 = e.Loop2300;
                            if (loop2300 == null) continue;

                            foreach (var l2300 in loop2300)
                            {
                                var clm = l2300.CLM_ClaimInformation;
                                if (clm == null) continue;

                                var claim = new Claim
                                {
                                    ClaimSubmitterId = clm.ClaimSubmittersIdentifier_1,
                                    TotalClaimChargeAmount = TryParseDecimal(clm.MonetaryAmount_2),
                                    FacilityCode = clm.FacilityCodeValue_05_1,
                                    ClaimFrequencyCode = clm.ClaimFrequencyCode_05_3
                                };

                                // Service lines: Loop2400 -> SV1
                                var loop2400 = l2300.Loop2400;
                                if (loop2400 != null)
                                {
                                    foreach (var l2400 in loop2400)
                                    {
                                        var sv1 = l2400.SV1_ProfessionalService;
                                        if (sv1 == null) continue;

                                        // Example: SV1*HC:99213*40*UN*1***1~
                                        var proc = sv1.CompositeMedicalProcedureIdentifier_1;
                                        var procCode = proc?.ProcedureCode_2; // 99213

                                        claim.ServiceLines.Add(new ServiceLine
                                        {
                                            ProcedureCode = procCode,
                                            LineItemChargeAmount = TryParseDecimal(sv1.MonetaryAmount_2),
                                            UnitOrBasis = sv1.UnitOrBasisForMeasurementCode_3,
                                            UnitCount = TryParseDecimal(sv1.Quantity_4)
                                        });
                                    }
                                }

                                st.Claims.Add(claim);
                            }
                        }
                    }
                }
            }
        }
    }

    // ✅ Culture-invariant decimal parsing
    private static decimal? TryParseDecimal(string? value)
        => decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var d)
            ? d
            : null;

    private static void EnsureInterchange(InterchangeHeader? interchange)
    {
        if (interchange is null)
            throw new InvalidOperationException("ISA not found; cannot create InterchangeHeader.");
    }

    // ✅ Safely detect HasErrors if the template exposes it (varies by EdiFabric/template versions)
    private static bool HasErrors(object item)
    {
        try
        {
            var prop = item.GetType().GetProperty("HasErrors", BindingFlags.Public | BindingFlags.Instance);
            return prop?.PropertyType == typeof(bool) && (bool)(prop.GetValue(item) ?? false);
        }
        catch
        {
            return false;
        }
    }
}