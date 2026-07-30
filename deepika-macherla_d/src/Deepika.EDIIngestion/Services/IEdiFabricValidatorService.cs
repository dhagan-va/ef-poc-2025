using System.Collections.Generic;

namespace Deepika.EDIIngestion.Services
{
    public interface IEdiFabricValidatorService
    {
        bool IsLicensed { get; }
        bool ValidateFile(string path, out List<string> errors);
    }
}
