using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using System.Reflection;
using System.Text.Json;
using EFPOC.Application.Interfaces;

namespace EFPOC.Application;

public sealed class TS837Parser
{
    ITS837PReader _reader;
    public TS837Parser(ITS837PReader reader)
    {
        _reader = reader;
    }

    public void Parse()
    {
        var claims = _reader.Read();

        foreach (var ediItem in claims)
        {
            PropertyInfo[] objectInfo = ediItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propInfo in objectInfo)
                Console.WriteLine($"{propInfo.Name}: \t{propInfo.GetValue(ediItem, null)}");

            Console.WriteLine();
        }
    }

}
