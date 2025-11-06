using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using EFPOC.FileIO;
using EFPOC.Application;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");
            var claims = new TS837PTxtReader().Read();
            var parser = new TS837Parser();
            Console.WriteLine("Console version using reflection (Doesn't unpack all properties yet):");
            parser.Parse(claims);
            Console.WriteLine("JSON Version: ");
            Console.WriteLine(parser.Serialize(claims));
        }
    }
}
