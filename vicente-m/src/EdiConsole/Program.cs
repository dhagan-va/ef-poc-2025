// See https://aka.ms/new-console-template for more information
using EdiConsole.Configuration;
using EdiConsole.Services;
using EdiFabric;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

//Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
//Console.WriteLine($"EdiDb: {builder.Configuration.GetConnectionString("EdiDb")}");

builder.Services.AddOptions<EdiFabricOptions>()
    .BindConfiguration(EdiFabricOptions.SectionName)
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.SerialKey),
        "EdiFabric:SerialKey is missing. For local development run: dotnet user-secrets set \"EdiFabric:SerialKey\" \"<your key>\"");


builder.Services.AddSingleton<IGreeter, Greeter>();

using var host = builder.Build();

// EdiFabric's license is process-wide (static), so set it once, before any EDI is read or written.
var ediFabricOptions = host.Services.GetRequiredService<IOptions<EdiFabricOptions>>().Value;
License.SetSerial(ediFabricOptions.SerialKey);

host.Services.GetRequiredService<IGreeter>().Greet();
