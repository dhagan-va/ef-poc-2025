using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;
using System.Net.Sockets;
using System.Configuration;

namespace EDI_NCPDP_Ingestion
{
    // ========================================
    // Moto Initializer
    // Automatic setup and teardown of Moto (local AWS mock) for demos
    // Adaptaed from code by Stephen Lantz
    // ========================================
    public class MotoInitializer
    {
        private readonly string _bucketName;
        private readonly string _filePrefix;
        private readonly AmazonS3Client _s3Client;
        private readonly bool _verbose;
        private Process? _motoProcess;  // Track Moto process so we can kill it on cleanup

        

        public MotoInitializer(AmazonS3Client s3Client, string bucketName, string filePrefix, bool verbose = true)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
            _filePrefix = filePrefix;
            _verbose = verbose;
        }

        // Helper method for conditional console output
        private void Log(string message)
        {
            if (_verbose) Console.Write(message);
        }

        private void LogLine(string message)
        {
            if (_verbose) Console.WriteLine(message);
        }

        // Run all initialization steps in sequence
        public async Task InitializeAsync()
        {
            // Step 1: Check if Moto is running, start if not
            await EnsureMotoRunningAsync();

            // Step 2: Clean up any existing bucket/files for fresh demo
            await CleanupExistingResourcesAsync();

            // Step 3: Create bucket if it doesn't exist
            await EnsureBucketExistsAsync();

            // Step 4: Upload sample files to S3
            await EnsureSampleFilesUploadedAsync();
        }

        // ========================================
        // Initialization steps
        // ========================================

        // Start Moto server if not already running
        private async Task EnsureMotoRunningAsync()
        {
            if (_verbose) Log("       -> Checking Moto on port 4566... ");

            if (IsPortOpen("localhost", 4566))
            {
                LogLine("already running");
                return;
            }

            LogLine("not running");
            Log("       -> Starting Moto server... ");

            // Configure process to run moto_server
            var startInfo = new ProcessStartInfo
            {
                FileName = "moto_server",
                Arguments = "-p 4566",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            // Prefer moto_env virtual environment if it exists
            var venvPath = Path.Combine(Directory.GetCurrentDirectory(), "moto_env", "Scripts", "moto_server.exe");

            if (File.Exists(venvPath))
            {
                startInfo.FileName = venvPath;
                startInfo.Arguments = "-p 4566";
            }

            try
            {
                _motoProcess = Process.Start(startInfo);

                // Poll port 4566 until Moto is ready (max 30 seconds)
                var maxWaitTime = TimeSpan.FromSeconds(30);
                var startTime = DateTime.Now;

                while (DateTime.Now - startTime < maxWaitTime)
                {
                    await Task.Delay(500);

                    if (IsPortOpen("localhost", 4566))
                    {
                        LogLine("started");
                        await Task.Delay(1000);  // Brief pause to let Moto stabilize
                        return;
                    }
                }

                throw new Exception("Moto server failed to start within 30 seconds");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to start Moto: {ex.Message}. Make sure moto is installed (run: .\\run-moto.ps1)");
            }
        }

        // Delete existing bucket for clean demo run
        private async Task CleanupExistingResourcesAsync()
        {
            // Silently clean up - no verbose output needed for this internal step
            try
            {
                await _s3Client.GetBucketLocationAsync(_bucketName);
                var listRequest = new ListObjectsV2Request { BucketName = _bucketName };
                var listResponse = await _s3Client.ListObjectsV2Async(listRequest);

                foreach (var obj in listResponse.S3Objects)
                {
                    await _s3Client.DeleteObjectAsync(_bucketName, obj.Key);
                }
                await _s3Client.DeleteBucketAsync(_bucketName);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Bucket doesn't exist - that's fine
            }
        }

        // Create S3 bucket if it doesn't exist
        private async Task EnsureBucketExistsAsync()
        {
            Log($"       -> Creating bucket '{_bucketName}'... ");

            try
            {
                await _s3Client.GetBucketLocationAsync(_bucketName);
                LogLine("exists");
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _s3Client.PutBucketAsync(_bucketName);
                LogLine("created");
            }
        }

        // Upload local sample files to S3 bucket
        private async Task EnsureSampleFilesUploadedAsync()
        {
            // Check what's already in S3
            var listRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = _filePrefix
            };

            var existingFiles = await _s3Client.ListObjectsV2Async(listRequest);
            var existingKeys = new HashSet<string>();

            // Check for existing Keys/Files. If any exist, add them to the HashSet.
            // These files will be used to prevent re-uploading.
            if (existingFiles.S3Objects != null && existingFiles.S3Objects.Count > 0)
            {
                foreach (var file in existingFiles.S3Objects)
                {
                    existingKeys.Add(file.Key);
                }
            }
            else
            {
                Log("\n       No files present in the specified prefix.");
            }

            // Look for sample_*.txt files in local samples folder
            var samplesDir = ConfigurationManager.AppSettings["LocalFileLocation"];

            if (!Directory.Exists(samplesDir))
            {
                if (_verbose) LogLine($"\n       Warning: samples/ directory not found");
                return;
            }

            var localFiles = Directory.GetFiles(samplesDir, "ClaimBilling*");

            if (localFiles.Length == 0)
            {
                if (_verbose) LogLine("\n       Warning: No sample files found");
                return;
            }

            var filesToUpload = localFiles.Where(f => !existingKeys.Contains(Path.GetFileName(f))).ToList();

            if (filesToUpload.Count == 0)
            {
                if (_verbose) LogLine("       -> Sample files already uploaded");
                return;
            }

            if (_verbose) LogLine($"       -> Uploading {filesToUpload.Count} sample file(s)");

            // Upload each file
            foreach (var filePath in filesToUpload)
            {
                var fileName = Path.GetFileName(filePath);
                var s3Key = _filePrefix + fileName;

                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = s3Key,
                    FilePath = filePath
                };

                await _s3Client.PutObjectAsync(putRequest);
            }
        }

        // ========================================
        // Helper methods
        // ========================================

        // Check if a TCP port is listening
        private static bool IsPortOpen(string host, int port)
        {
            try
            {
                using var client = new TcpClient();

                client.Connect(host, port);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            // Don't auto-kill Moto - useful for debugging after app exits
        }

        // ========================================
        // Cleanup (called at end of demo)
        // ========================================

        // Delete S3 bucket and stop Moto server
        public async Task CleanupAsync()
        {
            if (_verbose) Log("       -> Deleting S3 bucket... ");

            try
            {
                // Must delete all objects before deleting bucket
                var listRequest = new ListObjectsV2Request { BucketName = _bucketName };
                var listResponse = await _s3Client.ListObjectsV2Async(listRequest);

                foreach (var obj in listResponse.S3Objects)
                {
                    await _s3Client.DeleteObjectAsync(_bucketName, obj.Key);
                }

                // Delete the bucket
                await _s3Client.DeleteBucketAsync(_bucketName);

                LogLine("deleted");
            }
            catch (Exception ex)
            {
                LogLine($"failed: {ex.Message}");
            }

            // Stop Moto server if we started it (not if it was already running)
            if (_motoProcess != null && !_motoProcess.HasExited)
            {
                Log("       -> Stopping Moto server... ");

                try
                {
                    _motoProcess.Kill(entireProcessTree: true);
                    _motoProcess.WaitForExit(5000);

                    LogLine("stopped");
                }
                catch (Exception ex)
                {
                    LogLine($"failed: {ex.Message}");
                }
            }
        }
    }
}
