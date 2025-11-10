using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace X12EDI.Blazor.API.Controllers
{
    [ApiController]
    [Route("api/files")]
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

        [HttpGet("{id}")]
        public IActionResult GetFile(string id)
        {
            var file = _fileProvider.GetFileInfo(id);
            if (!file.Exists)
            {
                return NotFound();
            }

            return File(file.CreateReadStream(), "application/edi-x12", $"{id}.edi");
        }

        #endregion Public Methods
    }
}