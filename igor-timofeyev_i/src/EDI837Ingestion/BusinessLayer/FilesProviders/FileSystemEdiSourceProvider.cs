using Microsoft.Extensions.Configuration;

namespace EDI837Ingestion.BusinessLayer.FilesProviders
{
    public class FileSystemEdiSourceProvider : IEdiSourceProvider
    {
        private readonly string _filePath;

        public FileSystemEdiSourceProvider(IConfiguration config)
        {
            //env variable, or some other default fallback path
            _filePath = Environment.GetEnvironmentVariable("Edi837_Path") ?? config["FilePaths:Edi837Path"] ?? "C:\\Projects\\VA\\EDI 837\\igor-timofeyev_i\\samples";
        }
    
        public async IAsyncEnumerable<Stream> GetEdiStreamsAsync()
        {
            Console.WriteLine($"Getting file(s): {_filePath}");

            var payloads = new List<string>();
         
            if (Directory.Exists(_filePath))
            {
                string ediPayload = string.Empty;
                string searchPattern = Environment.GetEnvironmentVariable("FileSearchPattern") ?? "EDI837*";

                //find files to process
                string[] discoveredFiles = Directory.GetFiles(_filePath, searchPattern);

                if (discoveredFiles.Length == 0)
                {
                    Console.Error.WriteLine($"Error: No files found matching '{searchPattern}' inside {_filePath}");

                    yield break;
                }

                Console.WriteLine($"Discovered {discoveredFiles.Length} file(s) for ingestion.");

                //loop thru files and process
                foreach (string file in discoveredFiles)
                {
                    // Keep stream open until the calling ingestion loop finishes parsing it (no using statement)
                    Stream fileStream = File.OpenRead(file);

                    Console.WriteLine($"Successfully opened stream handle for file {file}.");

                    yield return fileStream;
                }
            }
            else
            {
                // Explicitly throw an exception if the folder path string configuration is bad
                throw new DirectoryNotFoundException($"Target ingestion directory does not exist: {_filePath}");
            }
        }          
    }
}
