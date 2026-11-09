using Microsoft.Extensions.Logging;

namespace EdiConsole.Services;

public interface IGreeter
{
    void Greet();
}

public sealed class Greeter(ILogger<Greeter> logger) : IGreeter
{
    public void Greet() => logger.LogInformation("Hello from dependency injection.");
}
