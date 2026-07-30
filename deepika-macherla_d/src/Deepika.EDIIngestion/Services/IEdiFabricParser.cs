using Deepika.EDIIngestion.Models;

namespace Deepika.EDIIngestion.Services
{
    public interface IEdiFabricParser
    {
        EdiInterchange? ParseFile(string path);
    }
}
