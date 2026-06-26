using Edi837Ingestion.Configuration;

var configuration = AppConfiguration.Build();
var ediFabric = configuration.GetEdiFabricOptions();

Console.WriteLine("EdiFabric serial key loaded.");
