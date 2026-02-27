
using Microsoft.EntityFrameworkCore;
using X12EDI837Ingestion.Domain.Entities;
using X12EDI837Ingestion.Domain;
using EdiFabric.Core.Model.Edi.Edifact;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Templates.X12004010;
using Microsoft.Extensions.Options;
using static X12EDI837Ingestion.Infrastructure.Repositories.X12EDI837IngestRepo;
using System.Security.Claims;
using System.Security.Principal;

namespace X12EDI837Ingestion.Infrastructure.Repositories;

public sealed class X12EDI837IngestRepo : IX12EDI837IngestRepo
{
    private readonly AppDbContext _db;

    public X12EDI837IngestRepo(AppDbContext db)
    {
        _db = db;
    }

    public sealed class X12IngestionRepository : IX12IngestionRepository
    {
        private readonly AppDbContext _db;

        public X12IngestionRepository(AppDbContext db) => _db = db;

        public async Task<long> InsertInterchangeAsync(
            InterchangeHeader interchange,
            CancellationToken ct = default)
        {
            if (interchange is null) throw new ArgumentNullException(nameof(interchange));

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                // 1) ISA
                _db.Interchanges.Add(interchange);
                await _db.SaveChangesAsync(ct); // generates interchange.Id

                // 2) GS (children of ISA)
                foreach (var gs in interchange.FunctionalGroups)
                {
                    gs.InterchangeHeaderId = interchange.Id;
                    _db.FunctionalGroups.Add(gs);
                }
                await _db.SaveChangesAsync(ct); // generates gs.Id values

                // 3) ST (children of GS)
                foreach (var gs in interchange.FunctionalGroups)
                {
                    foreach (var st in gs.TransactionSets)
                    {
                        st.FunctionalGroupHeaderId = gs.Id;
                        _db.TransactionSets.Add(st);
                    }
                }
                await _db.SaveChangesAsync(ct); // generates st.Id values

                // 4) Parties + Claims (children of ST)
                foreach (var gs in interchange.FunctionalGroups)
                {
                    foreach (var st in gs.TransactionSets)
                    {
                        foreach (var party in st.Parties)
                        {
                            party.TransactionSetHeaderId = st.Id;
                            _db.Parties.Add(party);
                        }

                        foreach (var claim in st.Claims)
                        {
                            claim.TransactionSetHeaderId = st.Id;
                            _db.Claims.Add(claim);
                        }
                    }
                }
                await _db.SaveChangesAsync(ct); // generates party.Id and claim.Id values

                // 5) ServiceLines (children of Claim)
                foreach (var gs in interchange.FunctionalGroups)
                {
                    foreach (var st in gs.TransactionSets)
                    {
                        foreach (var claim in st.Claims)
                        {
                            foreach (var line in claim.ServiceLines)
                            {
                                line.ClaimId = claim.Id;
                                _db.ServiceLines.Add(line);
                            }
                        }
                    }
                }
                await _db.SaveChangesAsync(ct);

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

}
