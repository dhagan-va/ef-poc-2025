using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using System.Reflection;
using System.Text.Json;

namespace EFPOC.Application;

public sealed class TS837Parser
{
    private JsonSerializerOptions _jsonSerializerOptions;

    public TS837Parser(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public TS837Parser() : this(new JsonSerializerOptions { WriteIndented = true }) {}

    public  string Serialize(IEnumerable<TS837P> claims)
    {
        return JsonSerializer.Serialize(claims, _jsonSerializerOptions);
    }
    public void Parse(IEnumerable<TS837P> claims)
    {
        foreach (var ediItem in claims)
        {
            PropertyInfo[] objectInfo = ediItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propInfo in objectInfo)
                Console.WriteLine($"{propInfo.Name}: \t{propInfo.GetValue(ediItem, null)}");

            Console.WriteLine();
        }
    }
}
