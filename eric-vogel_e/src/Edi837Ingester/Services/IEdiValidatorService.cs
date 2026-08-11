using Edi837Ingester.Data;
using EdiFabric.Core.Model.Edi;

namespace Edi837Ingester.Services
{
    public interface IEdiValidatorService
    {
        Task<IEnumerable<T>> ValidateItems<T>(IEnumerable<T> items, ClaimTypeEnum claimType,
        ValidationLevel validationLevel = ValidationLevel.SyntaxOnly_SNIP1) where T : EdiMessage;
    }
}
