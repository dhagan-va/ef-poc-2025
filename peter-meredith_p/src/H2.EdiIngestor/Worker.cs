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
    private readonly IX12N837Parser _parser;

    public Worker(
        IHostApplicationLifetime appLifetime,
        ILogger<Worker> logger,
        IOptions<EdiTestOptions> options,
        IOptions<Secrets> secrets,
        IServiceScopeFactory serviceScopeFactory,
        IX12N837Parser parser
    )
    {
        ArgumentNullException.ThrowIfNull(appLifetime);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(secrets);
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        ArgumentNullException.ThrowIfNull(parser);

        _appLifetime = appLifetime;
        _logger = logger;
        _options = options.Value;
        _secrets = secrets;
        _serviceScopeFactory = serviceScopeFactory;
        _parser = parser;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EdiIngestorDbContext>();
        try
        {
            await foreach (var claim in _parser.ReadFile(_options.FileName, stoppingToken))
            {
                await dbContext.Claims.AddAsync(claim, stoppingToken);
            }
            await dbContext.SaveChangesAsync(stoppingToken);
        }
        catch (InvalidX12NFileException exception)
        {
            _logger.LogError(
                exception,
                "Invalid X12N file while processing {File}",
                _options.FileName
            );
        }

        _logger.LogInformation("Finished processing file {File}", _options.FileName);
        _logger.LogInformation(
            "Connection string configured: {Configured}",
            !string.IsNullOrEmpty(_secrets.Value.ConnectionString)
        );

        _appLifetime.StopApplication();
    }
}
