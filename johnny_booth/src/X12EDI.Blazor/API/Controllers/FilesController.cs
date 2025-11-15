using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace X12EDI.Blazor.API.Controllers
{
    [ApiController]
    [Route("api/files/{bucketName?}")]
    public class FilesController : ControllerBase
    {
        #region Private Fields

        private readonly IFileProvider _fileProvider;

        #endregion Private Fields

        #region Public Constructors

        public FilesController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        // Add a HEAD request endpoint for efficiency
        // Clients can use this to check file existence and get size without downloading.
        [HttpHead("{id}")]
        public IActionResult CheckFile(string id)
        {
            IFileInfo file = _fileProvider.GetFileInfo(id);

            if (!file.Exists)
            {
                return NotFound();
            }

            // Set the Content-Length header
            Response.ContentLength = file.Length;

            // Add a last modified date
            Response.Headers.LastModified = file.LastModified.ToString("R");

            // Return OK with headers but no body
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            // 1. Get File Metadata (This remains synchronous/blocking, as IFileProvider.GetFileInfo is synchronous)
            IFileInfo file = _fileProvider.GetFileInfo(id);

            if (!file.Exists)
            {
                // File not found (HTTP 404)
                return NotFound();
            }

            // 2. Open the Stream (This is where the S3FileProvider blocks to get the stream)
            // We use 'using' to ensure the stream is disposed after the FileResult executes.
            Stream fileStream = file.CreateReadStream();

            if (fileStream == null)
            {
                // Defensive check if the stream somehow failed to open
                return NotFound("Could not open file stream.");
            }

            // 3. Optimized File Streaming
            // The File() method returns a FileStreamResult, which internally uses
            // the stream's ReadAsync method to copy the data to the HTTP response
            // asynchronously. This is the most efficient way to serve files in ASP.NET Core.

            // Note: The stream returned by IFileInfo.CreateReadStream() will be automatically
            // disposed of by the FileStreamResult once the transfer is complete.
            return File(
                fileStream,
                "application/edi-x12",
                $"{id}.edi"
            );
        }

        // This is the endpoint that gets hit upon going to ~/api/files/<bucket-name>
        [HttpGet]
        public IActionResult ListFiles(
             string bucketName
            )
        {
            // The bucketName parameter now holds the placeholder value (e.g., 'edibucket')
            // used in the S3 client configuration.

            // 1. Get the list of files from your IFileProvider (filtered by the prefix/delimiter)
            var directoryContents = _fileProvider.GetDirectoryContents(string.Empty);
            var fileKeys = directoryContents.Where(f => !f.IsDirectory).ToList();

            // 2. Construct the required S3 XML response
            var xmlBuilder = new System.Text.StringBuilder();

            // Root element and required namespace
            xmlBuilder.AppendLine(@"<ListBucketResult xmlns=""http://s3.amazonaws.com/doc/2006-03-01/"">");

            // Required header information
            xmlBuilder.AppendLine($@"<Name>{bucketName}</Name>"); // Use the name passed from the client
            xmlBuilder.AppendLine($@"<Prefix>&quot;&quot;</Prefix>");
            xmlBuilder.AppendLine($@"<KeyCount>{fileKeys.Count}</KeyCount>");
            xmlBuilder.AppendLine($@"<MaxKeys>1000</MaxKeys>");
            xmlBuilder.AppendLine($@"<IsTruncated>false</IsTruncated>");

            // File contents (the objects in the bucket)
            foreach (var file in fileKeys)
            {
                xmlBuilder.AppendLine($@"<Contents>");
                xmlBuilder.AppendLine($@"<Key>{file.Name}</Key>");
                // S3 expects the date in ISO 8601 format with Z for UTC
                xmlBuilder.AppendLine($@"<LastModified>{file.LastModified.UtcDateTime:yyyy-MM-ddTHH:mm:ss.fffZ}</LastModified>");
                xmlBuilder.AppendLine($@"<ETag>&quot;{file.Length}&quot;</ETag>");
                xmlBuilder.AppendLine($@"<Size>{file.Length}</Size>");
                xmlBuilder.AppendLine($@"<StorageClass>STANDARD</StorageClass>");
                xmlBuilder.AppendLine($@"</Contents>");
            }

            xmlBuilder.AppendLine(@"</ListBucketResult>");

            // 3. Return the XML content with the correct Content-Type
            return Content(xmlBuilder.ToString(), "application/xml");
        }
        #endregion Public Methods
    }
}