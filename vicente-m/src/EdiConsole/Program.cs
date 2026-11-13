using EdiConsole.Configuration;
using EdiConsole.Mapping;
using EdiConsole.Services;
using EdiFabric;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

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

// ── TEMPORARY PROBE: read one file, map its claims with ClaimMapper and print them.
var samplePath = args.Length > 0
    ? Path.GetFullPath(args[0])
    : throw new ArgumentException("Usage: dotnet run -- <path to .edi file>");
Console.WriteLine($"Reading {samplePath}");

var mapper = new ClaimMapper();
await using var stream = File.OpenRead(samplePath);
using var reader = new X12Reader(stream, "EdiFabric.Templates.Hipaa", new X12ReaderSettings());

while (await reader.ReadAsync())
{
    switch (reader.Item)
    {
        case ISA isa:
            Console.WriteLine($"ISA  interchange {isa.InterchangeControlNumber_13}, usage {isa.UsageIndicator_15}");
            break;
        case GS gs:
            Console.WriteLine($"GS   group {gs.GroupControlNumber_6}, guide {gs.VersionAndRelease_8}");
            break;
        case TS837P ts:
            Console.WriteLine($"ST   837P transaction {ts.ST.TransactionSetControlNumber_02}");
            foreach (var claim in mapper.Map(ts))
            {
                Console.WriteLine($"  Claim {claim.PatientControlNumber}: {claim.TotalCharge:0.00}, POS {claim.PlaceOfServiceCode}, frequency {claim.FrequencyCode}");
                Console.WriteLine($"    Billing provider {claim.BillingProvider.Name.LastOrOrganizationName}, NPI {claim.BillingProvider.Npi}, ZIP {claim.BillingProvider.Address.PostalCode}");
                Console.WriteLine($"    Subscriber       {claim.Subscriber.Name.FirstName} {claim.Subscriber.Name.LastOrOrganizationName}, member {claim.Subscriber.MemberId}, born {claim.Subscriber.DateOfBirth:yyyy-MM-dd}");
                Console.WriteLine($"    Patient          {claim.Patient.Name.FirstName} {claim.Patient.Name.LastOrOrganizationName}, relationship {claim.Patient.RelationshipToSubscriber}, born {claim.Patient.DateOfBirth:yyyy-MM-dd}");
                Console.WriteLine($"    Payer            {claim.Payer.Name} ({claim.Payer.PayerId})");
                foreach (var diagnosis in claim.Diagnoses)
                    Console.WriteLine($"    Diagnosis {diagnosis.Sequence}: {diagnosis.Qualifier} {diagnosis.Code}");
                foreach (var line in claim.ServiceLines)
                    Console.WriteLine($"    Line {line.LineNumber}: {line.ProcedureCode} {line.Charge:0.00} ({line.Units} {line.UnitBasis}), diagnosis {line.DiagnosisPointer1}, on {line.ServiceDateFrom:yyyy-MM-dd}");
            }
            break;
        default:
            Console.WriteLine($"{reader.Item?.GetType().Name}");
            break;
    }
}
