using EDI837.src.Services;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/EID837/GetEdi837PTransactionsByFileName/fileName
        [HttpGet("GetEdi837PTransactionsByFileName/{fileName}")]
        public IActionResult GetEdi837PTransactionsByFileName(string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();

            var validTransactions = this._ediParserService.ExtractValid837PTransactions(fileName, parsingErrors);

            return Ok(parsingErrors.Count > 0 ? parsingErrors : validTransactions);
        }

        //// GET: api/EID837/SaveEdi837POriginalTransactionsAsJsonByFileName/fileName
        [HttpGet("SaveEdi837POriginalTransactionsAsJsonByFileName/{fileName}")]
        public async Task<IActionResult> SaveEdi837POriginalTransactionsAsJsonByFileName(string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();

            var validTransactions = this._ediParserService.ExtractValid837PTransactions(fileName, parsingErrors);
            var processedClaim =  await this._edi837FileService.SaveOriginalClaim(validTransactions);
            
            return Ok(parsingErrors.Count > 0 ? parsingErrors : processedClaim);
        }

        //// GET: api/EID837/SaveEdi837PByFileName/fileName
        [HttpGet("SaveEdi837PByFileName/{fileName}")]
        public async Task<IActionResult> SaveEdi837PByFileName(string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();
            var validTransactions = this._ediParserService.ExtractValid837PTransactions(fileName, parsingErrors);
            
            var claims = await this._edi837FileService.Save837PClaims(validTransactions);

            return Ok(parsingErrors.Count > 0 ? parsingErrors : claims);
        }
    }
}
