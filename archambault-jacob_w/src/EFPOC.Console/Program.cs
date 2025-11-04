using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996"); 

            List<IEdiItem> ediItems;
            using (var reader = new X12Reader(File.OpenRead(@"../../samples/HIPAA/ClaimPayment.txt"), "EdiFabric.Templates.Hipaa",
            new X12ReaderSettings {ContinueOnError = true }))
            {
                ediItems = reader.ReadToEnd().ToList();
            }
            var claims = ediItems.OfType<TS837P>();
            Console.WriteLine($"Number of Claims: {ediItems.Count}");
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