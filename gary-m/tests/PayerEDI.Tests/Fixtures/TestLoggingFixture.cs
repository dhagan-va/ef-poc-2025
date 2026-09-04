using Microsoft.Extensions.Logging;

namespace PayerEDI.Tests.Fixtures;

public sealed class TestLoggingFixture : IDisposable
{
    private ILoggerFactory LoggerFactory { get; } =
        Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });
        });

    public ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

    public void Dispose()
    {
        LoggerFactory.Dispose();
    }
}
