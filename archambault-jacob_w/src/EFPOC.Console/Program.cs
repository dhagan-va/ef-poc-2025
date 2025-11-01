using EdiFabric.Core.Model.Edi;
using EdiFabric.Framework.Readers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var x12File = File.OpenRead(@"../../samples/HIPAA/ClaimPayment.txt");
            EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996"); 

            List<IEdiItem> ediItems;
            using (var reader = new X12Reader(x12File, "EdiFabric.Templates.X12",
            new X12ReaderSettings {ContinueOnError = true }))
            {
                ediItems = reader.ReadToEnd().ToList();
            }
        }
    }
}