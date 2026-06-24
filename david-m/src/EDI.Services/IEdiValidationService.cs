using EdiFabric.Core.Model.Edi;

namespace EDI.Services
{
    public interface IEdiValidationService
    {
        void Validate(IEdiItem transaction);
    }
}
