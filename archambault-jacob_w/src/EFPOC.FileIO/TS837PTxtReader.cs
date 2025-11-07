using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;

namespace EFPOC.FileIO;

public sealed class TS837PTxtReader
{
    public IEnumerable<TS837P> Read()
    {
        return Read(@"../../samples/HIPAA/ClaimPayment.txt");
    }

    public IEnumerable<TS837P> Read(string filePath)
    {
        List<IEdiItem> ediItems;
        using (var reader = new X12Reader(File.OpenRead(filePath), "EdiFabric.Templates.Hipaa",
                                          new X12ReaderSettings { ContinueOnError = true }))
        {
            ediItems = reader.ReadToEnd().ToList();
        }
        var claims = ediItems.OfType<TS837P>();
        return claims;
    }
    
}
