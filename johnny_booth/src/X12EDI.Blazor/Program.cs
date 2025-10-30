using X12EDI.Blazor.Components;
using X12EDI.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.ConfigureEdiFabric("");

builder.Services.AddEDIServices((options) => 
{
    options.SerialKey = Environment.GetEnvironmentVariable("EDIKEY");
});

builder.Services.AddLogging(config =>
{
    config.AddConsole(); // or AddDebug, AddEventSourceLogger, etc.
    config.SetMinimumLevel(LogLevel.Information);
});


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Verify serial key
var ediOptions = app.Services.GetRequiredService<EdiOptions>();
logger.LogInformation($"EdiFabric serial key {(string.IsNullOrEmpty(ediOptions.SerialKey) ? "not" : "")} set.");



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    logger.LogInformation($"EdiFabric serial key: {ediOptions.SerialKey}");

    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
