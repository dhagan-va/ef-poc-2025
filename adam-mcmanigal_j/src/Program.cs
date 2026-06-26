using Edi837Ingestion.Configuration;

var configuration = AppConfiguration.Build();
var ediFabric = configuration.GetEdiFabricOptions();

// Apply the EdiFabric license once at startup, before any parsing.
EdiFabricLicense.Apply(ediFabric);

Console.WriteLine("EdiFabric license applied.");
