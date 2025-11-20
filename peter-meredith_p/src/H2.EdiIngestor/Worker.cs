using System.Diagnostics;

using Microsoft.Extensions.Options;

namespace H2.EdiIngestor;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<Worker> _logger;
    private readonly EdiTestOptions _options;
    private readonly IOptions<Secrets> _secrets;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(
        IHostApplicationLifetime appLifetime,
        ILogger<Worker> logger,
        EdiTestOptions options,
        IOptions<Secrets> secrets,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _appLifetime = appLifetime;
        _logger = logger;
        _options = options;
        _secrets = secrets;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EdiIngestorDbContext>();
        X12N837Parser parser = new X12N837Parser();
        try
        {
            await foreach (var claim in parser.ReadFile(_options.FileName))
            {
                await dbContext.Claims.AddAsync(claim);
            }
            await dbContext.SaveChangesAsync();
        }
        catch (InvalidX12NFileException exception)
        {
            Console.WriteLine(exception.Message);
        }

        _logger.Log(LogLevel.Information, _options.FileName);
        _logger.LogInformation($"Connection string is {_secrets.Value.ConnectionString}");

        _appLifetime.StopApplication();
    }
}
