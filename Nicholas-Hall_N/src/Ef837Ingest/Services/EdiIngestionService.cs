using Edi837Ingestion.Data;
using Edi837Ingestion.Domain.Entities;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Ef837Ingest.Data;
using System.Reflection;

namespace Edi837Ingestion.Edi;

public class EdiIngestionService(IngestionDbContext db)
{
    private readonly IngestionDbContext _db = db;

    public async Task<int> Ingest837Async(Stream ediStream, CancellationToken ct = default)
    {
        // Resolve 837P template when ST01 == "837"
        using var reader = new X12Reader(
            ediStream,
            (isa, gs, st) =>
            {
                if (st.TransactionSetIdentifierCode_01 == "837")
                    return typeof(TS837P).GetTypeInfo(); 
                return null;
            },
            new X12ReaderSettings { Split = true });

        Interchange? currentInterchange = null;
        FunctionalGroup? currentGroup = null;
        var ingestedTransactions = 0;

        while (reader.Read())
        {
            ct.ThrowIfCancellationRequested();

            switch (reader.Item)
            {
                // ISA envelope
                case ISA isa:
                    currentInterchange = new Interchange
                    {
                        Isa01 = isa.AuthorizationInformationQualifier_1,
                        Isa02 = isa.AuthorizationInformation_2,
                        Isa03 = isa.SecurityInformationQualifier_3,
                        Isa04 = isa.SecurityInformation_4,
                        Isa05 = isa.SenderIDQualifier_5,    
                        Isa06 = isa.InterchangeSenderID_6,
                        Isa07 = isa.ReceiverIDQualifier_7,   
                        Isa08 = isa.InterchangeReceiverID_8,
                        Isa09Date = isa.InterchangeDate_9,
                        Isa10Time = isa.InterchangeTime_10,
                        ControlNumber = isa.InterchangeControlNumber_13
                    };
                    _db.Interchanges.Add(currentInterchange);
                    break;

                case GS gs when currentInterchange is not null:
                    currentGroup = new FunctionalGroup
                    {
                        Interchange = currentInterchange,
                        Gs01 = gs.CodeIdentifyingInformationType_1,
                        Gs02 = gs.SenderIDCode_2,
                        Gs03 = gs.ReceiverIDCode_3,
                        Gs04Date = gs.Date_4,
                        Gs05Time = gs.Time_5,
                        ControlNumber = gs.GroupControlNumber_6,
                        TransactionTypeCode = gs.TransactionTypeCode_7,
                        VersionAndRelease = gs.VersionAndRelease_8
                    };
                    _db.FunctionalGroups.Add(currentGroup);
                    break;

                case TS837P p when currentGroup is not null:
                    ingestedTransactions += await Persist837PAsync(currentGroup, p, ct);
                    break;

            }
        }

        await _db.SaveChangesAsync(ct);
        return ingestedTransactions;
    }

    private Task<int> Persist837PAsync(FunctionalGroup group, TS837P p, CancellationToken ct)
    {
        var tx = new TransactionSet
        {
            FunctionalGroup = group,
            St01 = p.ST.TransactionSetIdentifierCode_01,
            ControlNumber = p.ST.TransactionSetControlNumber_02,
            Bht02 = p.BHT_BeginningOfHierarchicalTransaction.TransactionSetPurposeCode_02,
            Bht06 = p.BHT_BeginningOfHierarchicalTransaction.TransactionTypeCode_06,
            RawJson = System.Text.Json.JsonSerializer.Serialize(p)
        };
        _db.TransactionSets.Add(tx);

        var first2300 = p.Loop2000A?
            .FirstOrDefault()?
            .Loop2000B?
            .FirstOrDefault()?
            .Loop2300?
            .FirstOrDefault();

        decimal? totalAmt = null;
        if (decimal.TryParse(first2300?.CLM_ClaimInformation?.TotalClaimChargeAmount_02, out var parsed))
            totalAmt = parsed;

        var claim = new ClaimStub
        {
            TransactionSet = tx,
            PatientControlNumber = first2300?.CLM_ClaimInformation?.PatientControlNumber_01 ?? string.Empty,
            TotalClaimCharge = totalAmt
        };
        _db.ClaimStubs.Add(claim);

        return Task.FromResult(1);
    }
}
