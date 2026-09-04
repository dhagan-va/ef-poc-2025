using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using PayerEDI.Data.Helpers;

namespace PayerEDI.Tests.Helpers;

public class EdiMessageXmlExtensionsTests
{
    [Fact]
    public void ToXml_SerializesObjectToXmlStringWithoutDeclaration()
    {
        var item = new TS837P();

        var xml = item.ToXml();

        Assert.NotNull(xml);
        Assert.DoesNotContain("<?xml", xml);
        Assert.Contains("<TS837P", xml);
    }

    [Fact]
    public void ToXml_ReturnsEmptyString_WhenObjectIsNull()
    {
        EdiMessage? item = null;

        var xml = item.ToXml();

        Assert.Equal(string.Empty, xml);
    }
}
