using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace EFPOC.FileIO;

public class TS837PTxtReader
{
    public IEnumerable<TS837P> Read()
    {
        List<IEdiItem> ediItems;
        using (var reader = new X12Reader(File.OpenRead(@"../../samples/HIPAA/ClaimPayment.txt"), "EdiFabric.Templates.Hipaa",
                                          new X12ReaderSettings { ContinueOnError = true }))
        {
            ediItems = reader.ReadToEnd().ToList();
        }
        var claims = ediItems.OfType<TS837P>();
        return claims;
    }
}
