using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Channels;
using X12EDI837Ingestion.Producer.Configuration;
using X12EDI837Ingestion.Producer.Interfaces;

namespace X12EDI837Ingestion.Producer.Services;

public sealed class ProducerService : IProducerService
{
    private readonly IS3StorageService _s3StorageService;
    private readonly ILogger<ProducerService> _logger;
    private readonly FileInformation _fileInfo;

    public ProducerService(
        IS3StorageService s3StorageService,
        IOptions<FileInformation> fileOptions,
        ILogger<ProducerService> logger)
    {
        _s3StorageService = s3StorageService;
        _logger = logger;
        _fileInfo = fileOptions.Value;
    }

    public async Task UploadFolderAsync(
        CancellationToken cancellationToken = default)
    {
        var files = RetrieveEdiFiles(_fileInfo.FilePath);
        if (files.Count == 0)
            return;



        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var key = $"incoming/{Path.GetFileName(file)}";

            _logger.LogInformation(
                "Uploading file '{FilePath}' as key '{Key}'...",
                file,
                key);

            await _s3StorageService.UploadFileAsync(key, file, cancellationToken);
        }

        _logger.LogInformation(
            "Uploaded {Count} file(s) from folder '{FolderPath}'.",
            files.Count,
            _fileInfo.FilePath);
    }

    public async Task UploadUsingWorkerPoolAsync(CancellationToken cancellationToken = default)
    {
     
        var files = RetrieveEdiFiles(_fileInfo.FilePath);

        if (files.Count == 0)
            return;

        var workerCount = _fileInfo.WorkerCount <= 0 ? 3 : _fileInfo.WorkerCount;
        var channelCapacity = _fileInfo.ChannelCapacity <= 0 ? files.Count : _fileInfo.ChannelCapacity;

        var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(channelCapacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = true,
            SingleReader = false
        });

        var writer = channel.Writer;
        var reader = channel.Reader;

        var workers = Enumerable.Range(1, workerCount)
            .Select(workerId => Task.Run(async () =>
            {
                await foreach (var file in reader.ReadAllAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var key = $"incoming/{Path.GetFileName(file)}";

                    _logger.LogInformation(
                        "Worker {WorkerId} uploading '{File}'...",
                        workerId,
                        file);

                    await _s3StorageService.UploadFileAsync(key, file, cancellationToken);

                    _logger.LogInformation(
                        "Worker {WorkerId} completed '{File}'.",
                        workerId,
                        file);
                }
            }, cancellationToken))
            .ToArray();

        foreach (var file in files)
        {
            await writer.WriteAsync(file, cancellationToken);
        }

        writer.Complete();

        await Task.WhenAll(workers);

        _logger.LogInformation(
            "Parallel upload completed for {Count} file(s).",
            files.Count);
    }

    private List<string> RetrieveEdiFiles(string dirPath)
    {
        if (string.IsNullOrWhiteSpace(dirPath))
            throw new InvalidOperationException("FileInformation:FilePath is missing.");

        if (!Directory.Exists(dirPath))
            throw new DirectoryNotFoundException($"Directory not found: {dirPath}");

        var files = Directory
            .EnumerateFiles(dirPath, "*", SearchOption.TopDirectoryOnly)
            .Where(file => string.Equals(Path.GetExtension(file), ".edi", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (files.Count == 0)
            _logger.LogWarning("No EDI files were found in folder: {DirectoryPath}", dirPath);

        return files;
    }
}