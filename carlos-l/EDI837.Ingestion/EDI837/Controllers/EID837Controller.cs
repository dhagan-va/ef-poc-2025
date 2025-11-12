using EDI837.src.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace EDI837.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EID837Controller : ControllerBase
    {
        private readonly IEdi837FileService _edi837FileService;
        private readonly IEdiParserService _ediParserService;
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;
        private readonly ILogger _logger;

        [ActivatorUtilitiesConstructor]
        public EID837Controller(IEdi837FileService edi837FileService, IConfiguration configuration, IFileProvider fileProvider, ILogger<EID837Controller> logger )
        {
            _edi837FileService = edi837FileService;
            _configuration = configuration;
            _fileProvider = fileProvider;
            _logger = logger;
            _ediParserService = new EdiParserService(_configuration,_fileProvider, _logger );
        }

        // GET: api/EID837/GetEdi837PTransactionsFileByName/fileName
        [HttpGet("GetEdi837PTransactionsFileByName/{fileName}")]
        public IActionResult GetEdi837PTransactionsFileByName(string fileName)
        {
            
            var stream = this._ediParserService.GetStreamByFileName(fileName);
            
            var valideTransactions = this._edi837FileService.ExtractValid837PTransactions(stream);
            return Ok(valideTransactions);
        }

        //// GET: api/EID837
        //[HttpGet]
        //public IActionResult Parse837File()
        //{

        //    return Ok("EDI837 Controller is working.");
        //}
    }
}
