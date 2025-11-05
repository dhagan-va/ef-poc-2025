using EDI837IngestionTask.Models;
using EDI837IngestionTask.Services;


namespace EDI837IngestionTask
{
    internal class Program
    {

        private static readonly Dictionary<string, string> ProcessedFileEtags = new();
        private static IngestionMode Mode = IngestionMode.Local;
        private static readonly List<string> TempFiles = new();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Main Service started.");

            Mode = ParseMode(args);
            Console.WriteLine($"Choose Running Mode is {Mode}");

            // Attempt to set EDI Serial key from .env file
            if (!EnvSetup.SetEdiSerialKey())
            {
                Console.WriteLine("Invalid Serial key. Skip remaining process!!!");
                return;
            }

            EnvSetup.GeneralInitalize();

            if (Mode == IngestionMode.S3)
            {
                // load S3 env and initialize
                EnvSetup.S3Initalize();
            }

            // create cancel signal
            var cts = new CancellationTokenSource();

            // press Ctrl + C to exist
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                Console.WriteLine("Shutdown requested...");
                cts.Cancel();
            };

            try
            {
                if (Mode == IngestionMode.S3)
                {
                    // scan S3 file and process file every 30s
                    while (!cts.Token.IsCancellationRequested)
                    {
                        await RunAsyncProcess();
                        Console.WriteLine($"Waiting {EnvSetup.PollingSeconds}s...");
                        var delayTask = Task.Delay(TimeSpan.FromSeconds(EnvSetup.PollingSeconds), cts.Token);
                        var cancelTask = Task.Run(() =>
                        {
                            while (!cts.Token.IsCancellationRequested)
                                Thread.Sleep(100);
                        });

                        await Task.WhenAny(delayTask, cancelTask);
                    }
                }
                else
                {
                    await RunAsyncProcess();
                    Console.WriteLine("Local processing completed. Existing...");
                }

            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Main Service stopped.");
            }
            finally
            {
                cleanTmp();
                Console.WriteLine("Main Service Existed.");
            }
        }


        /// <summary>
        /// main logic to handle Process files asynchronously
        /// </summary>
        static async Task RunAsyncProcess()
        {
            Console.WriteLine("Starting Ingestion Process...");

            try
            {
                var files = Mode == IngestionMode.S3 ? await S3Reader.ListFilesAsync() : LocalReader.ListFiles();

                if (files.Count == 0)
                {
                    Console.WriteLine("No Files found");
                }
                else
                {
                    var maxConcurrency = EnvSetup.MaxConcurrency;

                    var semaphore = new SemaphoreSlim(maxConcurrency);
                    var tasks = files.Select(async file =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            await ProcessSingleFileAsync(file);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error happens during processing file: {e.Message}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    await Task.WhenAll(tasks);
                }
                Console.WriteLine("Completed Ingestion Process!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }


        /// <summary>
        /// main logic to check whether file is processed, retrieve file, parse, and save into db.
        /// </summary>
        private static async Task ProcessSingleFileAsync(S3FileInfo file)
        {
            try
            {
                if (ProcessedFileEtags.TryGetValue(file.Key, out var oldEtag) && oldEtag == file.ETag)
                {
                    Console.WriteLine($"Skipping already processed file {file.Key} (ETag: {file.ETag})");
                    return;
                }
                Console.WriteLine($"Processing file: {file.Key} (ETag: {file.ETag})");

                string tempPath = Mode == IngestionMode.S3 ? await S3Reader.DownloadFromS3Async(file) : file.Key;

                if (Mode == IngestionMode.S3)
                {
                    TempFiles.Add(tempPath);
                }

                //read and parse file
                var claims = EdiReaderParser.ReadAndParse(tempPath);

                if (claims.Count > 0)
                {
                    //save into db
                    if (ClaimSaver.Save837P(claims))
                    {
                        Console.WriteLine("Store in DB Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Failed to Store in DB");
                    }
                }
                else
                {
                    Console.WriteLine("After Reading and Parse, no Data exists. Skip Save into DB step");
                }

                //update processed list
                ProcessedFileEtags[file.Key] = file.ETag;

                Console.WriteLine($"Completed {file.Key}");
            }
            catch (OperationCanceledException)
            {

                Console.WriteLine($"Cancelled {file.Key}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {file.Key} : {e.Message}");
            }
        }

        /// <summary>
        /// retrieve input mode, default is local
        /// </summary>
        private static IngestionMode ParseMode(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return IngestionMode.Local;
            }
            var modeIndex = Array.FindIndex(args, a => a.Equals("--mode", StringComparison.OrdinalIgnoreCase));
            if (modeIndex >= 0 && modeIndex + 1 < args.Length)
            {
                var value = args[modeIndex + 1].ToLowerInvariant();
                if (value.Contains("local", StringComparison.OrdinalIgnoreCase))
                {
                    return IngestionMode.Local;
                }
                if (value.Contains("s3", StringComparison.OrdinalIgnoreCase))
                {
                    return IngestionMode.S3;
                }
            }

            var joined = string.Join(" ", args).ToLowerInvariant();
            if (joined.Contains("local", StringComparison.OrdinalIgnoreCase))
            {
                return IngestionMode.Local;
            }
            if (joined.Contains("s3", StringComparison.OrdinalIgnoreCase))
            {
                return IngestionMode.S3;
            }
            return IngestionMode.Local;
        }

        /// <summary>
        /// clean temp files which download from S3
        /// </summary>
        private static void cleanTmp()
        {
            if (Mode == IngestionMode.S3)
            {
                Console.WriteLine("Cleaning up temporary files...");
                foreach (var tmpFile in TempFiles)
                {
                    try
                    {
                        if (File.Exists(tmpFile))
                        {
                            File.Delete(tmpFile);
                            Console.WriteLine($"...Deleted: {tmpFile}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Could not delete {tmpFile}: {e.Message}");
                    }
                }

            }
        }

    }

}