using X12EDI.Blazor.Components;
using X12EDI.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.ConfigureEdiFabric("");

builder.Services.AddEDIServices((options) => 
{
    options.SerialKey = "serial-key";
});

builder.Services.AddLogging(config =>
{
    config.AddConsole(); // or AddDebug, AddEventSourceLogger, etc.
    config.SetMinimumLevel(LogLevel.Information);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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
