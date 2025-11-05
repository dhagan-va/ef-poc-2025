using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using System.Text.Json;

namespace EFPOC.Application;

public sealed class TS837Parser
{
    public string Serialize(IEnumerable<TS837P> claims)
    {
        return JsonSerializer.Serialize(claims, new JsonSerializerOptions { WriteIndented = true });
    }
}
