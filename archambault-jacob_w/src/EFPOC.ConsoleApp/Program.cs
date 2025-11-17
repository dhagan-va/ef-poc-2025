using EFPOC.FileIO;
using EFPOC.Application;
using System;
using System.Text.Json;

namespace EFPOC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");
            Console.WriteLine("Console version using reflection (Doesn't unpack all properties yet):");
            new TS837Parser(new TS837PTxtReader()).Parse();
//            Console.WriteLine("JSON Version: ");
//            Console.WriteLine(JsonSerializer.Serialize(claims, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
