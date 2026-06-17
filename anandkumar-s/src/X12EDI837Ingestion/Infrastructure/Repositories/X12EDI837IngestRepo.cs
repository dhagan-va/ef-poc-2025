using EdiFabric.Core.Model.Edi.X12;
using Microsoft.EntityFrameworkCore;
using X12EDI837Ingestion.Domain;
using X12EDI837Ingestion.Domain.Entities;

namespace X12EDI837Ingestion.Infrastructure.Repositories;

public sealed class X12EDI837IngestRepo : IX12EDI837IngestRepo
{
    private readonly AppDbContext _db;

    public X12EDI837IngestRepo(AppDbContext db)
    {
        _db = db;
    }

    public async Task<long> InsertInterchangeAsync(InterchangeHeader interchange, CancellationToken ct = default)
    {
        if (interchange is null) throw new ArgumentNullException(nameof(interchange));

        // Hold the child graph aside so EF doesn't insert everything in step 1.
        var gsList = interchange.FunctionalGroups?.ToList() ?? new List<FunctionalGroupHeader>();
        interchange.FunctionalGroups?.Clear();

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            // 1) Insert ISA only
            _db.Interchanges.Add(interchange);
            await _db.SaveChangesAsync(ct);

            // 2) Insert GS only (no ST yet)
            foreach (var gs in gsList)
            {
                // Hold ST aside so EF doesn't insert them during GS insert
                var stList = gs.TransactionSets?.ToList() ?? new List<TransactionSetHeader>();
                gs.TransactionSets?.Clear();

                gs.InterchangeHeaderId = interchange.Id;
                _db.FunctionalGroups.Add(gs);
                await _db.SaveChangesAsync(ct);

                // 3) Insert ST only (no claims yet)
                foreach (var st in stList)
                {
                    var parties = st.Parties?.ToList() ?? new List<Party>();
                    var claims = st.Claims?.ToList() ?? new List<Claim>();
                    st.Parties?.Clear();
                    st.Claims?.Clear();

                    st.FunctionalGroupHeaderId = gs.Id;
                    _db.TransactionSets.Add(st);
                    await _db.SaveChangesAsync(ct);

                    // 4) Parties + Claims
                    foreach (var party in parties)
                    {
                        party.TransactionSetHeaderId = st.Id;
                        _db.Parties.Add(party);
                    }

                    foreach (var claim in claims)
                    {
                        var lines = claim.ServiceLines?.ToList() ?? new List<ServiceLine>();
                        claim.ServiceLines?.Clear();

                        claim.TransactionSetHeaderId = st.Id;
                        _db.Claims.Add(claim);
                        await _db.SaveChangesAsync(ct);

                        // 5) Service lines
                        foreach (var line in lines)
                        {
                            line.ClaimId = claim.Id;
                            _db.ServiceLines.Add(line);
                        }
                    }

                    await _db.SaveChangesAsync(ct);
                }
            }

            await tx.CommitAsync(ct);
            return interchange.Id;
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}