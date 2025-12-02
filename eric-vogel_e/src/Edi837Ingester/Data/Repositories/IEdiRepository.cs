using Edi837Ingester.Data.Entities;
using EdiFabric.Core.Model.Edi;

namespace Edi837Ingester.Data.Repositories;

public interface IEdiRepository
{
    Task SaveClaims<T>(List<T> items) where T : EdiMessage;
    Task<List<ProcessedClaim>> GetProcessedClaims(ClaimTypeEnum claimType);
    Task SaveProcessedClaims(IEnumerable<ProcessedClaim> processedClaims);
}