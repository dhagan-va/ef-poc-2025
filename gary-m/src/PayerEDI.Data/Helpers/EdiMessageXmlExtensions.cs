using System.Xml;
using System.Xml.Serialization;
using EdiFabric.Core.Model.Edi;

namespace PayerEDI.Data.Helpers;

public static class EdiMessageXmlExtensions
{
    private static readonly XmlWriterSettings DefaultSettings = new()
    {
        Indent = true,
        OmitXmlDeclaration = true,
    };

    public static string ToXml(this EdiMessage? value, XmlWriterSettings? settings = null)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var serializer = new XmlSerializer(value.GetType());
        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter, settings ?? DefaultSettings);
        serializer.Serialize(xmlWriter, value);

        return stringWriter.ToString();
    }
}
