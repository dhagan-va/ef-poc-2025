using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
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


        var uniqueId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var key = $"incoming/{uniqueId}/{Path.GetFileName(file)}";

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
        {
            _logger.LogInformation("No EDI files found in path '{Path}'.", _fileInfo.FilePath);
            return;
        }

        var workerCount = _fileInfo.WorkerCount <= 0 ? 3 : _fileInfo.WorkerCount;
        var channelCapacity = _fileInfo.ChannelCapacity <= 0 ? files.Count : _fileInfo.ChannelCapacity;

        var uniqueId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(channelCapacity)
        {
            FullMode = BoundedChannelFullMode.Wait, //Create back pressure when there is no space in the channel,
                                                    //so the producer will wait until there is space available to write more items
            SingleWriter = true, //Only one producer writes to a channel
            SingleReader = false //Mulitple consumers or workers can read from the channel concurrently
        });

        var writer = channel.Writer;
        var reader = channel.Reader;

        //Reads the file from the channel. If the channel does not have any files to read,
        //the worker will wait until there is a file available in the channel to read.
        //Once a file is read from the channel, the worker will upload the file to S3 using the IS3StorageService.
        //This process continues until all files have been read from the channel and uploaded to S3.
        var workers = Enumerable.Range(1, workerCount)
            .Select(workerId => Task.Run(async () =>
            {
                await foreach (var file in reader.ReadAllAsync(cancellationToken))
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var key = $"incoming/{uniqueId}/{Path.GetFileName(file)}";

                        _logger.LogInformation(
                            "Worker {WorkerId} uploading '{File}'...",
                            workerId,
                            file);

                        var response = await _s3StorageService.UploadFileAsync(key, file, cancellationToken);


                        if (response.HttpStatusCode == HttpStatusCode.OK)
                        {
                            _logger.LogInformation(
                                "Worker {WorkerId} completed '{File}'. ETag: {ETag}",
                                workerId,
                                file,
                                response.ETag);
                        }
                        else
                        {
                            _logger.LogWarning(
                                "Worker {WorkerId} upload returned non-OK status for '{File}'. Status: {Status}",
                                workerId,
                                file,
                                response.HttpStatusCode);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogWarning("Upload cancelled for file '{File}'", file);
                        throw; // important for graceful shutdown
                    }
                    catch (AmazonS3Exception ex)
                    {
                        _logger.LogError(ex,
                            "S3 error while uploading '{File}'. Code: {ErrorCode}",
                            file,
                            ex.ErrorCode);

                        // Process failed files in future
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Unexpected error while uploading '{File}'",
                            file);

                        
                    }
                }
            }, cancellationToken))
            .ToArray();

        //Writes the file to the channel. If the channel is full, the producer will wait until there is space available
        //in the channel to write more items.
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