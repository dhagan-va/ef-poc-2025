// See https://aka.ms/new-console-template for more information
using EdiConsole.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IGreeter, Greeter>();

using var host = builder.Build();
host.Services.GetRequiredService<IGreeter>().Greet();
