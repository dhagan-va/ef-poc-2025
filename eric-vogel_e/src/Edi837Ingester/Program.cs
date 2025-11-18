// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

EdiFabric.SerialKey.Set("c417cb9dd9d54297a55c032a74c87996");

var services = new ServiceCollection();
ConfigureServices(services);
var serviceProvider = services.BuildServiceProvider();

var parser = serviceProvider.GetRequiredService<Edi837Ingester.Services.EdiParser>();

void ConfigureServices(ServiceCollection serviceCollection)
{
    // get appsttings from appsettings.json
    serviceCollection.AddOptions();
    serviceCollection.AddLogging();
    serviceCollection.AddDbContext<Edi837Ingester.Data.AppDbContext>(options =>
        options.UseSqlServer("DefaultConnection"));
    serviceCollection.AddTransient<Edi837Ingester.Services.EdiParser>();
}


Console.WriteLine("Hello, World!");