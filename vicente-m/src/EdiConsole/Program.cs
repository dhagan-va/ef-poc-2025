using EdiConsole.Configuration;
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

// ── TEMPORARY PROBE: read one file and print what EdiFabric gives us. Replaced in step 4. ──
var samplePath = args.Length > 0
    ? Path.GetFullPath(args[0])
    : throw new ArgumentException("Usage: dotnet run -- <path to .edi file>");
Console.WriteLine($"Reading {samplePath}");

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
            foreach (var a in ts.Loop2000A)
            {
                var bp = a.AllNM1.Loop2010AA.NM1_BillingProviderName;
                Console.WriteLine($"  Billing provider {bp.ResponseContactLastorOrganizationName_03}, NPI {bp.ResponseContactIdentifier_09}");
                foreach (var b in a.Loop2000B)
                {
                    var sub = b.AllNM1.Loop2010BA.NM1_SubscriberName;
                    Console.WriteLine($"    Subscriber {sub.ResponseContactFirstName_04} {sub.ResponseContactLastorOrganizationName_03}, member {sub.ResponseContactIdentifier_09}");
                    foreach (var clm in b.Loop2300 ?? [])
                        Console.WriteLine($"      Claim {clm.CLM_ClaimInformation.PatientControlNumber_01}, total {clm.CLM_ClaimInformation.TotalClaimChargeAmount_02}, lines {clm.Loop2400.Count}");
                }
            }
            break;
        default:
            Console.WriteLine($"{reader.Item?.GetType().Name}");
            break;
    }
}
