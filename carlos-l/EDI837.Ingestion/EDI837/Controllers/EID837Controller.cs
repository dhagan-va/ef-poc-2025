using EDI837.src.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IS3FileService _s3FileService;

        public EID837Controller(
            IEdi837FileService edi837FileService, 
            ILogger<EID837Controller> logger,
            IEdiParserService ediParserService,
            IS3FileService s3FileService)
        {
            _edi837FileService = edi837FileService;
            _logger = logger;
            _ediParserService = ediParserService;
            _s3FileService = s3FileService;
        }

        #region Local files processing
        // GET: api/EID837/GetEdi837PTransactionsByFileName/fileName
        [HttpGet("GetEdi837PTransactionsByFileName/{fileName}")]
        public IActionResult GetEdi837PTransactionsByFileName(string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();
            Stream stream = this._ediParserService.GetStreamByFileName(fileName);
            if (stream != null)
            {
                var validTransactions = this._ediParserService.ExtractValid837PTransactions(stream, parsingErrors);
                return Ok(parsingErrors.Count > 0 ? parsingErrors : validTransactions);
            }
            else
            {
                var errors = string.Join(", ", parsingErrors);
                return Ok($"Unable to get Transactions {errors}");
            }
           
        }

        //// GET: api/EID837/SaveEdi837POriginalTransactionsAsJsonByFileName/fileName
        [HttpGet("SaveEdi837POriginalTransactionsAsJsonByFileName/{fileName}")]
        public async Task<IActionResult> SaveEdi837POriginalTransactionsAsJsonByFileName(string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();
            Stream stream = this._ediParserService.GetStreamByFileName(fileName);

            if (stream != null)
            {
                var validTransactions = this._ediParserService.ExtractValid837PTransactions(stream, parsingErrors);
                var processedClaim = await this._edi837FileService.SaveOriginalClaim(validTransactions);

                return Ok(parsingErrors.Count > 0 ? parsingErrors : processedClaim);
            }
            else
            {
                var errors = string.Join(", ", parsingErrors);
                return Ok($"Unable to get Transactions {errors}");
            }
               
        }

        //// GET: api/EID837/SaveEdi837PByFileName/fileName
        [HttpGet("SaveEdi837PByFileName/{fileName}")]
        public async Task<IActionResult> SaveEdi837PByFileName(string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();
            Stream stream = this._ediParserService.GetStreamByFileName(fileName);
            if (stream != null)
            {
                var validTransactions = this._ediParserService.ExtractValid837PTransactions(stream, parsingErrors);
                var claims = await this._edi837FileService.Save837PClaims(validTransactions);
                return Ok(parsingErrors.Count > 0 ? parsingErrors : claims);
            }
            else
            {
                var errors = string.Join(", ", parsingErrors);
                return Ok($"Unable to get Transactions {errors}");
            }
            
        }
        #endregion
        
        #region Moto AWS S3
        //// GET: api/EID837/SaveEdi837PByFileName/bucketName
        [HttpGet("S3IdeBucketExistsByBucketName/{bucketName}")]
        public async Task<IActionResult> S3IdeBucketExistsByBucketName(string bucketName = "edi-bucket")
        {
            var result = await this._s3FileService.BucketExistsAsync(bucketName);

            return Ok(result);
        }

        //// GET: api/EID837/S3SaveEdiFileByBucketAndFileName/bucketName/fileName
        [HttpGet("S3SaveEdiFileByBucketAndFileName/{bucketName}/{fileName}")]
        public async Task<IActionResult> S3SaveEdiFileByBucketAndFileName(string bucketName = "edi-bucket",string fileName = "837File.edi")
        {
            List<string> parsingErrors = new List<string>();
            Stream stream = await this._s3FileService.GetClaimStreamByFileNameAsync(bucketName, fileName);
            if (stream != null)
            {
                var validTransactions = this._ediParserService.ExtractValid837PTransactions(stream, parsingErrors);
                var claims = await this._edi837FileService.Save837PClaims(validTransactions);
                return Ok(parsingErrors.Count > 0 ? parsingErrors : claims);
            }
            else
            {
                var errors = string.Join(", ", parsingErrors);
                return Ok($"Unable to get Transactions {errors}");
            }
               
        }

        //// GET: api/EID837/S3ProcessFilesByBucketAndPrefix/bucketName/prefix
        [HttpGet("S3ProcessFilesByBucketAndPrefix/{bucketName}/{prefix}")]
        public async Task<IActionResult> S3ProcessFilesByBucketAndPrefix(string bucketName = "edi-bucket", string prefix = "837")
        {
            List<string> parsingErrors = new List<string>();
            var streamsResults = await this._s3FileService.GetClaimStreamsByBucketNameAndPrefixAsync(bucketName, prefix);
            if (streamsResults.Count() >= 1)
            {
                foreach (var streamResult in streamsResults)
                {
                    var validTransactions = this._ediParserService.ExtractValid837PTransactions(streamResult.FileStream, parsingErrors);
                    await this._edi837FileService.SaveOriginalClaim(validTransactions);
                    var claims = await this._edi837FileService.Save837PClaims(validTransactions);
                    await this._s3FileService.DeleteFileAsync(bucketName, streamResult.FileName);
                }

                string processedFiles = string.Join(", ", streamsResults.Select(s => s.FileName));
                return Ok(parsingErrors.Count > 0 ? string.Join(", ", parsingErrors) : $"Processed Files: {processedFiles}");
            }
            else
            {
                var errors = string.Join(", ", parsingErrors);
                return Ok($"Unable to get Transactions {errors}");
            }

        }
        #endregion
    }
}
