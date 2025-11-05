using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EFPOC.FileIO;
using System.Reflection;
using System.Text.Json;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");
            var claims = new TS837PTxtReader().Read();
            Console.WriteLine("Console version using reflection (Doesn't unpack all properties yet):");
            foreach (var ediItem in claims)
            {
                PropertyInfo[] objectInfo = ediItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var propInfo in objectInfo)
                    Console.WriteLine($"{propInfo.Name}: \t{propInfo.GetValue(ediItem, null)}");

                Console.WriteLine();
            }
            Console.WriteLine("JSON Version: ");
            Console.WriteLine(JsonSerializer.Serialize(claims, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
