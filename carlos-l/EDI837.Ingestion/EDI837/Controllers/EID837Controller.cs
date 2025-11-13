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
        private readonly ILogger _logger;

        public EID837Controller(
            IEdi837FileService edi837FileService, 
            ILogger<EID837Controller> logger, 
            IEdiParserService ediParserService )
        {
            _edi837FileService = edi837FileService;
            _logger = logger;
            _ediParserService = ediParserService;            
        }

        // GET: api/EID837/GetEdi837PTransactionsFileByName/fileName
        [HttpGet("GetEdi837PTransactionsFileByName/{fileName}")]
        public IActionResult GetEdi837PTransactionsFileByName(string fileName = "837File.edi")
        {
            var validTransactions = this._ediParserService.ExtractValid837PTransactions(fileName);
            return Ok(validTransactions);
        }

        //// GET: api/EID837/SaveEdi837POriginalTransactionsAsXMLFileByName
        [HttpGet("SaveEdi837POriginalTransactionsAsJsonFileByName/{fileName}")]
        public async Task<IActionResult> SaveEdi837POriginalTransactionsAsJsonFileByName(string fileName = "837File.edi")
        {
            var validTransactions = this._ediParserService.ExtractValid837PTransactions(fileName);
            var processedClaim =  await this._edi837FileService.SaveOriginalClaim(validTransactions);
            return Ok(processedClaim);
        }
    }
}
