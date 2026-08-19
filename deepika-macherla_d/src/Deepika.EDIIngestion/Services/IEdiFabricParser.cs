using Deepika.EDIIngestion.Models;

namespace Deepika.EDIIngestion.Services
{
    public interface IEdiFabricParser
    {
        // Parse the given EDI file and return one EdiInterchange per transaction (ST/SE)
        IEnumerable<Deepika.EDIIngestion.Models.EdiInterchange>? ParseFile(string path);
    }
}
