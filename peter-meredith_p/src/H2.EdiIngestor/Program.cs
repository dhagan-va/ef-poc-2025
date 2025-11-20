using H2.EdiIngestor;

using Microsoft.EntityFrameworkCore;

if (args.Length == 0 || string.IsNullOrEmpty(args[0]) || args[0].ToLower() == "--help")
{
    Console.WriteLine("dotnet run <837 filename>");
}

EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

builder.Services.Configure<Secrets>(builder.Configuration.GetSection(nameof(Secrets)));

builder.Services.AddDbContext<EdiIngestorDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EdiIngestor"));
});

// builder.Services.AddDbContext<EdiFabric.Templates.Hipaa5010.>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("EdiTest"));
// });

builder.Services.AddSingleton(new EdiTestOptions(args)).AddHostedService<Worker>();

var host = builder.Build();
host.Run();
